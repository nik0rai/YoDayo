//Change in methods some vals are ref
using DirectShowLib;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ZXing;

namespace LastTry
{
    public partial class Form1 : Form
    {
        Mat done = new Mat(); //job done rotated roi
        Mat priblizh = new Mat(); //first zoom

        Point startPoint, endPoint; Rectangle rectangle; //for drawing temp rectangle
        bool isAction = false, isDown = false; //for selecting roi in the image
        Mat temp_frame = new Mat(); Mat frame = new Mat(); //frame and temp_frame used for optical-flow
        private DsDevice[] cam_devices = null; //array of devices
        private int? camId = null; //id of selected cam (can be null if no cam is selected)
        private VideoCapture capture; //capture class for capturing and retreving frames from cam
        System.Threading.Mutex obj = new System.Threading.Mutex(); //mutex for preventing thread interception (cam can be used in different applications)
        Image<Bgr, byte> img, SenderImg;

        public Form1()
        {
            InitializeComponent();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                capture = new VideoCapture((int)camId);
                capture.ImageGrabbed += Capture_ImageGrabbed; //when we have a frame for current cam to show we need to process it, so here is event
                capture.Start();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error); //it`s basically thrown when we cannot connect to a cam or we didn`t select any
            }
        }
        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            try
            {
                obj.WaitOne();
                capture.Retrieve(frame); //get frame from cam   
                imageBox1.Image = frame; //set frame to an image

                Detect(); //find qr code

                //capture.Retrieve(temp_frame); //try to get temp frame
                //imageBox2.Image = temp_frame;


                obj.ReleaseMutex(); //after setting up image we can release mutex
            }
            catch (Exception exp) //if we have any error will try error 
            {
                MessageBox.Show(exp.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Selecting cam via combo-box in tool strip menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            camId = toolStripComboBox1.SelectedIndex; //we need to keep track of which cam is selected
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            cam_devices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice); //get all video input devices

            foreach (var item in cam_devices)
                toolStripComboBox1.Items.Add(item.Name); //add cams to combo-box so we will be able to select cams
        }

        private void Detect()
        {
            if (frame.Bitmap == null) return; //if we don`t have any image
            BarcodeReader qr = new BarcodeReader(); //class for reading qr codes
            var dete = qr.Decode(frame.Bitmap); //try to find and decaode

            if (dete is null) return; //if we didn`t found qr code we should try again (we can get null if we didn`t find any qr codes)

            Point[] arr = new Point[dete.ResultPoints.Length]; //array of points (we should do this sh*t bcz there is no type convertions)

            int flag = 0;
            foreach (var item in dete.ResultPoints) //for points to draw rectangle
            {
                arr[flag] = new Point((int)item.X, (int)item.Y); //Decode().ResultPoints have points in float type however Point class accepts only int
                //Console.WriteLine($"X = {(int)item.X}; Y = {(int)item.Y}"); //just to test and see vals
                flag++;
            }

            VectorOfPoint points = new VectorOfPoint(arr); //special list of points for opencv
            var rect = CvInvoke.BoundingRectangle(points); //create rectangle out of this points
            CvInvoke.Rectangle(frame, rect, new MCvScalar(0, 0, 255), 2); //draw rectangle of image of interest (ROI)

            #region try to use optical flow
            //Mat output = new Mat(); Mat outputE = new Mat(); //output should be outputed image and outputE (errors of images)
            //also we need temp and current array of points (points array will have points of ROI)
            //VectorOfPoint points1 = new VectorOfPoint();
            //CvInvoke.CalcOpticalFlowPyrLK(frame, temp_frame, points, points1, output, outputE, this.Size, 0, new MCvTermCriteria(3));
            #endregion
            imageBox1.Image = frame;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (frame is null || img is null || SenderImg is null) {
                Console.WriteLine("Welp gg");
                return;
            }

            Mat frame_gray = new Mat(); //grayscale img
            CvInvoke.CvtColor(frame, frame_gray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

            Point[] points = new Point[4]
            {
                new Point(rectangle.X, rectangle.Y),
                new Point(rectangle.Right, rectangle.Y),
                new Point(rectangle.X, rectangle.Bottom),
                new Point(rectangle.Right, rectangle.Bottom)
            };

            double theta = 0, optimum = 0;
            Mat dist = new Mat();

            Console.WriteLine("initial guess initialized!");

            Initial(frame_gray, points, theta, optimum);

            TransformImage(frame_gray, points, theta, optimum, dist);
            dist.ConvertTo(dist, Emgu.CV.CvEnum.DepthType.Cv8U, 255);
            priblizh = dist; //nach priblizh

            Mat optTau = GetTau(theta, optimum);
            done = TILT(frame_gray, optTau, points, 0.1);

            Point[] jobDone = new Point[4];
            TransformPoints(points, done, out jobDone);

            VectorOfPoint jobVector = new VectorOfPoint(jobDone);

            var rect = CvInvoke.BoundingRectangle(jobVector);
            CvInvoke.Rectangle(frame, rect, new MCvScalar(0, 0, 255), 2);

            Form3 form3 = new Form3(done);
            form3.Show();
        }

        #region TILT alg helper methods and tilt alg itself

        private void TransformPoints(Point[] points, Mat done, out Point[] jobDone)
        {

            Point[] po = points; //for perspective transform

            var w = Math.Abs(po[0].X - po[2].X);
            var h = Math.Abs(po[0].Y - po[2].Y);

            var sum = Point.Add(points[0], new Size(points[2].X, points[2].Y));
            var center = new PointF(sum.X * 0.5f, sum.Y * 0.5f);

            var temp = new Mat(4, 1, Emgu.CV.CvEnum.DepthType.Cv64F, done.NumberOfChannels);

            for (int i = 0; i < 4; i++)
            {
                double[] arr = new double[temp.Cols * temp.Rows];
                arr = (temp.GetData()).OfType<double>().ToArray();

                arr[i + 0] = points[i].X - center.X;
                arr[i + 1] = points[i].Y - center.Y;
                temp = CvInvoke.CvArrToMat(GCHandle.Alloc(arr, GCHandleType.Pinned).AddrOfPinnedObject());


                //temp[i][0] = points[i].X - center.X;
                //temp[i][1] = points[i].Y - center.Y;
            }

            Mat Tau_inv = new Mat(); CvInvoke.Invert(done, Tau_inv, Emgu.CV.CvEnum.DecompMethod.Normal);

            CvInvoke.PerspectiveTransform(temp, temp, Tau_inv);

            for (int i = 0; i < 4; i++)
            {
                double[] arr = new double[temp.Cols * temp.Rows];
                arr = (temp.GetData()).OfType<double>().ToArray();

                po[i].X = (int)(arr[i + 0] + center.X);
                po[i].Y = (int)(arr[i + 1] + center.Y);
            }

            jobDone = po;

        }

        /// <summary>
        /// Parametrized transformation
        /// </summary>
        /// <param name="theta"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private Mat GetTau(double theta, double t)
        {
            double[] arr = {
                Math.Cos(theta) , -Math.Cos(theta) , 0 ,
                 Math.Sin(theta) , Math.Cos(theta) , 0 ,
                0 ,                     0 ,         1 
            }; //I can`t even normally define Mat like in other programming languages   

            Mat Tau = new Mat(3,3, Emgu.CV.CvEnum.DepthType.Cv64F, 1); //1 chanell I guess grayscaled img
            Marshal.Copy(arr, 0, Tau.DataPointer, 3 * 3);
            //var Tau = CvInvoke.CvArrToMat(GCHandle.Alloc(arr, GCHandleType.Pinned).AddrOfPinnedObject()); 

            double[] other = {
                1, t ,
                0, 1
            };

            Mat Temp = new Mat(2, 2, Emgu.CV.CvEnum.DepthType.Cv64F, 1); 
            Marshal.Copy(other, 0, Temp.DataPointer, 2 * 2);
            //var Temp = CvInvoke.CvArrToMat(GCHandle.Alloc(other, GCHandleType.Pinned).AddrOfPinnedObject());

            Mat TauRes = new Mat(Tau, new Range(0, 2), new Range(0, 2)); //reshaping a resulting Mat but use Tau vals
            CvInvoke.Multiply(TauRes, Temp, TauRes);
            return TauRes;
        }

        /// <summary>
        /// This will use parametrized tau on image
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="roi_vals">Region Of Interest</param>
        private void TransformImage(Mat image, Point[] roi_vals, Mat first, Mat second)
        {
            Point[] points = roi_vals; //for perspective transform

            var w = Math.Abs(roi_vals[0].X - roi_vals[2].X);
            var h = Math.Abs(roi_vals[0].Y - roi_vals[2].Y);

            var sum = Point.Add(points[0], new Size(points[2].X, points[2].Y));
            var center = new PointF(sum.X * 0.5f, sum.Y * 0.5f);

            var temp = new Mat(4, 1, Emgu.CV.CvEnum.DepthType.Cv64F, image.NumberOfChannels);

            for (int i = 0; i < 4; i++)
            {
                double[] arr = new double[temp.Cols * temp.Rows];
                arr = (temp.GetData()).OfType<double>().ToArray();

                arr[i + 0] = points[i].X - center.X;
                arr[i + 1] = points[i].Y - center.Y;

                Marshal.Copy(arr, 0, temp.DataPointer, temp.Rows * temp.Cols);
                //temp = CvInvoke.CvArrToMat(GCHandle.Alloc(arr, GCHandleType.Pinned).AddrOfPinnedObject());


                //temp[i][0] = points[i].X - center.X;
                //temp[i][1] = points[i].Y - center.Y;
            }
            CvInvoke.PerspectiveTransform(temp, temp, first);

            for (int i = 0; i < 4; i++)
            {
                double[] arr = new double[temp.Cols * temp.Rows];
                arr = (temp.GetData()).OfType<double>().ToArray();

                points[i].X = (int)arr[i + 0] + w / 2;
                points[i].Y = (int)arr[i + 1] + h / 2;

                //points[i].X = temp[i][0] + w/2;
                //points[i].Y = temp[i][1] + h/2;
            }
            second = Mat.Zeros((int)h, (int)w, Emgu.CV.CvEnum.DepthType.Cv64F, second.NumberOfChannels); //in case it can be double

            VectorOfPoint roi = new VectorOfPoint(roi_vals);
            VectorOfPoint warped = new VectorOfPoint(points);

            Mat transpos = CvInvoke.GetPerspectiveTransform(roi, warped);
            CvInvoke.WarpPerspective(image, second, transpos, new Size(w, h), Emgu.CV.CvEnum.Inter.Lanczos4);
        }

        /// <summary>
        /// Same but we will use params of parametrization
        /// </summary>
        /// <param name="image"></param>
        /// <param name="roi"></param>
        /// <param name="theta"></param>
        /// <param name="t"></param>
        /// <param name="destiant"></param>
        private void TransformImage(Mat image, Point[] roi, double theta, double t, Mat destiant)
        {
            Point[] points = roi; //for perspective transform

            Mat Tau = GetTau(theta, t);

            var w = Math.Abs(roi[0].X - roi[2].X);
            var h = Math.Abs(roi[0].Y - roi[2].Y);

            var sum = Point.Add(points[0], new Size(points[2].X, points[2].Y));
            var center = new PointF(sum.X * 0.5f, sum.Y * 0.5f);

            var temp = new Mat(4, 1, Emgu.CV.CvEnum.DepthType.Cv64F, image.NumberOfChannels);

            for (int i = 0; i < 3; i++)
            {
                double[] arr = new double[temp.Cols * temp.Rows];
                arr = (temp.GetData()).OfType<double>().ToArray();

                arr[i + 0] = points[i].X - center.X;
                arr[i + 1] = points[i].Y - center.Y;

                Marshal.Copy(arr, 0, temp.DataPointer, temp.Rows * temp.Cols);

                //temp[i][0] = points[i].X - center.X;
                //temp[i][1] = points[i].Y - center.Y;
            }
            CvInvoke.PerspectiveTransform(temp, temp, Tau);

            for (int i = 0; i < 3; i++)
            {
                double[] arr = new double[temp.Cols * temp.Rows];
                arr = (temp.GetData()).OfType<double>().ToArray();

                points[i].X = (int)arr[i + 0] + w / 2;
                points[i].Y = (int)arr[i + 1] + h / 2;           

                //points[i].X = temp[i][0] + w/2;
                //points[i].Y = temp[i][1] + h/2;
            }

            destiant = Mat.Zeros((int)h, (int)w, Emgu.CV.CvEnum.DepthType.Cv64F, image.NumberOfChannels);

            throw new NotImplementedException(); //todo: convert rois and warped into Mat which has float32 type { {2, 5}, {6, 2}, {6, 2}, {1,0} } !FLOAT NUMBERS IS CREATED BY . => 3.6
            VectorOfPoint roiS = new VectorOfPoint(roi); //we should create mat of doubles [mat]
            VectorOfPoint warped = new VectorOfPoint(points);



            Console.WriteLine(warped.Size);

            Mat tr = CvInvoke.GetPerspectiveTransform(roiS, warped);
            CvInvoke.WarpPerspective(image, destiant, tr, new Size(w, h), Emgu.CV.CvEnum.Inter.Lanczos4);
        }

        /// <summary>
        /// We will need to minimaize this cost
        /// </summary>
        /// <param name="image">Input image</param>
        /// <returns>sum of resulting singular value matrix</returns>
        private double Cost(Mat image)
        {
            var temp = new Mat(); var normalized = new Mat();
            CvInvoke.Normalize(image, normalized); //if u thing that thes steps are useless welp it save all alterations in input Mat's
            CvInvoke.Divide(image, normalized, temp);

            var w = new Mat(); var u = new Mat(); var v = new Mat();
            CvInvoke.SVDecomp(image, w, u, v, Emgu.CV.CvEnum.SvdFlag.NoUV); //we will need only resulting singular value matrix
            return CvInvoke.Sum(w).V0; //first scalar value will be our sum
        }

        private double GarrotShrink(double element, double ratio)
        {
            double ans;
            if (Math.Abs(element) > ratio)
                ans = element - ((ratio * ratio) / element);
            else ans = 0;

            return ans;
        }

        private Mat Weakening(Mat image, double coef)
        {
            Mat result = new Mat(image.Size, Emgu.CV.CvEnum.DepthType.Cv64F, image.NumberOfChannels);
            for (int i = 0; i < image.Rows; i++)
            {
                for (int j = 0; j < image.Cols; j++)
                {
                    double[] arr = new double[result.Cols * result.Rows];
                    arr = (result.GetData()).OfType<double>().ToArray();

                    arr[i * j] = GarrotShrink(arr[i * j], coef);

                    Marshal.Copy(arr, 0, result.DataPointer, result.Cols * result.Rows);                  
                    //result[i, j] = GarrotShrink(result[i, j], coef); //element from mat             
                }
            }
            return result;
        }

        /// <summary>
        /// Initial guess
        /// </summary>
        /// <param name="image"></param>
        /// <param name="roi"></param>
        /// <param name="theta"></param>
        /// <param name="theta_second"></param>
        private void Initial(Mat image, Point[] roi, double theta, double theta_second)
        {
            double minAngle = Math.PI / 6, maxAngle = Math.PI / 6, stepsAngle = 10;
            double min = -0.5, max = 0.5, stepsAngle_s = 10;
            double cost_min = double.MaxValue;
            Mat destiant = new Mat();

            double step = (maxAngle - minAngle) / stepsAngle;
            double step_s = (max - min) / stepsAngle_s;

            double t = minAngle, _t = min;

            for (int i = 0; i < stepsAngle; i++)
            {
                _t = min;
                for (int j = 0; j < stepsAngle_s; j++)
                {
                    TransformImage(image, roi, t, _t, destiant);
                    double s = Cost(destiant);
                    if (s < cost_min)
                    {
                        cost_min = s;
                        theta = t;
                        theta_second = _t;
                    }
                    _t += step_s;
                }
                t += step;
            }
        }

        private void GetJacob(Mat image, Point[] roi_vals, Mat Tau, Mat Jacobian)
        {
            int pow = 8; //ammount of power of degrees of transformation (maslo maslyannoe)
            double e = 1e-3; //very low number for differential equasion

            var ImageFirst = new Mat(); var ImageSecond = new Mat(); 
            var Tau_prev = new Mat(); var Tau_mem = new Mat(); var diff = new Mat();

            //Region of ROI
            int n = Math.Abs(roi_vals[0].X - roi_vals[2].X);
            int m = Math.Abs(roi_vals[0].Y - roi_vals[2].Y);

            Jacobian = Mat.Zeros(n * m, pow, Emgu.CV.CvEnum.DepthType.Cv64F, image.NumberOfChannels); //RESULTING MATRIX

            //the 9th element is always equals 1 => will calc other 8
            for (int i = 0; i < pow; i++)
            {
                Tau_prev = Tau.Clone();
                Tau_mem = Tau.Clone();

                #region Tau_prev[i]+=e;
                double[] arr = new double[Tau_prev.Cols * Tau_prev.Rows];
                arr = (Tau_prev.GetData()).OfType<double>().ToArray();
                arr[i] += e;

                Marshal.Copy(arr, 0, Tau_prev.DataPointer, Tau_prev.Rows * Tau_prev.Cols);                
                #endregion

                #region Tau_mem[i]-=e;
                arr = new double[Tau_mem.Cols * Tau_mem.Rows];
                arr = (Tau_mem.GetData()).OfType<double>().ToArray();

                arr[i] -= e;

                Marshal.Copy(arr, 0, Tau_mem.DataPointer, Tau_mem.Rows * Tau_mem.Cols);
                #endregion

                //we need to change Tau_prev on i pos += e //cant alterate mats in c#
                //we need to change Tau_mem on i pos -= e //cant alterate mats in c#
                TransformImage(image, roi_vals, Tau_prev, ImageFirst);
                TransformImage(image, roi_vals, Tau_mem, ImageSecond);
                diff = ImageFirst - ImageSecond; //difference of two images
                CvInvoke.GaussianBlur(diff, diff, new Size(3, 3), 2); //reduce noize
                diff = diff.Reshape(1, m * n);
                diff /= (2 * e);
                diff.CopyTo(Jacobian.Col(i));
            }
        }

        private Mat TILT(Mat image, Mat tau, Point[] roi, double lambda)
        {
            int maxCycles = 100, cycle_cnt1 = 0, cycle_cnt2 = 0;
            bool conv1 = false, conv2 = false;

            Mat Tau_temp = new Mat(); Mat Dlt_Tau = new Mat(); Mat Dlt_Tau_prev = new Mat(); Mat JacobMat = new Mat();
            Mat Tau = tau.Clone();

            double Cst = double.MaxValue;

            int n = Math.Abs(roi[0].X - roi[2].X);
            int m = Math.Abs(roi[0].Y - roi[2].Y);
            int degree = 8;

            double moo, ratio = 1.25; //1.25 it is the ratio that always should increse

            while (!conv1)
            {
                //transform image and get jacobian
                TransformImage(image, roi, Tau, Tau_temp);
                GetJacob(image, roi, Tau, JacobMat);

                //linear job of optimization
                Mat linMat = Mat.Zeros(m, n, Emgu.CV.CvEnum.DepthType.Cv64F, image.NumberOfChannels);               

                //Weakining step
                moo = 1.25 / CvInvoke.Norm(Tau_temp);
                Dlt_Tau = Mat.Zeros(degree, 1, Emgu.CV.CvEnum.DepthType.Cv64F, image.NumberOfChannels);
                Dlt_Tau_prev = Mat.Zeros(degree, 1, Emgu.CV.CvEnum.DepthType.Cv64F, image.NumberOfChannels);

                Mat y = Tau_temp.Clone();
                Mat image_t = new Mat();

                cycle_cnt2 = 0;
                conv2 = false;

                Mat JacobInverted = new Mat();
                CvInvoke.Invert(JacobMat, JacobInverted, Emgu.CV.CvEnum.DecompMethod.Svd);

                while (!conv2)
                {
                    Mat first = new Mat();
                    CvInvoke.Multiply(JacobMat, Dlt_Tau, first);

                    Mat tmp = new Mat(); Mat u = new Mat(); Mat s = new Mat(); Mat v = new Mat();

                    CvInvoke.SVDecomp(Tau_temp + first.Reshape(1, m) - linMat + y / moo, s, u, v, Emgu.CV.CvEnum.SvdFlag.Default);
                    s = Weakening(s, 1 / moo);

                    //we need diagonal matrix
                    Mat ForDiagonal = Mat.Zeros(s.Rows, s.Rows, Emgu.CV.CvEnum.DepthType.Cv64F, s.NumberOfChannels);
                    s.CopyTo(ForDiagonal.Diag());

                    //U * W * V
                    var flag = new Mat();
                    CvInvoke.Multiply(u, ForDiagonal, flag); 
                    CvInvoke.Multiply(flag, v, image_t);

                    tmp = Tau_temp + first.Reshape(1, m) - image_t + y / moo;
                    linMat = Weakening(tmp, lambda / moo);

                    tmp = ((0 - Tau_temp) + image_t + linMat - y / moo); //((- I_Tau) + I0 + E - Y / mu) is it equal?
                    CvInvoke.Multiply(JacobInverted, tmp.Reshape(1, m * n), Dlt_Tau);

                    CvInvoke.Multiply(JacobMat, Dlt_Tau, tmp);
                    y += moo * (Tau_temp + tmp.Reshape(1, m) - image_t - linMat);

                    moo *= ratio;

                    //will use our counter of cycles
                    cycle_cnt2++;
                    if (cycle_cnt2 > maxCycles) break;

                    //does it work?
                    double minDouble = 0, maxDouble = 0; Point minPoint = new Point(0, 0); Point maxPoint = new Point(0, 0);
                    var flagger_fuckker = new Mat(); //cringe...
                    CvInvoke.AbsDiff(Dlt_Tau_prev, Dlt_Tau, flagger_fuckker);
                    CvInvoke.MinMaxLoc(flagger_fuckker, ref minDouble, ref maxDouble, ref minPoint, ref maxPoint);
                    if (cycle_cnt2 > 1 && maxDouble < 1e-3) conv2 = true;

                    Dlt_Tau_prev = Dlt_Tau.Clone(); //process of work
                }

                for (int i = 0; i < degree; i++)
                {
                    #region Tau[i] += Dlt_Tau[i];

                    double[] arr = new double[Tau.Cols * Tau.Rows];
                    arr = (Tau.GetData()).OfType<double>().ToArray();

                    double[] arr1 = new double[Dlt_Tau.Cols * Dlt_Tau.Rows];
                    arr1 = (Dlt_Tau.GetData()).OfType<double>().ToArray();

                    arr[i] += arr1[i];

                    Marshal.Copy(arr, 0, Tau.DataPointer, Tau.Rows * Tau.Cols);                  
                    #endregion
                    //we change Tau element on pos i to deltaTau element on pos i => Tau[i] += deltTau[i];
                }


                Mat destinated = new Mat();
                TransformImage(image, roi, Tau, destinated);

                double cst = Cost(destinated);
                double prevf = Cst - cst;
                Cst = cst;

                destinated.ConvertTo(destinated, Emgu.CV.CvEnum.DepthType.Cv8U, 255);
                done = destinated; //will save progress to done mat

                if (prevf <= 0) conv1 = true;
                cycle_cnt1++;
                if (cycle_cnt1 > maxCycles) break;
            }
            return Tau;
        } 
        #endregion

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (camId is null) return; //if cam is not selected

            camId = null;
            capture.Pause();
            capture.Dispose();
            capture.Stop();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if(result is DialogResult.OK)
            {
                var path = openFileDialog1.FileName;
                frame = new Mat(path); //frame from file              
                img = new Image<Bgr, byte>(path);
                imageBox1.Image = img; //set to picture box
            }
            else MessageBox.Show("Image should be selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is programm for qr-code detection and decoding using TILT alghoritm." +
                "\nCreator: nik0rai\nGroup: 15.11d-mo11\\19b\nTeam: Inadzuma", "About", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you shure you want to exit programm?", "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            switch (res)
            {

                case DialogResult.Yes:
                    Application.Exit();
                    break;
                case DialogResult.No: //well we still in application
                    break;
                default:
                    MessageBox.Show("WTF", "How did you do it? >.<"); //?wizardry?
                    break;
            }
        }

        private void imageBox1_MouseLeave(object sender, EventArgs e)
        {
            if (isAction) imageBox1.Cursor = Cursors.Cross;
            else imageBox1.Cursor = Cursors.Default;
        }

        private void imageBox1_MouseEnter(object sender, EventArgs e)
        {
            if (isAction) imageBox1.Cursor = Cursors.Cross;
            else imageBox1.Cursor = Cursors.Default;
        }

        private Rectangle TempRect()
        {
            rectangle = new Rectangle();
            rectangle.X = Math.Min(startPoint.X, endPoint.X);
            rectangle.Y = Math.Min(startPoint.Y, endPoint.Y);
            rectangle.Width = Math.Abs(startPoint.X - endPoint.X);
            rectangle.Height = Math.Abs(startPoint.Y - endPoint.Y);
            return rectangle;
        }

        private void imageBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isAction is true && isDown is true)
            {
                endPoint = e.Location;
                isDown = false;
                if (rectangle != null)
                {
                    img.ROI = rectangle;
                    SenderImg = img.Copy(); //save this temp croped image to show in other window
                    img.ROI = Rectangle.Empty;
                }
            }
        }

        private void imageBox1_Paint(object sender, PaintEventArgs e)
        {
            if (rectangle == null) return;
            e.Graphics.DrawRectangle(Pens.Blue, TempRect());
        }

        private void imageBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (isAction is false) return;
            isDown = true;
            startPoint = e.Location;
        }

        private void selectedImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(SenderImg);
            form2.Show();
        }

        private void imageBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(isAction && isDown)
            {
                endPoint = e.Location;
                imageBox1.Invalidate();
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (imageBox1.Image == null) return;

            if (isAction is true) isAction = false;
            else isAction = true;
        }
    }
}

using Emgu.CV;
using Emgu.CV.Structure;
using System.Windows.Forms;

namespace LastTry
{
    public partial class Form2 : Form
    {

        public Form2(Image<Bgr,byte> image)
        {
            InitializeComponent();
            if (image is null) return;

            this.Width = (image.Width + 16)*2;
            this.Height = (image.Height + 39)*2;
            this.Text = "Selected Region Of Interest";
            imageBox1.Image = image;
        }
    }
}

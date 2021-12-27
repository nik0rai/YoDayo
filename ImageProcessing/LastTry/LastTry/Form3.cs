using Emgu.CV;
using Emgu.CV.Structure;
using System.Windows.Forms;

namespace LastTry
{
    public partial class Form3 : Form
    {
        public Form3(Mat image)
        {
            InitializeComponent();
            if (image is null) return;
            imageBox1.Image = image;
        }
    }
}

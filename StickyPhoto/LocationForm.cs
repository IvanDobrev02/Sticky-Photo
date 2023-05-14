using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StickyPhoto
{
    public partial class LocationForm : Form
    {
        public Point mouseDownLocation;

        public LocationForm()
        {
            InitializeComponent();
        }

        private void LocationForm_Load(object sender, EventArgs e)
        {
            Bitmap bmp = CaptureMyScreen();
            pictureBox1.Image = bmp;
            pictureBox1.Size = bmp.Size;
        }

        public Bitmap CaptureMyScreen()
        {
            Bitmap captureBitmap = new Bitmap(1536, 864, PixelFormat.Format32bppArgb);
            Rectangle captureRectangle = Screen.AllScreens[0].Bounds;
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);
            captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);

            return captureBitmap;
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            FormMain.myPoint = e.Location;
            Close();
        }
    }
}

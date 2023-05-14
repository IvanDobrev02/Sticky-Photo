using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace StickyPhoto
{
    public partial class FormMain : Form
    {
        public static string Path = @"..\..\";
        string fileName = "History.txt";
        bool multiple = false;
        int timeLeft;
        int numberPhotos = 0;
        int n = 0;
        public static Point myPoint = new Point();
        List<Bitmap> bitmaps = new List<Bitmap>();

        public FormMain()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(300, 0);
        }


        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.S
                && e.KeyCode!= Keys.C
                && e.KeyCode != Keys.L
                && e.KeyCode != Keys.X)
            {
                return;
            }

            if (e.KeyCode == Keys.S)
            {
                multiple = false;
                timer1.Stop();
                var ofd = new OpenFileDialog();

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var bpm = new Bitmap(ofd.FileName);
                    pictureBox1.Size = bpm.Size;
                    pictureBox1.Image = bpm;
                    this.Size = bpm.Size;
                }

                ofd.Dispose();
            }
            else if (e.KeyCode == Keys.X)
            {
                if (multiple)
                {
                    IFormatter formatter = new BinaryFormatter();
                    string filepath = Path + $"{fileName}";

                    using (var fs = new FileStream(filepath, FileMode.Create))
                        formatter.Serialize(fs, bitmaps);
                }
                Environment.Exit(0);
            }
            else if (e.KeyCode == Keys.C)
            {
                multiple = true;
                timer1.Stop();
                bitmaps.Clear();
                List<string> imgNames = new List<string>();

                var ofd = new OpenFileDialog
                {
                    Multiselect = true,
                };

                ofd.ShowDialog();

                for (int i = 0; i < ofd.FileNames.Count(); i++)
                {
                   imgNames.Add(ofd.FileNames[i]);
                }

                for (int i = 0; i < imgNames.Count; i++)
                {
                    var bmp = new Bitmap(imgNames[i]);
                    bitmaps.Add(bmp);
                }

                numberPhotos = imgNames.Count;

                timer1.Interval = 5000;
                timer1.Start();
                timeLeft = 600000;

            }
            else if (e.KeyCode == Keys.L)
            {
                LocationForm lcf = new LocationForm();
                lcf.ShowDialog();

                this.Location = myPoint;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            if(n > numberPhotos - 1)
            {
                n = 0;
            }

            pictureBox1.Size = bitmaps[n].Size;
            pictureBox1.Image = bitmaps[n];
            this.Size = bitmaps[n].Size;

            n++;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.pictureBox1.UseWaitCursor = false;
            string filepath = Path + $"{fileName}"; 
            if (File.Exists(filepath))
            {
                string txt = File.ReadAllText(filepath);

                if (txt != "")
                {
                    IFormatter formatter = new BinaryFormatter();

                    using (var fs = new FileStream(filepath, FileMode.Open))
                    {
                        bitmaps = (List<Bitmap>)formatter.Deserialize(fs);
                    }

                    numberPhotos = bitmaps.Count;

                    timer1.Interval = 5000;
                    timer1.Start();

                    timeLeft = 600000;
                }
            }
            else
            {
                using (File.Create(Path + fileName));   
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                e.Cancel = true;
            }
        }
    }
}

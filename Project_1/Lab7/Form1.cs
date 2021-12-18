using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Lab4


{
    public partial class Form1 : Form
    {
        Image<Bgr, byte> inputImage = null;
        Image<Gray, int> labelImage = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnReview_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = openFileDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {
                    inputImage = new Image<Bgr, byte>(openFileDialog1.FileName);
                    tbPath.Text = openFileDialog1.FileName;
                    btnCalculate_Click(this, null);
                }
                else
                    MessageBox.Show("Файл не выбран", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            inputImage = new Image<Bgr, byte>(tbPath.Text);
            Image<Gray, byte> imageGray = inputImage.Convert<Gray, byte>();
            pictureBox1.Image = imageGray.ToBitmap();
            var mask = imageGray.ThresholdBinaryInv(new Gray(151), new Gray(255));
            Mat distanceTransofrm = new Mat();
            CvInvoke.DistanceTransform(mask, distanceTransofrm, null, Emgu.CV.CvEnum.DistType.L2, 3);
            CvInvoke.Normalize(distanceTransofrm, distanceTransofrm, 0, 255, Emgu.CV.CvEnum.NormType.MinMax);
            var markers = distanceTransofrm.ToImage<Gray, byte>()
                .ThresholdBinary(new Gray(16), new Gray(255));

            textBox1.Text =  (CvInvoke.ConnectedComponents(markers, markers) - 1).ToString();
            var finalMarkers = markers.Convert<Gray, Int32>();

            CvInvoke.Watershed(inputImage, finalMarkers);

            Image<Gray, byte> boundaries = finalMarkers.Convert<byte>(delegate (Int32 x)
            {
                return (byte)(x == -1 ? 255 : 0);
            });

            boundaries._Dilate(1);
            inputImage.SetValue(new Bgr(0, 0, 255), boundaries);
            //AddImage(img, "Watershed Segmentation");
            pictureBox3.Image = inputImage.ToBitmap();



            //inputImage = new Image<Bgr, byte>(tbPath.Text);
            ////Исходное изображение
            //Image<Gray, byte> imageGray = inputImage.Convert<Gray, byte>();
            //pictureBox1.Image = imageGray.ToBitmap();


            //Image<Gray, byte> mask = imageGray.Clone();

            //pictureBox2.Image = mask.ToBitmap();


            //textBox1.Text = CvInvoke.ConnectedComponents(mask, mask).ToString();

            //var finalMarkers = mask.Convert<Gray, int>();

            //CvInvoke.Normalize(finalMarkers, finalMarkers, 0, 255, NormType.MinMax);
            //pictureBox3.Image = finalMarkers.ToBitmap();

            //CvInvoke.Watershed(inputImage, finalMarkers);

            //Image<Gray, byte> boundaries = finalMarkers.Convert(delegate (int x)
            //{
            //    return (byte)(x == -1 ? 255 : 0);
            //});
            //boundaries._Dilate(2);
            //inputImage.SetValue(new Bgr(0, 0, 255), boundaries);
            ////pictureBox3.Image = (temp.Convert<Gray, byte>() + boundaries).ToBitmap();
            ////pictureBox4.Image = (inputImage + boundaries.Convert<Bgr, byte>()).ToBitmap();
            //pictureBox4.Image = inputImage.ToBitmap();

        }

        private void pictureBox3_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {
            if (labelImage != null)
            {
                int label = (int)labelImage[e.Y, e.X].Intensity;
                textBox1.Text = $"{label} {e.X} {e.Y}";

                var temp = labelImage.InRange(new Gray(label), new Gray(label));
                pictureBox5.Image = temp.Bitmap;
            }
        }
    }
}
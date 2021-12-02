using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab7
{
    public partial class Form1 : Form
    {
        Image<Bgr, byte> inputImage = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnReview_Click_1(object sender, EventArgs e)
        {
            
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
            //Исходное изображение
            Image<Gray, byte> imageGray = inputImage.Convert<Gray, byte>();
            pictureBox1.Image = imageGray.ToBitmap();


            pictureBox1.Image = imageGray.ToBitmap();
            Image<Gray, byte> mask = imageGray.Clone();
            //pictureBox2.Image = imageGrad.ToBitmap();


            //var threshold = CvInvoke.Threshold(imageGray, mask, 0, 255, ThresholdType.Otsu/* | ThresholdType.BinaryInv*/);
            //Mat kernel = CvInvoke.GetStructuringElement(ElementShape.Ellipse, new Size((int)numericUpDown2.Value, (int)numericUpDown2.Value), new Point(-1, -1));
            //mask = mask.MorphologyEx(MorphOp.Open, kernel, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());



            //mask = mask.ThresholdBinaryInv(new Gray((double)numericUpDown1.Value), new Gray(255));
            //var mask = imageGrad.ThresholdBinaryInv()
            //var mask = imageGrad.ThresholdBinaryInv(new Gray(32), new Gray(255));
            pictureBox2.Image = mask.ToBitmap();
            //Mat distanceTransofrm = new Mat();
            //CvInvoke.DistanceTransform(mask, distanceTransofrm, null, DistType.L2, 3);
            //Image<Gray, byte> markers = distanceTransofrm.ToImage<Gray, byte>();

            //CvInvoke.ConnectedComponents(markers, markers);
            //var temp = inputImage.Convert<Gray, byte>();
            //CvInvoke.ConnectedComponents(temp, temp);
            //CvInvoke.Normalize(temp, temp, 0, 255, NormType.MinMax);
            //CvInvoke.Normalize(markers, markers, 0, 255, NormType.MinMax);
            //Image<Gray, int> finalMarkers = markers.Convert<Gray, int>();


            ////pictureBox3.Image = labelImage.ToBitmap();

            //var tempDist = distanceTransofrm.ToImage<Gray, int>();
            //CvInvoke.Normalize(tempDist, tempDist, 0, 255, NormType.MinMax);
            //labelImage = finalMarkers;
            ////pictureBox3.Image = labelImage.ToBitmap();
            //pictureBox4.Image = temp.ToBitmap();

            textBox1.Text = CvInvoke.ConnectedComponents(mask, mask).ToString();

            var finalMarkers = mask.Convert<Gray, int>();

            CvInvoke.Normalize(finalMarkers, finalMarkers, 0, 255, NormType.MinMax);
            pictureBox3.Image = finalMarkers.ToBitmap();

            CvInvoke.Watershed(inputImage, finalMarkers);

            Image<Gray, byte> boundaries = finalMarkers.Convert(delegate (int x)
            {
                return (byte)(x == -1 ? 255 : 0);
            });
            boundaries._Dilate(2);
            inputImage.SetValue(new Bgr(0, 0, 255), boundaries);
            //pictureBox3.Image = (temp.Convert<Gray, byte>() + boundaries).ToBitmap();
            //pictureBox4.Image = (inputImage + boundaries.Convert<Bgr, byte>()).ToBitmap();
            pictureBox4.Image = inputImage.ToBitmap();
        }


    }
}

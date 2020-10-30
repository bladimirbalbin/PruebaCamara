using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AForge.Video.FFMPEG;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Video.VFW;
namespace CameraRecord
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo;
        public static Bitmap _latestFrame;
        Graphics g;
        private Bitmap video;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo VideoCaptureDevice in VideoCaptureDevices)
            {
                comboBox1.Items.Add(VideoCaptureDevice.Name);
            }
            comboBox1.SelectedIndex = 0;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[comboBox1.SelectedIndex].MonikerString);
            FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
            FinalVideo.Start();
        }

        void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            video = (Bitmap)eventArgs.Frame.Clone();
            g = Graphics.FromImage(video);
            g.DrawString(DateTime.Now.ToString(), new Font("Arial", 30), new SolidBrush(Color.Green), new PointF(2, 2));
            g.Dispose();
            pictureBox1.Image = video;


        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (FinalVideo.IsRunning)
            {
                FinalVideo.Stop();
            }
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {

            AVIWriter writer = new AVIWriter("wmv3");
            writer.Open("test.avi", 320, 240);
            Bitmap image = new Bitmap(320, 240);
            for (int i = 0; i < 240; i++)
            {
                image.SetPixel(i, i, Color.Red);
                writer.AddFrame(image);
            }
            writer.Close();
        }


    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using AForge.Video;
using AForge.Controls;
using AForge.Imaging;
using AForge.Genetic;
using System.Media;
using Vlc.DotNet.Forms;




//using AForge.Video.FFMPEG;




namespace PruebaCamara
{
    public partial class Form1 : Form
    {
        private bool HayDispositivos = false;
        private FilterInfoCollection MisDispositivos;
        private VideoCaptureDevice MiWebCam = null;
        private VideoSourcePlayer VideoSourcePlayer;
        //mediaPlayer = new MediaPlayer();
        Vlc.DotNet.Forms.VlcControl vlcControl = new VlcControl();
        //AForge aForge = new AForge();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargaDispositivos();
            
        }

        public void CargaDispositivos()
        {
            MisDispositivos = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            string laCamara = "";
            if (MisDispositivos.Count >0 ) 
            {
                for (int i = 0; i < MisDispositivos.Count; i++)
                {
                    comboBox1.Items.Add(MisDispositivos[i].Name.ToString());
                    if (MisDispositivos[i].Name.ToString()== "HP Truevision HD")
                    {
                        laCamara = MisDispositivos[i].Name.ToString();
                    }
                    HayDispositivos = true;
                }
                comboBox1.Text = laCamara;

            }
            else
            {
                HayDispositivos = false;
            }


        }
        public void CerrarWebCam()
        {
            if (MiWebCam != null && MiWebCam.IsRunning)
            {
                MiWebCam.SignalToStop();
                MiWebCam = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CerrarWebCam();
            int i = comboBox1.SelectedIndex;
            string NombreVideo = MisDispositivos[i].MonikerString;
            MiWebCam = new VideoCaptureDevice(NombreVideo);
            MiWebCam.NewFrame += new NewFrameEventHandler(Capturando);
            MiWebCam.Start();
            
        }

        private void Capturando(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap Imagen = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = Imagen;            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            CerrarWebCam();
        }




        
    }
}

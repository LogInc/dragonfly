using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Dragonfly
{
    /// <summary>
    /// DroneVision handles all the Computer Vision stages including
    /// 
    /// Image Acquisition
    /// 
    /// Image Pre-Processing
    /// 
    /// Object Detection and Recognition
    /// 
    /// DroneVision detects and locates objects of interest, providing useful output
    /// that can be used for maneuvering the drone.
    /// </summary>
    public static class DroneVision
    {
        private static Capture capture;
        private static DispatcherTimer timer_job;

        public static void Initialize()
        {
            capture = new Capture();
            timer_job = new DispatcherTimer();
            timer_job.Tick += new EventHandler(Job_Tick);
            timer_job.Interval = new TimeSpan(0, 0, 0, 0, 1);
        }

        public static void Start()
        {
            timer_job.Start();
        }

        public static void Stop()
        {
            timer_job.Stop();
        }

        private static void Job_Tick(object sender, EventArgs e)
        {
            Mat frame = new Mat();
            capture.Retrieve(frame);

            Mat gray = new Mat();
            CvInvoke.CvtColor(frame, gray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
            CvInvoke.EqualizeHist(gray, gray);

            Mat temp = new Mat();
            CvInvoke.GaussianBlur(gray, temp, new Size(3, 3), 1);
            gray = temp;

            List<Rectangle> detectedFaces = new List<Rectangle>();
            FaceDetector.DetectFace(gray, detectedFaces);
            foreach (var face in detectedFaces)
            {
                CvInvoke.Rectangle(gray, face, new MCvScalar(255, 0, 0));
            }

            CvInvoke.Imshow("Gray", gray);
        }
    }
}

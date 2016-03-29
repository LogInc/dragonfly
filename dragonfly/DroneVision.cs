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
        /// <summary>
        /// Used to specify which device to select for capturing images.
        /// </summary>
        public enum VideoSource
        {
            Drone,
            Webcam,
        }

        private static Capture capture;
        private static DispatcherTimer timer_job;
        private static Mat captured_frame = new Mat();
        private static Mat composed_image = new Mat();

        public static void Initialize(VideoSource source=VideoSource.Webcam)
        {
            capture = new Capture();
            timer_job = new DispatcherTimer();
            timer_job.Tick += new EventHandler(Job_Tick);
            timer_job.Interval = new TimeSpan(0, 0, 0, 0, 1); // 1 ms
        }

        public static Emgu.CV.Image<Bgr, Byte> GetImage()
        {
            if (composed_image.Bitmap != null)
            {
                var image = composed_image.ToImage<Emgu.CV.Structure.Bgr, Byte>();
                return image;
            }
            return null;
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
            Mat gray = new Mat();
            Mat temp = new Mat();

            capture.Retrieve(captured_frame);
            composed_image = captured_frame;
            // Color to gray conversion
            CvInvoke.CvtColor(captured_frame, gray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

            // Histogram equalization
            CvInvoke.EqualizeHist(gray, gray);

            // Noise removal
            CvInvoke.GaussianBlur(gray, temp, new Size(3, 3), 1);
            gray = temp;

            List<Rectangle> detectedFaces = new List<Rectangle>();
            FaceDetector.DetectFace(gray, detectedFaces);
            // Draw rectangles around each face
            foreach (var face in detectedFaces)
            {
                CvInvoke.Rectangle(temp, face, new MCvScalar(255, 0, 0));
            }

            CvInvoke.Imshow("Faces", temp);
        }
    }
}

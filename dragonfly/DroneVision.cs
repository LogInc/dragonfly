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
    /// DroneVision handles all the Computer Vision stages.
    /// 
    /// It captures a live feed and detect objects of interest that affect the
    /// drone's decision
    /// </summary>
    public static class DroneVision
    {
        /// <summary>
        /// Used to specify which device to select for capturing images.
        /// </summary>
        public enum VideoSource
        {
            /// <summary>
            /// Video source is the live stream from the drone
            /// </summary>
            Drone,
            /// <summary>
            /// Video source is a webcam connected to the computer
            /// </summary>
            Webcam,
        }

        private static Capture capture;
        private static DispatcherTimer timer_job;
        private static Mat captured_frame = new Mat();
        private static Mat composed_image = new Mat();

        /// <summary>
        /// Initializes the class with the given video source.
        /// 
        /// This needs to be called at least once.
        /// </summary>
        /// <param name="source">Source of video stream to use</param>
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
            Mat temp = new Mat();

            capture.Retrieve(captured_frame);
            composed_image = captured_frame;

            temp = ImageProcessor.PreprocessImage(captured_frame);

            List<Rectangle> detectedFaces = new List<Rectangle>();
            ImageProcessor.DetectFace(temp, detectedFaces);
            // Draw rectangles around each face
            foreach (var face in detectedFaces)
            {
                CvInvoke.Rectangle(composed_image, face, new MCvScalar(255, 0, 0));
                CvInvoke.Rectangle(temp, face, new MCvScalar(255, 0, 0));
            }

           CvInvoke.Imshow("Faces", temp);
        }
    }
}

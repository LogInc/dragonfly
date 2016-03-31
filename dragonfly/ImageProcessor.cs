using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Emgu.CV;
using Emgu.CV.Structure;

namespace Dragonfly
{
    /// <summary>
    /// ImageProcessor contains functionality to perform computer vision operations
    /// on the images including
    /// 
    /// Pre-Processing
    ///
    /// Feature Extraction
    /// 
    /// Object Detection and Recognition
    /// </summary>
    class ImageProcessor
    {
        private static CascadeClassifier face = new CascadeClassifier("data\\haarcascade_frontalface_default.xml");

        /// <summary>
        /// Applies preprocessing stage to the given colored image after converting
        /// it into gray-scale.
        /// </summary>
        /// <param name="image">The image to preprocess</param>
        /// <returns>The preprocessed gray image</returns>
        public static Mat PreprocessImage(Mat image)
        {
            Mat gray = new Mat();
            Mat temp = new Mat();
            // Color to gray conversion
            CvInvoke.CvtColor(image, gray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

            // Histogram equalization
            CvInvoke.EqualizeHist(gray, gray);

            // Noise removal
            CvInvoke.GaussianBlur(gray, temp, new Size(3, 3), 1);

            return temp;
        }

        /// <summary>
        /// Detects faces in the given image.
        /// </summary>
        /// <param name="image">Gray-scale image</param>
        /// <param name="faces">List of Rectangle objects bounding the detected faces</param>
        /// <remarks>
        /// This method currently has hard-coded values which may need to be provided
        /// as arguments
        /// </remarks>
        public static void DetectFace(Mat image, List<Rectangle> faces)
        {
            Rectangle[] detectedFaces = face.DetectMultiScale(image, 1.1, 10, new Size(20, 20), new Size(200, 200));
            faces.AddRange(detectedFaces);
        }
    }
}

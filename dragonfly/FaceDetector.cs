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
    public static class FaceDetector
    {
        private static CascadeClassifier face;
        //private static CascadeClassifier eye = new CascadeClassifier("data\\haarcascade_eye.xml");

        public static void Initialize()
        {
            face = new CascadeClassifier("data\\haarcascade_frontalface_default.xml");
        }

        public static void DetectFace(Mat image, List<Rectangle> faces)
        {
            UMat gray = new UMat();
            CvInvoke.CvtColor(image, gray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
            CvInvoke.EqualizeHist(gray, gray);
            Rectangle[] detectedFaces = face.DetectMultiScale(gray, 1.1, 10, new Size(20, 20));
            faces.AddRange(detectedFaces);
        }
    }
}

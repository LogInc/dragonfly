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
        private static CascadeClassifier face = new CascadeClassifier("data\\haarcascade_frontalface_default.xml");

        public static void DetectFace(Mat image, List<Rectangle> faces)
        {
            Rectangle[] detectedFaces = face.DetectMultiScale(image, 1.1, 10, new Size(20, 20), new Size(200, 200));
            faces.AddRange(detectedFaces);
        }
    }
}

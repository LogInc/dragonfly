using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Emgu.CV;
using System.Runtime.InteropServices;

namespace Dragonfly
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        /// <summary>
        /// Converts an Emgu CV image object to a BitmapSource object that can
        /// be used as a source to a WPF control.
        /// </summary>
        /// <param name="image">The image to convert</param>
        /// <returns>Convert image</returns>
        public static BitmapSource ToBitmapSource(IImage image)
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap();

                BitmapSource bs = System.Windows.Interop
                  .Imaging.CreateBitmapSourceFromHBitmap(
                  ptr,
                  IntPtr.Zero,
                  Int32Rect.Empty,
                  System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(ptr); //release the HBitmap
                return bs;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DroneVision.Initialize();
            DroneVision.Start();

            Task imageUpdateTask = Task.Run(() => UpdateImageTask());
        }

        private void UpdateImageTask()
        {
            while (true)
            {
                IImage image = DroneVision.GetImage();
                if (image != null)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        ImageSource source = ToBitmapSource(image);
                        image_droneview.Source = source;
                    }));
                }
            }
        }
    }
}

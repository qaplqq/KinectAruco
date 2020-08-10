using Emgu.CV;
using Emgu.CV.Aruco;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using FaceDetection;
using ImageManipulationExtensionMethods;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;

namespace KinectWpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor _kinectSensor;


        private DetectorParameters _detectorParameters;

        private VectorOfInt _allIds = new VectorOfInt();
        private VectorOfVectorOfPointF _allCorners = new VectorOfVectorOfPointF();
        private VectorOfInt _markerCounterPerFrame = new VectorOfInt();

        Mat _frameCopy = new Mat();

        private Dictionary _dict;

        private Dictionary ArucoDictionary
        {
            get
            {
                if (_dict == null)
                    _dict = new Dictionary(Dictionary.PredefinedDictionaryName.Dict4X4_100);
                return _dict;
            }

        }


        void ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {


            using (var frame = e.OpenColorImageFrame())
            {
                if (frame == null)
                    return;


                using (VectorOfInt ids = new VectorOfInt())
                using (VectorOfVectorOfPointF corners = new VectorOfVectorOfPointF())
                using (VectorOfVectorOfPointF rejected = new VectorOfVectorOfPointF())
                using (Image<Bgr, byte> image = frame.ToOpenCVImage<Bgr, byte>())
                {
                    Image<Bgr, byte> img2 = image.Clone();

                    ArucoInvoke.DetectMarkers(img2, ArucoDictionary, corners, ids, _detectorParameters, rejected);

                    if (ids.Size > 0)
                    {
                        String textVar1 = "";
                        for (int i = 0; i < ids.Size; i++)
                            textVar1 = textVar1 + ids[i] + " ";


                        ArucoInvoke.DrawDetectedMarkers(img2, corners, ids, new MCvScalar(0, 255, 0));


                    }



                    rgbImage.Source = img2.ToBitmapSource();
                }
            }
        }

                void Init1()
        {
            _detectorParameters = DetectorParameters.GetDefault();


            this.Loaded += delegate
            {
                _kinectSensor = KinectSensor.KinectSensors[0];
                _kinectSensor.ColorFrameReady += ColorFrameReady;
                _kinectSensor.ColorStream.Enable();

                _kinectSensor.Start();

            };

        }


        public MainWindow()
        {
            InitializeComponent();
            Init1();

        }
    }
}

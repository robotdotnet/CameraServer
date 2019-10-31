using FRC.CameraServer.OpenCvSharp;
using OpenCvSharp;
using System;
using System.Threading;

namespace FRC.CameraServer.Dev
{
    class Program
    {
        static void Main(string[] args)
        {
            UsbCamera camera = new UsbCamera("Cam", 0);
            MjpegServer server = new MjpegServer("server", 1181);
            server.Source = camera;

            MjpegServer server2 = new MjpegServer("server2", 1182);
            CvSink sink = new CvSink("Sink");
            CvSource source = new CvSource("Source", PixelFormat.GRAY, 320, 240, 30);

            Cv2.NamedWindow("Window");

            sink.Source = camera;
            server2.Source = source;

            Mat image = new Mat();
            Mat gray = new Mat();

            while (true)
            {
                if (sink.GrabFrame(image) == 0)
                {
                    continue;
                }

                Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
                Cv2.ImShow("Window", image);
                Cv2.WaitKey(1);
                source.PutFrame(gray);
            }

            Console.ReadLine();
        }
    }
}

using FRC.CameraServer.Interop;
using OpenCvSharp;
using System;

namespace FRC.CameraServer.OpenCvSharp
{
    public class CvSource : ImageSource
    {
        public CvSource(string name, VideoMode mode)
            : base(CsCore.CreateRawSource(name.AsSpan(), mode))
        {

        }

        public CvSource(string name, PixelFormat pixelFormat, int width, int height, int fps)
    : base(CsCore.CreateRawSource(name.AsSpan(), new VideoMode(pixelFormat, width, height, fps)))
        {

        }

        public unsafe void PutFrame(Mat image)
        {
            int channels = image.Channels();
            if (channels != 1 && channels != 3)
            {
                throw new VideoException("Unsupported Image Type");
            }
            var imageType = channels == 1 ? PixelFormat.GRAY : PixelFormat.BGR;
            CS_RawFrame rawFrame = new CS_RawFrame
            {
                data = image.DataPointer,
                width = image.Width,
                height = image.Height,
                pixelFormat = (int)imageType,
                totalData = (int)image.Total() * channels
            };
            rawFrame.dataLength = rawFrame.totalData;
            CsCore.PutRawSourceFrame(Handle, rawFrame);
        }
    }
}

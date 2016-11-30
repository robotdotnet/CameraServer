using OpenCvSharp;
using static CameraServer.Native.NativeMethods;

namespace CameraServer
{
    public class CvSink : VideoSink
    {
        public CvSink(string name) : base(CreateCvSink(name))
        {

        }

        public override string Description
        {
            set
            {
                SetSinkDescription(m_handle, value);
            }
        }

        public long GrabFrame(Mat image)
        {
            return (long)GrabSinkFrame(m_handle, image.CvPtr);
        }

        public string GetError()
        {
            return GetSinkError(m_handle);
        }

        public bool Enabled
        {
            set
            {
                SetSinkEnabled(m_handle, value);
            }
        }
    }
}

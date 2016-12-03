using OpenCvSharp;
using NativeMethods = CSCore.Native.NativeMethods;

namespace CSCore
{
    public class CvSink : VideoSink
    {
        public CvSink(string name) : base(NativeMethods.CreateCvSink(name))
        {

        }

        public override string Description
        {
            set
            {
                NativeMethods.SetSinkDescription(m_handle, value);
            }
        }

        public long GrabFrame(Mat image)
        {
            return (long)NativeMethods.GrabSinkFrame(m_handle, image.CvPtr);
        }

        public string GetError()
        {
            return NativeMethods.GetSinkError(m_handle);
        }

        public bool Enabled
        {
            set
            {
                NativeMethods.SetSinkEnabled(m_handle, value);
            }
        }
    }
}

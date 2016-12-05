using System.Collections.Generic;
using OpenCvSharp;
using NativeMethods = CSCore.Native.NativeMethods;

namespace CSCore
{
    public class CvSource : VideoSource
    {
        public CvSource(string name, VideoMode mode) 
            : base (NativeMethods.CreateCvSource(name, mode.PixelFormat, mode.Width, mode.Height, mode.FPS))
        {

        }

        public CvSource(string name, PixelFormat pixelFormat, int width, int height, int fps)
            : base(NativeMethods.CreateCvSource(name, pixelFormat, width, height, fps))
        {

        }

        public void PutFrame(Mat image)
        {
            NativeMethods.PutSourceFrame(m_handle, image.CvPtr);
        }

        public void NotifyError(string msg)
        {
            NativeMethods.NotifySourceError(m_handle, msg);
        }

        public override bool Connected
        {
            set
            {
                NativeMethods.SetSourceConnected(m_handle, value);
            }
        }

        public override string Description
        {
            set
            {
                NativeMethods.SetSourceDescription(m_handle, value);
            }
        }

        public VideoProperty CreateProperty(string name, PropertyKind kind, int minimum, int maximum, int step, int defaultValue, int value)
        {
            return new VideoProperty(
                NativeMethods.CreateSourceProperty(m_handle, name, kind, minimum, maximum, step, defaultValue, value));
        }

        public void SetEnumPropertyChoices(VideoProperty property, IList<string> choices)
        {
            NativeMethods.SetSourceEnumPropertyChoices(m_handle, property.m_handle, choices);
        }
    }
}

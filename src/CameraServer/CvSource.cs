using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emgu.CV;
using static CameraServer.Native.Interop;

namespace CameraServer
{
    public class CvSource : VideoSource
    {
        public CvSource(string name)
        {
            m_status = 0;
            UIntPtr size;
            byte[] nameArr = CreateUTF8String(name, out size);
            m_handle = CS_CreateCvSource(nameArr, ref m_status);
        }

        public void PutFrame(Mat image)
        {
            m_status = 0;
            CvInvoke.Flip();
            image.
            CS_PutSourceFrame
        }

        public void NotifyError(string msg)
        {
            
        }

        public bool Connected
        {
            set
            {
                
            }
        }

        public VideoProperty CreateProperty(string name, PropertyType type)
        {
            
        }

        public VideoProperty CreateProperty(string name, PropertyType type, Action<VideoProperty> onChange)
        {
            
        }

        public void RemoveProperty(VideoProperty property)
        {
            
        }

        public void RemoveProperty(string name)
        {
            
        }
    }
}

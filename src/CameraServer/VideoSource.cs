using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CameraServer.Native.Interop;

namespace CameraServer
{
    public class VideoSource : IDisposable
    {
        protected int m_status = 0;
        private int m_handle = 0;

        public string Name
        {
            get
            {
                m_status = 0;
                IntPtr strPtr = CS_GetSourceName(m_handle, ref m_status);
                if (m_status == 0) return string.Empty;
                string retVal = ReadUTF8String(strPtr);
                CS_FreeString(strPtr);
                return retVal;
            }
        }

        public string Description
        {
            get
            {
                m_status = 0;
                IntPtr strPtr = CS_GetSourceDescription(m_handle, ref m_status);
                if (m_status == 0) return string.Empty;
                string retVal = ReadUTF8String(strPtr);
                CS_FreeString(strPtr);
                return retVal;
            }
        }

        protected internal VideoSource(int handle)
        {
            m_handle = handle;
        }

        public VideoSource()
        {
            m_handle = 0;
        }

        public VideoSource(VideoSource copyFrom)
        {
            // TODO, Fix this
        }

        public void Dispose()
        {
            m_status = 0;
            if (m_handle != 0)
            {
                CS_ReleaseSource(m_handle, ref m_status);
            }
        }

        public long GetLastFrameTime()
        {
            m_status = 0;
            return (long)CS_GetSourceLastFrameTime(m_handle, ref m_status);
        }

        public bool IsConnected()
        {
            m_status = 0;
            return CS_IsSourceConnected(m_handle, ref m_status);
        }

        public VideoProperty GetProperty(string name)
        {
            
        }

        public IReadOnlyList<VideoProperty> EnumerateProperties()
        {
            
        }

        public int GetLastStatus() => m_status;

        public static IReadOnlyList<VideoSource> EnumerateSources()
        {
            
        }

        public void Swap(VideoSource first, VideoSource second)
        {
            int firstStatus = first.m_status;
            first.m_status = second.m_status;
            second.m_status = firstStatus;

            int firstHandle = first.m_handle;
            first.m_handle = second.m_handle;
            second.m_handle = firstHandle;
        }

    }
}

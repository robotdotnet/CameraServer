using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static CameraServer.Native.Interop;

namespace CameraServer
{
    public class VideoSource : IDisposable
    {
        protected int m_status = 0;
        protected int m_handle = 0;

        internal int GetHandle => m_handle;

        public bool IsValid => m_handle != 0;

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
            m_status = 0;
            UIntPtr size;
            byte[] nameArr = CreateUTF8String(name, out size);
            return new VideoProperty(CS_GetSourceProperty(m_handle, nameArr, ref m_status));
        }

        public IReadOnlyList<VideoProperty> EnumerateProperties()
        {
            int status = 0;
            int count = 0;
            IntPtr propArr = CS_EnumerateSourceProperties(m_handle, ref count, ref status);
            List<VideoProperty> properties = new List<VideoProperty>(count);

            for (int i = 0; i < count; i++)
            {
                int handle = Marshal.ReadInt32(propArr, i);
                properties.Add(new VideoProperty(handle));
            }
            // TODO: Free Array
            return properties;
        }

        public int GetLastStatus() => m_status;

        public static IReadOnlyList<VideoSource> EnumerateSources()
        {
            int status = 0;
            int count = 0;
            IntPtr sourceArray = CS_EnumerateSources(ref count, ref status);
            List<VideoSource> sources = new List<VideoSource>(count);

            for (int i = 0; i < count; i++)
            {
                int handle = Marshal.ReadInt32(sourceArray, i);
                sources.Add(new VideoSource(handle));
            }
            CS_ReleaseEnumeratedSources(sourceArray, count);
            return sources;
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

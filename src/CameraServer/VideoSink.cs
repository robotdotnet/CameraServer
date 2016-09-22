using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static CameraServer.Native.Interop;

namespace CameraServer
{
    public class VideoSink : IDisposable
    {
        protected int m_handle;
        protected int m_status;

        public bool IsValid => m_handle != 0;

        public VideoSink()
        {
            m_handle = 0;
        }

        protected internal VideoSink(int handle)
        {
            m_handle = handle;
        }

        public void Dispose()
        {
            m_status = 0;
            if (m_handle != 0)
            {
                CS_ReleaseSink(m_handle, ref m_status);
            }
        }

        public string Name
        {
            get
            {
                m_status = 0;
                IntPtr strPtr = CS_GetSinkName(m_handle, ref m_status);
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
                IntPtr strPtr = CS_GetSinkDescription(m_handle, ref m_status);
                if (m_status == 0) return string.Empty;
                string retVal = ReadUTF8String(strPtr);
                CS_FreeString(strPtr);
                return retVal;
            }
        }

        public VideoSource Source
        {
            get
            {
                m_status = 0;
                return new VideoSource(CS_GetSinkSource(m_handle, ref m_status));
            }
            set
            {
                m_status = 0;
                if (!value.IsValid)
                {
                    CS_SetSinkSource(m_handle, 0, ref m_status);
                }
                else
                {
                    CS_SetSinkSource(m_handle, value.GetHandle, ref m_status);
                }
            }
        }

        public VideoProperty GetSourceProperty(string name)
        {
            m_status = 0;
            UIntPtr size;
            return new VideoProperty(CS_GetSinkSourceProperty(m_handle, CreateUTF8String(name, out size), ref m_status));
        }

        public int GetLastStatus()
        {
            return m_status;
        }

        public static IReadOnlyList<VideoSink> EnumerateSinks()
        {
            int status = 0;
            int count = 0;
            IntPtr sinkArray = CS_EnumerateSinks(ref count, ref status);
            List<VideoSink> sinks = new List<VideoSink>(count);

            for (int i = 0; i < count; i++)
            {
                int handle = Marshal.ReadInt32(sinkArray, i);
                sinks.Add(new VideoSink(handle));
            }
            CS_ReleaseEnumeratedSinks(sinkArray, count);
            return sinks;
        }


    }
}

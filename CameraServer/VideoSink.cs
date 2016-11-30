using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static CameraServer.Native.NativeMethods;

namespace CameraServer
{
    public class VideoSink : IDisposable
    {
        protected int m_handle;

        public bool IsValid => m_handle != 0;

        public int Handle => m_handle;

        protected internal VideoSink(int handle)
        {
            m_handle = handle;
        }

        public void Dispose()
        {
            if (m_handle != 0)
            {
                ReleaseSink(m_handle);
            }
            m_handle = 0;
        }

        public override bool Equals(object other)
        {
            if (this == other) return true;
            if (other == null) return false;
            VideoSink otherSink = other as VideoSink;
            if (otherSink == null) return false;
            return m_handle == otherSink.m_handle;
        }

        public override int GetHashCode()
        {
            return m_handle;
        }

        public SinkKind Kind => GetSinkKind(m_handle);

        public virtual string Name
        {
            get
            {
                return GetSinkName(m_handle);
            }
            set
            {

            }
        }

        public virtual string Description
        {
            get
            {
                return GetSinkDescription(m_handle);
            }
            set
            {

            }
        }

        public VideoSource Source
        {
            get
            {
                return new VideoSource(GetSinkSource(m_handle));
            }
            set
            {
                SetSinkSource(m_handle, value.Handle);
            }
        }

        public VideoProperty GetSourceProperty(string name)
        {
            return new VideoProperty(GetSinkSourceProperty(m_handle, name));
        }

        public static List<VideoSink> EnumerateSinks()
        {
            List<int> sinkHandles = Native.NativeMethods.EnumerateSinks();
            List<VideoSink> sinks = new List<VideoSink>(sinkHandles.Count);
            foreach(var h in sinkHandles)
            {
                sinks.Add(new VideoSink(h));
            }
            return sinks;
        }


    }
}

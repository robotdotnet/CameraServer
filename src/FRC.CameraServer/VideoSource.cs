using System;
using System.Collections.Generic;

namespace CSCore
{
    public class VideoSource : IDisposable
    {
        protected int m_handle;

        protected internal VideoSource(int handle)
        {
            m_handle = handle;
        }

        public void Dispose()
        {
            if (m_handle != 0)
            {
                NativeMethods.ReleaseSource(m_handle);
            }
            m_handle = 0;
        }

        public bool IsValid => m_handle != 0;

        public int Handle => m_handle;

        public override bool Equals(object other)
        {
            if (this == other) return true;
            if (other == null) return false;
            VideoSource otherSource = other as VideoSource;
            if (otherSource == null) return false;
            return m_handle == otherSource.m_handle;
        }

        public override int GetHashCode() => m_handle;

        public SourceKind Kind => NativeMethods.GetSourceKind(m_handle);

        public virtual string Name
        {
            get
            {
                return NativeMethods.GetSourceName(m_handle);
            }
            set
            {

            }
        }

        public virtual string Description
        {
            get
            {
                return NativeMethods.GetSourceDescription(m_handle);
            }
            set
            {

            }
        }

        public long GetLastFrameTime() => (long)NativeMethods.GetSourceLastFrameTime(m_handle);

        public virtual bool Connected
        {
            get
            {
                return NativeMethods.IsSourceConnected(m_handle);
            }
            set
            {

            }
        }

        public VideoProperty GetProperty(string name)
        {
            return new VideoProperty(NativeMethods.GetSourceProperty(m_handle, name));
        }

        public List<VideoProperty> EnumerateProperties()
        {
            var handles = NativeMethods.EnumerateSourceProperties(m_handle);
            List<VideoProperty> properties = new List<VideoProperty>(handles.Count);
            foreach(var h in handles)
            {
                properties.Add(new VideoProperty(h));
            }
            return properties;
        }

        public VideoMode GetVideoMode() => NativeMethods.GetSourceVideoMode(m_handle);
        public bool SetVideoMode(VideoMode mode)
        {
            return NativeMethods.SetSourceVideoMode(m_handle, mode);
        }

        public bool SetVideoMode(PixelFormat pixelFormat, int width, int height, int fps)
        {
            return NativeMethods.SetSourceVideoMode(m_handle, pixelFormat, width, height, fps);
        }

        public bool SetPixelFormat(PixelFormat format)
        {
            return NativeMethods.SetSourcePixelFormat(m_handle, format);
        }

        public bool SetResolution(int width, int height)
        {
            return NativeMethods.SetSourceResolution(m_handle, width, height);
        }

        public bool SetFPS(int fps)
        {
            return NativeMethods.SetSourceFPS(m_handle, fps);
        }

        public List<VideoMode> EnumerateVideoModes()
        {
            return NativeMethods.EnumerateSourceVideoModes(m_handle);
        }

        public List<VideoSink> EnumerateSinks()
        {
            var handles = NativeMethods.EnumerateSourceSinks(m_handle);
            List<VideoSink> sinks = new List<VideoSink>(handles.Count);
            foreach (var h in handles)
            {
                sinks.Add(new VideoSink(h));
            }
            return sinks;
        }

        public static List<VideoSource> EnumerateSources()
        {
            var handles = NativeMethods.EnumerateSources();
            List<VideoSource> sources = new List<VideoSource>(handles.Count);
            foreach (var h in handles)
            {
                sources.Add(new VideoSource(h));
            }
            return sources;
        }
    }


}

using System;
using System.Collections.Generic;

namespace CSCore
{
    /// <summary>
    /// A sink for video that provides a sequence of frames
    /// </summary>
    /// <remarks>
    /// Each frame may consist of multiple images (e.g. from a stereo or depth camera); 
    /// these are called channels
    /// </remarks>
    public class VideoSource : IDisposable
    {
        /// <summary>
        /// Creates a new VideoSource from a handle
        /// </summary>
        /// <param name="handle">The handle to create from</param>
        protected internal VideoSource(int handle)
        {
            Handle = handle;
        }

        /// <summary>
        /// Disposes of the source
        /// </summary>
        public void Dispose()
        {
            if (Handle != 0)
            {
                NativeMethods.ReleaseSource(Handle);
            }
            Handle = 0;
        }

        /// <summary>
        /// Gets if the sink is valid
        /// </summary>
        public bool IsValid => Handle != 0;

        /// <summary>
        /// Gets the handle associated with this sink
        /// </summary>
        public int Handle { get; protected set; }

        /// <summary>
        /// Checks if the 2 objects are equal
        /// </summary>
        /// <param name="other">The other object to check</param>
        /// <returns>True if the objects are equal</returns>
        public override bool Equals(object other)
        {
            if (this == other) return true;
            VideoSource otherSource = other as VideoSource;
            return Handle == otherSource?.Handle;
        }

        /// <summary>
        /// Gets the HashCode for this object
        /// </summary>
        /// <returns>The hashcode (the handle is the hash code)</returns>
        public override int GetHashCode() => Handle;

        /// <summary>
        /// Gets the kind of this source
        /// </summary>
        public SourceKind Kind => NativeMethods.GetSourceKind(Handle);

        /// <summary>
        /// Gets or sets the name of this source
        /// </summary>
        public virtual string Name
        {
            get
            {
                return NativeMethods.GetSourceName(Handle);
            }
            set
            {

            }
        }

        /// <summary>
        /// Gets or sets the decription of this source
        /// </summary>
        public virtual string Description
        {
            get
            {
                return NativeMethods.GetSourceDescription(Handle);
            }
            set
            {

            }
        }

        /// <summary>
        /// Get the last time a frame was captured
        /// </summary>
        /// <returns>Relative time of the last frame captured</returns>
        public long GetLastFrameTime() => (long)NativeMethods.GetSourceLastFrameTime(Handle);

        /// <summary>
        /// Gets or sets if the source is currently connected and providing images
        /// </summary>
        public virtual bool Connected
        {
            get
            {
                return NativeMethods.IsSourceConnected(Handle);
            }
            set
            {

            }
        }

        /// <summary>
        /// Gets a VideoProperty from this sink
        /// </summary>
        /// <param name="name">The property to get</param>
        /// <returns>The property</returns>
        public VideoProperty GetProperty(string name)
        {
            return new VideoProperty(NativeMethods.GetSourceProperty(Handle, name));
        }

        /// <summary>
        /// Enumerates all properties for this sink
        /// </summary>
        /// <returns>List of all properties</returns>
        public List<VideoProperty> EnumerateProperties()
        {
            var handles = NativeMethods.EnumerateSourceProperties(Handle);
            List<VideoProperty> properties = new List<VideoProperty>(handles.Count);
            foreach(var h in handles)
            {
                properties.Add(new VideoProperty(h));
            }
            return properties;
        }
        
        /// <summary>
        /// Gets the current video mode
        /// </summary>
        /// <returns>The current video mode</returns>
        public VideoMode GetVideoMode() => NativeMethods.GetSourceVideoMode(Handle);

        /// <summary>
        /// Sets the video mode
        /// </summary>
        /// <param name="mode">The video mode to set</param>
        /// <returns>True on success</returns>
        public bool SetVideoMode(VideoMode mode)
        {
            return NativeMethods.SetSourceVideoMode(Handle, mode);
        }

        /// <summary>
        /// Sets the video mode
        /// </summary>
        /// <param name="pixelFormat">The PixelFormat</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <param name="fps">The fps</param>
        /// <returns>True on success</returns>
        public bool SetVideoMode(PixelFormat pixelFormat, int width, int height, int fps)
        {
            return NativeMethods.SetSourceVideoMode(Handle, pixelFormat, width, height, fps);
        }

        /// <summary>
        /// Sets the pixel format
        /// </summary>
        /// <param name="format">The PixelFormat</param>
        /// <returns>True on success</returns>
        public bool SetPixelFormat(PixelFormat format)
        {
            return NativeMethods.SetSourcePixelFormat(Handle, format);
        }

        /// <summary>
        /// Sets the resolution
        /// </summary>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <returns>True on success</returns>
        public bool SetResolution(int width, int height)
        {
            return NativeMethods.SetSourceResolution(Handle, width, height);
        }

        /// <summary>
        /// Sets the FPS
        /// </summary>
        /// <param name="fps">The fps</param>
        /// <returns>True on success</returns>
        public bool SetFPS(int fps)
        {
            return NativeMethods.SetSourceFPS(Handle, fps);
        }

        /// <summary>
        /// Enumerate all known video modes for this source.
        /// </summary>
        /// <returns>A list of all video modes for this source</returns>
        public List<VideoMode> EnumerateVideoModes()
        {
            return NativeMethods.EnumerateSourceVideoModes(Handle);
        }

        /// <summary>
        /// Enumerate all sinks connected to this source.
        /// </summary>
        /// <returns>A list of sinks</returns>
        public List<VideoSink> EnumerateSinks()
        {
            var handles = NativeMethods.EnumerateSourceSinks(Handle);
            List<VideoSink> sinks = new List<VideoSink>(handles.Count);
            foreach (var h in handles)
            {
                sinks.Add(new VideoSink(h));
            }
            return sinks;
        }

        /// <summary>
        /// Enumerate all existing sources
        /// </summary>
        /// <returns>A list of sources</returns>
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

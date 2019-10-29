using FRC.CameraServer.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRC.CameraServer
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
        protected internal VideoSource(CS_Source handle)
        {
            Handle = handle;
        }

        /// <summary>
        /// Disposes of the source
        /// </summary>
        public void Dispose()
        {
            if (Handle.IsValid())
            {
                CsCore.ReleaseSource(Handle);
            }
            Handle = new CS_Source(0);
        }

        /// <summary>
        /// Gets if the sink is valid
        /// </summary>
        public bool IsValid => Handle.IsValid();

        /// <summary>
        /// Gets the handle associated with this sink
        /// </summary>
        public CS_Source Handle { get; protected set; }

        /// <summary>
        /// Checks if the 2 objects are equal
        /// </summary>
        /// <param name="other">The other object to check</param>
        /// <returns>True if the objects are equal</returns>
        public override bool Equals(object other)
        {
            if (this is VideoSource vs)
            {
                return Handle == vs.Handle;
            }
            return false;
        }

        /// <summary>
        /// Gets the HashCode for this object
        /// </summary>
        /// <returns>The hashcode (the handle is the hash code)</returns>
        public override int GetHashCode() => Handle.Get();

        /// <summary>
        /// Gets the kind of this source
        /// </summary>
        public SourceKind Kind => CsCore.GetSourceKind(Handle);

        /// <summary>
        /// Gets or sets the name of this source
        /// </summary>
        public virtual string Name
        {
            get
            {
                return CsCore.GetSourceName(Handle);
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
                return CsCore.GetSourceDescription(Handle);
            }
            set
            {

            }
        }

        /// <summary>
        /// Get the last time a frame was captured
        /// </summary>
        /// <returns>Relative time of the last frame captured</returns>
        public long GetLastFrameTime() => (long)CsCore.GetSourceLastFrameTime(Handle);

        /// <summary>
        /// Gets or sets if the source is currently connected and providing images
        /// </summary>
        public virtual bool Connected
        {
            get
            {
                return CsCore.IsSourceConnected(Handle);
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
            return new VideoProperty(CsCore.GetSourceProperty(Handle, name.AsSpan()));
        }

        /// <summary>
        /// Enumerates all properties for this sink
        /// </summary>
        /// <returns>List of all properties</returns>
        public List<VideoProperty> EnumerateProperties()
        {
            var handles = CsCore.EnumerateSourceProperties(Handle);
            List<VideoProperty> properties = new List<VideoProperty>(handles.Length);
            foreach (var h in handles)
            {
                properties.Add(new VideoProperty(h));
            }
            return properties;
        }

        /// <summary>
        /// Gets the current video mode
        /// </summary>
        /// <returns>The current video mode</returns>
        public VideoMode GetVideoMode() => CsCore.GetSourceVideoMode(Handle);

        /// <summary>
        /// Sets the video mode
        /// </summary>
        /// <param name="mode">The video mode to set</param>
        /// <returns>True on success</returns>
        public bool SetVideoMode(VideoMode mode)
        {
            return CsCore.SetSourceVideoMode(Handle, mode);
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
            return CsCore.SetSourceVideoModeDiscrete(Handle, pixelFormat, width, height, fps);
        }

        /// <summary>
        /// Sets the pixel format
        /// </summary>
        /// <param name="format">The PixelFormat</param>
        /// <returns>True on success</returns>
        public bool SetPixelFormat(PixelFormat format)
        {
            return CsCore.SetSourcePixelFormat(Handle, format);
        }

        /// <summary>
        /// Sets the resolution
        /// </summary>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <returns>True on success</returns>
        public bool SetResolution(int width, int height)
        {
            return CsCore.SetSourceResolution(Handle, width, height);
        }

        /// <summary>
        /// Sets the FPS
        /// </summary>
        /// <param name="fps">The fps</param>
        /// <returns>True on success</returns>
        public bool SetFPS(int fps)
        {
            return CsCore.SetSourceFPS(Handle, fps);
        }

        /// <summary>
        /// Enumerate all known video modes for this source.
        /// </summary>
        /// <returns>A list of all video modes for this source</returns>
        public List<VideoMode> EnumerateVideoModes()
        {
            return new List<VideoMode>(CsCore.EnumerateSourceVideoModes(Handle));
        }

        /// <summary>
        /// Enumerate all sinks connected to this source.
        /// </summary>
        /// <returns>A list of sinks</returns>
        public List<VideoSink> EnumerateSinks()
        {
            var handles = CsCore.EnumerateSourceSinks(Handle);
            List<VideoSink> sinks = new List<VideoSink>(handles.Length);
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
            var handles = CsCore.EnumerateSources();
            List<VideoSource> sources = new List<VideoSource>(handles.Length);
            foreach (var h in handles)
            {
                sources.Add(new VideoSource(h));
            }
            return sources;
        }
    }
}

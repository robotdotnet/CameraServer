using FRC.CameraServer.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    public class VideoSink : IDisposable
    {
        /// <summary>
        /// Gets if the sink is valid
        /// </summary>
        public bool IsValid => Handle.IsValid();

        /// <summary>
        /// Gets the handle associated with this sink
        /// </summary>
        public CS_Sink Handle { get; protected set; }

        /// <summary>
        /// Creates a new VideoSink
        /// </summary>
        /// <param name="handle">The handle to create from</param>
        protected internal VideoSink(CS_Sink handle)
        {
            Handle = handle;
        }

        /// <summary>
        /// Disposes of the sink
        /// </summary>
        public void Dispose()
        {
            if (Handle.IsValid())
            {
                CsCore.ReleaseSink(Handle);
            }
            Handle = new CS_Sink(0);
        }

        /// <summary>
        /// Checks if the 2 objects are equal
        /// </summary>
        /// <param name="other">The other object to check</param>
        /// <returns>True if the objects are equal</returns>
        public override bool Equals(object other)
        {
            if (this is VideoSink vs)
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
        /// Gets the kind of this sink
        /// </summary>
        public SinkKind Kind => CsCore.GetSinkKind(Handle);

        /// <summary>
        /// Gets or sets the name of this sink
        /// </summary>
        public virtual string Name
        {
            get
            {
                return CsCore.GetSinkName(Handle);
            }
            set
            {

            }
        }

        /// <summary>
        /// Gets or sets the description of this sink
        /// </summary>
        public virtual string Description
        {
            get
            {
                return CsCore.GetSinkDescription(Handle);
            }
            set
            {

            }
        }

        /// <summary>
        /// Gets or sets the <see cref="VideoSource"/> attached to this sink
        /// </summary>
        [AllowNull]
        public VideoSource Source
        {
            get
            {
                return new VideoSource(CsCore.GetSinkSource(Handle));
            }
            set
            {
                if (value == null)
                {
                    CsCore.SetSinkSource(Handle, new CS_Source(0));
                }
                else
                {
                    CsCore.SetSinkSource(Handle, value.Handle);
                }
            }
        }

        public bool SetConfigJson(string config)
        {
            return CsCore.SetSinkConfigJson(Handle, config.AsSpan());
        }

        public string GetConfigJson()
        {
            return CsCore.GetSinkConfigJson(Handle);
        }

        /// <summary>
        /// Gets a VideoProperty from this sink
        /// </summary>
        /// <param name="name">The property to get</param>
        /// <returns>The property</returns>
        public VideoProperty GetSourceProperty(string name)
        {
            return new VideoProperty(CsCore.GetSinkSourceProperty(Handle, name.AsSpan()));
        }

        /// <summary>
        /// Enumerates all existing sinks
        /// </summary>
        /// <returns>A list of all existing sinks</returns>
        public static List<VideoSink> EnumerateSinks()
        {
            var sinkHandles = CsCore.EnumerateSinks();
            List<VideoSink> sinks = new List<VideoSink>(sinkHandles.Length);
            foreach (var h in sinkHandles)
            {
                sinks.Add(new VideoSink(h));
            }
            return sinks;
        }


    }
}

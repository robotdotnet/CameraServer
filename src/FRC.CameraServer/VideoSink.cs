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
    public class VideoSink : IDisposable
    {
        /// <summary>
        /// Gets if the sink is valid
        /// </summary>
        public bool IsValid => Handle != 0;

        /// <summary>
        /// Gets the handle associated with this sink
        /// </summary>
        public int Handle { get; protected set; }

        /// <summary>
        /// Creates a new VideoSink
        /// </summary>
        /// <param name="handle">The handle to create from</param>
        protected internal VideoSink(int handle)
        {
            Handle = handle;
        }

        /// <summary>
        /// Disposes of the sink
        /// </summary>
        public void Dispose()
        {
            if (Handle != 0)
            {
                NativeMethods.ReleaseSink(Handle);
            }
            Handle = 0;
        }

        /// <summary>
        /// Checks if the 2 objects are equal
        /// </summary>
        /// <param name="other">The other object to check</param>
        /// <returns>True if the objects are equal</returns>
        public override bool Equals(object other)
        {
            if (this == other) return true;
            VideoSink otherSink = other as VideoSink;
            return Handle == otherSink?.Handle;
        }

        /// <summary>
        /// Gets the HashCode for this object
        /// </summary>
        /// <returns>The hashcode (the handle is the hash code)</returns>
        public override int GetHashCode() => Handle;

        /// <summary>
        /// Gets the kind of this sink
        /// </summary>
        public SinkKind Kind => NativeMethods.GetSinkKind(Handle);

        /// <summary>
        /// Gets or sets the name of this sink
        /// </summary>
        public virtual string Name
        {
            get
            {
                return NativeMethods.GetSinkName(Handle);
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
                return NativeMethods.GetSinkDescription(Handle);
            }
            set
            {

            }
        }

        /// <summary>
        /// Gets or sets the <see cref="VideoSource"/> attached to this sink
        /// </summary>
        public VideoSource Source
        {
            get
            {
                return new VideoSource(NativeMethods.GetSinkSource(Handle));
            }
            set
            {
                NativeMethods.SetSinkSource(Handle, value.Handle);
            }
        }

        /// <summary>
        /// Gets a VideoProperty from this sink
        /// </summary>
        /// <param name="name">The property to get</param>
        /// <returns>The property</returns>
        public VideoProperty GetSourceProperty(string name)
        {
            return new VideoProperty(NativeMethods.GetSinkSourceProperty(Handle, name));
        }

        /// <summary>
        /// Enumerates all existing sinks
        /// </summary>
        /// <returns>A list of all existing sinks</returns>
        public static List<VideoSink> EnumerateSinks()
        {
            List<int> sinkHandles = NativeMethods.EnumerateSinks();
            List<VideoSink> sinks = new List<VideoSink>(sinkHandles.Count);
            foreach(var h in sinkHandles)
            {
                sinks.Add(new VideoSink(h));
            }
            return sinks;
        }


    }
}

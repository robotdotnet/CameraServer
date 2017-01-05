namespace CSCore
{
    /// <summary>
    /// A video event
    /// </summary>
    public class VideoEvent
    {
        internal VideoEvent(EventKind kind, int source, int sink, string name, PixelFormat pixelFormat,
             int width, int height, int fps, int property, PropertyKind propertyKind,
             int value, string valueStr)
        {
            this.Kind = kind;
            this.SourceHandle = source;
            this.SinkHandle = sink;
            this.Name = name;
            this.Mode = new VideoMode(pixelFormat, width, height, fps);
            this.PropertyHandle = property;
            this.PropertyKind = propertyKind;
            this.Value = value;
            this.ValueStr = valueStr;
        }

        /// <summary>
        /// The event kind
        /// </summary>
        public EventKind Kind { get; }

        // Valid for kSource* and kSink* respectively
        /// <summary>
        /// The source handle if the event happened on a source
        /// </summary>
        public int SourceHandle { get; }
        /// <summary>
        /// The sink handle if the event happened on a sink
        /// </summary>
        public int SinkHandle { get; }

        // Source/sink name
        /// <summary>
        /// The sink/source name
        /// </summary>
        public string Name { get; }

        // Fields for kSourceVideoModeChanged event
        /// <summary>
        /// The <see cref="VideoMode"/> for a <see cref="EventKind.SourceVideoModeChanged"/> event
        /// </summary>
        public VideoMode Mode { get; }

        // Fields for kSourceProperty* events
        /// <summary>
        /// The property handle for a SourceProperty event
        /// </summary>
        public int PropertyHandle { get; }
        /// <summary>
        /// The property kind for a SourceProperty event
        /// </summary>
        public PropertyKind PropertyKind { get; }
        /// <summary>
        /// The value for a SourceProperty event
        /// </summary>
        public int Value { get; }
        /// <summary>
        /// The string version of a SourceProperty event
        /// </summary>
        public string ValueStr { get; }

        /// <summary>
        /// Gets a VideoSource from an event
        /// </summary>
        /// <returns>The VideoSource of the event</returns>
        public VideoSource GetSource()
        {
            return new VideoSource(NativeMethods.CopySource(SourceHandle));
        }

        /// <summary>
        /// Gets a VideoSink from an event
        /// </summary>
        /// <returns>The VideoSink of the event</returns>
        public VideoSink GetSink()
        {
            return new VideoSink(NativeMethods.CopySink(SinkHandle));
        }

        /// <summary>
        /// Gets a <see cref="VideoProperty"/> from the event
        /// </summary>
        /// <returns>The VideoProperty of the event</returns>
        public VideoProperty GetProperty()
        {
            return new VideoProperty(PropertyHandle, PropertyKind);
        }

    }
}

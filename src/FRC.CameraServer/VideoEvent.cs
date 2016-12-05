using CSCore.Native;

namespace CSCore
{
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

        public EventKind Kind { get; }

        // Valid for kSource* and kSink* respectively
        public int SourceHandle { get; }
        public int SinkHandle { get; }

        // Source/sink name
        public string Name { get; }

        // Fields for kSourceVideoModeChanged event
        public VideoMode Mode { get; }

        // Fields for kSourceProperty* events
        private int PropertyHandle { get; }
        public PropertyKind PropertyKind { get; }
        public int Value { get; }
        public string ValueStr { get; }

        public VideoSource GetSource()
        {
            return new VideoSource(NativeMethods.CopySource(SourceHandle));
        }

        public VideoSink GetSink()
        {
            return new VideoSink(NativeMethods.CopySink(SinkHandle));
        }

        public VideoProperty GetProperty()
        {
            return new VideoProperty(PropertyHandle, PropertyKind);
        }

    }
}

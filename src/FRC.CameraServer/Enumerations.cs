using System;
// ReSharper disable InconsistentNaming

namespace CSCore
{
    /// <summary>
    /// Error status values returned from native methods
    /// </summary>
    public enum StatusValue
    {
        /// <summary>
        /// Writing a property failed
        /// </summary>
        PropertyWriteFailed = 2000,
        /// <summary>
        /// No Errors
        /// </summary>
        Ok = 0,
        /// <summary>
        /// A handle was invalid or does not exist
        /// </summary>
        InvalidHandle = -2000,    // handle was invalid (does not exist)
        /// <summary>
        /// A handle had a wrong subtype
        /// </summary>
        WrongHandleSubtype = -2001,
        /// <summary>
        /// An invalid property was passed
        /// </summary>
        InvalidProperty = -2002,
        /// <summary>
        /// A wrong property type was passed
        /// </summary>
        WrongPropertyType = -2003,
        /// <summary>
        /// A property could not be read
        /// </summary>
        PropertyReadFailed = -2004,
        /// <summary>
        /// A source is disconnected
        /// </summary>
        SourceIsDisconnected = -2005,
        /// <summary>
        /// A camera was given an empty value
        /// </summary>
        EmptyValue = -2006,
        /// <summary>
        /// An HTTP camera was given a bad URL
        /// </summary>
        BadUrl = -2007
    }

    /// <summary>
    /// The Pixel Format for cameras
    /// </summary>
    public enum PixelFormat
    {
        /// <summary>
        /// The format is unknwon
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// MJPEG
        /// </summary>
        Mjpeg,
        /// <summary>
        /// YUYV
        /// </summary>
        YUYV,
        /// <summary>
        /// RGB565
        /// </summary>
        RGB565,
        /// <summary>
        /// BGR
        /// </summary>
        BGR,
        /// <summary>
        /// Grayscale
        /// </summary>
        GRAY
    }

    /// <summary>
    /// The log level to use for the logger
    /// </summary>
    public enum LogLevel
    {
        ///
        LogCritical = 50,
        ///
        LogError = 40,
        ///
        LogWarning = 30,
        ///
        LogInfo = 20,
        ///
        LogDebug = 10,
        ///
        LogDebug1 = 9,
        ///
        LogDebug2 = 8,
        ///
        LogDebug3 = 7,
        ///
        LogDebug4 = 6
    }

    /// <summary>
    /// The kind of the property
    /// </summary>
    public enum PropertyKind
    {
        /// <summary>
        /// None set
        /// </summary>
        None = 0,
        /// <summary>
        /// Bool type
        /// </summary>
        Boolean = 1,
        /// <summary>
        /// Integer type
        /// </summary>
        Integer = 2,
        /// <summary>
        /// String type
        /// </summary>
        String = 4,
        /// <summary>
        /// Enum type
        /// </summary>
        Enum = 8
    }

    /// <summary>
    /// The kind of source
    /// </summary>
    public enum SourceKind
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// A USB camera
        /// </summary>
        Usb = 1,
        /// <summary>
        /// An HTTP Camera or stream
        /// </summary>
        Http = 2,
        /// <summary>
        /// An OpenCV image source
        /// </summary>
        CV = 4,
    }

    /// <summary>
    /// The kind of HTTP stream
    /// </summary>
    public enum HttpCameraKind
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Coming from MJPEGStreamer
        /// </summary>
        MjpegStreamer = 1,
        /// <summary>
        /// Coming from CsCore
        /// </summary>
        CsCore = 2,
        /// <summary>
        /// Coming from an Axis camera
        /// </summary>
        Axis = 3
    }

    /// <summary>
    /// The kind of sink
    /// </summary>
    public enum SinkKind
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// MJPEG Stream
        /// </summary>
        Mjpeg = 2,
        /// <summary>
        /// OpenCV output
        /// </summary>
        CV = 4
    }

    /// <summary>
    /// The kind of event that occured
    /// </summary>
    [Flags]
    public enum EventKind : int
    {
        /// <summary>
        /// A source was created
        /// </summary>
        SourceCreated = 0x0001,
        /// <summary>
        /// A source was destroyed
        /// </summary>
        SourceDestroyed = 0x0002,
        /// <summary>
        /// A source was connected
        /// </summary>
        SourceConnected = 0x0004,
        /// <summary>
        /// A source was disconnected
        /// </summary>
        SourceDisconnected = 0x0008,
        /// <summary>
        /// The video modes of a source were updated
        /// </summary>
        SourceVideoModesUpdated = 0x0010,
        /// <summary>
        /// The video mode of a source was changed
        /// </summary>
        SourceVideoModeChanged = 0x0020,
        /// <summary>
        /// A new source property was created
        /// </summary>
        SourcePropertyCreated = 0x0040,
        /// <summary>
        /// A source property was updated
        /// </summary>
        SourcePropertyValueUpdated = 0x0080,
        /// <summary>
        /// The choices for a source property were updated
        /// </summary>
        SourcePropertyChoicesUpdated = 0x0100,
        /// <summary>
        /// A sinks source was changed
        /// </summary>
        SinkSourceChanged = 0x0200,
        /// <summary>
        /// A sink was created
        /// </summary>
        SinkCreated = 0x0400,
        /// <summary>
        /// A sink was destroyed
        /// </summary>
        SinkDestroyed = 0x0800,
        /// <summary>
        /// A sink was enabled
        /// </summary>
        SinkEnabled = 0x1000,
        /// <summary>
        /// A sink was disabled
        /// </summary>
        SinkDisabled = 0x2000,
        /// <summary>
        /// The system network interfaces were changed
        /// </summary>
        NetworkInterfacesChanged = 0x4000
    }

    /// <summary>
    /// Source event kind
    /// </summary>
    public enum SourceEvent
    {
        /// <summary>
        /// A source was created
        /// </summary>
        SourceCreated = 0x01,
        /// <summary>
        /// A source was destroyed
        /// </summary>
        SourceDestroyed = 0x02,
        /// <summary>
        /// A source was connected
        /// </summary>
        SourceConnected = 0x04,
        /// <summary>
        /// A source was disconnected
        /// </summary>
        SourceDisconnected = 0x08
    }

    /// <summary>
    /// Sink event kind
    /// </summary>
    public enum SinkEvent
    {
        /// <summary>
        /// A sink was created
        /// </summary>
        SinkCreated = 0x01,
        /// <summary>
        /// A sink was destroyed
        /// </summary>
        SinkDestroyed = 0x02,
        /// <summary>
        /// A sink was enabled
        /// </summary>
        SinkEnabled = 0x04,
        /// <summary>
        /// A sink was disabled
        /// </summary>
        SinkDisabled = 0x08
    }
}

﻿using System;

namespace CSCore
{
    public enum StatusValue
    {
        PropertyWriteFailed = 2000,
        Ok = 0,
        InvalidHandle = -2000,    // handle was invalid (does not exist)
        WrongHandleSubtype = -2001,
        InvalidProperty = -2002,
        WrongPropertyType = -2003,
        PropertyReadFailed = -2004,
        SourceIsDisconnected = -2005,
        EmptyValue = -2006,
        BadUrl = -2007
    }

    public enum PixelFormat
    {
        Unknown = 0,
        Mjpeg,
        YUYV,
        RGB565,
        BGR,
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

    public enum PropertyKind
    {
        None = 0,
        Boolean = 1,
        Integer = 2,
        String = 4,
        Enum = 8
    }

    public enum SourceKind
    {
        Unknown = 0,
        Usb = 1,
        Http = 2,
        CV = 4,
    }

    public enum HttpCameraKind
    {
        Unknown = 0,
        MjpegStreamer = 1,
        CsCore = 2,
        Axis = 3
    }

    public enum SinkKind
    {
        Unknown = 0,
        Mjpeg = 2,
        CV = 4
    }

    [Flags]
    public enum EventKind : uint
    {
        SourceCreated = 0x0001,
        SourceDestroyed = 0x0002,
        SourceConnected = 0x0004,
        SourceDisconnected = 0x0008,
        SourceVideoModesUpdated = 0x0010,
        SourceVideoModeChanged = 0x0020,
        SourcePropertyCreated = 0x0040,
        SourcePropertyValueUpdated = 0x0080,
        SourcePropertyChoicesUpdated = 0x0100,
        SinkSourceChanged = 0x0200,
        SinkCreated = 0x0400,
        SinkDestroyed = 0x0800,
        SinkEnabled = 0x1000,
        SinkDisabled = 0x2000,
        NetworkInterfacesChanged = 0x4000
    }

    public enum SourceEvent
    {
        SourceCreated = 0x01,
        SourceDestroyed = 0x02,
        SourceConnected = 0x04,
        SourceDisconnected = 0x08
    }

    public enum SinkEvent
    {
        SinkCreated = 0x01,
        SinkDestroyed = 0x02,
        SinkEnabled = 0x04,
        SinkDisabled = 0x08
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CameraServer
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
        SourceIsDisconnected = -2005
    }

    public enum PropertyType
    {
        PropNone = 0,
        PropBoolean = 1,
        PropInteger = 2,
        PropString = 4,
        PropEnum = 8
    };

    public enum SourceEvent
    {
        SourceCreated = 0x01,
        SourceDestroyed = 0x02,
        SourceConnected = 0x04,
        SourceDisconnected = 0x08
    };

    public enum SinkEvent
    {
        SinkCreated = 0x01,
        SinkDestroyed = 0x02,
        SinkEnabled = 0x04,
        SinkDisabled = 0x08
    };
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static CameraServer.Native.Interop;

namespace CameraServer.Native
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct CSEvent
    {
        private EventKind kind;
        private int source;
        private int sink;

        private IntPtr name;

        private VideoMode mode;

        private int property;
        private PropertyKind propertyKind;
        private int value;
        private IntPtr valueStr;

        public VideoEvent ToManaged()
        {
            return new VideoEvent(kind, source, sink, ReadUTF8String(name), mode.PixelFormat, mode.Width, mode.Height, mode.FPS, property, propertyKind, value, ReadUTF8String(valueStr));
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CSUSBCameraInfo
    {
        private int dev;
        private IntPtr path;
        private IntPtr name;

        internal UsbCameraInfo ToManaged()
        {
            return  new UsbCameraInfo(dev, ReadUTF8String(path), ReadUTF8String(name));
        }
    }
}

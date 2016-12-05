using System;
using System.Runtime.InteropServices;

namespace CSCore.Native
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
            return new VideoEvent(kind, source, sink, Interop.ReadUTF8String(name), mode.PixelFormat, mode.Width, mode.Height, mode.FPS, property, propertyKind, value, Interop.ReadUTF8String(valueStr));
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
            return  new UsbCameraInfo(dev, Interop.ReadUTF8String(path), Interop.ReadUTF8String(name));
        }
    }
}

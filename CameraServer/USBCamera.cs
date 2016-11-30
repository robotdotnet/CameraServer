using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CameraServer.Native;
using static CameraServer.Native.Interop;

namespace CameraServer
{
    public class USBCamera : VideoSource
    {
        public USBCamera(string name, int dev)
        {
            UIntPtr size;
            byte[] nameArr = CreateUTF8String(name, out size);
            m_handle = CS_CreateUSBSourceDev(nameArr, dev, ref m_status);
        }

        public USBCamera(string name, string path)
        {
            UIntPtr size;
            byte[] nameArr = CreateUTF8String(name, out size);
            byte[] pathArr = CreateUTF8String(path, out size);
            m_handle = CS_CreateUSBSourcePath(nameArr, pathArr, ref m_status);
        }

        public static IReadOnlyList<UsbCameraInfo> EnumerateUSBCameras()
        {
            int status = 0;
            int count = 0;
            IntPtr camArr = CS_EnumerateUSBCameras(ref count, ref status);

#pragma warning disable CS0618
            int ptrSize = Marshal.SizeOf(typeof(IntPtr));
#pragma warning restore CS0618
            List<UsbCameraInfo> list = new List<UsbCameraInfo>(count);
            for (int i = 0; i < count; i++)
            {
                IntPtr ptr = new IntPtr(camArr.ToInt64() + ptrSize * i);
                CSUSBCameraInfo info = (CSUSBCameraInfo)Marshal.PtrToStructure(ptr, typeof(CSUSBCameraInfo));
                list.Add(info.ToManaged());

            }
            CS_FreeEnumeratedUSBCameras(camArr, count);
            return list;
        }
    }
}

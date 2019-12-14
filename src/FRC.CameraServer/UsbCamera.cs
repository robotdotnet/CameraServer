using FRC.CameraServer.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRC.CameraServer
{
    public class UsbCamera : VideoCamera
    {
        /// <summary>
        /// Create a source for a USB camera based on device number.
        /// </summary>
        /// <param name="name">Source name (arbitrary unique identifier)</param>
        /// <param name="dev">Devices number (e.g. 0 for /dev/video0)</param>
        public UsbCamera(string name, int dev)
            : base(CsCore.CreateUsbCamera(name.AsSpan(), dev))
        {
        }

        /// <summary>
        /// Create a source for a USB camera based on device path.
        /// </summary>
        /// <param name="name">Source name (arbitrary device identifier)</param>
        /// <param name="path">Path to device (e.g. "/dev/video0" on linux)</param>
        public UsbCamera(string name, string path)
            : base(CsCore.CreateUsbCamera(name.AsSpan(), path.AsSpan()))
        {
        }

        /// <summary>
        /// Enumerate USB cameras on the local system
        /// </summary>
        /// <returns>List of USB Camera information (one for each camera)</returns>
        public static UsbCameraInfo[] EnumerateUsbCameras()
        {
            return CsCore.EnumerateUsbCameras();
        }

        /// <summary>
        /// Gets the path to the device
        /// </summary>
        public string Path => CsCore.GetUsbCameraPath(Handle);

        /// <summary>
        /// Gets the full camera information of the device
        /// </summary>
        public UsbCameraInfo Info => CsCore.GetUsbCameraInfo(Handle);

        public int ConnectVerbose
        {
            set
            {
                CsCore.SetProperty(CsCore.GetSourceProperty(Handle, "connect_verbose".AsSpan()), value);
            }
        }
    }
}

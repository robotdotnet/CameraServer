using System.Collections.Generic;

namespace CSCore
{
    /// <summary>
    /// A source that represents a USB camera.
    /// </summary>
    public class UsbCamera : VideoCamera
    {
        /// <summary>
        /// Create a source for a USB camera based on device number.
        /// </summary>
        /// <param name="name">Source name (arbitrary unique identifier)</param>
        /// <param name="dev">Devices number (e.g. 0 for /dev/video0)</param>
        public UsbCamera(string name, int dev)
            : base(NativeMethods.CreateUsbCameraDev(name, dev))
        {
        }

        /// <summary>
        /// Create a source for a USB camera based on device path.
        /// </summary>
        /// <param name="name">Source name (arbitrary device identifier)</param>
        /// <param name="path">Path to device (e.g. "/dev/video0" on linux)</param>
        public UsbCamera(string name, string path)
            : base(NativeMethods.CreateUsbCameraPath(name, path))
        {
        }

        /// <summary>
        /// Enumerate USB cameras on the local system
        /// </summary>
        /// <returns>List of USB Camera information (one for each camera)</returns>
        public static List<UsbCameraInfo> EnumerateUsbCameras()
        {
            return NativeMethods.EnumerateUsbCameras();
        }

        /// <summary>
        /// Gets the path to the device
        /// </summary>
        public string Path => NativeMethods.GetUsbCameraPath(Handle);
    }
}

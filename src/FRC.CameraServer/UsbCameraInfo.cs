namespace CSCore
{
    /// <summary>
    /// Info for a USB camera
    /// </summary>
    public struct UsbCameraInfo
    {
        /// <summary>
        /// The device id (e.g. N in "/dev/videoN" on linux)
        /// </summary>
        public int Device { get; }
        /// <summary>
        /// The path to the device (e.g "/dev/video0" on linux)
        /// </summary>
        public string Path { get; }
        /// <summary>
        /// The name of the device as provided by the vendor and driver
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Create a new USBCameraInfo
        /// </summary>
        /// <param name="device">The device ID</param>
        /// <param name="path">The device path</param>
        /// <param name="name">The device name</param>
        public UsbCameraInfo(int device, string path, string name)
        {
            Device = device;
            Path = path;
            Name = name;
        }
    }
}

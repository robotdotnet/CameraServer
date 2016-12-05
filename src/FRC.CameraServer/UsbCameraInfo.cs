namespace CSCore
{
    public struct UsbCameraInfo
    {
        public int Device { get; }
        public string Path { get; }
        public string Name { get; }

        public UsbCameraInfo(int device, string path, string name)
        {
            Device = device;
            Path = path;
            Name = name;
        }
    }
}

using FRC.CameraServer.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRC.CameraServer
{
    public readonly struct UsbCameraInfo
    {
        public int Dev { get; }
        public string Path { get; }
        public string Name { get; }
        public string[] OtherPaths { get; }
        public int VendorId { get; }
        public int ProductId { get; }

        public UsbCameraInfo(int dev, string path, string name, string[] otherPaths, int vendorId,
            int productId)
        {
            Dev = dev;
            Path = path;
            Name = name;
            OtherPaths = otherPaths;
            VendorId = vendorId;
            ProductId = productId;
        }

        internal unsafe UsbCameraInfo(CS_UsbCameraInfo* info)
        {
            Dev = info->dev;

            Path = UTF8String.ReadUTF8String(info->path);
            Name = UTF8String.ReadUTF8String(info->name);

            OtherPaths = new string[info->otherPathsCount];
            for (int i = 0; i < OtherPaths.Length; i++)
            {
                OtherPaths[i] = UTF8String.ReadUTF8String(info->otherPaths[i]);
            }

            VendorId = info->vendorId;
            ProductId = info->productId;
        }
    }
}

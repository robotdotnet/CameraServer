using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CameraServer
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VideoMode
    {
        public PixelFormat PixelFormat { get; }
        public int Width { get; }
        public int Height { get; }
        public int FPS { get; }

        public VideoMode(PixelFormat format, int w, int h, int f)
        {
            PixelFormat = format;
            Width = w;
            Height = h;
            FPS = f;
        }
    }
}

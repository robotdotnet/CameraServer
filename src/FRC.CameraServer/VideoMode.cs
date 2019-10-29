using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FRC.CameraServer
{
    /// <summary>
    /// VideoModes for Sinks and Sources
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct VideoMode
    {
        /// <summary>
        /// The PixelFormat for the video
        /// </summary>
        public PixelFormat PixelFormat { get; }
        /// <summary>
        /// The width for the video
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// The height for the video
        /// </summary>
        public int Height { get; }
        /// <summary>
        /// The FPS of the video
        /// </summary>
        public int FPS { get; }

        /// <summary>
        /// Creates a new VideoMode
        /// </summary>
        /// <param name="format">The PixelFormat</param>
        /// <param name="w">The width</param>
        /// <param name="h">The height</param>
        /// <param name="f">The FPS</param>
        public VideoMode(PixelFormat format, int w, int h, int f)
        {
            PixelFormat = format;
            Width = w;
            Height = h;
            FPS = f;
        }
    }
}

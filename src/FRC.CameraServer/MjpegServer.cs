using FRC.CameraServer.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRC.CameraServer
{
    public class MjpegServer : VideoSink
    {
        /// <summary>
        /// Create an MJPEG-over-HTTP server sink
        /// </summary>
        /// <param name="name">Sink name (arbitrary unique identifier)</param>
        /// <param name="listenAddress">TCP listen address (emptry string for all addresses)</param>
        /// <param name="port">TCP port number</param>
        public MjpegServer(string name, string listenAddress, int port)
            : base(CsCore.CreateMjpegServer(name.AsSpan(), listenAddress.AsSpan(), port))
        {

        }

        /// <summary>
        /// Create a MJPEG-over-HTTP server sink.
        /// </summary>
        /// <param name="name">Sink name (arbitrary unique identifier)</param>
        /// <param name="port">TCP port number</param>
        public MjpegServer(string name, int port) : this(name, "", port)
        {

        }

        public string ListenAddress => CsCore.GetMjpegServerListenAddress(Handle);
        public int Port => CsCore.GetMjpegServerPort(Handle);

        public void SetResolution(int width, int height)
        {
            CsCore.SetProperty(CsCore.GetSinkProperty(Handle, "width".AsSpan()), width);
            CsCore.SetProperty(CsCore.GetSinkProperty(Handle, "height".AsSpan()), height);
        }

        public void SetFPS(int fps)
        {
            CsCore.SetProperty(CsCore.GetSinkProperty(Handle, "fps".AsSpan()), fps);
        }

        public void SetCompression(int compression)
        {
            CsCore.SetProperty(CsCore.GetSinkProperty(Handle, "compression".AsSpan()), compression);
        }

        public void SetDefaultCompression(int compression)
        {
            CsCore.SetProperty(CsCore.GetSinkProperty(Handle, "default_compression".AsSpan()), compression);
        }
    }
}

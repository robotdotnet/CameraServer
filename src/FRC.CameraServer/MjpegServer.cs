namespace CSCore
{
    /// <summary>
    /// A sink that acts as a MJPEG-over-HTTP network server.
    /// </summary>
    public class MjpegServer : VideoSink
    {
        /// <summary>
        /// Create an MJPEG-over-HTTP server sink
        /// </summary>
        /// <param name="name">Sink name (arbitrary unique identifier)</param>
        /// <param name="listenAddress">TCP listen address (emptry string for all addresses)</param>
        /// <param name="port">TCP port number</param>
        public MjpegServer(string name, string listenAddress, int port) 
            : base (NativeMethods.CreateMjpegServer(name, listenAddress, port))
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

        /// <summary>
        /// Get the listen address of the server
        /// </summary>
        public string ListenAddress => NativeMethods.GetMjpegServerListenAddress(Handle);

        /// <summary>
        /// Get the port number of the server
        /// </summary>
        public int Port => NativeMethods.GetMjpegServerPort(Handle);
    }
}

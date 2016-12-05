using CSCore.Native;

namespace CSCore
{
    public class MjpegServer : VideoSink
    {
        public MjpegServer(string name, string listenAddress, int port) 
            : base (NativeMethods.CreateMjpegServer(name, listenAddress, port))
        {

        }

        public MjpegServer(string name, int port) : this(name, "", port)
        {
            
        }

        public string ListenAddress => NativeMethods.GetMjpegServerListenAddress(m_handle);
        public int Port => NativeMethods.GetMjpegServerPort(m_handle);
    }
}

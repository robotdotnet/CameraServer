using CSCore.Native;

namespace CSCore
{
    public class MJPEGServer : VideoSink
    {
        public MJPEGServer(string name, string listenAddress, int port) 
            : base (NativeMethods.CreateMJPEGServer(name, listenAddress, port))
        {

        }

        public MJPEGServer(string name, int port) : this(name, "", port)
        {
            
        }

        public string ListenAddress => NativeMethods.GetMJPEGServerListenAddress(m_handle);
        public int Port => NativeMethods.GetMJPEGServerPort(m_handle);
    }
}

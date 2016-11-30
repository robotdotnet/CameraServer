using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CameraServer.Native.NativeMethods;

namespace CameraServer
{
    public class MJPEGServer : VideoSink
    {
        public MJPEGServer(string name, string listenAddress, int port) 
            : base (CreateMJPEGServer(name, listenAddress, port))
        {

        }

        public MJPEGServer(string name, int port) : this(name, "", port)
        {
            
        }

        public string ListenAddress => GetMJPEGServerListenAddress(m_handle);
        public int Port => GetMJPEGServerPort(m_handle);
    }
}

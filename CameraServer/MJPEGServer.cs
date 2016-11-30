using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CameraServer.Native.Interop;

namespace CameraServer
{
    public class MJPEGServer : VideoSink
    {
        public MJPEGServer(string name, string listenAddress, int port) : 

        public MJPEGServer(string name, int port) : this(name, "", port)
        {
            
        }
    }
}

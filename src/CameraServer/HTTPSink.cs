using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CameraServer.Native.Interop;

namespace CameraServer
{
    public class HTTPSink : VideoSink
    {
        public HTTPSink(string name, string listenAddress, int port)
        {
            UIntPtr size;
            byte[] nameArr = CreateUTF8String(name, out size);
            byte[] listenArr = CreateUTF8String(listenAddress, out size);
            m_handle = CS_CreateHTTPSink(nameArr, listenArr, port, ref m_status);
        }

        public HTTPSink(string name, int port) : this(name, "", port)
        {
            
        }
    }
}

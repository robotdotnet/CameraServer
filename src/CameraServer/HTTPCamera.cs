using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CameraServer.Native.Interop;

namespace CameraServer
{
    public class HTTPCamera : VideoSource
    {
        public HTTPCamera(string name, string url)
        {
            UIntPtr size;
            byte[] namePtr = CreateUTF8String(name, out size);
            byte[] 
            m_handle = CS_CreateHTTPSource()
        }
    }
}

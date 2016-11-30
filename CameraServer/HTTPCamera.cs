using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CameraServer.Native.NativeMethods;

namespace CameraServer
{
    public class HTTPCamera : VideoSource
    {
        public HTTPCamera(string name, string url) : base(CreateHTTPCamera(name, url))
        {

        }
    }
}

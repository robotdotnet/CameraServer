using CSCore.Native;

namespace CSCore
{
    public class HTTPCamera : VideoSource
    {
        public HTTPCamera(string name, string url) : base(NativeMethods.CreateHTTPCamera(name, url))
        {

        }
    }
}

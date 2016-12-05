using CSCore.Native;

namespace CSCore
{
    public class HttpCamera : VideoSource
    {
        public HttpCamera(string name, string url) : base(NativeMethods.CreateHttpCamera(name, url))
        {

        }
    }
}

using CSCore.Native;
using System.Collections.Generic;

namespace CSCore
{
    public class HttpCamera : VideoSource
    {
        public HttpCamera(string name, string url) 
            : base(NativeMethods.CreateHttpCamera(name, url, HttpCameraKind.Unknown))
        {

        }

        public HttpCamera(string name, string url, HttpCameraKind kind)
            : base(NativeMethods.CreateHttpCamera(name, url, kind))
        {

        }

        public HttpCamera(string name, IList<string> urls) 
            : base(NativeMethods.CreateHttpCameraMulti(name, urls, HttpCameraKind.Unknown))
        {

        }

        public HttpCamera(string name, IList<string> urls, HttpCameraKind kind)
            : base(NativeMethods.CreateHttpCameraMulti(name, urls, kind))
        {

        }

        public HttpCameraKind CameraKind => NativeMethods.GetHttpCameraKind(m_handle);

        public void SetUrls(IList<string> urls)
        {
            NativeMethods.SetHttpCameraUrls(m_handle, urls);
        }

        public List<string> GetUrls()
        {
            return NativeMethods.GetHttpCameraUrls(m_handle);
        }
    }
}

using System.Collections.Generic;

namespace CSCore
{
    /// <summary>
    /// A sources that represents an MJPEG-over-HTTP (IP) camera
    /// </summary>
    public class HttpCamera : VideoCamera
    {
        /// <summary>
        /// Create a source for a MJPEG-over-HTTP (IP) camera.
        /// </summary>
        /// <param name="name">Source name (arbitrary unique identifier)</param>
        /// <param name="url">Camera URL (e.g. "http://10.x.y.11/video/stream.mjpg")</param>
        public HttpCamera(string name, string url) 
            : base(NativeMethods.CreateHttpCamera(name, url, HttpCameraKind.Unknown))
        {

        }

        /// <summary>
        /// Create a source for a MJPEG-over-HTTP (IP) camera.
        /// </summary>
        /// <param name="name">Source name (arbitrary unique identifier)</param>
        /// <param name="url">Camera URL (e.g. "http://10.x.y.11/video/stream.mjpg")</param>
        /// <param name="kind">The camera kind</param>
        public HttpCamera(string name, string url, HttpCameraKind kind)
            : base(NativeMethods.CreateHttpCamera(name, url, kind))
        {

        }

        /// <summary>
        /// Create a source for a MJPEG-over-HTTP (IP) camera.
        /// </summary>
        /// <param name="name">Source name (arbitrary unique identifier)</param>
        /// <param name="urls">Array of camera URLs</param>
        public HttpCamera(string name, IList<string> urls) 
            : base(NativeMethods.CreateHttpCameraMulti(name, urls, HttpCameraKind.Unknown))
        {

        }

        /// <summary>
        /// Create a source for a MJPEG-over-HTTP (IP) camera.
        /// </summary>
        /// <param name="name">Source name (arbitrary unique identifier)</param>
        /// <param name="urls">Array of camera URLs</param>
        /// <param name="kind">The camera kind</param>
        public HttpCamera(string name, IList<string> urls, HttpCameraKind kind)
            : base(NativeMethods.CreateHttpCameraMulti(name, urls, kind))
        {

        }

        /// <summary>
        /// Gets the kind of HTTP camera.
        /// </summary>
        /// <remarks>
        /// Note that autodetection can result in returning a different value then
        /// the camera was created with
        /// </remarks>
        public HttpCameraKind CameraKind => NativeMethods.GetHttpCameraKind(Handle);

        /// <summary>
        /// Change the URLs used to connect to the camera.
        /// </summary>
        /// <param name="urls"></param>
        public void SetUrls(IList<string> urls)
        {
            NativeMethods.SetHttpCameraUrls(Handle, urls);
        }

        /// <summary>
        /// Gets the URLs used to connect to the camera.
        /// </summary>
        /// <returns>A list of urls that can connect to the camera</returns>
        public List<string> GetUrls()
        {
            return NativeMethods.GetHttpCameraUrls(Handle);
        }
    }
}

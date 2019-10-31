using FRC.CameraServer.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRC.CameraServer
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
            : base(CsCore.CreateHttpCamera(name.AsSpan(), url.AsSpan(), HttpCameraKind.Unknown))
        {

        }

        /// <summary>
        /// Create a source for a MJPEG-over-HTTP (IP) camera.
        /// </summary>
        /// <param name="name">Source name (arbitrary unique identifier)</param>
        /// <param name="url">Camera URL (e.g. "http://10.x.y.11/video/stream.mjpg")</param>
        /// <param name="kind">The camera kind</param>
        public HttpCamera(string name, string url, HttpCameraKind kind)
            : base(CsCore.CreateHttpCamera(name.AsSpan(), url.AsSpan(), kind))
        {

        }

        ///// <summary>
        ///// Create a source for a MJPEG-over-HTTP (IP) camera.
        ///// </summary>
        ///// <param name="name">Source name (arbitrary unique identifier)</param>
        ///// <param name="urls">Array of camera URLs</param>
        //public HttpCamera(string name, IList<string> urls)
        //    : base(NativeMethods.CreateHttpCameraMulti(name, urls, HttpCameraKind.Unknown))
        //{

        //}

        ///// <summary>
        ///// Create a source for a MJPEG-over-HTTP (IP) camera.
        ///// </summary>
        ///// <param name="name">Source name (arbitrary unique identifier)</param>
        ///// <param name="urls">Array of camera URLs</param>
        ///// <param name="kind">The camera kind</param>
        //public HttpCamera(string name, IList<string> urls, HttpCameraKind kind)
        //    : base(NativeMethods.CreateHttpCameraMulti(name, urls, kind))
        //{

        //}

        /// <summary>
        /// Gets the kind of HTTP camera.
        /// </summary>
        /// <remarks>
        /// Note that autodetection can result in returning a different value then
        /// the camera was created with
        /// </remarks>
        public HttpCameraKind CameraKind => CsCore.GetHttpCameraKind(Handle);

        ///// <summary>
        ///// Change the URLs used to connect to the camera.
        ///// </summary>
        ///// <param name="urls"></param>
        //public void SetUrls(IList<string> urls)
        //{
        //    NativeMethods.SetHttpCameraUrls(Handle, urls);
        //}

        ///// <summary>
        ///// Gets the URLs used to connect to the camera.
        ///// </summary>
        ///// <returns>A list of urls that can connect to the camera</returns>
        //public List<string> GetUrls()
        //{
        //    return NativeMethods.GetHttpCameraUrls(Handle);
        //}
    }
}

using System.Collections.Generic;

namespace CSCore
{
    /// <summary>
    /// A source that represents an Axis IP Camera.
    /// </summary>
    public class AxisCamera : HttpCamera
    {

        private static string HostToUrl(string host)
        {
            return $"http://{host}/mjpeg/video.mjpg";
        }

        private static List<string> HostToUrl(ICollection<string> hosts)
        {
            List<string> urls = new List<string>(hosts.Count);
            foreach (string host in hosts)
            {
                urls.Add(HostToUrl(host));
            }
            return urls;
        }

        /// <summary>
        /// Create a source for an Axis IP Camera.
        /// </summary>
        /// <param name="name">The source name (arbitrary unique identifier)</param>
        /// <param name="host">Camera host IP or DNS name</param>
        public AxisCamera(string name, string host) 
            : base (name, HostToUrl(host), HttpCameraKind.Axis)
        {

        }

        /// <summary>
        /// Create a source for an Axis IP Camera
        /// </summary>
        /// <param name="name">The source name (arbitrary unique identifier)</param>
        /// <param name="hosts">List of host names to try and connect to (IP/DNS names)</param>
        public AxisCamera(string name, ICollection<string> hosts)
            : base(name, HostToUrl(hosts), HttpCameraKind.Axis)
        {
            
        }

        
    }
}
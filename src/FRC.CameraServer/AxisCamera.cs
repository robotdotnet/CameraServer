using System.Collections.Generic;

namespace CSCore
{
    public class AxisCamera : HttpCamera
    {
        private static string HostToUrl(string host)
        {
            return $"http://{host}/mjpeg/video.mjpg";
        }

        private static List<string> HostToUrl(IList<string> hosts)
        {
            List<string> urls = new List<string>(hosts.Count);
            for(int i = 0; i < hosts.Count; i++)
            {
                urls.Add(HostToUrl(hosts[i]));
            }
            return urls;
        }

        public AxisCamera(string name, string host) 
            : base (name, HostToUrl(host), HttpCameraKind.Axis)
        {

        }

        public AxisCamera(string name, IList<string> hosts)
            : base(name, HostToUrl(hosts), HttpCameraKind.Axis)
        {
            
        }

        
    }
}
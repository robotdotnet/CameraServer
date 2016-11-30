using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraServer
{
    public class VideoException : Exception
    {
        public VideoException(string msg) : base (msg)
        {
        }

        public override string ToString()
        {
            return $"VideoException [{base.ToString()}]";
        }
    }
}

using System;

namespace CSCore
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

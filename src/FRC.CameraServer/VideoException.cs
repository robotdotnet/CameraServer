using System;

namespace CSCore
{
    /// <summary>
    /// Exception raised by the CameraServer
    /// </summary>
    public class VideoException : Exception
    {
        /// <summary>
        /// Creates a new VideoException
        /// </summary>
        /// <param name="msg">The message for the exception</param>
        public VideoException(string msg) : base (msg)
        {
        }

        /// <summary>
        /// Prints the exception as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"VideoException [{base.ToString()}]";
        }
    }
}

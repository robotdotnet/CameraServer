using System.Collections.Generic;
using OpenCvSharp;

namespace CSCore
{
    /// <summary>
    /// A source that allows setting OpenCV images as the frame source
    /// </summary>
    public class CvSource : VideoSource
    {
        /// <summary>
        /// Creates an OpenCV Source.
        /// </summary>
        /// <param name="name">Source name (arbitrary unique identifier)</param>
        /// <param name="mode"><see cref="VideoMode"/> to be generated</param>
        public CvSource(string name, VideoMode mode) 
            : base (NativeMethods.CreateCvSource(name, mode.PixelFormat, mode.Width, mode.Height, mode.FPS))
        {

        }

        /// <summary>
        /// Create an OpenCV Source.
        /// </summary>
        /// <param name="name">Source name (arbitrary unique identifier)</param>
        /// <param name="pixelFormat"><see cref="PixelFormat"/> to generate</param>
        /// <param name="width">Width to generate</param>
        /// <param name="height">Height to generate</param>
        /// <param name="fps">FPS to generate</param>
        public CvSource(string name, PixelFormat pixelFormat, int width, int height, int fps)
            : base(NativeMethods.CreateCvSource(name, pixelFormat, width, height, fps))
        {

        }

        /// <summary>
        /// Put an OpenCV Image and notify sinks.
        /// </summary>
        /// <remarks>
        /// Only 8-bit single channel or 3-channel (with BGR channel order) images
        /// are supported. If the format, depth, or channel are wrong, use
        /// <see cref="Mat.ConvertTo"/> or <see cref="Cv2.CvtColor"/> to convert
        /// it first.
        /// </remarks>
        /// <param name="image">The OpenCV image to put</param>
        public void PutFrame(Mat image)
        {
            NativeMethods.PutSourceFrame(Handle, image.CvPtr);
        }

        /// <summary>
        /// Signal sinks that an error has occured.
        /// </summary>
        /// <param name="msg">The message to notify sinks with</param>
        public void NotifyError(string msg)
        {
            NativeMethods.NotifySourceError(Handle, msg);
        }

        /// <summary>
        /// Sets the source connection status
        /// </summary>
        public override bool Connected
        {
            set
            {
                NativeMethods.SetSourceConnected(Handle, value);
            }
        }

        /// <summary>
        /// Sets the source description
        /// </summary>
        public override string Description
        {
            set
            {
                NativeMethods.SetSourceDescription(Handle, value);
            }
        }

        /// <summary>
        /// Creates a <see cref="VideoProperty"/>
        /// </summary>
        /// <param name="name">Property name</param>
        /// <param name="kind">Property kind</param>
        /// <param name="minimum">Minimum value</param>
        /// <param name="maximum">Maximum value</param>
        /// <param name="step">Step value</param>
        /// <param name="defaultValue">Default value</param>
        /// <param name="value">Current value</param>
        /// <returns>The created <see cref="VideoProperty"/></returns>
        public VideoProperty CreateProperty(string name, PropertyKind kind, int minimum, int maximum, int step, int defaultValue, int value)
        {
            return new VideoProperty(
                NativeMethods.CreateSourceProperty(Handle, name, kind, minimum, maximum, step, defaultValue, value));
        }

        /// <summary>
        /// Configure enum property choices
        /// </summary>
        /// <param name="property">The property to set</param>
        /// <param name="choices">The property choices</param>
        public void SetEnumPropertyChoices(VideoProperty property, IList<string> choices)
        {
            NativeMethods.SetSourceEnumPropertyChoices(Handle, property.m_handle, choices);
        }
    }
}

using OpenCvSharp;

namespace CSCore
{
    /// <summary>
    /// A sink for user code to accept video frames as OpenCV images
    /// </summary>
    public class CvSink : VideoSink
    {
        /// <summary>
        /// Create a sink for accepting OpenCV images
        /// </summary>
        /// <remarks>
        /// <see cref="GrabFrame"/> must be called on the created sink to get
        /// each new frame
        /// </remarks>
        /// <param name="name">Source name (arbitrary unique identifier</param>
        public CvSink(string name) : base(NativeMethods.CreateCvSink(name))
        {

        }

        /// <summary>
        /// Sets the description for this sink
        /// </summary>
        public override string Description
        {
            set
            {
                NativeMethods.SetSinkDescription(Handle, value);
            }
        }

        /// <summary>
        /// Wait for the next frame and get the image
        /// </summary>
        /// <remarks>
        /// The provided image will have three 3-bit channels stored in BGR order
        /// </remarks>
        /// <param name="image">The <see cref="Mat"/> to store the image in</param>
        /// <returns>Frame time, or 0 on error (call <see cref="GetError"/> to obtain 
        /// the error message</returns>
        public long GrabFrame(Mat image)
        {
            return (long)NativeMethods.GrabSinkFrame(Handle, image.CvPtr);
        }

        /// <summary>
        /// Get the latest error string. 
        /// </summary>
        /// <remarks>
        /// Call if <see cref="GrabFrame"/> returns 0 to determine what the error is
        /// </remarks>
        /// <returns></returns>
        public string GetError()
        {
            return NativeMethods.GetSinkError(Handle);
        }

        /// <summary>
        /// Enable or disable getting new frames
        /// </summary>
        /// <remarks>
        /// Disabling will cause <see cref="GrabFrame"/> to not return. This can
        /// be used to save processor resources when frames are not needed
        /// </remarks>
        public bool Enabled
        {
            set
            {
                NativeMethods.SetSinkEnabled(Handle, value);
            }
        }
    }
}

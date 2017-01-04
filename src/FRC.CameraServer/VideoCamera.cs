namespace CSCore
{
    /// <summary>
    /// A source that represents a video camera.
    /// </summary>
    public class VideoCamera : VideoSource
    {
        private readonly object m_mutex = new object();

        /// <summary>
        /// Constants for camera white balance
        /// </summary>
        public class WhiteBalance
        {
            /// <summary>
            /// Fixed indoor
            /// </summary>
            public const int FixedIndoor = 3000;
            /// <summary>
            /// Fixed outdoor 1
            /// </summary>
            public const int FixedOutdoor1 = 4000;
            /// <summary>
            /// Fixed outdoor 2
            /// </summary>
            public const int FixedOutdoor2 = 5000;
            /// <summary>
            /// Fixed Flourescent 1
            /// </summary>
            public const int FixedFluorescent1 = 5100;
            /// <summary>
            /// Fixed Flourescent 2
            /// </summary>
            public const int FixedFlourescent2 = 5200;
        }

        /// <summary>
        /// Creates a VideoCamera from a handle
        /// </summary>
        protected VideoCamera(int handle) : base(handle)
        {

        }

        /// <summary>
        /// Gets or sets the brightness, as a percentage (0-100).
        /// </summary>
        public int Brightness
        {
            get
            {
                lock (m_mutex)
                {
                return NativeMethods.GetCameraBrightness(Handle);
                }
            }
            set
            {
                lock (m_mutex)
                {
                NativeMethods.SetCameraBrightness(Handle, value);
                }
            }
        }

        /// <summary>
        /// Set the white balance to auto.
        /// </summary>
        public void SetWhiteBalanceAuto()
        {
            lock (m_mutex)
            {
                NativeMethods.SetCameraWhiteBalanceAuto(Handle);
            }
        }

        /// <summary>
        /// Set the white balance to hold current.
        /// </summary>
        public void SetWhiteBalanceHoldCurrent()
        {
            lock (m_mutex)
            {
                NativeMethods.SetCameraWhiteBalanceHoldCurrent(Handle);
            }
        }

        /// <summary>
        /// Set the white balance to manual, with specified color temperature.
        /// </summary>
        public void SetWhiteBalanceManual(int value)
        {
            lock (m_mutex)
            {
                NativeMethods.SetCameraWhiteBalanceManual(Handle, value);
            }
        }

        /// <summary>
        /// Set the exposure to auto aperture.
        /// </summary>
        public void SetExposureAuto()
        {
            lock (m_mutex)
            {
                NativeMethods.SetCameraExposureAuto(Handle);
            }
        }

        /// <summary>
        /// Set the exposure to hold current.
        /// </summary>
        public void SetExposureHoldCurrent()
        {
            lock (m_mutex)
            {
                NativeMethods.SetCameraExposureHoldCurrent(Handle);
            }
        }

        /// <summary>
        /// Set the exposure to manual, as a percentage (0-100).
        /// </summary>
        public void SetExposureManual(int value)
        {
            lock (m_mutex)
            {
                NativeMethods.SetCameraExposureManual(Handle, value);
            }
        }
    }
}

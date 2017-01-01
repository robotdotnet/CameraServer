using System.Collections.Generic;

namespace CSCore
{
    /// <summary>
    /// A source that represents a USB camera.
    /// </summary>
    public class UsbCamera : VideoSource
    {
        private readonly object m_mutex = new object();

        private const string PropWbAuto = "white_balance_temperature_auto";
        private const string PropWbValue = "white_balance_temperature";
        private const string PropExAuto = "exposure_auto";
        private const string PropExValue = "exposure_absolute";
        private const string PropBrValue = "brightness";

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

        // Cached to avoid duplicate string lookups
        private VideoProperty m_wbAuto;
        private VideoProperty m_wbValue;
        private VideoProperty m_exAuto;
        private VideoProperty m_exValue;
        private VideoProperty m_brValue;

        /// <summary>
        /// Create a source for a USB camera based on device number.
        /// </summary>
        /// <param name="name">Source name (arbitrary unique identifier)</param>
        /// <param name="dev">Devices number (e.g. 0 for /dev/video0)</param>
        public UsbCamera(string name, int dev)
            : base(NativeMethods.CreateUsbCameraDev(name, dev))
        {
        }

        /// <summary>
        /// Create a source for a USB camera based on device path.
        /// </summary>
        /// <param name="name">Source name (arbitrary device identifier)</param>
        /// <param name="path">Path to device (e.g. "/dev/video0" on linux)</param>
        public UsbCamera(string name, string path)
            : base(NativeMethods.CreateUsbCameraPath(name, path))
        {
        }

        /// <summary>
        /// Enumerate USB cameras on the local system
        /// </summary>
        /// <returns>List of USB Camera information (one for each camera)</returns>
        public static List<UsbCameraInfo> EnumerateUsbCameras()
        {
            return NativeMethods.EnumerateUsbCameras();
        }

        /// <summary>
        /// Gets the path to the device
        /// </summary>
        public string Path => NativeMethods.GetUsbCameraPath(Handle);

        /// <summary>
        /// Gets or sets the brightness as a percentage (0-100).
        /// </summary>
        public int Brightness
        {
            set
            {
                if (value > 100)
                {
                    value = 100;
                }
                else if (value < 0)
                {
                    value = 0;
                }
                lock (m_mutex)
                {
                    if (m_brValue == null) m_brValue = GetProperty(PropBrValue);
                    m_brValue.Set(value);
                }
            }
            get
            {
                lock (m_mutex)
                {
                    if (m_brValue == null) m_brValue = GetProperty(PropBrValue);
                    return m_brValue.Get();
                }
            }
        }

        /// <summary>
        /// Set the white balance to auto
        /// </summary>
        public void SetWhiteBalanceAuto()
        {
            lock (m_mutex)
            {
                if (m_wbAuto == null) m_wbAuto = GetProperty(PropWbAuto);
            m_wbAuto.Set(1);  // auto
            }
        }

        /// <summary>
        /// Set the white balance to hold current
        /// </summary>
        public void SetWhiteBalanceHoldCurrent()
        {
            lock (m_mutex)
            {
                if (m_wbAuto == null) m_wbAuto = GetProperty(PropWbAuto);
            m_wbAuto.Set(0);  // manual
            }
        }

        /// <summary>
        /// Set the white balance to manual, with specified color temperature
        /// </summary>
        /// <param name="value"></param>
        public void SetWhiteBalanceManual(int value)
        {
            lock (m_mutex)
            {
                if (m_wbAuto == null) m_wbAuto = GetProperty(PropWbAuto);
            m_wbAuto.Set(0);  // manual
            if (m_wbValue == null) m_wbValue = GetProperty(PropWbValue);
            m_wbValue.Set(value);
            }
        }

        /// <summary>
        /// Set the exposure to auto aperature.
        /// </summary>
        public void SetExposureAuto()
        {
            lock (m_mutex)
            {
                if (m_exAuto == null) m_exAuto = GetProperty(PropExAuto);
            m_exAuto.Set(0);  // auto; yes, this is opposite of white balance.
            }
        }

        /// <summary>
        /// Set the exposure to hold current.
        /// </summary>
        public void SetExposureHoldCurrent()
        {
            lock (m_mutex)
            {
                if (m_exAuto == null) m_exAuto = GetProperty(PropExAuto);
            m_exAuto.Set(1);  // manual
            }
        }

        /// <summary>
        /// Set the exposure to manual, as a percentage (0-100).
        /// </summary>
        public void SetExposureManual(int value)
        {
            lock (m_mutex)
            {
                if (m_exAuto == null) m_exAuto = GetProperty(PropExAuto);
                m_exAuto.Set(1);  // manual
                if (value > 100)
                {
                    value = 100;
                }
                else if (value < 0)
                {
                    value = 0;
                }
                if (m_exValue == null) m_exValue = GetProperty(PropExValue);
                m_exValue.Set(value);
            }
        }
    }
}

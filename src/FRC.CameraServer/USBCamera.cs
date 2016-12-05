using System.Collections.Generic;
using CSCore.Native;

namespace CSCore
{
    public class USBCamera : VideoSource
    {
        private readonly object m_mutex = new object();

        private const string PropWbAuto = "white_balance_temperature_auto";
        private const string PropWbValue = "white_balance_temperature";
        private const string PropExAuto = "exposure_auto";
        private const string PropExValue = "exposure_absolute";
        private const string PropBrValue = "brightness";

        public class WhiteBalance
        {
            public const int FixedIndoor = 3000;
            public const int FixedOutdoor1 = 4000;
            public const int FixedOutdoor2 = 5000;
            public const int FixedFluorescent1 = 5100;
            public const int FixedFlourescent2 = 5200;
        }

        // Cached to avoid duplicate string lookups
        private VideoProperty m_wbAuto;
        private VideoProperty m_wbValue;
        private VideoProperty m_exAuto;
        private VideoProperty m_exValue;
        private VideoProperty m_brValue;

        public USBCamera(string name, int dev)
            : base(NativeMethods.CreateUSBCameraDev(name, dev))
        {
        }

        public USBCamera(string name, string path)
            : base(NativeMethods.CreateUSBCameraPath(name, path))
        {
        }

        public static List<UsbCameraInfo> EnumerateUSBCameras()
        {
            return NativeMethods.EnumerateUSBCameras();
        }

        public string Path => NativeMethods.GetUSBCameraPath(m_handle);

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

        /// Set the white balance to auto.
        public void SetWhiteBalanceAuto()
        {
            lock (m_mutex)
            {
                if (m_wbAuto == null) m_wbAuto = GetProperty(PropWbAuto);
            m_wbAuto.Set(1);  // auto
            }
        }

        /// Set the white balance to hold current.
        public void SetWhiteBalanceHoldCurrent()
        {
            lock (m_mutex)
            {
                if (m_wbAuto == null) m_wbAuto = GetProperty(PropWbAuto);
            m_wbAuto.Set(0);  // manual
            }
        }

        /// Set the white balance to manual, with specified color temperature.
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

        /// Set the exposure to auto aperature.
        public void SetExposureAuto()
        {
            lock (m_mutex)
            {
                if (m_exAuto == null) m_exAuto = GetProperty(PropExAuto);
            m_exAuto.Set(0);  // auto; yes, this is opposite of white balance.
            }
        }

        /// Set the exposure to hold current.
        public void SetExposureHoldCurrent()
        {
            lock (m_mutex)
            {
                if (m_exAuto == null) m_exAuto = GetProperty(PropExAuto);
            m_exAuto.Set(1);  // manual
            }
        }

        /// Set the exposure to manual, as a percentage (0-100).
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

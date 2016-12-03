using System;
using CSCore.Native;

namespace CSCore
{
    public class VideoListener : IDisposable
    {
        public VideoListener(Action<VideoEvent> listener, int eventMask, bool immediateNotify)
        {
            m_handle = NativeMethods.AddListener(listener, eventMask, immediateNotify);
        }

        public void Dispose()
        {
            if (m_handle != 0)
            {
                NativeMethods.RemoveListener(m_handle);
            }
            m_handle = 0;
        }

        public bool IsValid => m_handle != 0;

        private int m_handle;
    }
}

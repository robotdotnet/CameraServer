using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CameraServer.Native.NativeMethods;

namespace CameraServer
{
    public class VideoListener : IDisposable
    {
        public VideoListener(Action<VideoEvent> listener, int eventMask, bool immediateNotify)
        {
            m_handle = AddListener(listener, eventMask, immediateNotify);
        }

        public void Dispose()
        {
            if (m_handle != 0)
            {
                RemoveListener(m_handle);
            }
            m_handle = 0;
        }

        public bool IsValid => m_handle != 0;

        private int m_handle;
    }
}

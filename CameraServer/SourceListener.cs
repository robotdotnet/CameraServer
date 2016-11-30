using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CameraServer.Native.Interop;

namespace CameraServer
{
    public class SourceListener : IDisposable
    {
        private int m_handle;

        private Action<string, VideoSource, int> m_callback;

        private SourceListenerCallback m_nativeCallback;

        public SourceListener()
        {
            m_handle = 0;
        }

        public SourceListener(Action<string, VideoSource, int> callback, int eventMask)
        {
            int status = 0;
            m_callback = callback;
            m_nativeCallback = (data, name, source, evnt) =>
            {
                string nme = ReadUTF8String(name);
                m_callback?.Invoke(nme, new VideoSource(source), evnt);
            };
            m_handle = CS_AddSourceListener(IntPtr.Zero, m_nativeCallback, eventMask, ref status);
        }

        public void Dispose()
        {
            int status = 0;
            if (m_handle != 0)
            {
                CS_RemoveSourceListener(m_handle, ref status);
            }
        }
    }
}

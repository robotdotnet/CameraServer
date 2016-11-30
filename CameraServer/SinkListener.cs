using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CameraServer.Native.Interop;

namespace CameraServer
{
    public class SinkListener : IDisposable
    {
        private int m_handle;

        private Action<string, VideoSink, int> m_callback;

        private SinkListenerCallback m_nativeCallback;

        public SinkListener()
        {
            m_handle = 0;
        }

        public SinkListener(Action<string, VideoSink, int> callback, int eventMask)
        {
            int status = 0;
            m_callback = callback;
            m_nativeCallback = (data, name, source, evnt) =>
            {
                string nme = ReadUTF8String(name);
                m_callback?.Invoke(nme, new VideoSink(source), evnt);
            };
            m_handle = CS_AddSinkListener(IntPtr.Zero, m_nativeCallback, eventMask, ref status);
        }

        public void Dispose()
        {
            int status = 0;
            if (m_handle != 0)
            {
                CS_RemoteSinkListener(m_handle, ref status);
            }
        }
    }
}

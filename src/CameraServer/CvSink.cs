using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emgu.CV;
using static CameraServer.Native.Interop;
using Emgu.CV.Structure;

namespace CameraServer
{
    public class CvSink : VideoSink
    {
        private Action<long> m_callback;

        private CS_ProcessFrame m_nativeCallback;

        public CvSink(string name)
        {
            UIntPtr size;
            byte[] nameArr = CreateUTF8String(name, out size);
            m_handle = CS_CreateCvSink(nameArr, ref m_status);
        }

        public CvSink(string name, Action<long> processFrame)
        {
            UIntPtr size;
            byte[] nameArr = CreateUTF8String(name, out size);
            m_callback = processFrame;
            m_nativeCallback = (data, time) =>
            {
                m_callback?.Invoke((long)time);
            };
            m_handle = CS_CreateCvSinkCallback(nameArr, IntPtr.Zero, m_nativeCallback, ref m_status);
        }

        public long GrabFrame(out Mat image)
        {
            m_status = 0;
            MCvMat nativeMat = new MCvMat();
            ulong ret = CS_GrabSinkFrame(m_handle, ref nativeMat, ref m_status);
            image = CvInvoke.CvArrToMat(nativeMat.Data, false, false, 0);
            return (long)ret;
        }

        public string GetError()
        {
            m_status = 0;
            IntPtr str = CS_GetSinkError(m_handle, ref m_status);
            string ret = ReadUTF8String(str);
            CS_FreeString(str);
            return ret;
        }

        public bool Enabled
        {
            set
            {
                m_status = 0;
                CS_SetSinkEnabled(m_handle, value, ref m_status);
            }
        }
    }
}

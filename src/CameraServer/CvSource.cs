using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emgu.CV;
using static CameraServer.Native.Interop;

namespace CameraServer
{
    public class CvSource : VideoSource
    {
        public CvSource(string name)
        {
            m_status = 0;
            UIntPtr size;
            byte[] nameArr = CreateUTF8String(name, out size);
            m_handle = CS_CreateCvSource(nameArr, ref m_status);
        }

        public void PutFrame(Mat image)
        {
            // TODO: Figure this out
            throw new NotImplementedException();
        }

        public void NotifyError(string msg)
        {
            m_status = 0;
            UIntPtr size;
            byte[] msgArr = CreateUTF8String(msg, out size);
            CS_NotifySourceError(m_handle, msgArr, ref m_status);
        }

        public bool Connected
        {
            set
            {
                m_status = 0;
                CS_SetSourceConnected(m_handle, value, ref m_status);
            }
        }

        public VideoProperty CreateProperty(string name, PropertyType type)
        {
            m_status = 0;
            UIntPtr size;
            byte[] nameArr = CreateUTF8String(name, out size);
            return new VideoProperty(CS_CreateSourceProperty(m_handle, nameArr, type, ref m_status));
        }

        private ConcurrentDictionary<string, CS_OnChange> m_onChangeCallbacks = new ConcurrentDictionary<string, CS_OnChange>();

        public VideoProperty CreateProperty(string name, PropertyType type, Action<VideoProperty> onChange)
        {
            m_status = 0;
            UIntPtr size;
            byte[] nameArr = CreateUTF8String(name, out size);

            CS_OnChange nativeOnChange = (data, property) =>
            {
                onChange?.Invoke(new VideoProperty(property));
            };

            m_onChangeCallbacks.TryAdd(name, nativeOnChange);

            return new VideoProperty(CS_CreateSourcePropertyCallback(m_handle, nameArr, type, IntPtr.Zero, nativeOnChange, ref m_status));
        }

        public void RemoveProperty(VideoProperty property)
        {
            string name = property.Name;
            m_status = 0;

            CS_OnChange native;
            m_onChangeCallbacks.TryRemove(name, out native);
            CS_RemoveSourceProperty(m_handle, property.Handle, ref m_status);
        }

        public void RemoveProperty(string name)
        {
            m_status = 0;

            CS_OnChange native;
            m_onChangeCallbacks.TryRemove(name, out native);
            UIntPtr size;
            byte[] nameArr = CreateUTF8String(name, out size);
            CS_RemoveSourcePropertyByName(m_handle, nameArr, ref m_status);
        }
    }
}

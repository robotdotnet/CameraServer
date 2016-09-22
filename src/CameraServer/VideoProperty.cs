using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CameraServer.Native.Interop;

namespace CameraServer
{
    public class VideoProperty
    {
        private int m_handle = 0;

        private int m_status;

        internal int Handle => m_handle;

        public string Name
        {
            get
            {
                m_status = 0;
                IntPtr str = CS_GetPropertyName(m_status, ref m_status);
                if (m_status != 0) return String.Empty;
                string retVal = ReadUTF8String(str);
                CS_FreeString(str);
                return retVal;
            }
        }

        public PropertyType PropType { get; } = PropertyType.PropNone;

        public bool IsBoolean => PropType == PropertyType.PropBoolean;
        public bool IsInteger => PropType == PropertyType.PropInteger;
        public bool IsString => PropType == PropertyType.PropString;
        public bool IsEnum => PropType == PropertyType.PropEnum;

        public int Get()
        {
            m_status = 0;
            return CS_GetProperty(m_handle, ref m_status);
        }

        public void Set(int value)
        {
            m_status = 0;
            CS_SetProperty(m_handle, value, ref m_status);
        }

        public int GetMin()
        {
            m_status = 0;
            return CS_GetPropertyMin(m_handle, ref m_status);
        }

        public int GetMax()
        {
            m_status = 0;
            return CS_GetPropertyMax(m_handle, ref m_status);
        }

        public int GetStep()
        {
            m_status = 0;
            return CS_GetPropertyStep(m_handle, ref m_status);
        }

        public int GetDefault()
        {
            m_status = 0;
            return CS_GetPropertyDefault(m_handle, ref m_status);
        }

        public string GetString()
        {
            m_status = 0;
            IntPtr str = CS_GetStringProperty(m_status, ref m_status);
            if (m_status != 0) return String.Empty;
            string retVal = ReadUTF8String(str);
            CS_FreeString(str);
            return retVal;
        }

        public void SetString(string str)
        {
            UIntPtr size;
            byte[] strArr = CreateUTF8String(str, out size);
            m_status = 0;
            CS_SetStringProperty(m_handle, strArr, ref m_status);
        }

        public IReadOnlyList<string> GetChoices()
        {
            m_status = 0;
            int count = 0;
            IntPtr arr = CS_GetEnumPropertyChoices(m_handle, ref count, ref m_status);
            if (count == 0) return null;
            var retList = ReadStringArrayFromPtr(arr, count);
            CS_FreeEnumPropertyChoices(arr, count);
            return retList;
        }

        public int GetLastStatus() => m_status;

        internal VideoProperty(int handle)
        {
            m_status = 0;
            m_handle = handle;
            PropType = handle == 0 ? PropertyType.PropNone : CS_GetPropertyType(handle, ref m_status);
        } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CameraServer.Native.NativeMethods;

namespace CameraServer
{
    public class VideoProperty
    {
        internal int m_handle;
        private PropertyKind m_kind;

        public string Name => GetPropertyName(m_handle);

        public PropertyKind Kind => m_kind;

        public bool IsValid => m_kind != PropertyKind.None;

        public bool IsBoolean => m_kind == PropertyKind.Boolean;

        public bool IsInteger => m_kind == PropertyKind.Integer;

        public bool IsString => m_kind == PropertyKind.String;

        public bool IsEnum => m_kind == PropertyKind.Enum;

        public int Get()
        {
            return GetProperty(m_handle);
        }

        public void Set(int value)
        {
            SetProperty(m_handle, value);
        }

        public int GetMin()
        {
            return GetPropertyMin(m_handle);
        }

        public int GetMax()
        {
            return GetPropertyMax(m_handle);
        }

        public int GetStep()
        {
            return GetPropertyStep(m_handle);
        }

        public int GetDefault()
        {
            return GetPropertyDefault(m_handle);
        }

        // String-specific functions
        public string GetString()
        {
            return GetStringProperty(m_handle);
        }

        public void SetString(string value)
        {
            SetStringProperty(m_handle, value);
        }

        public List<string> GetChoices()
        {
            return GetEnumPropertyChoices(m_handle);
        }

        internal VideoProperty(int handle)
        {
            m_handle = handle;
            m_kind = GetPropertyKind(m_handle);
        }

        internal VideoProperty(int handle, PropertyKind kind)
        {
            m_handle = handle;
            m_kind = kind;
        }
    }
}

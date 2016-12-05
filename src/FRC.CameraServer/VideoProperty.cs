using System.Collections.Generic;
using CSCore.Native;

namespace CSCore
{
    public class VideoProperty
    {
        internal int m_handle;
        private PropertyKind m_kind;

        public string Name => NativeMethods.GetPropertyName(m_handle);

        public PropertyKind Kind => m_kind;

        public bool IsValid => m_kind != PropertyKind.None;

        public bool IsBoolean => m_kind == PropertyKind.Boolean;

        public bool IsInteger => m_kind == PropertyKind.Integer;

        public bool IsString => m_kind == PropertyKind.String;

        public bool IsEnum => m_kind == PropertyKind.Enum;

        public int Get()
        {
            return NativeMethods.GetProperty(m_handle);
        }

        public void Set(int value)
        {
            NativeMethods.SetProperty(m_handle, value);
        }

        public int GetMin()
        {
            return NativeMethods.GetPropertyMin(m_handle);
        }

        public int GetMax()
        {
            return NativeMethods.GetPropertyMax(m_handle);
        }

        public int GetStep()
        {
            return NativeMethods.GetPropertyStep(m_handle);
        }

        public int GetDefault()
        {
            return NativeMethods.GetPropertyDefault(m_handle);
        }

        // String-specific functions
        public string GetString()
        {
            return NativeMethods.GetStringProperty(m_handle);
        }

        public void SetString(string value)
        {
            NativeMethods.SetStringProperty(m_handle, value);
        }

        public List<string> GetChoices()
        {
            return NativeMethods.GetEnumPropertyChoices(m_handle);
        }

        internal VideoProperty(int handle)
        {
            m_handle = handle;
            m_kind = NativeMethods.GetPropertyKind(m_handle);
        }

        internal VideoProperty(int handle, PropertyKind kind)
        {
            m_handle = handle;
            m_kind = kind;
        }
    }
}

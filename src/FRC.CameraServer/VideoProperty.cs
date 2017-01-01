using System.Collections.Generic;

namespace CSCore
{
    /// <summary>
    /// Video Properties for sources
    /// </summary>
    public class VideoProperty
    {
        internal int m_handle;
        private PropertyKind m_kind;

        /// <summary>
        /// The name of the property
        /// </summary>
        public string Name => NativeMethods.GetPropertyName(m_handle);

        /// <summary>
        /// The kind of property
        /// </summary>
        public PropertyKind Kind => m_kind;

        /// <summary>
        /// If the property is valid
        /// </summary>
        public bool IsValid => m_kind != PropertyKind.None;

        /// <summary>
        /// If the property is bool type
        /// </summary>
        public bool IsBoolean => m_kind == PropertyKind.Boolean;

        /// <summary>
        /// If the property is integer type
        /// </summary>
        public bool IsInteger => m_kind == PropertyKind.Integer;

        /// <summary>
        /// If the property is string type
        /// </summary>
        public bool IsString => m_kind == PropertyKind.String;

        /// <summary>
        /// If the property is enum type
        /// </summary>
        public bool IsEnum => m_kind == PropertyKind.Enum;

        /// <summary>
        /// Gets the current value of the property
        /// </summary>
        /// <returns>The integer representation of the property</returns>
        public int Get()
        {
            return NativeMethods.GetProperty(m_handle);
        }

        /// <summary>
        /// Sets the current value of the property
        /// </summary>
        /// <param name="value">The integer value to set the property to</param>
        public void Set(int value)
        {
            NativeMethods.SetProperty(m_handle, value);
        }

        /// <summary>
        /// Gets the minimum value of the property
        /// </summary>
        /// <returns>The minimum value</returns>
        public int GetMin()
        {
            return NativeMethods.GetPropertyMin(m_handle);
        }

        /// <summary>
        /// Gets the maximum value of the property
        /// </summary>
        /// <returns>The maximum value</returns>
        public int GetMax()
        {
            return NativeMethods.GetPropertyMax(m_handle);
        }

        /// <summary>
        /// Gets the step value of the property
        /// </summary>
        /// <returns>The step value</returns>
        public int GetStep()
        {
            return NativeMethods.GetPropertyStep(m_handle);
        }

        /// <summary>
        /// Gets the default value of the property
        /// </summary>
        /// <returns>The default value</returns>
        public int GetDefault()
        {
            return NativeMethods.GetPropertyDefault(m_handle);
        }

        // String-specific functions
        /// <summary>
        /// Gets the string representation of the property
        /// </summary>
        /// <returns>The string value</returns>
        public string GetString()
        {
            return NativeMethods.GetStringProperty(m_handle);
        }

        /// <summary>
        /// Sets the string representation of the property
        /// </summary>
        /// <param name="value"></param>
        public void SetString(string value)
        {
            NativeMethods.SetStringProperty(m_handle, value);
        }

        /// <summary>
        /// Gets a list of choices for the property
        /// </summary>
        /// <returns>List of property choices</returns>
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

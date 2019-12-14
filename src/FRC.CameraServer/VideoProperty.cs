using FRC.CameraServer.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace FRC.CameraServer
{
    /// <summary>
    /// Video Properties for sources
    /// </summary>
    public class VideoProperty
    {
        internal CS_Property m_handle;

        /// <summary>
        /// The name of the property
        /// </summary>
        public string Name => CsCore.GetPropertyName(m_handle);

        /// <summary>
        /// The kind of property
        /// </summary>
        public PropertyKind Kind { get; }

        /// <summary>
        /// If the property is valid
        /// </summary>
        public bool IsValid => Kind != PropertyKind.None;

        /// <summary>
        /// If the property is bool type
        /// </summary>
        public bool IsBoolean => Kind == PropertyKind.Boolean;

        /// <summary>
        /// If the property is integer type
        /// </summary>
        public bool IsInteger => Kind == PropertyKind.Integer;

        /// <summary>
        /// If the property is string type
        /// </summary>
        public bool IsString => Kind == PropertyKind.String;

        /// <summary>
        /// If the property is enum type
        /// </summary>
        public bool IsEnum => Kind == PropertyKind.Enum;

        /// <summary>
        /// Gets the current value of the property
        /// </summary>
        /// <returns>The integer representation of the property</returns>
        public int Get()
        {
            return CsCore.GetProperty(m_handle);
        }

        /// <summary>
        /// Sets the current value of the property
        /// </summary>
        /// <param name="value">The integer value to set the property to</param>
        public void Set(int value)
        {
            CsCore.SetProperty(m_handle, value);
        }

        /// <summary>
        /// Gets the minimum value of the property
        /// </summary>
        /// <returns>The minimum value</returns>
        public int GetMin()
        {
            return CsCore.GetPropertyMin(m_handle);
        }

        /// <summary>
        /// Gets the maximum value of the property
        /// </summary>
        /// <returns>The maximum value</returns>
        public int GetMax()
        {
            return CsCore.GetPropertyMax(m_handle);
        }

        /// <summary>
        /// Gets the step value of the property
        /// </summary>
        /// <returns>The step value</returns>
        public int GetStep()
        {
            return CsCore.GetPropertyStep(m_handle);
        }

        /// <summary>
        /// Gets the default value of the property
        /// </summary>
        /// <returns>The default value</returns>
        public int GetDefault()
        {
            return CsCore.GetPropertyDefault(m_handle);
        }

        // String-specific functions
        /// <summary>
        /// Gets the string representation of the property
        /// </summary>
        /// <returns>The string value</returns>
        public string GetString()
        {
            return CsCore.GetStringProperty(m_handle);
        }

        /// <summary>
        /// Sets the string representation of the property
        /// </summary>
        /// <param name="value"></param>
        public void SetString(string value)
        {
            CsCore.SetStringProperty(m_handle, value.AsSpan());
        }

        /// <summary>
        /// Gets a list of choices for the property
        /// </summary>
        /// <returns>List of property choices</returns>
        public List<string> GetChoices()
        {
            return new List<string>(CsCore.GetEnumPropertyChoices(m_handle));
        }

        internal VideoProperty(CS_Property handle)
        {
            m_handle = handle;
            Kind = CsCore.GetPropertyKind(m_handle);
        }

        internal VideoProperty(CS_Property handle, PropertyKind kind)
        {
            m_handle = handle;
            Kind = kind;
        }
    }

}

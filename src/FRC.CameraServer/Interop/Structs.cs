using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FRC.CameraServer.Interop
{
    /// <summary>
    /// CS Bool for interop
    /// </summary>
    public readonly struct CsBool
    {
        private readonly int m_value;

        /// <summary>
        /// Creates an CS Bool from an int
        /// </summary>
        /// <param name="value">value</param>
        public CsBool(int value)
        {
            this.m_value = value;
        }

        /// <summary>
        /// Creates an CS Bool from a bool
        /// </summary>
        /// <param name="value"></param>
        public CsBool(bool value)
        {
            this.m_value = value ? 1 : 0;
        }

        /// <summary>
        /// Gets the value
        /// </summary>
        /// <returns>value</returns>
        public bool Get()
        {
            return m_value != 0;
        }

        /// <summary>
        /// Converts a bool to an CS Bool
        /// </summary>
        /// <param name="value">bool balue</param>
        public static implicit operator CsBool(bool value)
        {
            return new CsBool(value);
        }
    }

    /// <summary>
    /// CS Status for interop
    /// </summary>
    public readonly struct CsStatus
    {
        private readonly int m_value;

        /// <summary>
        /// Creates a CS Status from an int
        /// </summary>
        /// <param name="value">value</param>
        public CsStatus(int value)
        {
            this.m_value = value;
        }

        /// <summary>
        /// Gets the value
        /// </summary>
        /// <returns>value</returns>
        public int Get()
        {
            return m_value;
        }

        public bool IsValid() {
            return m_value == 0;
        }

        /// <summary>
        /// Converts a int to an CS Status
        /// </summary>
        /// <param name="value">int value</param>
        public static implicit operator CsStatus(int value)
        {
            return new CsStatus(value);
        }
    }
}
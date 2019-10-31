using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FRC.CameraServer.Interop
{
    /// <summary>
    /// CS Bool for interop
    /// </summary>
    public readonly ref struct CS_Bool
    {
        private readonly int m_value;

        /// <summary>
        /// Creates an CS Bool from an int
        /// </summary>
        /// <param name="value">value</param>
        public CS_Bool(int value)
        {
            this.m_value = value;
        }

        /// <summary>
        /// Creates an CS Bool from a bool
        /// </summary>
        /// <param name="value"></param>
        public CS_Bool(bool value)
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
        public static implicit operator CS_Bool(bool value)
        {
            return new CS_Bool(value);
        }
    }

    /// <summary>
    /// CS Status for interop
    /// </summary>
    public readonly ref struct CS_Status
    {
        private readonly int m_value;

        /// <summary>
        /// Creates a CS Status from an int
        /// </summary>
        /// <param name="value">value</param>
        public CS_Status(int value)
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
        public static implicit operator CS_Status(int value)
        {
            return new CS_Status(value);
        }
    }

    public unsafe ref struct CS_Event
    {
        public EventKind kind;
        public CS_Source source;
        public CS_Sink sink;
        public byte* name;
        public VideoMode mode;
        public CS_Property property;
        public PropertyKind propertyKind;
        public int value;
        public byte* valueStr;
    }

    public unsafe ref struct CS_UsbCameraInfo
    {
        public int dev;
        public byte* path;
        public byte* name;
        public int otherPathsCount;
        public byte** otherPaths;
        public int vendorId;
        public int productId;
    }

    public unsafe struct CS_RawFrame
    {
        public byte* data;
        public int dataLength;
        public int pixelFormat;
        public int width;
        public int height;
        public int totalData;
    }
}
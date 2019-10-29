using System;
using System.Runtime.CompilerServices;

namespace FRC.CameraServer.Interop
{
    /// <summary>
    /// Low level CS Core Handle
    /// </summary>
    public readonly struct CS_Handle
    {
        private readonly int m_value;

        /// <summary>
        /// Create a handle from an int.
        /// </summary>
        /// <param name="value">handle value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CS_Handle(int value)
        {
            m_value = value;
        }

        /// <summary>
        /// Gets the raw handle value
        /// </summary>
        /// <returns>The raw handle value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Get()
        {
            return m_value;
        }

        /// <summary>
        /// Checks equality between another object
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>True if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return false; // Because ref struct, can never be equal
        }

        /// <summary>
        /// Checks equality between another Handle
        /// </summary>
        /// <param name="other">Handle to check</param>
        /// <returns>True if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CS_Handle other)
        {
            return m_value == other.m_value;
        }

        /// <summary>
        /// Gets Hash Code of Handle
        /// </summary>
        /// <returns>Handle Value as Hash Code</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return m_value.GetHashCode();
        }

        /// <summary>
        /// Gets if 2 handles are equal
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <param name="rhs">Right Hand Side</param>
        /// <returns>true if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator==(CS_Handle lhs, CS_Handle rhs)
        {
            return lhs.m_value == rhs.m_value;
        }

        /// <summary>
        /// Gets if 2 handles are not equal
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <param name="rhs">Right Hand Side</param>
        /// <returns>true if not equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator!=(CS_Handle lhs, CS_Handle rhs)
        {
            return lhs.m_value != rhs.m_value;
        }
    }

    /// <summary>
    /// Low Level NT Core Instance Handle
    /// </summary>
    public readonly struct CS_Property
    {
        private readonly CS_Handle m_value;

        /// <summary>
        /// Creates a new handle
        /// </summary>
        /// <param name="value">handle value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CS_Property(int value)
        {
            m_value = new CS_Handle(value);
        }

        /// <summary>
        /// Gets the raw handle value
        /// </summary>
        /// <returns>The raw handle value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Get()
        {
            return m_value.Get();
        }

        /// <summary>
        /// Converts a handle to a base handle
        /// </summary>
        /// <param name="value">The current handle</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator CS_Handle(CS_Property value)
        {
            return value.m_value;
        }

        /// <summary>
        /// Checks equality between another object
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>True if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return false;
        }

        /// <summary>
        /// Checks equality between another Handle
        /// </summary>
        /// <param name="other">Handle to check</param>
        /// <returns>True if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CS_Property other)
        {
            return m_value == other.m_value;
        }

        /// <summary>
        /// Gets Hash Code of Handle
        /// </summary>
        /// <returns>Handle Value as Hash Code</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return m_value.GetHashCode();
        }

        /// <summary>
        /// Gets if 2 handles are equal
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <param name="rhs">Right Hand Side</param>
        /// <returns>true if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(CS_Property lhs, CS_Property rhs)
        {
            return lhs.m_value == rhs.m_value;
        }

        /// <summary>
        /// Gets if 2 handles are not equal
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <param name="rhs">Right Hand Side</param>
        /// <returns>true if not equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(CS_Property lhs, CS_Property rhs)
        {
            return lhs.m_value != rhs.m_value;
        }
    }

    /// <summary>
    /// Low Level NT Core Instance Handle
    /// </summary>
    public readonly struct CS_Listener
    {
        private readonly CS_Handle m_value;

        /// <summary>
        /// Creates a new handle
        /// </summary>
        /// <param name="value">handle value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CS_Listener(int value)
        {
            m_value = new CS_Handle(value);
        }

        /// <summary>
        /// Gets the raw handle value
        /// </summary>
        /// <returns>The raw handle value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Get()
        {
            return m_value.Get();
        }

        /// <summary>
        /// Converts a handle to a base handle
        /// </summary>
        /// <param name="value">The current handle</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator CS_Handle(CS_Listener value)
        {
            return value.m_value;
        }

        /// <summary>
        /// Checks equality between another object
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>True if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return false;
        }

        /// <summary>
        /// Checks equality between another Handle
        /// </summary>
        /// <param name="other">Handle to check</param>
        /// <returns>True if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CS_Listener other)
        {
            return m_value == other.m_value;
        }

        /// <summary>
        /// Gets Hash Code of Handle
        /// </summary>
        /// <returns>Handle Value as Hash Code</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return m_value.GetHashCode();
        }

        /// <summary>
        /// Gets if 2 handles are equal
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <param name="rhs">Right Hand Side</param>
        /// <returns>true if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(CS_Listener lhs, CS_Listener rhs)
        {
            return lhs.m_value == rhs.m_value;
        }

        /// <summary>
        /// Gets if 2 handles are not equal
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <param name="rhs">Right Hand Side</param>
        /// <returns>true if not equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(CS_Listener lhs, CS_Listener rhs)
        {
            return lhs.m_value != rhs.m_value;
        }
    }

    /// <summary>
    /// Low Level NT Core Instance Handle
    /// </summary>
    public readonly struct CS_Sink
    {
        private readonly CS_Handle m_value;

        /// <summary>
        /// Creates a new handle
        /// </summary>
        /// <param name="value">handle value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CS_Sink(int value)
        {
            m_value = new CS_Handle(value);
        }

        /// <summary>
        /// Gets the raw handle value
        /// </summary>
        /// <returns>The raw handle value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Get()
        {
            return m_value.Get();
        }

        /// <summary>
        /// Converts a handle to a base handle
        /// </summary>
        /// <param name="value">The current handle</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator CS_Handle(CS_Sink value)
        {
            return value.m_value;
        }

        /// <summary>
        /// Checks equality between another object
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>True if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return false;
        }

        /// <summary>
        /// Checks equality between another Handle
        /// </summary>
        /// <param name="other">Handle to check</param>
        /// <returns>True if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CS_Sink other)
        {
            return m_value == other.m_value;
        }

        /// <summary>
        /// Gets Hash Code of Handle
        /// </summary>
        /// <returns>Handle Value as Hash Code</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return m_value.GetHashCode();
        }

        /// <summary>
        /// Gets if 2 handles are equal
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <param name="rhs">Right Hand Side</param>
        /// <returns>true if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(CS_Sink lhs, CS_Sink rhs)
        {
            return lhs.m_value == rhs.m_value;
        }

        /// <summary>
        /// Gets if 2 handles are not equal
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <param name="rhs">Right Hand Side</param>
        /// <returns>true if not equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(CS_Sink lhs, CS_Sink rhs)
        {
            return lhs.m_value != rhs.m_value;
        }
    }

    /// <summary>
    /// Low Level NT Core Instance Handle
    /// </summary>
    public readonly struct CS_Source
    {
        private readonly CS_Handle m_value;

        /// <summary>
        /// Creates a new handle
        /// </summary>
        /// <param name="value">handle value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CS_Source(int value)
        {
            m_value = new CS_Handle(value);
        }

        /// <summary>
        /// Gets the raw handle value
        /// </summary>
        /// <returns>The raw handle value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Get()
        {
            return m_value.Get();
        }

        /// <summary>
        /// Converts a handle to a base handle
        /// </summary>
        /// <param name="value">The current handle</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator CS_Handle(CS_Source value)
        {
            return value.m_value;
        }

        /// <summary>
        /// Checks equality between another object
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>True if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return false;
        }

        /// <summary>
        /// Checks equality between another Handle
        /// </summary>
        /// <param name="other">Handle to check</param>
        /// <returns>True if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CS_Source other)
        {
            return m_value == other.m_value;
        }

        /// <summary>
        /// Gets Hash Code of Handle
        /// </summary>
        /// <returns>Handle Value as Hash Code</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return m_value.GetHashCode();
        }

        /// <summary>
        /// Gets if 2 handles are equal
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <param name="rhs">Right Hand Side</param>
        /// <returns>true if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(CS_Source lhs, CS_Source rhs)
        {
            return lhs.m_value == rhs.m_value;
        }

        /// <summary>
        /// Gets if 2 handles are not equal
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <param name="rhs">Right Hand Side</param>
        /// <returns>true if not equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(CS_Source lhs, CS_Source rhs)
        {
            return lhs.m_value != rhs.m_value;
        }
    }
}
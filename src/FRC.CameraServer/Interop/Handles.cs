using System;
using System.Runtime.CompilerServices;

namespace FRC.CameraServer.Interop
{
    /// <summary>
    /// Low level CS Core Handle
    /// </summary>
    public readonly struct CsHandle : IEquatable<CsHandle> 
    {
private readonly int m_value;

        /// <summary>
        /// Create a handle from an int.
        /// </summary>
        /// <param name="value">handle value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CsHandle(int value)
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
            if (obj is CsHandle v)
            {
                return Equals(v);
            }
            return false;
        }

        /// <summary>
        /// Checks equality between another Handle
        /// </summary>
        /// <param name="other">Handle to check</param>
        /// <returns>True if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CsHandle other)
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
        public static bool operator==(CsHandle lhs, CsHandle rhs)
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
        public static bool operator!=(CsHandle lhs, CsHandle rhs)
        {
            return lhs.m_value != rhs.m_value;
        }
    }

    /// <summary>
    /// Low Level NT Core Instance Handle
    /// </summary>
    public readonly struct CsProperty
    {
        private readonly CsHandle m_value;

        /// <summary>
        /// Creates a new handle
        /// </summary>
        /// <param name="value">handle value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CsProperty(int value)
        {
            m_value = new CsHandle(value);
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
        public static implicit operator CsHandle(CsProperty value)
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
            if (obj is CsProperty v)
            {
                return Equals(v);
            }
            return false;
        }

        /// <summary>
        /// Checks equality between another Handle
        /// </summary>
        /// <param name="other">Handle to check</param>
        /// <returns>True if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CsProperty other)
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
        public static bool operator ==(CsProperty lhs, CsProperty rhs)
        {
            return lhs.m_value == rhs.m_value;
        }

        /// <summary>
        /// Gets if 2 handles are not equal
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <param name="rhs">Right Hand Side</param>
        /// <returns>true if not equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(CsProperty lhs, CsProperty rhs)
        {
            return lhs.m_value != rhs.m_value;
        }
    }

    /// <summary>
    /// Low Level NT Core Instance Handle
    /// </summary>
    public readonly struct CsListener
    {
        private readonly CsHandle m_value;

        /// <summary>
        /// Creates a new handle
        /// </summary>
        /// <param name="value">handle value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CsListener(int value)
        {
            m_value = new CsHandle(value);
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
        public static implicit operator CsHandle(CsListener value)
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
            if (obj is CsListener v)
            {
                return Equals(v);
            }
            return false;
        }

        /// <summary>
        /// Checks equality between another Handle
        /// </summary>
        /// <param name="other">Handle to check</param>
        /// <returns>True if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CsListener other)
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
        public static bool operator ==(CsListener lhs, CsListener rhs)
        {
            return lhs.m_value == rhs.m_value;
        }

        /// <summary>
        /// Gets if 2 handles are not equal
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <param name="rhs">Right Hand Side</param>
        /// <returns>true if not equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(CsListener lhs, CsListener rhs)
        {
            return lhs.m_value != rhs.m_value;
        }
    }

    /// <summary>
    /// Low Level NT Core Instance Handle
    /// </summary>
    public readonly struct CsSink
    {
        private readonly CsHandle m_value;

        /// <summary>
        /// Creates a new handle
        /// </summary>
        /// <param name="value">handle value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CsSink(int value)
        {
            m_value = new CsHandle(value);
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
        public static implicit operator CsHandle(CsSink value)
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
            if (obj is CsSink v)
            {
                return Equals(v);
            }
            return false;
        }

        /// <summary>
        /// Checks equality between another Handle
        /// </summary>
        /// <param name="other">Handle to check</param>
        /// <returns>True if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CsSink other)
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
        public static bool operator ==(CsSink lhs, CsSink rhs)
        {
            return lhs.m_value == rhs.m_value;
        }

        /// <summary>
        /// Gets if 2 handles are not equal
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <param name="rhs">Right Hand Side</param>
        /// <returns>true if not equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(CsSink lhs, CsSink rhs)
        {
            return lhs.m_value != rhs.m_value;
        }
    }

    /// <summary>
    /// Low Level NT Core Instance Handle
    /// </summary>
    public readonly struct CsSource
    {
        private readonly CsHandle m_value;

        /// <summary>
        /// Creates a new handle
        /// </summary>
        /// <param name="value">handle value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CsSource(int value)
        {
            m_value = new CsHandle(value);
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
        public static implicit operator CsHandle(CsSource value)
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
            if (obj is CsSource v)
            {
                return Equals(v);
            }
            return false;
        }

        /// <summary>
        /// Checks equality between another Handle
        /// </summary>
        /// <param name="other">Handle to check</param>
        /// <returns>True if equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CsSource other)
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
        public static bool operator ==(CsSource lhs, CsSource rhs)
        {
            return lhs.m_value == rhs.m_value;
        }

        /// <summary>
        /// Gets if 2 handles are not equal
        /// </summary>
        /// <param name="lhs">Left Hand Side</param>
        /// <param name="rhs">Right Hand Side</param>
        /// <returns>true if not equal</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(CsSource lhs, CsSource rhs)
        {
            return lhs.m_value != rhs.m_value;
        }
    }
}
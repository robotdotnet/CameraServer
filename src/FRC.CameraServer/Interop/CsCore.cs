using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using FRC.NativeLibraryUtilities;

namespace FRC.CameraServer.Interop
{
    public static class CsCore 
    {
        private static ICsCore m_cscore;

        private unsafe readonly static char* NullTerminator;

        static CsCore() 
        {
            unsafe 
            {
                NullTerminator = (char*)Marshal.AllocHGlobal(sizeof(char));
                *NullTerminator = '\0';
            }

            var nativeLoader = new NativeLibraryLoader();

            string[] commandArgs = Environment.GetCommandLineArgs();
            foreach (var commandArg in commandArgs)
            {
                //search for a line with the prefix "-cscore:"
                if (commandArg.ToLower().Contains("-cscore:"))
                {
                    //Split line to get the library.
                    int splitLoc = commandArg.IndexOf(':');
                    string file = commandArg.Substring(splitLoc + 1);

                    //If the file exists, just return it so dlopen can load it.
                    if (File.Exists(file))
                    {
                        nativeLoader.LoadNativeLibrary<ICsCore>(file, true);
                        m_cscore = nativeLoader.LoadNativeInterface<ICsCore>();
                        return;
                    }
                }
            }

            const string resourceRoot = "FRC.CameraServer.DesktopLibraries.libraries.";

            nativeLoader.AddLibraryLocation(OsType.Windows32,
                resourceRoot + "windows.x86.cscorejni.dll");
            nativeLoader.AddLibraryLocation(OsType.Windows64,
                resourceRoot + "windows.x86_64.cscorejni.dll");
            nativeLoader.AddLibraryLocation(OsType.Linux64,
                resourceRoot + "linux.x86_64.libcscorejni.so");
            nativeLoader.AddLibraryLocation(OsType.MacOs64,
                resourceRoot + "osx.x86_64.libcscorejni.dylib");
            nativeLoader.AddLibraryLocation(OsType.roboRIO, "ntcore");

            nativeLoader.LoadNativeLibraryFromReflectedAssembly("FRC.CameraServer.DesktopLibraries");
            m_cscore = nativeLoader.LoadNativeInterface<ICsCore>();
        }

        private static Span<T> GetSpanOrBuffer<T>(Span<T> store, int length)
        {
            return store.Length >= length ? store.Slice(0, length) : new T[length];
        }

        [DoesNotReturn]
        private static void ThrowStatusException(CsStatus status)
        {
            StatusValue s = (StatusValue)status.Get();
            string msg = $"unknown error code={status}";
            switch(s)
            {
                case StatusValue.PropertyWriteFailed:
                    msg = "property write failed";
                    break;
                case StatusValue.InvalidHandle:
                    msg = "invalid handle";
                    break;
                case StatusValue.WrongHandleSubtype:
                    msg = "wrong handle subtype";
                    break;
                case StatusValue.InvalidProperty:
                    msg = "invalid property";
                    break;
                case StatusValue.WrongPropertyType:
                    msg = "wrong property type";
                    break;
                case StatusValue.EmptyValue:
                    msg = "empty value";
                    break;
                case StatusValue.BadUrl:
                    msg = "bad URL";
                    break;
                case StatusValue.PropertyReadFailed:
                    msg = "read failed";
                    break;
                case StatusValue.SourceIsDisconnected:
                    msg = "source is disconnected";
                    break;
            }
            throw new VideoException(msg);
        }

        public static unsafe PropertyKind GetPropertyKind(CsProperty property) 
        {
            CsStatus status = 0;
            var retVal = m_cscore.CS_GetPropertyKind(property, &status);
            if (!status.IsValid()) 
            {
                ThrowStatusException(status);
            }
            return retVal;
        }

        public static unsafe string GetPropertyName(CsProperty property)
        {
            CsStatus status = 0;
            var buf = m_cscore.CS_GetPropertyName(property, &status);
            if (!status.IsValid()) {
                ThrowStatusException(status);
            }

            string ret = UTF8String.ReadUTF8String(buf);
            m_cscore.CS_FreeString(buf);
            return ret;

        }

        public static unsafe CsSource CreateUsbCamera(ReadOnlySpan<char> name, int dev) 
        {
            if (name.IsEmpty) 
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Cannot have an empty name");
            }

            fixed (char* p = name)
            {
                var dLen = Encoding.UTF8.GetByteCount(p, name.Length);
                Span<byte> dSpan = dLen <= 256 ? stackalloc byte[dLen == 0 ? 1 : dLen] : new byte[dLen];
                fixed (byte* d = dSpan)
                {
                    Encoding.UTF8.GetBytes(p, name.Length, d, dLen);
                    CsStatus status = 0;
                    var ret = m_cscore.CS_CreateUsbCameraDev(d, dev, &status);
                    if (!status.IsValid()) {
                        ThrowStatusException(status);
                    }
                    return ret;
                }
            }
        }

        public static unsafe CsSource CreateUsbCamera(ReadOnlySpan<char> name, ReadOnlySpan<char> path) 
        {
            if (name.IsEmpty) 
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Cannot have an empty name");
            }

            if (path.IsEmpty) 
            {
                throw new ArgumentOutOfRangeException(nameof(path), "Cannot have an empty path");
            }

            fixed (char* n = name)
            fixed (char* p = path)
            {
                var nLen = Encoding.UTF8.GetByteCount(n, name.Length);
                Span<byte> dSpan = nLen <= 256 ? stackalloc byte[nLen] : new byte[nLen];
                var pLen = Encoding.UTF8.GetByteCount(p, path.Length);
                Span<byte> pSpan = pLen <= 256 ? stackalloc byte[pLen] : new byte[pLen];
                fixed (byte* d = dSpan)
                fixed (byte* pp = pSpan)
                {
                    Encoding.UTF8.GetBytes(n, name.Length, d, nLen);
                    Encoding.UTF8.GetBytes(p, path.Length, pp, pLen);
                    CsStatus status = 0;
                    var ret = m_cscore.CS_CreateUsbCameraPath(d, pp, &status);
                    if (!status.IsValid()) {
                        ThrowStatusException(status);
                    }
                    return ret;
                }
            }
        }
    }
}
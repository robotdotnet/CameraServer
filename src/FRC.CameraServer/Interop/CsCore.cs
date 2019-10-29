using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using FRC.NativeLibraryUtilities;

#nullable enable

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

        
        private static void ThrowStatusException(int status)
        {
            StatusValue s = (StatusValue)status;
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

        private static void CheckStatus(int status, [DoesNotReturnIf(false)]bool isValid)
        {
            if (!isValid)
            {
                ThrowStatusException(status);
            }
        }

        public static unsafe PropertyKind GetPropertyKind(CS_Property property) 
        {
            int status = 0;
            var retVal = m_cscore.CS_GetPropertyKind(property, &status);
            CheckStatus(status, status != 0);

            return retVal;
        }

        public static unsafe string GetPropertyName(CS_Property property)
        {
            int status = 0;
            var buf = m_cscore.CS_GetPropertyName(property, &status);
            CheckStatus(status, status != 0);

            string ret = UTF8String.ReadUTF8String(buf);
            m_cscore.CS_FreeString(buf);
            return ret;

        }

        public static unsafe int GetProperty(CS_Property property)
        {
            int status = 0;
            var ret = m_cscore.CS_GetProperty(property, &status);
            CheckStatus(status, status != 0);
            return ret;
        }

        public static unsafe int GetPropertyMin(CS_Property property)
        {
            int status = 0;
            var ret = m_cscore.CS_GetPropertyMin(property, &status);
            CheckStatus(status, status != 0);
            return ret;
        }

        public static unsafe int GetPropertyMax(CS_Property property)
        {
            int status = 0;
            var ret = m_cscore.CS_GetPropertyMax(property, &status);
            CheckStatus(status, status != 0);
            return ret;
        }

        public static unsafe int GetPropertyStep(CS_Property property)
        {
            int status = 0;
            var ret = m_cscore.CS_GetPropertyStep(property, &status);
            CheckStatus(status, status != 0);
            return ret;
        }

        public static unsafe int GetPropertyDefault(CS_Property property)
        {
            int status = 0;
            var ret = m_cscore.CS_GetPropertyDefault(property, &status);
            CheckStatus(status, status != 0);
            return ret;
        }

        public static unsafe string GetStringProperty(CS_Property property)
        {
            int status = 0;
            var buf = m_cscore.CS_GetStringProperty(property, &status);
            CheckStatus(status, status != 0);

            string ret = UTF8String.ReadUTF8String(buf);
            m_cscore.CS_FreeString(buf);
            return ret;

        }

        public static unsafe void SetStringProperty(CS_Property property, ReadOnlySpan<char> value)
        {
            fixed (char* p = value.IsEmpty ? new ReadOnlySpan<char>(NullTerminator, 1) : value)
            {
                var dLen = Encoding.UTF8.GetByteCount(p, value.Length);
                Span<byte> dSpan = dLen <= 256 ? stackalloc byte[dLen == 0 ? 1 : dLen] : new byte[dLen];
                fixed (byte* d = dSpan)
                {
                    Encoding.UTF8.GetBytes(p, value.Length, d, dLen);
                    int status = 0;
                    m_cscore.CS_SetStringProperty(property, d, &status);
                    CheckStatus(status, status != 0);
                }
            }
        }

        public static unsafe string[] GetEnumPropertyChoices(CS_Property property)
        {
            int status = 0;
            int count = 0;
            byte** values = m_cscore.CS_GetEnumPropertyChoices(property, &count, &status);
            CheckStatus(status, status != 0);
            string[] toRet = new string[count];
            for (int i = 0; i< toRet.Length; i++)
            {
                toRet[i] = UTF8String.ReadUTF8String(values[i]);
            }
            m_cscore.CS_FreeEnumPropertyChoices(values, count);
            return toRet;
        }

        public static unsafe CS_Source CreateUsbCamera(ReadOnlySpan<char> name, int dev) 
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
                    int status = 0;
                    var ret = m_cscore.CS_CreateUsbCameraDev(d, dev, &status);
                    CheckStatus(status, status != 0);
                    return ret;
                }
            }
        }

        public static unsafe CS_Source CreateUsbCamera(ReadOnlySpan<char> name, ReadOnlySpan<char> path) 
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
                    int status = 0;
                    var ret = m_cscore.CS_CreateUsbCameraPath(d, pp, &status);
                    CheckStatus(status, status != 0);
                    return ret;
                }
            }
        }

        public static unsafe CS_Source CreateHttpCamera(ReadOnlySpan<char> name, ReadOnlySpan<char> path, HttpCameraKind kind)
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
                    int status = 0;
                    var ret = m_cscore.CS_CreateHttpCamera(d, pp, kind, &status);
                    CheckStatus(status, status != 0);
                    return ret;
                }
            }
        }

        public static unsafe ulong GrabRawSinkFrame(CS_Sink sink, ref CS_RawFrame rawImage)
        {
            int status = 0;
            CS_RawFrame localFrame = rawImage;
            var ret = m_cscore.CS_GrabRawSinkFrame(sink, &localFrame, &status);
            CheckStatus(status, status != 0);
            rawImage = localFrame;
            return ret;
        }

        public static unsafe ulong GrabRawSinkFrameTimeout(CS_Sink sink, ref CS_RawFrame rawImage, double timeout)
        {
            int status = 0;
            CS_RawFrame localFrame = rawImage;
            var ret = m_cscore.CS_GrabRawSinkFrameTimeout(sink, &localFrame, timeout, &status);
            CheckStatus(status, status != 0);
            rawImage = localFrame;
            return ret;
        }

        public static unsafe CS_Sink CreateRawSink(ReadOnlySpan<char> name)
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
                    int status = 0;
                    var ret = m_cscore.CS_CreateRawSink(d, &status);
                    CheckStatus(status, status != 0);
                    return ret;
                }
            }
        }

        public static unsafe CS_Source CreateRawSource(ReadOnlySpan<char> name, CS_VideoMode mode)
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
                    int status = 0;
                    var ret = m_cscore.CS_CreateRawSource(d, &mode, &status);
                    CheckStatus(status, status != 0);
                    return ret;
                }
            }
        }

        public static unsafe void PutRawSourceFrame(CS_Source source, CS_RawFrame rawFrame)
        {
            int status = 0;
            m_cscore.CS_PutRawSourceFrame(source, &rawFrame, &status);
            CheckStatus(status, status != 0);
        }

        public static unsafe SourceKind GetSourceKind(CS_Source source)
        {
            int status = 0;
            var ret = m_cscore.CS_GetSourceKind(source, &status);
            CheckStatus(status, status != 0);
            return ret;
        }

        public static unsafe string GetSourceName(CS_Source source)
        {
            int status = 0;
            var buf = m_cscore.CS_GetSourceName(source, &status);
            CheckStatus(status, status != 0);

            string ret = UTF8String.ReadUTF8String(buf);
            m_cscore.CS_FreeString(buf);
            return ret;
        }

        public static unsafe string GetSourceDescription(CS_Source source)
        {
            int status = 0;
            var buf = m_cscore.CS_GetSourceDescription(source, &status);
            CheckStatus(status, status != 0);

            string ret = UTF8String.ReadUTF8String(buf);
            m_cscore.CS_FreeString(buf);
            return ret;
        }

        public static unsafe ulong GetSourceLastFrameTime(CS_Source source)
        {
            int status = 0;
            var ret = m_cscore.CS_GetSourceLastFrameTime(source, &status);
            CheckStatus(status, status != 0);
            return ret;
        }

        public static unsafe void SetSourceConnectionStrategy(CS_Source source, ConnectionStrategy strategy)
        {
            int status = 0;
            m_cscore.CS_SetSourceConnectionStrategy(source, strategy, &status);
            CheckStatus(status, status != 0);
        }

        public static unsafe bool IsSourceConnected(CS_Source source)
        {
            int status = 0;
            var ret = m_cscore.CS_IsSourceConnected(source, &status);
            CheckStatus(status, status != 0);
            return ret.Get();
        }

        public static unsafe bool IsSourceEnabled(CS_Source source)
        {
            int status = 0;
            var ret = m_cscore.CS_IsSourceEnabled(source, &status);
            CheckStatus(status, status != 0);
            return ret.Get();
        }

        public static unsafe CS_Property GetSourceProperty(CS_Source source, ReadOnlySpan<char> name)
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
                    int status = 0;
                    var ret = m_cscore.CS_GetSourceProperty(source, d, &status);
                    CheckStatus(status, status != 0);
                    return ret;
                }
            }
        }

        public static unsafe CS_Property[] CS_EnumerateSourceProperties(CS_Source source)
        {
            int status = 0;
            int count = 0;
            CS_Property* values = m_cscore.CS_EnumerateSourceProperties(source, &count, &status);
            CheckStatus(status, status != 0);
            CS_Property[] toRet = new CS_Property[count];
            for (int i = 0; i < toRet.Length; i++)
            {
                toRet[i] = values[i];
            }
            m_cscore.CS_FreeEnumeratedProperties(values, count);
            return toRet;
        }

        public static unsafe void GetSourceVideoMode(CS_Source source, out CS_VideoMode videoMode)
        {
            int status = 0;
            CS_VideoMode mode;
            m_cscore.CS_GetSourceVideoMode(source, &mode, &status);
            CheckStatus(status, status != 0);
            videoMode = mode;
        }

        public static unsafe bool SetSourceVideoMode(CS_Source source, CS_VideoMode videoMode)
        {
            int status = 0;
            var ret = m_cscore.CS_SetSourceVideoMode(source, &videoMode, &status);
            CheckStatus(status, status != 0);
            return ret.Get();
        }

        public static unsafe bool SetSourceVideoModeDiscrete(CS_Source source, PixelFormat pixelFormat, int width, int height, int fps)
        {
            int status = 0;
            var ret = m_cscore.CS_SetSourceVideoModeDiscrete(source, pixelFormat, width, height, fps, &status);
            CheckStatus(status, status != 0);
            return ret.Get();
        }

        public static unsafe bool SetSourcePixelFormat(CS_Source source, PixelFormat pixelFormat)
        {
            int status = 0;
            var ret = m_cscore.CS_SetSourcePixelFormat(source, pixelFormat, &status);
            CheckStatus(status, status != 0);
            return ret.Get();
        }
        public static unsafe bool SetSourceResolution(CS_Source source, int width, int height)
        {
            int status = 0;
            var ret = m_cscore.CS_SetSourceResolution(source, width, height, &status);
            CheckStatus(status, status != 0);
            return ret.Get();
        }
        public static unsafe bool SetSourceFPS(CS_Source source, int fps)
        {
            int status = 0;
            var ret = m_cscore.CS_SetSourceFPS(source, fps, &status);
            CheckStatus(status, status != 0);
            return ret.Get();
        }

        public static unsafe bool SetSourceConfigJson(CS_Source source, ReadOnlySpan<char> config)
        {
            if (config.IsEmpty)
            {
                throw new ArgumentOutOfRangeException(nameof(config), "Cannot have an empty config");
            }

            fixed (char* p = config)
            {
                var dLen = Encoding.UTF8.GetByteCount(p, config.Length);
                Span<byte> dSpan = dLen <= 256 ? stackalloc byte[dLen == 0 ? 1 : dLen] : new byte[dLen];
                fixed (byte* d = dSpan)
                {
                    Encoding.UTF8.GetBytes(p, config.Length, d, dLen);
                    int status = 0;
                    var ret = m_cscore.CS_SetSourceConfigJson(source, d, &status);
                    CheckStatus(status, status != 0);
                    return ret.Get();
                }
            }
        }

        public static unsafe string GetSourceConfigJson(CS_Source source)
        {
            int status = 0;
            var buf = m_cscore.CS_GetSourceConfigJson(source, &status);
            CheckStatus(status, status != 0);

            string ret = UTF8String.ReadUTF8String(buf);
            m_cscore.CS_FreeString(buf);
            return ret;
        }

        public static unsafe CS_VideoMode[] EnumerateSourceVideoModes(CS_Source source)
        {
            int status = 0;
            int count = 0;
            CS_VideoMode* values = m_cscore.CS_EnumerateSourceVideoModes(source, &count, &status);
            CheckStatus(status, status != 0);
            CS_VideoMode[] toRet = new CS_VideoMode[count];
            for (int i = 0; i < toRet.Length; i++)
            {
                toRet[i] = values[i];
            }
            m_cscore.CS_FreeEnumeratedVideoModes(values, count);
            return toRet;
        }

        public static unsafe CS_Sink[] EnumerateSourceSinks(CS_Source source)
        {
            int status = 0;
            int count = 0;
            CS_Sink* values = m_cscore.CS_EnumerateSourceSinks(source, &count, &status);
            CheckStatus(status, status != 0);
            CS_Sink[] toRet = new CS_Sink[count];
            for (int i = 0; i < toRet.Length; i++)
            {
                toRet[i] = values[i];
            }
            m_cscore.CS_ReleaseEnumeratedSinks(values, count);
            return toRet;
        }

        public static unsafe CS_Source CopySource(CS_Source source)
        {
            int status = 0;
            var ret = m_cscore.CS_CopySource(source, &status);
            CheckStatus(status, status != 0);
            return ret;
        }

        public static unsafe void ReleaseSource(CS_Source source)
        {
            int status = 0;
            m_cscore.CS_ReleaseSource(source, &status);
            CheckStatus(status, status != 0);
        }

        public static unsafe void SetCameraBrightness(CS_Source source, int brightness)
        {
            int status = 0;
            m_cscore.CS_SetCameraBrightness(source, brightness, &status);
            CheckStatus(status, status != 0);
        }

        public static unsafe int GetCameraBrightness(CS_Source source)
        {
            int status = 0;
            var ret = m_cscore.CS_GetCameraBrightness(source, &status);
            CheckStatus(status, status != 0);
            return ret;
        }

        public static unsafe void SetCameraWhiteBalanceAuto(CS_Source source)
        {
            int status = 0;
            m_cscore.CS_SetCameraWhiteBalanceAuto(source, &status);
            CheckStatus(status, status != 0);
        }

        public static unsafe void SetCameraWhiteBalanceHoldCurrent(CS_Source source)
        {
            int status = 0;
            m_cscore.CS_SetCameraWhiteBalanceHoldCurrent(source, &status);
            CheckStatus(status, status != 0);
        }

        public static unsafe void SetCameraWhiteBalanceManual(CS_Source source, int value)
        {
            int status = 0;
            m_cscore.CS_SetCameraWhiteBalanceManual(source, value, &status);
            CheckStatus(status, status != 0);
        }

        public static unsafe void SetCameraExposureAuto(CS_Source source)
        {
            int status = 0;
            m_cscore.CS_SetCameraExposureAuto(source, &status);
            CheckStatus(status, status != 0);
        }

        public static unsafe void SetCameraExposureHoldCurrent(CS_Source source)
        {
            int status = 0;
            m_cscore.CS_SetCameraExposureHoldCurrent(source, &status);
            CheckStatus(status, status != 0);
        }

        public static unsafe void SetCameraExposureManual(CS_Source source, int value)
        {
            int status = 0;
            m_cscore.CS_SetCameraExposureManual(source, value, &status);
            CheckStatus(status, status != 0);
        }

        public static unsafe string GetUsbCameraPath(CS_Source source)
        {
            int status = 0;
            var buf = m_cscore.CS_GetUsbCameraPath(source, &status);
            CheckStatus(status, status != 0);

            string ret = UTF8String.ReadUTF8String(buf);
            m_cscore.CS_FreeString(buf);
            return ret;
        }

        public static unsafe HttpCameraKind GetHttpCameraKind(CS_Source source)
        {
            int status = 0;
            var ret = m_cscore.CS_GetHttpCameraKind(source, &status);
            CheckStatus(status, status != 0);
            return ret;
        }

        
    }
}
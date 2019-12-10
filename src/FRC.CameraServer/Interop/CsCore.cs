using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private readonly static ICsCore m_cscore;

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
                        var cscore2 = nativeLoader.LoadNativeInterface<ICsCore>();
                        m_cscore = cscore2 ?? throw new Exception("Failed to load native interface?");
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
            var cscore = nativeLoader.LoadNativeInterface<ICsCore>();
            m_cscore = cscore ?? throw new Exception("Failed to load native interface?");
        }
        
        private static void ThrowStatusException(int status)
        {
            StatusValue s = (StatusValue)status;
           
            var val = s switch
            {
                StatusValue.PropertyWriteFailed => "property write failed",
                StatusValue.InvalidHandle => "invalid handle",
                StatusValue.WrongHandleSubtype => "wrong handle subtype",
                StatusValue.InvalidProperty => "invalid property",
                StatusValue.WrongPropertyType => "wrong property type",
                StatusValue.EmptyValue => "empty value",
                StatusValue.BadUrl => "bad URL",
                StatusValue.PropertyReadFailed => "read failed",
                StatusValue.SourceIsDisconnected => "source is disconnected",
                _ => s.ToString(),
            };
            string msg = $"unknown error code={val}";
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
            CheckStatus(status, status == 0);

            return retVal;
        }

        public static unsafe string GetPropertyName(CS_Property property)
        {
            int status = 0;
            var buf = m_cscore.CS_GetPropertyName(property, &status);
            CheckStatus(status, status == 0);

            string ret = UTF8String.ReadUTF8String(buf);
            m_cscore.CS_FreeString(buf);
            return ret;

        }

        public static unsafe int GetProperty(CS_Property property)
        {
            int status = 0;
            var ret = m_cscore.CS_GetProperty(property, &status);
            CheckStatus(status, status == 0);
            return ret;
        }

        public static unsafe int GetPropertyMin(CS_Property property)
        {
            int status = 0;
            var ret = m_cscore.CS_GetPropertyMin(property, &status);
            CheckStatus(status, status == 0);
            return ret;
        }

        public static unsafe int GetPropertyMax(CS_Property property)
        {
            int status = 0;
            var ret = m_cscore.CS_GetPropertyMax(property, &status);
            CheckStatus(status, status == 0);
            return ret;
        }

        public static unsafe int GetPropertyStep(CS_Property property)
        {
            int status = 0;
            var ret = m_cscore.CS_GetPropertyStep(property, &status);
            CheckStatus(status, status == 0);
            return ret;
        }

        public static unsafe int GetPropertyDefault(CS_Property property)
        {
            int status = 0;
            var ret = m_cscore.CS_GetPropertyDefault(property, &status);
            CheckStatus(status, status == 0);
            return ret;
        }

        public static unsafe string GetStringProperty(CS_Property property)
        {
            int status = 0;
            var buf = m_cscore.CS_GetStringProperty(property, &status);
            CheckStatus(status, status == 0);

            string ret = UTF8String.ReadUTF8String(buf);
            m_cscore.CS_FreeString(buf);
            return ret;

        }

        public static unsafe void SetProperty(CS_Property property, int value)
        {
            int status = 0;
            m_cscore.CS_SetProperty(property, value, &status);
            CheckStatus(status, status == 0);
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
                    CheckStatus(status, status == 0);
                }
            }
        }

        public static unsafe string[] GetEnumPropertyChoices(CS_Property property)
        {
            int status = 0;
            int count = 0;
            byte** values = m_cscore.CS_GetEnumPropertyChoices(property, &count, &status);
            CheckStatus(status, status == 0);
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
                    CheckStatus(status, status == 0);
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
                    CheckStatus(status, status == 0);
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
                    CheckStatus(status, status == 0);
                    return ret;
                }
            }
        }

        public static unsafe ulong GrabRawSinkFrame(CS_Sink sink, ref CS_RawFrame rawImage)
        {
            int status = 0;
            CS_RawFrame localFrame = rawImage;
            var ret = m_cscore.CS_GrabRawSinkFrame(sink, &localFrame, &status);
            CheckStatus(status, status == 0);
            rawImage = localFrame;
            return ret;
        }

        public static unsafe ulong GrabRawSinkFrameTimeout(CS_Sink sink, ref CS_RawFrame rawImage, double timeout)
        {
            int status = 0;
            CS_RawFrame localFrame = rawImage;
            var ret = m_cscore.CS_GrabRawSinkFrameTimeout(sink, &localFrame, timeout, &status);
            CheckStatus(status, status == 0);
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
                    CheckStatus(status, status == 0);
                    return ret;
                }
            }
        }

        public static unsafe CS_Source CreateRawSource(ReadOnlySpan<char> name, VideoMode mode)
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
                    CheckStatus(status, status == 0);
                    return ret;
                }
            }
        }

        public static unsafe void PutRawSourceFrame(CS_Source source, CS_RawFrame rawFrame)
        {
            int status = 0;
            m_cscore.CS_PutRawSourceFrame(source, &rawFrame, &status);
            CheckStatus(status, status == 0);
        }

        public static unsafe SourceKind GetSourceKind(CS_Source source)
        {
            int status = 0;
            var ret = m_cscore.CS_GetSourceKind(source, &status);
            CheckStatus(status, status == 0);
            return ret;
        }

        public static unsafe string GetSourceName(CS_Source source)
        {
            int status = 0;
            var buf = m_cscore.CS_GetSourceName(source, &status);
            CheckStatus(status, status == 0);

            string ret = UTF8String.ReadUTF8String(buf);
            m_cscore.CS_FreeString(buf);
            return ret;
        }

        public static unsafe string GetSourceDescription(CS_Source source)
        {
            int status = 0;
            var buf = m_cscore.CS_GetSourceDescription(source, &status);
            CheckStatus(status, status == 0);

            string ret = UTF8String.ReadUTF8String(buf);
            m_cscore.CS_FreeString(buf);
            return ret;
        }

        public static unsafe ulong GetSourceLastFrameTime(CS_Source source)
        {
            int status = 0;
            var ret = m_cscore.CS_GetSourceLastFrameTime(source, &status);
            CheckStatus(status, status == 0);
            return ret;
        }

        public static unsafe void SetSourceConnectionStrategy(CS_Source source, ConnectionStrategy strategy)
        {
            int status = 0;
            m_cscore.CS_SetSourceConnectionStrategy(source, strategy, &status);
            CheckStatus(status, status == 0);
        }

        public static unsafe bool IsSourceConnected(CS_Source source)
        {
            int status = 0;
            var ret = m_cscore.CS_IsSourceConnected(source, &status);
            CheckStatus(status, status == 0);
            return ret.Get();
        }

        public static unsafe bool IsSourceEnabled(CS_Source source)
        {
            int status = 0;
            var ret = m_cscore.CS_IsSourceEnabled(source, &status);
            CheckStatus(status, status == 0);
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
                    CheckStatus(status, status == 0);
                    return ret;
                }
            }
        }

        public static unsafe CS_Property[] EnumerateSourceProperties(CS_Source source)
        {
            int status = 0;
            int count = 0;
            CS_Property* values = m_cscore.CS_EnumerateSourceProperties(source, &count, &status);
            CheckStatus(status, status == 0);
            CS_Property[] toRet = new CS_Property[count];
            for (int i = 0; i < toRet.Length; i++)
            {
                toRet[i] = values[i];
            }
            m_cscore.CS_FreeEnumeratedProperties(values, count);
            return toRet;
        }

        public static unsafe VideoMode GetSourceVideoMode(CS_Source source)
        {
            int status = 0;
            VideoMode mode;
            m_cscore.CS_GetSourceVideoMode(source, &mode, &status);
            CheckStatus(status, status == 0);
            return mode;
        }

        public static unsafe bool SetSourceVideoMode(CS_Source source, VideoMode videoMode)
        {
            int status = 0;
            var ret = m_cscore.CS_SetSourceVideoMode(source, &videoMode, &status);
            CheckStatus(status, status == 0);
            return ret.Get();
        }

        public static unsafe bool SetSourceVideoModeDiscrete(CS_Source source, PixelFormat pixelFormat, int width, int height, int fps)
        {
            int status = 0;
            var ret = m_cscore.CS_SetSourceVideoModeDiscrete(source, pixelFormat, width, height, fps, &status);
            CheckStatus(status, status == 0);
            return ret.Get();
        }

        public static unsafe bool SetSourcePixelFormat(CS_Source source, PixelFormat pixelFormat)
        {
            int status = 0;
            var ret = m_cscore.CS_SetSourcePixelFormat(source, pixelFormat, &status);
            CheckStatus(status, status == 0);
            return ret.Get();
        }
        public static unsafe bool SetSourceResolution(CS_Source source, int width, int height)
        {
            int status = 0;
            var ret = m_cscore.CS_SetSourceResolution(source, width, height, &status);
            CheckStatus(status, status == 0);
            return ret.Get();
        }
        public static unsafe bool SetSourceFPS(CS_Source source, int fps)
        {
            int status = 0;
            var ret = m_cscore.CS_SetSourceFPS(source, fps, &status);
            CheckStatus(status, status == 0);
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
                    CheckStatus(status, status == 0);
                    return ret.Get();
                }
            }
        }

        public static unsafe string GetSourceConfigJson(CS_Source source)
        {
            int status = 0;
            var buf = m_cscore.CS_GetSourceConfigJson(source, &status);
            CheckStatus(status, status == 0);

            string ret = UTF8String.ReadUTF8String(buf);
            m_cscore.CS_FreeString(buf);
            return ret;
        }

        public static unsafe VideoMode[] EnumerateSourceVideoModes(CS_Source source)
        {
            int status = 0;
            int count = 0;
            VideoMode* values = m_cscore.CS_EnumerateSourceVideoModes(source, &count, &status);
            CheckStatus(status, status == 0);
            VideoMode[] toRet = new VideoMode[count];
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
            CheckStatus(status, status == 0);
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
            CheckStatus(status, status == 0);
            return ret;
        }

        public static unsafe void ReleaseSource(CS_Source source)
        {
            int status = 0;
            m_cscore.CS_ReleaseSource(source, &status);
            CheckStatus(status, status == 0);
        }

        public static unsafe void SetCameraBrightness(CS_Source source, int brightness)
        {
            int status = 0;
            m_cscore.CS_SetCameraBrightness(source, brightness, &status);
            CheckStatus(status, status == 0);
        }

        public static unsafe int GetCameraBrightness(CS_Source source)
        {
            int status = 0;
            var ret = m_cscore.CS_GetCameraBrightness(source, &status);
            CheckStatus(status, status == 0);
            return ret;
        }

        public static unsafe void SetCameraWhiteBalanceAuto(CS_Source source)
        {
            int status = 0;
            m_cscore.CS_SetCameraWhiteBalanceAuto(source, &status);
            CheckStatus(status, status == 0);
        }

        public static unsafe void SetCameraWhiteBalanceHoldCurrent(CS_Source source)
        {
            int status = 0;
            m_cscore.CS_SetCameraWhiteBalanceHoldCurrent(source, &status);
            CheckStatus(status, status == 0);
        }

        public static unsafe void SetCameraWhiteBalanceManual(CS_Source source, int value)
        {
            int status = 0;
            m_cscore.CS_SetCameraWhiteBalanceManual(source, value, &status);
            CheckStatus(status, status == 0);
        }

        public static unsafe void SetCameraExposureAuto(CS_Source source)
        {
            int status = 0;
            m_cscore.CS_SetCameraExposureAuto(source, &status);
            CheckStatus(status, status == 0);
        }

        public static unsafe void SetCameraExposureHoldCurrent(CS_Source source)
        {
            int status = 0;
            m_cscore.CS_SetCameraExposureHoldCurrent(source, &status);
            CheckStatus(status, status == 0);
        }

        public static unsafe void SetCameraExposureManual(CS_Source source, int value)
        {
            int status = 0;
            m_cscore.CS_SetCameraExposureManual(source, value, &status);
            CheckStatus(status, status == 0);
        }

        public static unsafe string GetUsbCameraPath(CS_Source source)
        {
            int status = 0;
            var buf = m_cscore.CS_GetUsbCameraPath(source, &status);
            CheckStatus(status, status == 0);

            string ret = UTF8String.ReadUTF8String(buf);
            m_cscore.CS_FreeString(buf);
            return ret;
        }

        public static unsafe UsbCameraInfo GetUsbCameraInfo(CS_Source source)
        {
            int status = 0;
            var buf = m_cscore.CS_GetUsbCameraInfo(source, &status);
            CheckStatus(status, status == 0);

            UsbCameraInfo toRet = new UsbCameraInfo(buf);
            m_cscore.CS_FreeUsbCameraInfo(buf);
            return toRet;
        }

        public static unsafe HttpCameraKind GetHttpCameraKind(CS_Source source)
        {
            int status = 0;
            var ret = m_cscore.CS_GetHttpCameraKind(source, &status);
            CheckStatus(status, status == 0);
            return ret;
        }

        public static unsafe SinkKind GetSinkKind(CS_Sink sink)
        {
            int status = 0;
            var ret = m_cscore.CS_GetSinkKind(sink, &status);
            CheckStatus(status, status == 0);
            return ret;
        }

        public static unsafe string GetSinkName(CS_Sink sink)
        {
            int status = 0;
            var buf = m_cscore.CS_GetSinkName(sink, &status);
            CheckStatus(status, status == 0);

            string ret = UTF8String.ReadUTF8String(buf);
            m_cscore.CS_FreeString(buf);
            return ret;
        }

        public static unsafe string GetSinkDescription(CS_Sink sink)
        {
            int status = 0;
            var buf = m_cscore.CS_GetSinkDescription(sink, &status);
            CheckStatus(status, status == 0);

            string ret = UTF8String.ReadUTF8String(buf);
            m_cscore.CS_FreeString(buf);
            return ret;
        }

        public static unsafe CS_Source GetSinkSource(CS_Sink sink)
        {
            int status = 0;
            var ret = m_cscore.CS_GetSinkSource(sink, &status);
            CheckStatus(status, status == 0);
            return ret;
        }

        public static unsafe void SetSinkSource(CS_Sink sink, CS_Source source)
        {
            int status = 0;
            m_cscore.CS_SetSinkSource(sink, source,&status);
            CheckStatus(status, status == 0);
        }

        public static unsafe CS_Property GetSinkSourceProperty(CS_Sink sink, ReadOnlySpan<char> name)
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
                    var ret = m_cscore.CS_GetSinkSourceProperty(sink, d, &status);
                    CheckStatus(status, status == 0);
                    return ret;
                }
            }
        }

        public static unsafe bool SetSinkConfigJson(CS_Sink source, ReadOnlySpan<char> config)
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
                    var ret = m_cscore.CS_SetSinkConfigJson(source, d, &status);
                    CheckStatus(status, status == 0);
                    return ret.Get();
                }
            }
        }

        public static unsafe string GetSinkConfigJson(CS_Sink source)
        {
            int status = 0;
            var buf = m_cscore.CS_GetSinkConfigJson(source, &status);
            CheckStatus(status, status == 0);

            string ret = UTF8String.ReadUTF8String(buf);
            m_cscore.CS_FreeString(buf);
            return ret;
        }


        public static unsafe CS_Property GetSinkProperty(CS_Sink sink, ReadOnlySpan<char> name)
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
                    var ret = m_cscore.CS_GetSinkProperty(sink, d, &status);
                    CheckStatus(status, status == 0);
                    return ret;
                }
            }
        }

        public static unsafe void ReleaseSink(CS_Sink sink)
        {
            int status = 0;
            m_cscore.CS_ReleaseSink(sink, &status);
            CheckStatus(status, status == 0);
        }


        public static unsafe CS_Source[] EnumerateSources()
        {
            int status = 0;
            int count = 0;
            CS_Source* values = m_cscore.CS_EnumerateSources(&count, &status);
            CheckStatus(status, status == 0);
            CS_Source[] toRet = new CS_Source[count];
            for (int i = 0; i < toRet.Length; i++)
            {
                toRet[i] = values[i];
            }
            m_cscore.CS_ReleaseEnumeratedSources(values, count);
            return toRet;
        }

        public static unsafe CS_Sink[] EnumerateSinks()
        {
            int status = 0;
            int count = 0;
            CS_Sink* values = m_cscore.CS_EnumerateSinks(&count, &status);
            CheckStatus(status, status == 0);
            CS_Sink[] toRet = new CS_Sink[count];
            for (int i = 0; i < toRet.Length; i++)
            {
                toRet[i] = values[i];
            }
            m_cscore.CS_ReleaseEnumeratedSinks(values, count);
            return toRet;
        }

        private static readonly Dictionary<CS_Listener, CsListenerEvent> listenerMap = new Dictionary<CS_Listener, CsListenerEvent>();
        private static readonly object lockObject = new object();

        public static unsafe CS_Listener AddListener(VideoEventDelegate videoEvent, EventKind mask, bool immediateNotify)
        {
            void listenerEvent(void* data, CS_Event* csEvent)
            {
                videoEvent(new RefVideoEvent(csEvent));
            }
            IntPtr listenerEventPtr = Marshal.GetFunctionPointerForDelegate((CsListenerEvent)listenerEvent);
            int status = 0;
            var listener = m_cscore.CS_AddListener(null, listenerEventPtr, (int)mask, immediateNotify ? 1 : 0, &status);
            CheckStatus(status, status == 0);
            lock (lockObject)
            {
                listenerMap.Add(listener, listenerEvent);
            }
            return listener;
        }

        public static unsafe void RemoveListener(CS_Listener listener)
        {
            int status = 0;
            m_cscore.CS_RemoveListener(listener, &status);
            lock (lockObject)
            {
                listenerMap.Remove(listener);
            }
            CheckStatus(status, status == 0);
        }

        public static unsafe void SetTelemetryPeriod(double seconds)
        {
            m_cscore.CS_SetTelemetryPeriod(seconds);
        }

        public static unsafe double GetTelemetryElapsedTime()
        {
            return m_cscore.CS_GetTelemetryElapsedTime();
        }

        public static unsafe long GetTelemetryValue(CS_Handle handle, TelemetryKind kind)
        {
            int status = 0;
            var ret = m_cscore.CS_GetTelemetryValue(handle, kind, &status);
            CheckStatus(status, status == 0);
            return ret;
        }

        public static unsafe double GetTelemetryAverageValue(CS_Handle handle, TelemetryKind kind)
        {
            int status = 0;
            var ret = m_cscore.CS_GetTelemetryAverageValue(handle, kind, &status);
            CheckStatus(status, status == 0);
            return ret;
        }

        public static unsafe UsbCameraInfo[] EnumerateUsbCameras()
        {
            int status = 0;
            int count = 0;
            var ret = m_cscore.CS_EnumerateUsbCameras(&count, &status);
            CheckStatus(status, status == 0);

            UsbCameraInfo[] cameras = new UsbCameraInfo[count];
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i] = new UsbCameraInfo(ret + i);
            }

            m_cscore.CS_FreeEnumeratedUsbCameras(ret, count);
            return cameras;
        }

        public static unsafe CS_Sink CreateMjpegServer(ReadOnlySpan<char> name, ReadOnlySpan<char> listenAddress, int port)
        {
            if (name.IsEmpty)
            {
                throw new ArgumentOutOfRangeException(nameof(name), "Cannot have an empty name");
            }

            fixed (char* n = name)
            fixed (char* p = listenAddress.IsEmpty ? new ReadOnlySpan<char>(NullTerminator, 1) : listenAddress)
            {
                var nLen = Encoding.UTF8.GetByteCount(n, name.Length);
                Span<byte> dSpan = nLen <= 256 ? stackalloc byte[nLen] : new byte[nLen];
                var pLen = Encoding.UTF8.GetByteCount(p, listenAddress.Length);
                Span<byte> pSpan = pLen <= 256 ? stackalloc byte[pLen == 0 ? 1 : pLen] : new byte[pLen];
                fixed (byte* d = dSpan)
                fixed (byte* pp = pSpan)
                {
                    Encoding.UTF8.GetBytes(n, name.Length, d, nLen);
                    Encoding.UTF8.GetBytes(p, listenAddress.Length, pp, pLen);
                    int status = 0;
                    var ret = m_cscore.CS_CreateMjpegServer(d, pp, port, &status);
                    CheckStatus(status, status == 0);
                    return ret;
                }
            }
        }

        public static unsafe string GetMjpegServerListenAddress(CS_Sink sink)
        {
            int status = 0;
            var ret = m_cscore.CS_GetMjpegServerListenAddress(sink, &status);
            CheckStatus(status, status == 0);
            var retVal = UTF8String.ReadUTF8String(ret);
            m_cscore.CS_FreeString(ret);
            return retVal;
        }

        public static unsafe int GetMjpegServerPort(CS_Sink sink)
        {
            int status = 0;
            var ret = m_cscore.CS_GetMjpegServerPort(sink, &status);
            CheckStatus(status, status == 0);
            return ret;
        }

        public static unsafe void SetSourceDescription(CS_Source source, ReadOnlySpan<char> description)
        {
            if (description.IsEmpty)
            {
                throw new ArgumentOutOfRangeException(nameof(description), "Cannot have an empty name");
            }

            fixed (char* p = description)
            {
                var dLen = Encoding.UTF8.GetByteCount(p, description.Length);
                Span<byte> dSpan = dLen <= 256 ? stackalloc byte[dLen == 0 ? 1 : dLen] : new byte[dLen];
                fixed (byte* d = dSpan)
                {
                    Encoding.UTF8.GetBytes(p, description.Length, d, dLen);
                    int status = 0;
                    m_cscore.CS_SetSourceDescription(source, d, &status);
                    CheckStatus(status, status == 0);
                }
            }
        }

        public static unsafe void SetSourceConnected(CS_Source source, bool connected)
        {
            int status = 0;
            m_cscore.CS_SetSourceConnected(source, connected, &status);
            CheckStatus(status, status == 0);
        }

        public static unsafe void NotifySourceError(CS_Source source, ReadOnlySpan<char> description)
        {
            if (description.IsEmpty)
            {
                throw new ArgumentOutOfRangeException(nameof(description), "Cannot have an empty name");
            }

            fixed (char* p = description)
            {
                var dLen = Encoding.UTF8.GetByteCount(p, description.Length);
                Span<byte> dSpan = dLen <= 256 ? stackalloc byte[dLen == 0 ? 1 : dLen] : new byte[dLen];
                fixed (byte* d = dSpan)
                {
                    Encoding.UTF8.GetBytes(p, description.Length, d, dLen);
                    int status = 0;
                    m_cscore.CS_NotifySourceError(source, d, &status);
                    CheckStatus(status, status == 0);
                }
            }
        }

        public static unsafe CS_Property CreateSourceProperty(CS_Source source, ReadOnlySpan<char> name,
                                      PropertyKind kind,
                                      int minimum,
                                      int maximum,
                                      int step,
                                      int defaultValue,
                                      int value)
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
                    var ret = m_cscore.CS_CreateSourceProperty(source, d, kind, minimum, maximum, step, defaultValue, value, &status);
                    CheckStatus(status, status == 0);
                    return ret;
                }
            }
        }

        public static unsafe void SetSourceEnumPropertyChoices(CS_Source source, CS_Property property, params string[] choices)
        {
            if (choices.Length == 0) return;

            byte** strings = (byte**)Marshal.AllocHGlobal(choices.Length * sizeof(byte));
            DisposableNativeString[] disposableStrings = new DisposableNativeString[choices.Length];

            for (int i = 0; i < choices.Length; i++)
            {
                disposableStrings[i] = UTF8String.CreateUTF8DisposableString(choices[i]);
                strings[i] = disposableStrings[i].Buffer;
            }

            int status = 0;
            m_cscore.CS_SetSourceEnumPropertyChoices(source, property, strings, choices.Length, &status);
            for (int i = 0; i < choices.Length; i++)
            {
                disposableStrings[i].Dispose();
            }
            CheckStatus(status, status == 0);
        }

        public static unsafe void SetSinkDescription(CS_Sink sink, ReadOnlySpan<char> description)
        {
            if (description.IsEmpty)
            {
                throw new ArgumentOutOfRangeException(nameof(description), "Cannot have an empty name");
            }

            fixed (char* p = description)
            {
                var dLen = Encoding.UTF8.GetByteCount(p, description.Length);
                Span<byte> dSpan = dLen <= 256 ? stackalloc byte[dLen == 0 ? 1 : dLen] : new byte[dLen];
                fixed (byte* d = dSpan)
                {
                    Encoding.UTF8.GetBytes(p, description.Length, d, dLen);
                    int status = 0;
                    m_cscore.CS_SetSinkDescription(sink, d, &status);
                    CheckStatus(status, status == 0);
                }
            }
        }

        public static unsafe string GetSinkError(CS_Sink sink)
        {
            int status = 0;
            var ret = m_cscore.CS_GetSinkError(sink, &status);
            CheckStatus(status, status == 0);
            var retVal = UTF8String.ReadUTF8String(ret);
            m_cscore.CS_FreeString(ret);
            return retVal;
        }

        public static unsafe void SetSinkEnabled(CS_Sink sink, bool connected)
        {
            int status = 0;
            m_cscore.CS_SetSinkEnabled(sink, connected, &status);
            CheckStatus(status, status == 0);
        }
    }


}
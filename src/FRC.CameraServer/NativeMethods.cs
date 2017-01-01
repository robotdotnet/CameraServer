using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using CSCore.Native;

namespace CSCore
{
    /// <summary>
    /// This delegate is use to specify the log function called back from the library
    /// </summary>
    /// <param name="level">The level of the current log</param>
    /// <param name="file">The file the log was called from</param>
    /// <param name="line">The line the log was called from</param>
    /// <param name="msg">The message of the log</param>
    public delegate void LogFunc(LogLevel level, string file, int line, string msg);

    public static class NativeMethods
    {
        private static bool CheckStatus(int status)
        {
            if (status != 0)
            {
                StatusValue s = (StatusValue)status;
                string msg = null;
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
                    default:
                        {
                            msg = $"unknown error code={status}";
                            break;
                        }
                }
                throw new VideoException(msg);
            }
            return status == 0;
        }

        public static int CreateCvSink(string name)
        {
            UIntPtr size;
            byte[] str = Interop.CreateUTF8String(name, out size);
            int status = 0;
            int ret = Interop.CS_CreateCvSink(str, ref status);
            CheckStatus(status);
            return ret;
        }

        public static void SetSinkDescription(int handle, string description)
        {
            UIntPtr size;
            byte[] str = Interop.CreateUTF8String(description, out size);
            int status = 0;
            Interop.CS_SetSinkDescription(handle, str, ref status);
            CheckStatus(status);
        }

        public static ulong GrabSinkFrame(int handle, IntPtr nativeObj)
        {
            int status = 0;
            ulong ret = Interop.CS_GrabSinkFrameCpp(handle, nativeObj, ref status);
            CheckStatus(status);
            return ret;
        }

        public static string GetSinkError(int handle)
        {
            int status = 0;
            IntPtr ret = Interop.CS_GetSinkError(handle, ref status);
            CheckStatus(status);
            string sRet = Interop.ReadUTF8String(ret);
            Interop.CS_FreeString(ret);
            return sRet;
        }

        public static void SetSinkEnabled(int handle, bool enabled)
        {
            int status = 0;
            Interop.CS_SetSinkEnabled(handle, enabled, ref status);
            CheckStatus(status);
        }

        public static void ReleaseSink(int handle)
        {
            int status = 0;
            Interop.CS_ReleaseSink(handle, ref status);
            CheckStatus(status);
        }

        public static SinkKind GetSinkKind(int handle)
        {
            int status = 0;
            SinkKind ret = Interop.CS_GetSinkKind(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static string GetSinkName(int handle)
        {
            int status = 0;
            IntPtr ret = Interop.CS_GetSinkName(handle, ref status);
            CheckStatus(status);
            string sRet = Interop.ReadUTF8String(ret);
            Interop.CS_FreeString(ret);
            return sRet;
        }

        public static string GetSinkDescription(int handle)
        {
            int status = 0;
            IntPtr ret = Interop.CS_GetSinkDescription(handle, ref status);
            CheckStatus(status);
            string sRet = Interop.ReadUTF8String(ret);
            Interop.CS_FreeString(ret);
            return sRet;
        }

        public static void SetSinkSource(int sinkHandle, int sourceHandle)
        {
            int status = 0;
            Interop.CS_SetSinkSource(sinkHandle, sourceHandle, ref status);
            CheckStatus(status);
        }

        public static int GetSinkSource(int handle)
        {
            int status = 0;
            int ret = Interop.CS_GetSinkSource(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int GetSinkSourceProperty(int handle, string name)
        {
            UIntPtr size;
            byte[] str = Interop.CreateUTF8String(name, out size);
            int status = 0;
            int prop = Interop.CS_GetSinkSourceProperty(handle, str, ref status);
            CheckStatus(status);
            return prop;
        }

        public static List<int> EnumerateSinks()
        {
            int status = 0;
            int count = 0;
            IntPtr sinkArray = Interop.CS_EnumerateSinks(ref count, ref status);
            CheckStatus(status);
            List<int> sinks = new List<int>(count);

            for (int i = 0; i < count; i++)
            {
                int handle = Marshal.ReadInt32(sinkArray, i);
                sinks.Add(handle);
            }
            Interop.CS_ReleaseEnumeratedSinks(sinkArray, count);
            return sinks;
        }

        public static void ReleaseSource(int handle)
        {
            int status = 0;
            Interop.CS_ReleaseSource(handle, ref status);
            CheckStatus(status);
        }

        public static SourceKind GetSourceKind(int handle)
        {
            int status = 0;
            SourceKind ret = Interop.CS_GetSourceKind(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static string GetSourceName(int handle)
        {
            int status = 0;
            IntPtr ret = Interop.CS_GetSourceName(handle, ref status);
            CheckStatus(status);
            string sRet = Interop.ReadUTF8String(ret);
            Interop.CS_FreeString(ret);
            return sRet;
        }

        public static string GetSourceDescription(int handle)
        {
            int status = 0;
            IntPtr ret = Interop.CS_GetSourceDescription(handle, ref status);
            CheckStatus(status);
            string sRet = Interop.ReadUTF8String(ret);
            Interop.CS_FreeString(ret);
            return sRet;
        }

        public static ulong GetSourceLastFrameTime(int handle)
        {
            int status = 0;
            ulong ret = Interop.CS_GetSourceLastFrameTime(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static bool IsSourceConnected(int handle)
        {
            int status = 0;
            bool ret = Interop.CS_IsSourceConnected(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int GetSourceProperty(int handle, string name)
        {
            UIntPtr size;
            byte[] str = Interop.CreateUTF8String(name, out size);
            int status = 0;
            int prop = Interop.CS_GetSourceProperty(handle, str, ref status);
            CheckStatus(status);
            return prop;
        }

        public static List<int> EnumerateSourceProperties(int handle)
        {
            int status = 0;
            int count = 0;
            IntPtr propertyArray = Interop.CS_EnumerateSourceProperties(handle, ref count, ref status);
            CheckStatus(status);
            List<int> properties = new List<int>(count);

            for (int i = 0; i < count; i++)
            {
                int h = Marshal.ReadInt32(propertyArray, i);
                properties.Add(h);
            }
            Interop.CS_ReleaseEnumeratedSources(propertyArray, count);
            return properties;
        }

        public static VideoMode GetSourceVideoMode(int handle)
        {
            int status = 0;
            VideoMode mode = new VideoMode();
            Interop.CS_GetSourceVideoMode(handle, ref mode, ref status);
            CheckStatus(status);
            return mode;
        }

        public static bool SetSourceVideoMode(int handle, VideoMode mode)
        {
            int status = 0;
            bool ret = Interop.CS_SetSourceVideoModeDiscrete(handle, mode.PixelFormat, mode.Width, mode.Height, mode.FPS, ref status);
            CheckStatus(status);
            return ret;
        }

        public static bool SetSourceVideoMode(int handle, PixelFormat pixelFormat, int width, int height, int fps)
        {
            int status = 0;
            bool ret = Interop.CS_SetSourceVideoModeDiscrete(handle, pixelFormat, width, height, fps, ref status);
            CheckStatus(status);
            return ret;
        }

        public static bool SetSourcePixelFormat(int handle, PixelFormat format)
        {
            int status = 0;
            bool ret = Interop.CS_SetSourcePixelFormat(handle, format, ref status);
            CheckStatus(status);
            return ret;
        }

        public static bool SetSourceResolution(int handle, int width, int height)
        {
            int status = 0;
            bool ret = Interop.CS_SetSourceResolution(handle, width, height, ref status);
            CheckStatus(status);
            return ret;
        }

        public static bool SetSourceFPS(int handle, int fps)
        {
            int status = 0;
            bool ret = Interop.CS_SetSourceFPS(handle, fps, ref status);
            CheckStatus(status);
            return ret;
        }

        public static List<VideoMode> EnumerateSourceVideoModes(int handle)
        {
            int status = 0;
            int count = 0;
            #if !NETSTANDARD
            int modeSize = Marshal.SizeOf(typeof(VideoMode));
            #else
            int modeSize = Marshal.SizeOf<VideoMode>();
            #endif
            IntPtr modeArray = Interop.CS_EnumerateSourceVideoModes(handle, ref count, ref status);
            CheckStatus(status);
            List<VideoMode> modes = new List<VideoMode>(count);

            for (int i = 0; i < count; i++)
            {
                IntPtr data = new IntPtr(modeArray.ToInt64() + modeSize * i);
#pragma warning disable CS0618
                var con = (VideoMode)Marshal.PtrToStructure(data, typeof(VideoMode));
#pragma warning restore CS0618
                modes.Add(con);
            }
            Interop.CS_FreeEnumeratedVideoModes(modeArray, modeSize);
            return modes;
        }

        public static List<int> EnumerateSourceSinks(int handle)
        {
            int status = 0;
            int count = 0;
            IntPtr sinkArray = Interop.CS_EnumerateSourceSinks(handle, ref count, ref status);
            CheckStatus(status);
            List<int> sinks = new List<int>(count);

            for (int i = 0; i < count; i++)
            {
                int h = Marshal.ReadInt32(sinkArray, i);
                sinks.Add(h);
            }
            Interop.CS_ReleaseEnumeratedSinks(sinkArray, count);
            return sinks;
        }

        public static List<int> EnumerateSources()
        {
            int status = 0;
            int count = 0;
            IntPtr sourceArray = Interop.CS_EnumerateSources(ref count, ref status);
            CheckStatus(status);
            List<int> sources = new List<int>(count);

            for (int i = 0; i < count; i++)
            {
                int h = Marshal.ReadInt32(sourceArray, i);
                sources.Add(h);
            }
            Interop.CS_ReleaseEnumeratedSources(sourceArray, count);
            return sources;
        }

        public static string GetPropertyName(int handle)
        {
            int status = 0;
            IntPtr ret = Interop.CS_GetPropertyName(handle, ref status);
            CheckStatus(status);
            string sRet = Interop.ReadUTF8String(ret);
            Interop.CS_FreeString(ret);
            return sRet;
        }

        public static PropertyKind GetPropertyKind(int handle)
        {
            int status = 0;
            PropertyKind ret = Interop.CS_GetPropertyKind(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int GetProperty(int handle)
        {
            int status = 0;
            int ret = Interop.CS_GetProperty(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static void SetProperty(int handle, int value)
        {
            int status = 0;
            Interop.CS_SetProperty(handle, value, ref status);
            CheckStatus(status);
        }

        public static int GetPropertyMin(int handle)
        {
            int status = 0;
            int ret = Interop.CS_GetPropertyMin(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int GetPropertyMax(int handle)
        {
            int status = 0;
            int ret = Interop.CS_GetPropertyMax(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int GetPropertyDefault(int handle)
        {
            int status = 0;
            int ret = Interop.CS_GetPropertyDefault(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int GetPropertyStep(int handle)
        {
            int status = 0;
            int ret = Interop.CS_GetPropertyStep(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static string GetStringProperty(int handle)
        {
            int status = 0;
            IntPtr ret = Interop.CS_GetStringProperty(handle, ref status);
            CheckStatus(status);
            string sRet = Interop.ReadUTF8String(ret);
            Interop.CS_FreeString(ret);
            return sRet;
        }

        public static void SetStringProperty(int handle, string value)
        {
            UIntPtr size;
            byte[] str = Interop.CreateUTF8String(value, out size);
            int status = 0;
            Interop.CS_SetStringProperty(handle, str, ref status);
            CheckStatus(status);
        }

        public static List<string> GetEnumPropertyChoices(int handle)
        {
            int status = 0;
            int count = 0;
            #if !NETSTANDARD
            int ptrSize = Marshal.SizeOf(typeof(IntPtr));
            #else
            int ptrSize = Marshal.SizeOf<IntPtr>();
            #endif
            IntPtr choicesArray = Interop.CS_GetEnumPropertyChoices(handle, ref count, ref status);
            CheckStatus(status);
            List<string> choices = new List<string>(count);

            for (int i = 0; i < count; i++)
            {
                IntPtr h = new IntPtr(choicesArray.ToInt64() + ptrSize * i);
                choices.Add(Interop.ReadUTF8String(h));
            }
            Interop.CS_FreeEnumPropertyChoices(choicesArray, count);
            return choices;
        }

        public static int CreateCvSource(string name, PixelFormat pixelFormat, int width, int height, int fps)
        {
            UIntPtr size;
            byte[] str = Interop.CreateUTF8String(name, out size);
            VideoMode mode = new VideoMode(pixelFormat, width, height, fps);
            int status = 0;
            int ret = Interop.CS_CreateCvSource(str, ref mode, ref status);
            CheckStatus(status);
            return ret;
        }

        public static void PutSourceFrame(int handle, IntPtr nativeObj)
        {
            int status = 0;
            Interop.CS_PutSourceFrameCpp(handle, nativeObj, ref status);
            CheckStatus(status);
        }

        public static void NotifySourceError(int handle, string msg)
        {
            UIntPtr size;
            byte[] str = Interop.CreateUTF8String(msg, out size);
            int status = 0;
            Interop.CS_NotifySourceError(handle, str, ref status);
            CheckStatus(status);
        }

        public static void SetSourceConnected(int handle, bool connected)
        {
            int status = 0;
            Interop.CS_SetSourceConnected(handle, connected, ref status);
            CheckStatus(status);
        }

        public static void SetSourceDescription(int handle, string name)
        {
            UIntPtr size;
            byte[] str = Interop.CreateUTF8String(name, out size);
            int status = 0;
            Interop.CS_SetSourceDescription(handle, str, ref status);
            CheckStatus(status);
        }

        public static int CreateSourceProperty(int handle, string name, PropertyKind kind, int minimum, int maximum, int step, int defaultValue, int value)
        {
            UIntPtr size;
            byte[] str = Interop.CreateUTF8String(name, out size);
            int status = 0;
            int ret = Interop.CS_CreateSourceProperty(handle, str, kind, minimum, maximum, step, defaultValue, value, ref status);
            CheckStatus(status);
            return ret;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct StringWrite : IDisposable
        {
            internal readonly IntPtr str;

            public StringWrite(string vStr)
            {
                int bytes = Encoding.UTF8.GetByteCount(vStr);
                str = Marshal.AllocHGlobal(bytes * sizeof(byte));
                byte[] buffer = new byte[bytes];
                Encoding.UTF8.GetBytes(vStr, 0, vStr.Length, buffer, 0);
                Marshal.Copy(buffer, 0, str, bytes);
            }

            public void Dispose()
            {
                Marshal.FreeHGlobal(str);
            }
        }


        public static void SetSourceEnumPropertyChoices(int handle, int propertyHandle, IList<string> choices)
        {
            StringWrite[] nativeChoices = new StringWrite[choices.Count];
            for(int i = 0; i < choices.Count; i++)
            {
                nativeChoices[i] = new StringWrite(choices[i]);
            }

            try
            {
                int status = 0;
                Interop.CS_SetSourceEnumPropertyChoices(handle, propertyHandle, nativeChoices, nativeChoices.Length, ref status);
                CheckStatus(status);
            }
            finally
            {
                for (int i = 0; i < nativeChoices.Length; i++)
                {
                    nativeChoices[i].Dispose();
                }
            }
        }

        public static int CreateMjpegServer(string name, string listenAddress, int port)
        {
            UIntPtr size;
            byte[] nStr = Interop.CreateUTF8String(name, out size);
            byte[] aStr = Interop.CreateUTF8String(listenAddress, out size);
            int status = 0;
            int ret = Interop.CS_CreateMjpegServer(nStr, aStr, port, ref status);
            CheckStatus(status);
            return ret;
        }

        public static string GetMjpegServerListenAddress(int handle)
        {
            int status = 0;
            IntPtr ret = Interop.CS_GetMjpegServerListenAddress(handle, ref status);
            CheckStatus(status);
            string sRet = Interop.ReadUTF8String(ret);
            Interop.CS_FreeString(ret);
            return sRet;
        }

        public static int GetMjpegServerPort(int handle)
        {
            int status = 0;
            int ret = Interop.CS_GetMjpegServerPort(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int CreateUsbCameraDev(string name, int dev)
        {
            UIntPtr size;
            byte[] nStr = Interop.CreateUTF8String(name, out size);
            int status = 0;
            int ret = Interop.CS_CreateUsbCameraDev(nStr, dev, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int CreateUsbCameraPath(string name, string path)
        {
            UIntPtr size;
            byte[] nStr = Interop.CreateUTF8String(name, out size);
            byte[] pStr = Interop.CreateUTF8String(path, out size);
            int status = 0;
            int ret = Interop.CS_CreateUsbCameraPath(nStr, pStr, ref status);
            CheckStatus(status);
            return ret;
        }

        public static List<UsbCameraInfo> EnumerateUsbCameras()
        {
            int status = 0;
            int count = 0;
            IntPtr camArr = Interop.CS_EnumerateUsbCameras(ref count, ref status);

            #if !NETSTANDARD
            int ptrSize = Marshal.SizeOf(typeof(IntPtr));
            #else
            int ptrSize = Marshal.SizeOf<IntPtr>();
            #endif
            List<UsbCameraInfo> list = new List<UsbCameraInfo>(count);
            for (int i = 0; i < count; i++)
            {
                IntPtr ptr = new IntPtr(camArr.ToInt64() + ptrSize * i);
                #if !NETSTANDARD
                CSUsbCameraInfo info = (CSUsbCameraInfo)Marshal.PtrToStructure(ptr, typeof(CSUsbCameraInfo));
                #else
                CSUsbCameraInfo info = Marshal.PtrToStructure<CSUsbCameraInfo>(ptr);
                #endif
                list.Add(info.ToManaged());

            }
            Interop.CS_FreeEnumeratedUsbCameras(camArr, count);
            return list;
        }

        public static string GetUsbCameraPath(int handle)
        {
            int status = 0;
            IntPtr ret = Interop.CS_GetUsbCameraPath(handle, ref status);
            CheckStatus(status);
            string sRet = Interop.ReadUTF8String(ret);
            Interop.CS_FreeString(ret);
            return sRet;
        }

        public static int CreateHttpCamera(string name, string url, HttpCameraKind kind)
        {
            UIntPtr size;
            byte[] nStr = Interop.CreateUTF8String(name, out size);
            byte[] uStr = Interop.CreateUTF8String(url, out size);
            int status = 0;
            int ret = Interop.CS_CreateHttpCamera(nStr, uStr, kind, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int CreateHttpCameraMulti(string name, IList<string> urls, HttpCameraKind kind)
        {
            UIntPtr size;
            byte[] nStr = Interop.CreateUTF8String(name, out size);
            StringWrite[] nativeChoices = new StringWrite[urls.Count];
            for(int i = 0; i < urls.Count; i++)
            {
                nativeChoices[i] = new StringWrite(urls[i]);
            }
            try
            {
                int status = 0;
                int ret = Interop.CS_CreateHttpCameraMulti(nStr, nativeChoices, nativeChoices.Length, kind, ref status);
                CheckStatus(status);
                return ret;
            }
            finally
            {
                for (int i = 0; i < nativeChoices.Length; i++)
                {
                    nativeChoices[i].Dispose();
                }
            }
        }

        public static HttpCameraKind GetHttpCameraKind(int source)
        {
            int status = 0;
            HttpCameraKind ret = Interop.CS_GetHttpCameraKind(source, ref status);
            CheckStatus(status);
            return ret;
        }

        public static void SetHttpCameraUrls(int source, IList<string> urls)
        {
            StringWrite[] nativeChoices = new StringWrite[urls.Count];
            for(int i = 0; i < urls.Count; i++)
            {
                nativeChoices[i] = new StringWrite(urls[i]);
            }
            try
            {
                int status = 0;
                Interop.CS_SetHttpCameraUrls(source, nativeChoices, nativeChoices.Length, ref status);
                CheckStatus(status);
            }
            finally
            {
                for (int i = 0; i < nativeChoices.Length; i++)
                {
                    nativeChoices[i].Dispose();
                }
            }
        }

        public static List<string> GetHttpCameraUrls(int source)
        {
            int count = 0;
            #if !NETSTANDARD
            int ptrSize = Marshal.SizeOf(typeof(IntPtr));
            #else
            int ptrSize = Marshal.SizeOf<IntPtr>();
            #endif
            int status = 0;
            var arr = Interop.CS_GetHttpCameraUrls(source, ref count, ref status);
            CheckStatus(status);
            List<string> urls = new List<string>(count);

            for (int i = 0; i < count; i++)
            {
                IntPtr h = new IntPtr(arr.ToInt64() + ptrSize * i);
                urls.Add(Interop.ReadUTF8String(h));
            }
            Interop.CS_FreeHttpCameraUrls(arr, count);
            return urls;
        }

        public static int CopySource(int handle)
        {
            int status = 0;
            int ret = Interop.CS_CopySource(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int CopySink(int handle)
        {
            int status = 0;
            int ret = Interop.CS_CopySink(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        private static Dictionary<int, Interop.CS_ListenerCallback> s_listenerCallbacks = new Dictionary<int, Interop.CS_ListenerCallback>();

        public static int AddListener(Action<VideoEvent> listener, int eventMask, bool immediateNotify)
        {
            Interop.CS_ListenerCallback modCallback = (IntPtr data, ref CSEvent evnt) =>
            {
                listener(evnt.ToManaged());
            };

            int status = 0;
            int ret = Interop.CS_AddListener(IntPtr.Zero, modCallback, eventMask, immediateNotify ? 1 : 0, ref status);
            CheckStatus(status);
            s_listenerCallbacks.Add(ret, modCallback);
            return ret;
        }

        public static void RemoveListener(int handle)
        {
            int status = 0;
            Interop.CS_RemoveListener(handle, ref status);
            CheckStatus(status);
            if (s_listenerCallbacks.ContainsKey(handle))
            {
                s_listenerCallbacks.Remove(handle);
            }
        }

        public static string GetHostName()
        {
            var s = Interop.CS_GetHostname();
            string ret = Interop.ReadUTF8String(s);
            Interop.CS_FreeString(s);
            return ret;
        }

        public static List<string> GetNetworkInterfaces()
        {
            int count = 0;
            #if !NETSTANDARD
            int ptrSize = Marshal.SizeOf(typeof(IntPtr));
            #else
            int ptrSize = Marshal.SizeOf<IntPtr>();
            #endif
            var arr = Interop.CS_GetNetworkInterfaces(ref count);
            List<string> interfaces = new List<string>(count);

            for (int i = 0; i < count; i++)
            {
                IntPtr h = new IntPtr(arr.ToInt64() + ptrSize * i);
                interfaces.Add(Interop.ReadUTF8String(h));
            }
            Interop.CS_FreeNetworkInterfaces(arr, count);
            return interfaces;
        }

        private static Interop.CS_LogFunc s_nativeLog;

        /// <summary>
        /// Assigns a method to be called whenever a log statement occurs in the internal
        /// network table library.
        /// </summary>
        /// <param name="func">The log function to assign.</param>
        /// <param name="minLevel">The minimum level to log.</param>
        public static void SetLogger(LogFunc func, LogLevel minLevel)
        {
            s_nativeLog = (level, file, line, msg) =>
            {
                string message = Interop.ReadUTF8String(msg);
                string fileName = Interop.ReadUTF8String(file);

                func((LogLevel)level, fileName, (int)line, message);
            };

            Interop.CS_SetLogger(s_nativeLog, (uint)minLevel);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static CameraServer.Native.Interop;

namespace CameraServer.Native
{
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
            byte[] str = CreateUTF8String(name, out size);
            int status = 0;
            int ret = CS_CreateCvSink(str, ref status);
            CheckStatus(status);
            return ret;
        }

        public static void SetSinkDescription(int handle, string description)
        {
            UIntPtr size;
            byte[] str = CreateUTF8String(description, out size);
            int status = 0;
            CS_SetSinkDescription(handle, str, ref status);
            CheckStatus(status);
        }

        public static ulong GrabSinkFrame(int handle, IntPtr nativeObj)
        {
            int status = 0;
            ulong ret = CS_GrabSinkFrameCpp(handle, nativeObj, ref status);
            CheckStatus(status);
            return ret;
        }

        public static string GetSinkError(int handle)
        {
            int status = 0;
            IntPtr ret = CS_GetSinkError(handle, ref status);
            CheckStatus(status);
            string sRet = ReadUTF8String(ret);
            CS_FreeString(ret);
            return sRet;
        }

        public static void SetSinkEnabled(int handle, bool enabled)
        {
            int status = 0;
            CS_SetSinkEnabled(handle, enabled, ref status);
            CheckStatus(status);
        }

        public static void ReleaseSink(int handle)
        {
            int status = 0;
            CS_ReleaseSink(handle, ref status);
            CheckStatus(status);
        }

        public static SinkKind GetSinkKind(int handle)
        {
            int status = 0;
            SinkKind ret = CS_GetSinkKind(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static string GetSinkName(int handle)
        {
            int status = 0;
            IntPtr ret = CS_GetSinkName(handle, ref status);
            CheckStatus(status);
            string sRet = ReadUTF8String(ret);
            CS_FreeString(ret);
            return sRet;
        }

        public static string GetSinkDescription(int handle)
        {
            int status = 0;
            IntPtr ret = CS_GetSinkDescription(handle, ref status);
            CheckStatus(status);
            string sRet = ReadUTF8String(ret);
            CS_FreeString(ret);
            return sRet;
        }

        public static void SetSinkSource(int sinkHandle, int sourceHandle)
        {
            int status = 0;
            CS_SetSinkSource(sinkHandle, sourceHandle, ref status);
            CheckStatus(status);
        }

        public static int GetSinkSource(int handle)
        {
            int status = 0;
            int ret = CS_GetSinkSource(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int GetSinkSourceProperty(int handle, string name)
        {
            UIntPtr size;
            byte[] str = CreateUTF8String(name, out size);
            int status = 0;
            int prop = CS_GetSinkSourceProperty(handle, str, ref status);
            CheckStatus(status);
            return prop;
        }

        public static List<int> EnumerateSinks()
        {
            int status = 0;
            int count = 0;
            IntPtr sinkArray = CS_EnumerateSinks(ref count, ref status);
            CheckStatus(status);
            List<int> sinks = new List<int>(count);

            for (int i = 0; i < count; i++)
            {
                int handle = Marshal.ReadInt32(sinkArray, i);
                sinks.Add(handle);
            }
            CS_ReleaseEnumeratedSinks(sinkArray, count);
            return sinks;
        }

        public static void ReleaseSource(int handle)
        {
            int status = 0;
            CS_ReleaseSource(handle, ref status);
            CheckStatus(status);
        }

        public static SourceKind GetSourceKind(int handle)
        {
            int status = 0;
            SourceKind ret = CS_GetSourceKind(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static string GetSourceName(int handle)
        {
            int status = 0;
            IntPtr ret = CS_GetSourceName(handle, ref status);
            CheckStatus(status);
            string sRet = ReadUTF8String(ret);
            CS_FreeString(ret);
            return sRet;
        }

        public static string GetSourceDescription(int handle)
        {
            int status = 0;
            IntPtr ret = CS_GetSourceDescription(handle, ref status);
            CheckStatus(status);
            string sRet = ReadUTF8String(ret);
            CS_FreeString(ret);
            return sRet;
        }

        public static ulong GetSourceLastFrameTime(int handle)
        {
            int status = 0;
            ulong ret = CS_GetSourceLastFrameTime(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static bool IsSourceConnected(int handle)
        {
            int status = 0;
            bool ret = CS_IsSourceConnected(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int GetSourceProperty(int handle, string name)
        {
            UIntPtr size;
            byte[] str = CreateUTF8String(name, out size);
            int status = 0;
            int prop = CS_GetSourceProperty(handle, str, ref status);
            CheckStatus(status);
            return prop;
        }

        public static List<int> EnumerateSourceProperties(int handle)
        {
            int status = 0;
            int count = 0;
            IntPtr propertyArray = CS_EnumerateSourceProperties(handle, ref count, ref status);
            CheckStatus(status);
            List<int> properties = new List<int>(count);

            for (int i = 0; i < count; i++)
            {
                int h = Marshal.ReadInt32(propertyArray, i);
                properties.Add(h);
            }
            CS_ReleaseEnumeratedSources(propertyArray, count);
            return properties;
        }

        public static VideoMode GetSourceVideoMode(int handle)
        {
            int status = 0;
            VideoMode mode = new VideoMode();
            CS_GetSourceVideoMode(handle, ref mode, ref status);
            CheckStatus(status);
            return mode;
        }

        public static bool SetSourceVideoMode(int handle, VideoMode mode)
        {
            int status = 0;
            bool ret = CS_SetSourceVideoModeDiscrete(handle, mode.PixelFormat, mode.Width, mode.Height, mode.FPS, ref status);
            CheckStatus(status);
            return ret;
        }

        public static bool SetSourceVideoMode(int handle, PixelFormat pixelFormat, int width, int height, int fps)
        {
            int status = 0;
            bool ret = CS_SetSourceVideoModeDiscrete(handle, pixelFormat, width, height, fps, ref status);
            CheckStatus(status);
            return ret;
        }

        public static bool SetSourcePixelFormat(int handle, PixelFormat format)
        {
            int status = 0;
            bool ret = CS_SetSourcePixelFormat(handle, format, ref status);
            CheckStatus(status);
            return ret;
        }

        public static bool SetSourceResolution(int handle, int width, int height)
        {
            int status = 0;
            bool ret = CS_SetSourceResolution(handle, width, height, ref status);
            CheckStatus(status);
            return ret;
        }

        public static bool SetSourceFPS(int handle, int fps)
        {
            int status = 0;
            bool ret = CS_SetSourceFPS(handle, fps, ref status);
            CheckStatus(status);
            return ret;
        }

        public static List<VideoMode> EnumerateSourceVideoModes(int handle)
        {
            int status = 0;
            int count = 0;
            int modeSize = Marshal.SizeOf(typeof(VideoMode));
            IntPtr modeArray = CS_EnumerateSourceVideoModes(handle, ref count, ref status);
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
            CS_FreeEnumeratedVideoModes(modeArray, modeSize);
            return modes;
        }

        public static List<int> EnumerateSourceSinks(int handle)
        {
            int status = 0;
            int count = 0;
            IntPtr sinkArray = CS_EnumerateSourceSinks(handle, ref count, ref status);
            CheckStatus(status);
            List<int> sinks = new List<int>(count);

            for (int i = 0; i < count; i++)
            {
                int h = Marshal.ReadInt32(sinkArray, i);
                sinks.Add(h);
            }
            CS_ReleaseEnumeratedSinks(sinkArray, count);
            return sinks;
        }

        public static List<int> EnumerateSources()
        {
            int status = 0;
            int count = 0;
            IntPtr sourceArray = CS_EnumerateSources(ref count, ref status);
            CheckStatus(status);
            List<int> sources = new List<int>(count);

            for (int i = 0; i < count; i++)
            {
                int h = Marshal.ReadInt32(sourceArray, i);
                sources.Add(h);
            }
            CS_ReleaseEnumeratedSources(sourceArray, count);
            return sources;
        }

        public static string GetPropertyName(int handle)
        {
            int status = 0;
            IntPtr ret = CS_GetPropertyName(handle, ref status);
            CheckStatus(status);
            string sRet = ReadUTF8String(ret);
            CS_FreeString(ret);
            return sRet;
        }

        public static PropertyKind GetPropertyKind(int handle)
        {
            int status = 0;
            PropertyKind ret = CS_GetPropertyKind(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int GetProperty(int handle)
        {
            int status = 0;
            int ret = CS_GetProperty(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static void SetProperty(int handle, int value)
        {
            int status = 0;
            CS_SetProperty(handle, value, ref status);
            CheckStatus(status);
        }

        public static int GetPropertyMin(int handle)
        {
            int status = 0;
            int ret = CS_GetPropertyMin(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int GetPropertyMax(int handle)
        {
            int status = 0;
            int ret = CS_GetPropertyMax(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int GetPropertyDefault(int handle)
        {
            int status = 0;
            int ret = CS_GetPropertyDefault(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int GetPropertyStep(int handle)
        {
            int status = 0;
            int ret = CS_GetPropertyStep(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static string GetStringProperty(int handle)
        {
            int status = 0;
            IntPtr ret = CS_GetStringProperty(handle, ref status);
            CheckStatus(status);
            string sRet = ReadUTF8String(ret);
            CS_FreeString(ret);
            return sRet;
        }

        public static void SetStringProperty(int handle, string value)
        {
            UIntPtr size;
            byte[] str = CreateUTF8String(value, out size);
            int status = 0;
            CS_SetStringProperty(handle, str, ref status);
            CheckStatus(status);
        }

        public static List<string> GetEnumPropertyChoices(int handle)
        {
            int status = 0;
            int count = 0;
            IntPtr choicesArray = CS_GetEnumPropertyChoices(handle, ref count, ref status);
            CheckStatus(status);
            List<string> choices = new List<string>(count);

            for (int i = 0; i < count; i++)
            {
                IntPtr h = Marshal.ReadIntPtr(choicesArray, i);
                choices.Add(ReadUTF8String(h));
            }
            CS_FreeEnumPropertyChoices(choicesArray, count);
            return choices;
        }

        public static int CreateCvSource(string name, PixelFormat pixelFormat, int width, int height, int fps)
        {
            UIntPtr size;
            byte[] str = CreateUTF8String(name, out size);
            VideoMode mode = new VideoMode(pixelFormat, width, height, fps);
            int status = 0;
            int ret = CS_CreateCvSource(str, ref mode, ref status);
            CheckStatus(status);
            return ret;
        }

        public static void PutSourceFrame(int handle, IntPtr nativeObj)
        {
            int status = 0;
            CS_PutSourceFrameCpp(handle, nativeObj, ref status);
            CheckStatus(status);
        }

        public static void NotifySourceError(int handle, string msg)
        {
            UIntPtr size;
            byte[] str = CreateUTF8String(msg, out size);
            int status = 0;
            CS_NotifySourceError(handle, str, ref status);
            CheckStatus(status);
        }

        public static void SetSourceConnected(int handle, bool connected)
        {
            int status = 0;
            CS_SetSourceConnected(handle, connected, ref status);
            CheckStatus(status);
        }

        public static void SetSourceDescription(int handle, string name)
        {
            UIntPtr size;
            byte[] str = CreateUTF8String(name, out size);
            int status = 0;
            CS_SetSourceDescription(handle, str, ref status);
            CheckStatus(status);
        }

        public static int CreateSourceProperty(int handle, string name, PropertyKind kind, int minimum, int maximum, int step, int defaultValue, int value)
        {
            UIntPtr size;
            byte[] str = CreateUTF8String(name, out size);
            int status = 0;
            int ret = CS_CreateSourceProperty(handle, str, kind, minimum, maximum, step, defaultValue, value, ref status);
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
                CS_SetSourceEnumPropertyChoices(handle, propertyHandle, nativeChoices, nativeChoices.Length, ref status);
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

        public static int CreateMJPEGServer(string name, string listenAddress, int port)
        {
            UIntPtr size;
            byte[] nStr = CreateUTF8String(name, out size);
            byte[] aStr = CreateUTF8String(listenAddress, out size);
            int status = 0;
            int ret = CS_CreateMJPEGServer(nStr, aStr, port, ref status);
            CheckStatus(status);
            return ret;
        }

        public static string GetMJPEGServerListenAddress(int handle)
        {
            int status = 0;
            IntPtr ret = CS_GetMJPEGServerListenAddress(handle, ref status);
            CheckStatus(status);
            string sRet = ReadUTF8String(ret);
            CS_FreeString(ret);
            return sRet;
        }

        public static int GetMJPEGServerPort(int handle)
        {
            int status = 0;
            int ret = CS_GetMJPEGServerPort(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int CreateUSBCameraDev(string name, int dev)
        {
            UIntPtr size;
            byte[] nStr = CreateUTF8String(name, out size);
            int status = 0;
            int ret = CS_CreateUSBCameraDev(nStr, dev, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int CreateUSBCameraPath(string name, string path)
        {
            UIntPtr size;
            byte[] nStr = CreateUTF8String(name, out size);
            byte[] pStr = CreateUTF8String(path, out size);
            int status = 0;
            int ret = CS_CreateUSBCameraPath(nStr, pStr, ref status);
            CheckStatus(status);
            return ret;
        }

        public static List<UsbCameraInfo> EnumerateUSBCameras()
        {
            int status = 0;
            int count = 0;
            IntPtr camArr = CS_EnumerateUSBCameras(ref count, ref status);

#pragma warning disable CS0618
            int ptrSize = Marshal.SizeOf(typeof(IntPtr));
#pragma warning restore CS0618
            List<UsbCameraInfo> list = new List<UsbCameraInfo>(count);
            for (int i = 0; i < count; i++)
            {
                IntPtr ptr = new IntPtr(camArr.ToInt64() + ptrSize * i);
                CSUSBCameraInfo info = (CSUSBCameraInfo)Marshal.PtrToStructure(ptr, typeof(CSUSBCameraInfo));
                list.Add(info.ToManaged());

            }
            CS_FreeEnumeratedUSBCameras(camArr, count);
            return list;
        }

        public static string GetUSBCameraPath(int handle)
        {
            int status = 0;
            IntPtr ret = CS_GetUSBCameraPath(handle, ref status);
            CheckStatus(status);
            string sRet = ReadUTF8String(ret);
            CS_FreeString(ret);
            return sRet;
        }

        public static int CreateHTTPCamera(string name, string url)
        {
            UIntPtr size;
            byte[] nStr = CreateUTF8String(name, out size);
            byte[] uStr = CreateUTF8String(url, out size);
            int status = 0;
            int ret = CS_CreateHTTPCamera(nStr, uStr, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int CopySource(int handle)
        {
            int status = 0;
            int ret = CS_CopySource(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        public static int CopySink(int handle)
        {
            int status = 0;
            int ret = CS_CopySink(handle, ref status);
            CheckStatus(status);
            return ret;
        }

        private static Dictionary<int, CS_ListenerCallback> s_listenerCallbacks = new Dictionary<int, CS_ListenerCallback>();

        public static int AddListener(Action<VideoEvent> listener, int eventMask, bool immediateNotify)
        {
            CS_ListenerCallback modCallback = (IntPtr data, ref CSEvent evnt) =>
            {
                listener(evnt.ToManaged());
            };

            int status = 0;
            int ret = CS_AddListener(IntPtr.Zero, modCallback, eventMask, immediateNotify ? 1 : 0, ref status);
            CheckStatus(status);
            s_listenerCallbacks.Add(ret, modCallback);
            return ret;
        }

        public static void RemoveListener(int handle)
        {
            int status = 0;
            CS_RemoveListener(handle, ref status);
            CheckStatus(status);
            if (s_listenerCallbacks.ContainsKey(handle))
            {
                s_listenerCallbacks.Remove(handle);
            }
        }

        public static string GetHostName()
        {
            var s = CS_GetHostname();
            string ret = ReadUTF8String(s);
            CS_FreeString(s);
            return ret;
        }

        public static List<string> GetNetworkInterfaces()
        {
            int count = 0;
            var arr = CS_GetNetworkInterfaces(ref count);
            List<string> interfaces = new List<string>(count);

            for (int i = 0; i < count; i++)
            {
                IntPtr h = Marshal.ReadIntPtr(arr, i);
                interfaces.Add(ReadUTF8String(h));
            }
            CS_FreeNetworkInterfaces(arr, count);
            return interfaces;
        }
    }
}

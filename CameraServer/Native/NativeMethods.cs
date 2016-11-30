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
        public static int CreateCvSink(string name)
        {
            UIntPtr size;
            byte[] str = CreateUTF8String(name, out size);
            int status = 0;
            int ret = CS_CreateCvSink(str, ref status);
            // TODO: Check status
            return ret;
        }

        public static void SetSinkDescription(int handle, string description)
        {
            UIntPtr size;
            byte[] str = CreateUTF8String(description, out size);
            int status = 0;
            CS_SetSinkDescription(handle, str, ref status);
            // TODO: Check status
        }

        public static ulong GrabSinkFrame(int handle, IntPtr nativeObj)
        {
            int status = 0;
            ulong ret = CS_GrabSinkFrame(handle, nativeObj, ref status);
            // TODO: Check Status
            return ret;
        }

        public static string GetSinkError(int handle)
        {
            int status = 0;
            IntPtr ret = CS_GetSinkError(handle, ref status);
            // TODO: Check Status
            string sRet = ReadUTF8String(ret);
            CS_FreeString(ret);
            return sRet;
        }

        public static void SetSinkEnabled(int handle, bool enabled)
        {
            int status = 0;
            CS_SetSinkEnabled(handle, enabled, ref status);
            // TODO: Check Status
        }

        public static void ReleaseSink(int handle)
        {
            int status = 0;
            CS_ReleaseSink(handle, ref status);
            // TODO: Check Status
        }

        public static SinkKind GetSinkKind(int handle)
        {
            int status = 0;
            SinkKind ret = CS_GetSinkKind(handle, ref status);
            // TODO: Check Status
            return ret;
        }

        public static string GetSinkName(int handle)
        {
            int status = 0;
            IntPtr ret = CS_GetSinkName(handle, ref status);
            // TODO: Check Status
            string sRet = ReadUTF8String(ret);
            CS_FreeString(ret);
            return sRet;
        }

        public static string GetSinkDescription(int handle)
        {
            int status = 0;
            IntPtr ret = CS_GetSinkDescription(handle, ref status);
            // TODO: Check Status
            string sRet = ReadUTF8String(ret);
            CS_FreeString(ret);
            return sRet;
        }

        public static void SetSinkSource(int sinkHandle, int sourceHandle)
        {
            int status = 0;
            CS_SetSinkSource(sinkHandle, sourceHandle, ref status);
            // TODO: Check Status
        }

        public static int GetSinkSource(int handle)
        {
            int status = 0;
            int ret = CS_GetSinkSource(handle, ref status);
            // TODO: Check Status
            return ret;
        }

        public static int GetSinkSourceProperty(int handle, string name)
        {
            UIntPtr size;
            byte[] str = CreateUTF8String(name, out size);
            int status = 0;
            int prop = CS_GetSinkSourceProperty(handle, str, ref status);
            // TODO: Check Status
            return prop;
        }

        public static List<int> EnumerateSinks()
        {
            int status = 0;
            int count = 0;
            IntPtr sinkArray = CS_EnumerateSinks(ref count, ref status);
            // TODO: Check Status
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
            // TODO: Check Status
        }

        public static SourceKind GetSourceKind(int handle)
        {
            int status = 0;
            SourceKind ret = CS_GetSourceKind(handle, ref status);
            // TODO: Check Status
            return ret;
        }

        public static string GetSourceName(int handle)
        {
            int status = 0;
            IntPtr ret = CS_GetSourceName(handle, ref status);
            // TODO: Check Status
            string sRet = ReadUTF8String(ret);
            CS_FreeString(ret);
            return sRet;
        }

        public static string GetSourceDescription(int handle)
        {
            int status = 0;
            IntPtr ret = CS_GetSourceDescription(handle, ref status);
            // TODO: Check Status
            string sRet = ReadUTF8String(ret);
            CS_FreeString(ret);
            return sRet;
        }

        public static ulong GetSourceLastFrameTime(int handle)
        {
            int status = 0;
            ulong ret = CS_GetSourceLastFrameTime(handle, ref status);
            // TODO: Check Status
            return ret;
        }

        public static bool IsSourceConnected(int handle)
        {
            int status = 0;
            bool ret = CS_IsSourceConnected(handle, ref status);
            // TODO: Check Status
            return ret;
        }

        public static int GetSourceProperty(int handle, string name)
        {
            UIntPtr size;
            byte[] str = CreateUTF8String(name, out size);
            int status = 0;
            int prop = CS_GetSourceProperty(handle, str, ref status);
            // TODO: Check Status
            return prop;
        }

        public static List<int> EnumerateSourceProperties(int handle)
        {
            int status = 0;
            int count = 0;
            IntPtr propertyArray = CS_EnumerateSourceProperties(handle, ref count, ref status);
            // TODO: Check Status
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
            // TODO: Check Status
            return mode;
        }

        public static bool SetSourceVideoMode(int handle, VideoMode mode)
        {
            int status = 0;
            bool ret = CS_SetSourceVideoModeDiscrete(handle, mode.PixelFormat, mode.Width, mode.Height, mode.FPS, ref status);
            // TODO: Check Status
            return ret;
        }

        public static bool SetSourceVideoMode(int handle, PixelFormat pixelFormat, int width, int height, int fps)
        {
            int status = 0;
            bool ret = CS_SetSourceVideoModeDiscrete(handle, pixelFormat, width, height, fps, ref status);
            // TODO: Check Status
            return ret;
        }

        public static bool SetSourcePixelFormat(int handle, PixelFormat format)
        {
            int status = 0;
            bool ret = CS_SetSourcePixelFormat(handle, format, ref status);
            // TODO: Check Status
            return ret;
        }

        public static bool SetSourceResolution(int handle, int width, int height)
        {
            int status = 0;
            bool ret = CS_SetSourceResolution(handle, width, height, ref status);
            // TODO: Check Status
            return ret;
        }

        public static bool SetSourceFPS(int handle, int fps)
        {
            int status = 0;
            bool ret = CS_SetSourceFPS(handle, fps, ref status);
            // TODO: Check Status
            return ret;
        }

        public static List<VideoMode> EnumerateSourceVideoModes(int handle)
        {
            int status = 0;
            int count = 0;
            int modeSize = Marshal.SizeOf(typeof(VideoMode));
            IntPtr modeArray = CS_EnumerateSourceVideoModes(handle, ref count, ref status);
            // TODO: Check Status
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
            // TODO: Check Status
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
            // TODO: Check Status
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
            // TODO: Check Status
            string sRet = ReadUTF8String(ret);
            CS_FreeString(ret);
            return sRet;
        }

        public static PropertyKind GetPropertyKind(int handle)
        {
            int status = 0;
            PropertyKind ret = CS_GetPropertyKind(handle, ref status);
            // TODO: Check Status
            return ret;
        }

        public static int GetProperty(int handle)
        {
            int status = 0;
            int ret = CS_GetProperty(handle, ref status);
            // TODO: Check Status
            return ret;
        }

        public static void SetProperty(int handle, int value)
        {
            int status = 0;
            CS_SetProperty(handle, value, ref status);
            // TODO: Check Status
        }

        public static int GetPropertyMin(int handle)
        {
            int status = 0;
            int ret = CS_GetPropertyMin(handle, ref status);
            // TODO: Check Status
            return ret;
        }

        public static int GetPropertyMax(int handle)
        {
            int status = 0;
            int ret = CS_GetPropertyMax(handle, ref status);
            // TODO: Check Status
            return ret;
        }

        public static int GetPropertyDefault(int handle)
        {
            int status = 0;
            int ret = CS_GetPropertyDefault(handle, ref status);
            // TODO: Check Status
            return ret;
        }

        public static int GetPropertyStep(int handle)
        {
            int status = 0;
            int ret = CS_GetPropertyStep(handle, ref status);
            // TODO: Check Status
            return ret;
        }

        public static string GetStringProperty(int handle)
        {
            int status = 0;
            IntPtr ret = CS_GetStringProperty(handle, ref status);
            // TODO: Check Status
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
            // TODO: Check status
        }

        public static List<string> GetEnumPropertyChoices(int handle)
        {
            int status = 0;
            int count = 0;
            IntPtr choicesArray = CS_GetEnumPropertyChoices(handle, ref count, ref status);
            // TODO: Check Status
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
            // TODO: Check Status
            return ret;
        }

        public static void PutSourceFrame(int handle, IntPtr nativeObj)
        {
            int status = 0;
            CS_PutSourceFrame(handle, nativeObj, ref status);
            // TODO: Check Status
        }

        public static void NotifySourceError(int handle, string msg)
        {
            UIntPtr size;
            byte[] str = CreateUTF8String(msg, out size);
            int status = 0;
            CS_NotifySourceError(handle, str, ref status);
            // TODO: Check Status
        }

        public static void SetSourceConnected(int handle, bool connected)
        {
            int status = 0;
            CS_SetSourceConnected(handle, connected, ref status);
            // TODO: Check Status
        }

        public static void SetSourceDescription(int handle, string name)
        {
            UIntPtr size;
            byte[] str = CreateUTF8String(name, out size);
            int status = 0;
            CS_SetSourceDescription(handle, str, ref status);
            // TODO: Check Status
        }

        public static int CreateSourceProperty(int handle, string name, PropertyKind kind, int minimum, int maximum, int step, int defaultValue, int value)
        {
            UIntPtr size;
            byte[] str = CreateUTF8String(name, out size);
            int status = 0;
            int ret = CS_CreateSourceProperty(handle, str, kind, minimum, maximum, step, defaultValue, value, ref status);
            // TODO: Check Status
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
                // TODO: Check Status
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
            int ret = CS_CreateMJPEGServerDelegate
            // TODO: Check Status
            return ret;
        }
    }
}

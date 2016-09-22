using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using CameraServer.Native.LibraryUtilities;

namespace CameraServer.Native
{
    internal class ExcludeFromCodeCoverageAttribute : Attribute
    {

    }
#if !NETSTANDARD
    [SuppressUnmanagedCodeSecurity]
#endif
    [ExcludeFromCodeCoverage]
    internal class Interop
    {
        private static readonly bool s_libraryLoaded;
        // ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
        internal static NativeLibraryLoader NativeLoader { get; }
        private static readonly string s_libraryLocation;
        private static readonly bool s_useCommandLineFile;
        // ReSharper restore PrivateFieldCanBeConvertedToLocalVariable
        private static readonly bool s_runFinalizer;

        // private constructor. Only used for our unload finalizer
        private Interop() { }
        private void Ping() { } // Used to force compilation
        // static variable used only for interop purposes
        private static readonly Interop finalizeInterop = new Interop();
        ~Interop()
        {
            // If we did not successfully get constructed, we don't need to destruct
            if (!s_runFinalizer) return;
            //Sets logger to null so no logger gets called back.

            NativeLoader.LibraryLoader.UnloadLibrary();

            try
            {
                //Don't delete file if we are using a specified file.
                if (!s_useCommandLineFile && File.Exists(s_libraryLocation))
                {
                    File.Delete(s_libraryLocation);
                }
            }
            catch
            {
                //Any errors just ignore.
            }
        }


        static Interop()
        {
            if (!s_libraryLoaded)
            {
                try
                {
                    finalizeInterop.Ping();
                    string[] commandArgs = Environment.GetCommandLineArgs();
                    foreach (var commandArg in commandArgs)
                    {
                        //search for a line with the prefix "-cameraserver:"
                        if (commandArg.ToLower().Contains("-cameraserver:"))
                        {
                            //Split line to get the library.
                            int splitLoc = commandArg.IndexOf(':');
                            string file = commandArg.Substring(splitLoc + 1);

                            //If the file exists, just return it so dlopen can load it.
                            if (File.Exists(file))
                            {
                                s_libraryLocation = file;
                                s_useCommandLineFile = true;
                            }
                        }
                    }

                    const string resourceRoot = "CameraServer.Native.Libraries.";

                    if (s_useCommandLineFile)
                    {
                        NativeLoader = new NativeLibraryLoader();
                        NativeLoader.LoadNativeLibrary<Interop>(s_libraryLocation, true);
                    }
                    else if (File.Exists("/usr/local/frc/bin/frcRunRobot.sh"))
                    {
                        NativeLoader = new NativeLibraryLoader();
                        // RoboRIO
                        NativeLoader.LoadNativeLibrary<Interop>(new RoboRioLibraryLoader(), resourceRoot + "roborio.libcameraserver.so");
                        s_libraryLocation = NativeLoader.LibraryLocation;
                    }
                    else
                    {
                        NativeLoader = new NativeLibraryLoader();
                        NativeLoader.AddLibraryLocation(OsType.Windows32,
                            resourceRoot + "x86.cameraserver.dll");
                        NativeLoader.AddLibraryLocation(OsType.Windows64,
                            resourceRoot + "amd64.cameraserver.dll");
                        NativeLoader.AddLibraryLocation(OsType.Linux32,
                            resourceRoot + "x86.libcameraserver.so");
                        NativeLoader.AddLibraryLocation(OsType.Linux64,
                            resourceRoot + "amd64.libcameraserver.so");
                        NativeLoader.AddLibraryLocation(OsType.MacOs32,
                            resourceRoot + "x86.libcameraserver.dylib");
                        NativeLoader.AddLibraryLocation(OsType.MacOs64,
                            resourceRoot + "amd64.libcameraserver.dylib");

                        NativeLoader.LoadNativeLibrary<Interop>();
                        s_libraryLocation = NativeLoader.LibraryLocation;
                    }

                    NativeDelegateInitializer.SetupNativeDelegates<Interop>(NativeLoader);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    Environment.Exit(1);
                }
                s_runFinalizer = true;
                s_libraryLoaded = true;
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_OnChange(IntPtr data, int property);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_ProcessFrame(IntPtr data, ulong time);



        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate PropertyType CS_GetPropertyTypeDelegate(int property, ref int status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_GetPropertyNameDelegate(int property, ref int status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_GetPropertyDelegate(int property, ref int status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_SetPropertyDelegate(int property, int value, ref int status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_GetPropertyMinDelegate(int property, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_GetPropertyMaxDelegate(int property, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_GetPropertyStepDelegate(int property, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_GetPropertyDefaultDelegate(int property, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_GetStringPropertyDelegate(int property, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_SetStringPropertyDelegate(int property, byte[] value,
                                  ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_GetEnumPropertyChoicesDelegate(int property, ref int count,
                                         ref int status);

        //
        // Source Creation Functions
        //
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_CreateUSBSourceDevDelegate(byte[] name, int dev, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_CreateUSBSourcePathDelegate(byte[] name, byte[] path,
                                         ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_CreateHTTPSourceDelegate(byte[] name, byte[] url,
                                      ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_CreateCvSourceDelegate(byte[] name, ref int status);

        //
        // Source Functions
        //
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_GetSourceNameDelegate(int source, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_GetSourceDescriptionDelegate(int source, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate ulong CS_GetSourceLastFrameTimeDelegate(int source, ref int status);
        [return: MarshalAs(UnmanagedType.Bool)]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool CS_IsSourceConnectedDelegate(int source, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_GetSourcePropertyDelegate(int source, byte[] name,
                                         ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_EnumerateSourcePropertiesDelegate(int source, ref int count,
                                                  ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_CopySourceDelegate(int source, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_ReleaseSourceDelegate(int source, ref int status);

        //
        // OpenCV Source Functions
        //
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_PutSourceFrameDelegate(int source, IntPtr image,
                       ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_NotifySourceErrorDelegate(int source, byte[] msg, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_SetSourceConnectedDelegate(int source, [MarshalAs(UnmanagedType.Bool)]bool connected,
                                   ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_CreateSourcePropertyDelegate(int source, byte[] name,
                                    PropertyType type,
                                    ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_CreateSourcePropertyCallbackDelegate(
            int source, byte[] name, PropertyType type, IntPtr data,
            CS_OnChange onChange, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_RemoveSourcePropertyDelegate(int source, int property,
                                     ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_RemoveSourcePropertyByNameDelegate(int source, byte[] name,
                                           ref int status);

        //
        // Sink Creation Functions
        //
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_CreateHTTPSinkDelegate(byte[] name, byte[] listenAddress, int port,
                                  ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_CreateCvSinkDelegate(byte[] name, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_CreateCvSinkCallbackDelegate(byte[] name, IntPtr data,
                                        CS_ProcessFrame processFrame,
                                ref int status);

        //
        // Sink Functions
        //
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_GetSinkNameDelegate(int sink, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_GetSinkDescriptionDelegate(int sink, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_SetSinkSourceDelegate(int sink, int source, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_GetSinkSourcePropertyDelegate(int sink, byte[] name,
                                             ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_GetSinkSourceDelegate(int sink, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_CopySinkDelegate(int sink, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_ReleaseSinkDelegate(int sink, ref int status);

        //
        // OpenCV Sink Functions
        //
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate ulong CS_GrabSinkFrameDelegate(int sink, IntPtr image, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_GetSinkErrorDelegate(int sink, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_SetSinkEnabledDelegate(int sink, [MarshalAs(UnmanagedType.Bool)]bool enabled, ref int status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void SourceListenerCallback(IntPtr data, IntPtr name, int source, int sourceEvent);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_AddSourceListenerDelegate(
            IntPtr data, SourceListenerCallback callback, int eventMask, ref int status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_RemoveSourceListenerDelegate(int handle, ref int status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void SinkListenerCallback(IntPtr data, IntPtr name, int sink, int sinkEvent);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_AddSinkListenerDelegate(
            IntPtr data, SinkListenerCallback callback, int eventMask, ref int status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_RemoteSinkListenerDelegate(int handle, ref int status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_EnumerateUSBCamerasDelegate(ref int count, ref int status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_FreeEnumeratedUSBCamerasDelegate(IntPtr cameras, int count);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_EnumerateSourcesDelegate(ref int count, ref int status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_ReleaseEnumeratedSourcesDelegate(IntPtr sources, ref int count);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_EnumerateSinksDelegate(ref int count, ref int status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_ReleaseEnumeratedSinksDelegate(IntPtr sinks, ref int count);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_FreeStringDelegate(IntPtr str);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_FreeEnumPropertyChoicesDelegate(IntPtr choices, int count);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_FreeEnumeratedPropertiesDelegate(IntPtr properties, int count);

        [NativeDelegate]
        internal static CS_GetPropertyTypeDelegate CS_GetPropertyType;
        [NativeDelegate]
        internal static CS_GetPropertyNameDelegate CS_GetPropertyName;
        [NativeDelegate]
        internal static CS_GetPropertyDelegate CS_GetProperty;
        [NativeDelegate]
        internal static CS_SetPropertyDelegate CS_SetProperty;
        [NativeDelegate]
        internal static CS_GetPropertyMinDelegate CS_GetPropertyMin;
        [NativeDelegate]
        internal static CS_GetPropertyMaxDelegate CS_GetPropertyMax;
        [NativeDelegate]
        internal static CS_GetPropertyStepDelegate CS_GetPropertyStep;
        [NativeDelegate]
        internal static CS_GetPropertyDefaultDelegate CS_GetPropertyDefault;
        [NativeDelegate]
        internal static CS_GetStringPropertyDelegate CS_GetStringProperty;
        [NativeDelegate]
        internal static CS_SetStringPropertyDelegate CS_SetStringProperty;
        [NativeDelegate]
        internal static CS_GetEnumPropertyChoicesDelegate CS_GetEnumPropertyChoices;
        [NativeDelegate]
        internal static CS_CreateUSBSourceDevDelegate CS_CreateUSBSourceDev;
        [NativeDelegate]
        internal static CS_CreateUSBSourcePathDelegate CS_CreateUSBSourcePath;
        [NativeDelegate]
        internal static CS_CreateHTTPSourceDelegate CS_CreateHTTPSource;
        [NativeDelegate]
        internal static CS_CreateCvSourceDelegate CS_CreateCvSource;
        [NativeDelegate]
        internal static CS_GetSourceNameDelegate CS_GetSourceName;
        [NativeDelegate]
        internal static CS_GetSourceDescriptionDelegate CS_GetSourceDescription;
        [NativeDelegate]
        internal static CS_GetSourceLastFrameTimeDelegate CS_GetSourceLastFrameTime;
        [NativeDelegate]
        internal static CS_IsSourceConnectedDelegate CS_IsSourceConnected;
        [NativeDelegate]
        internal static CS_GetSourcePropertyDelegate CS_GetSourceProperty;
        [NativeDelegate]
        internal static CS_EnumerateSourcePropertiesDelegate CS_EnumerateSourceProperties;
        [NativeDelegate]
        internal static CS_CopySourceDelegate CS_CopySource;
        [NativeDelegate]
        internal static CS_ReleaseSourceDelegate CS_ReleaseSource;
        [NativeDelegate]
        internal static CS_PutSourceFrameDelegate CS_PutSourceFrame;
        [NativeDelegate]
        internal static CS_NotifySourceErrorDelegate CS_NotifySourceError;
        [NativeDelegate]
        internal static CS_SetSourceConnectedDelegate CS_SetSourceConnected;
        [NativeDelegate]
        internal static CS_CreateSourcePropertyDelegate CS_CreateSourceProperty;
        [NativeDelegate]
        internal static CS_CreateSourcePropertyCallbackDelegate CS_CreateSourcePropertyCallback;
        [NativeDelegate]
        internal static CS_RemoveSourcePropertyDelegate CS_RemoveSourceProperty;
        [NativeDelegate]
        internal static CS_RemoveSourcePropertyByNameDelegate CS_RemoveSourcePropertyByName;
        [NativeDelegate]
        internal static CS_CreateHTTPSinkDelegate CS_CreateHTTPSink;
        [NativeDelegate]
        internal static CS_CreateCvSinkDelegate CS_CreateCvSink;
        [NativeDelegate]
        internal static CS_CreateCvSinkCallbackDelegate CS_CreateCvSinkCallback;
        [NativeDelegate]
        internal static CS_GetSinkNameDelegate CS_GetSinkName;
        [NativeDelegate]
        internal static CS_GetSinkDescriptionDelegate CS_GetSinkDescription;
        [NativeDelegate]
        internal static CS_SetSinkSourceDelegate CS_SetSinkSource;
        [NativeDelegate]
        internal static CS_GetSinkSourcePropertyDelegate CS_GetSinkSourceProperty;
        [NativeDelegate]
        internal static CS_GetSinkSourceDelegate CS_GetSinkSource;
        [NativeDelegate]
        internal static CS_CopySinkDelegate CS_CopySink;
        [NativeDelegate]
        internal static CS_ReleaseSinkDelegate CS_ReleaseSink;
        [NativeDelegate]
        internal static CS_GrabSinkFrameDelegate CS_GrabSinkFrame;
        [NativeDelegate]
        internal static CS_GetSinkErrorDelegate CS_GetSinkError;
        [NativeDelegate]
        internal static CS_SetSinkEnabledDelegate CS_SetSinkEnabled;
        [NativeDelegate]
        internal static CS_AddSourceListenerDelegate CS_AddSourceListener;
        [NativeDelegate]
        internal static CS_RemoveSourceListenerDelegate CS_RemoveSourceListener;
        [NativeDelegate]
        internal static CS_AddSinkListenerDelegate CS_AddSinkListener;
        [NativeDelegate]
        internal static CS_RemoteSinkListenerDelegate CS_RemoteSinkListener;
        [NativeDelegate]
        internal static CS_EnumerateUSBCamerasDelegate CS_EnumerateUSBCameras;
        [NativeDelegate]
        internal static CS_FreeEnumeratedUSBCamerasDelegate CS_FreeEnumeratedUSBCameras;
        [NativeDelegate]
        internal static CS_EnumerateSourcesDelegate CS_EnumerateSources;
        [NativeDelegate]
        internal static CS_ReleaseEnumeratedSourcesDelegate CS_ReleaseEnumeratedSources;
        [NativeDelegate]
        internal static CS_EnumerateSinksDelegate CS_EnumerateSinks;
        [NativeDelegate]
        internal static CS_ReleaseEnumeratedSinksDelegate CS_ReleaseEnumeratedSinks;
        [NativeDelegate]
        internal static CS_FreeStringDelegate CS_FreeString;
        [NativeDelegate]
        internal static CS_FreeEnumPropertyChoicesDelegate CS_FreeEnumPropertyChoices;
        [NativeDelegate]
        internal static CS_FreeEnumeratedPropertiesDelegate CS_FreeEnumeratedProperties;

        internal static byte[] CreateUTF8String(string str, out UIntPtr size)
        {
            var bytes = Encoding.UTF8.GetByteCount(str);

            var buffer = new byte[bytes + 1];
            size = (UIntPtr)bytes;
            Encoding.UTF8.GetBytes(str, 0, str.Length, buffer, 0);
            buffer[bytes] = 0;
            return buffer;
        }

        internal static string ReadUTF8String(IntPtr ptr)
        {
            var data = new List<byte>();
            var off = 0;
            while (true)
            {
                var ch = Marshal.ReadByte(ptr, off++);
                if (ch == 0)
                {
                    break;
                }
                data.Add(ch);
            }
            return Encoding.UTF8.GetString(data.ToArray());
        }

        internal static IReadOnlyList<string> ReadStringArrayFromPtr(IntPtr ptr, int size)
        {
#pragma warning disable CS0618
            int ptrSize = Marshal.SizeOf(typeof(IntPtr));
#pragma warning restore CS0618
            List<string> strList = new List<string>(size);
            for (int i = 0; i < size; i++)
            {
                IntPtr stringStart = new IntPtr(ptr.ToInt64() + ptrSize * i);
                strList.Add(ReadUTF8String(stringStart));
            }
            return strList;
        }
    }
}

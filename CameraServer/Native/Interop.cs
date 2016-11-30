﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using CameraServer.Native.LibraryUtilities;
using OpenCvSharp;
using static CameraServer.Native.NativeMethods;

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


                    if (File.Exists("/usr/local/frc/bin/frcRunRobot.sh"))
                    {
                        NativeLoader = new NativeLibraryLoader();
                        // RoboRIO
                        if (s_useCommandLineFile)
                        {
                            NativeLoader.LoadNativeLibrary<Interop>(new RoboRioLibraryLoader(), s_libraryLocation, true);
                        }
                        else
                        {
                            NativeLoader.LoadNativeLibrary<Interop>(new RoboRioLibraryLoader(), resourceRoot + "roborio.libcscore.so");
                            s_libraryLocation = NativeLoader.LibraryLocation;
                        }
                    }
                    else
                    {
                        NativeLoader = new NativeLibraryLoader();
                        NativeLoader.AddLibraryLocation(OsType.Windows32,
                            resourceRoot + "x86.cscore.dll");
                        NativeLoader.AddLibraryLocation(OsType.Windows64,
                            resourceRoot + "amd64.cscore.dll");
                        NativeLoader.AddLibraryLocation(OsType.Linux32,
                            resourceRoot + "x86.libcscore.so");
                        NativeLoader.AddLibraryLocation(OsType.Linux64,
                            resourceRoot + "amd64.libcscore.so");
                        NativeLoader.AddLibraryLocation(OsType.MacOs32,
                            resourceRoot + "x86.libcscore.dylib");
                        NativeLoader.AddLibraryLocation(OsType.MacOs64,
                            resourceRoot + "amd64.libcscore.dylib");

                        if (s_useCommandLineFile)
                        {
                            NativeLoader.LoadNativeLibrary<Interop>(new RoboRioLibraryLoader(), s_libraryLocation, true);
                        }
                        else
                        {
                            NativeLoader.LoadNativeLibrary<Interop>();
                            s_libraryLocation = NativeLoader.LibraryLocation;
                        }
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
        internal delegate void CS_ProcessFrame(IntPtr data, ulong time);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_ListenerCallback(IntPtr data, ref CSEvent evnt);


        // Property Functions
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate PropertyKind CS_GetPropertyKindDelegate(int property, ref int status);

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
        internal delegate int CS_CreateUSBCameraDevDelegate(byte[] name, int dev, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_CreateUSBCameraPathDelegate(byte[] name, byte[] path,
                                         ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_CreateHTTPCameraDelegate(byte[] name, byte[] url,
                                      ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_CreateCvSourceDelegate(byte[] name, ref VideoMode mode, ref int status);

        //
        // Source Functions
        //

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate SourceKind CS_GetSourceKindDelegate(int source, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_GetSourceNameDelegate(int source, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_GetSourceDescriptionDelegate(int source, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate ulong CS_GetSourceLastFrameTimeDelegate(int source, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal delegate bool CS_IsSourceConnectedDelegate(int source, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_GetSourcePropertyDelegate(int source, byte[] name,
                                         ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_EnumerateSourcePropertiesDelegate(int source, ref int count,
                                                  ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_GetSourceVideoModeDelegate(int source, ref VideoMode mode, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal delegate bool CS_SetSourceVideoModeDelegate(int source, ref VideoMode mode, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal delegate bool CS_SetSourceVideoModeDiscreteDelegate(int source, PixelFormat pixelFormat, int width, int height, int fps, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal delegate bool CS_SetSourcePixelFormatDelegate(int source, PixelFormat pixelFormat, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal delegate bool CS_SetSourceResolutionDelegate(int source, int width, int height, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal delegate bool CS_SetSourceFPSDelegate(int source, int fps, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_EnumerateSourceVideoModesDelegate(int source, ref int count, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_EnumerateSourceSinksDelegate(int source, ref int count, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_CopySourceDelegate(int source, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_ReleaseSourceDelegate(int source, ref int status);

        // USBCameraSourceFunctions
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_GetUSBCameraPathDelegate(int source, ref int status);

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
        internal delegate void CS_SetSourceDescriptionDelegate(int source, byte[] description, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_CreateSourcePropertyDelegate(int source, byte[] name,
                                    PropertyKind type, int minimum, int maximum, int step, int defaultValue, int value,
                                    ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_SetSourceEnumPropertyChoicesDelegate(int source, int property, StringWrite[] choices, int count, ref int status);

        //
        // Sink Creation Functions
        //
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_CreateMJPEGServerDelegate(byte[] name, byte[] listenAddress, int port,
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
        internal delegate SinkKind CS_GetSinkKindDelegate(int sink, ref int status);
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

        // MJPEGServer Sink Functions
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_GetMJPEGServerListenAddressDelegate(int sink, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_GetMJPEGServerPortDelegate(int sink, ref int status);


        //
        // OpenCV Sink Functions
        //
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_SetSinkDescriptionDelegate(int sink, byte[] description, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate ulong CS_GrabSinkFrameDelegate(int sink, IntPtr image, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_GetSinkErrorDelegate(int sink, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_SetSinkEnabledDelegate(int sink, [MarshalAs(UnmanagedType.Bool)]bool enabled, ref int status);


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate int CS_AddListenerDelegate(IntPtr data, CS_ListenerCallback callback, int eventMask, int immediateNotify, ref int status);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_RemoveListenerDelegate(int handle, ref int status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_EnumerateUSBCamerasDelegate(ref int count, ref int status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_FreeEnumeratedUSBCamerasDelegate(IntPtr cameras, int count);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_EnumerateSourcesDelegate(ref int count, ref int status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_ReleaseEnumeratedSourcesDelegate(IntPtr sources, int count);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_EnumerateSinksDelegate(ref int count, ref int status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_ReleaseEnumeratedSinksDelegate(IntPtr sinks, int count);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_FreeStringDelegate(IntPtr str);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_FreeEnumPropertyChoicesDelegate(IntPtr choices, int count);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_FreeEnumeratedPropertiesDelegate(IntPtr properties, int count);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_FreeEnumeratedVideoModesDelegate(IntPtr properties, int count);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_GetHostnameDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CS_GetNetworkInterfacesDelegate(ref int count);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void CS_FreeNetworkInterfacesDelegate(IntPtr interfaces, int count);

        [NativeDelegate]
        internal static CS_GetPropertyKindDelegate CS_GetPropertyKind;
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
        internal static CS_CreateUSBCameraDevDelegate CS_CreateUSBCameraDev;
        [NativeDelegate]
        internal static CS_CreateUSBCameraPathDelegate CS_CreateUSBCameraPath;
        [NativeDelegate]
        internal static CS_CreateHTTPCameraDelegate CS_CreateHTTPCamera;
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
        [NativeDelegate]
        internal static CS_SetSinkDescriptionDelegate CS_SetSinkDescription;
        [NativeDelegate]
        internal static CS_GetSinkKindDelegate CS_GetSinkKind;
        [NativeDelegate]
        internal static CS_GetSourceKindDelegate CS_GetSourceKind;
        [NativeDelegate]
        internal static CS_GetSourceVideoModeDelegate CS_GetSourceVideoMode;
        [NativeDelegate]
        internal static CS_SetSourceVideoModeDiscreteDelegate CS_SetSourceVideoModeDiscrete;
        [NativeDelegate]
        internal static CS_SetSourcePixelFormatDelegate CS_SetSourcePixelFormat;
        [NativeDelegate]
        internal static CS_SetSourceResolutionDelegate CS_SetSourceResolution;
        [NativeDelegate]
        internal static CS_SetSourceFPSDelegate CS_SetSourceFPS;
        [NativeDelegate]
        internal static CS_EnumerateSourceVideoModesDelegate CS_EnumerateSourceVideoModes;
        [NativeDelegate]
        internal static CS_EnumerateSourceSinksDelegate CS_EnumerateSourceSinks;
        [NativeDelegate]
        internal static CS_FreeEnumeratedVideoModesDelegate CS_FreeEnumeratedVideoModes;
        [NativeDelegate]
        internal static CS_SetSourceDescriptionDelegate CS_SetSourceDescription;
        [NativeDelegate]
        internal static CS_SetSourceEnumPropertyChoicesDelegate CS_SetSourceEnumPropertyChoices;
        [NativeDelegate]
        internal static CS_CreateMJPEGServerDelegate CS_CreateMJPEGServer;
        [NativeDelegate]
        internal static CS_GetMJPEGServerListenAddressDelegate CS_GetMJPEGServerListenAddress;
        [NativeDelegate]
        internal static CS_GetMJPEGServerPortDelegate CS_GetMJPEGServerPort;
        

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
    }
}
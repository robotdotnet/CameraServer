using FRC.NativeLibraryUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FRC.CameraServer.Interop
{
    public class CsCoreHelper
    {
        public static bool LoadOnStaticInit { get; set; } = true;

        public static ICsCore? Load()
        {
            var nativeLoader = new NativeLibraryLoader();

            string[] commandArgs = Environment.GetCommandLineArgs();
            foreach (var commandArg in commandArgs)
            {
                //search for a line with the prefix "-ntcore:"
                if (commandArg.ToLower().Contains("-ntcore:"))
                {
                    //Split line to get the library.
                    int splitLoc = commandArg.IndexOf(':');
                    string file = commandArg.Substring(splitLoc + 1);

                    //If the file exists, just return it so dlopen can load it.
                    if (File.Exists(file))
                    {
                        nativeLoader.LoadNativeLibrary<ICsCore>(file, true);
                        return nativeLoader.LoadNativeInterface<ICsCore>();
                    }
                }
            }

            if (nativeLoader.TryLoadNativeLibraryPath("cscorejni"))
            {
                return nativeLoader.LoadNativeInterface<ICsCore>();
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
            return nativeLoader.LoadNativeInterface<ICsCore>();
        }

    }
}

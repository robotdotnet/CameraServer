# CameraServer

**BuildStatus**

| Windows                 |  Linux/Mac              | NuGet                 |
| ------------------------|-------------------------|-----------------------|
| [![Build status][1]][2] | Coming soon!            | [![NuGet][7]][8]      |

[1]: https://ci.appveyor.com/api/projects/status/9m0c758eqrqh8u66?svg=true
[2]: https://ci.appveyor.com/project/robotdotnet/cameraserver
[7]: https://img.shields.io/nuget/vpre/FRC.CameraServer.svg
[8]: https://www.nuget.org/packages/FRC.CameraServer

CameraServer is a .NET wrapper for the WPILib [cscore](https://github.com/wpilibsuite/cscore) library.

## Supported Platforms
CameraServer uses [FRC-OpenCvSharp](https://github.com/robotdotnet/FRC-OpenCvSharp) for its native libraries, however not all platforms currently supported by FRC-OpenCvSharp are supported by CameraServer.
* .NET 4.5 or higher
* .NET Standard 1.5
    * roboRIO (uses the FRC extension to set up native libraries)
    Support for desktop operating systems is currently a WIP, with linux being the closest

## Installation

CameraServer is included by default in WPILib templates. If needed for another system, the wrapper can be installed via NuGet with the link above. Note for any system other then the roboRIO,
you will need to add *FRC OpenCvSharp.DesktopLibraries* as a dependency manually

## Building the libraries
To build the .NET libraries, you will need .NET Core installed.
Once you have that, clone the repo, and run `.\netcore.psi -build` from a Powershell prompt on Windows

## License
See [LICENSE.txt](LICENSE.txt)

## Contributors

Thad House (@thadhouse)

[cscore](https://github.com/wpilibsuite/cscore)
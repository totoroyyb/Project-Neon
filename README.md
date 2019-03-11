# Project-Neon

## Introduction

This project just barely start, aiming to build a modern, gorgeous, and efficient media player for UWP, utilizing the Fluent Design for UI and leveraging FFmpeg to render media files. Benefit from [FFmpegInteropX](https://github.com/ffmpeginteropx/FFmpegInteropX) which is based on [Microsoft/FFmpegInterop](https://github.com/Microsoft/FFmpegInterop) 

## How To Build

*The instructions listed below are partially excerpted from README of [FFmpegInteropX](https://github.com/ffmpeginteropx/FFmpegInteropX) and  [Microsoft/FFmpegInterop](https://github.com/Microsoft/FFmpegInterop)*

### Prerequisites For Building FFmpegInteropX

- Visual Studio 2017 15.9.x

- Optional components from Visual Studio Installer:

  - Universal Windows Platform tools
  - VC++ 2017 version 15.9 v14.16 latest v141 tools
  - Win 10 SDK (10.0.15063.0) for uwp: c#, vb, js
  - Win 10 SDK (10.0.15063.0) for uwp: c++
  - Visual C++ compilers and libraries for ARM64
  - Visual C++ compilers and libraries for ARM
  - C++ UWP tools for ARM64
  - C++ runtime for UWP

### Prerequisites For Building Project-Neon

*	Visual Studio 2017
*	Windows 10 SDK 1809 (10.0.17763.0) and Windows 10 SDK 1803 (10.0.17134.0)
*	[FFmpegInteropX](https://github.com/ffmpeginteropx/FFmpegInteropX)

### Build

* First, Get the source code of Project-Neon

  You can click Download on the web page or clone it by command lines

	git clone https://github.com/totoroyyb/Project-Neon.git

* After downloading the source file, cd to the root directory of solution and then clone the source files of FFmpegInterop

	git clone --recursive https://github.com/ffmpeginteropx/FFmpegInteropX.git

* Installing ffmpeg build tools

  Now that you have the FFmpeg source code, you can follow the instructions on how to [build FFmpeg for WinRT](https://trac.ffmpeg.org/wiki/CompilationGuide/WinRT) apps. *Follow the setup instruction very carefully to avoid build issues!! Be very careful not to miss a single step. If you have problems building ffmpeg, go through these steps again, since chances are high that you missed some detail.*

* Building ffmpeg with Visual Studio 2017

  After installing the ffmpeg build tools, you can invoke `BuildFFmpeg_VS2017.bat` from a normal cmd prompt. It builds all Windows 10 versions of ffmpeg (x86, x64, ARM and ARM64). 

  Note: You need Visual Studio 2017 15.9.0 or higher to build the ARM64 version of ffmpeg!

* Building the FFmpegInterop Library

  After building ffmpeg with the steps above, you should find the ffmpeg libraries in the `ffmpeg/Build/<platform\>/<architecture\>` folders.

  Now you can build the FFmpegInterop library. 

  Simply open the Visual Studio solution file `FFmpegInterop.sln`, set one of the MediaPlayer[CS/CPP/JS] sample projects as StartUp project, and run. FFmpegInterop should build cleanly giving you the interop object as well as the selected sample MediaPlayer (C++, C# or JS) that show how to connect the MediaStreamSource to a MediaElement or Video tag for playback.

* So far, you should have done all the steps for building FFmpegInteropX and Project-Neon. Since the reference of ffmpeg library has been added into the configuration file of Project-Neon, you can right click the build button and run it.

## Contribution
Any contributions are all appreciated by making pull requests.

Thanks for all your help.

## Contact and Issue
If you find any issues, please feel free to open a issue or just contact me by emailing. 

Here is my email address: [totoroq@outlook.com](mailto:totoroq@outlook.com)

## License
This project is under license of **Apache-2.0**, for detail, please see [LICENSE](LICENSE).

## Libraries used in this project
| Library | License |
| :-----: | :-----: |
| [FFmpegInteropX](https://github.com/ffmpeginteropx/FFmpegInteropX) | [Apach-2.0](http://www.apache.org/licenses/LICENSE-2.0) |
| [Microsoft/FFmpegInterop](https://github.com/Microsoft/FFmpegInterop) | [Apach-2.0](https://github.com/Microsoft/FFmpegInterop/blob/master/LICENSE) |
| [FFmpeg/FFmpeg](https://github.com/FFmpeg/FFmpeg) | [LGPL(partially GPL)](https://github.com/FFmpeg/FFmpeg/blob/master/LICENSE.md) |


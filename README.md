# Project-Neon

## Introduction

This project just barely start, aiming to build a modern, gorgeous, and effcient media player for UWP, utilizing the Fluent Design for UI and leveraging FFmpeg to render media files. Benefit from FFmpegInterop made by Microsoft, for detail please see [Microsoft/FFmpegInterop](https://github.com/Microsoft/FFmpegInterop)

## How To Build

### Prerequisite
*	Visual Studio 2017
*	Windows 10 SDK (10.0.17134.0)
*	Windows 10 SDK (10.0.16299.0)
* [Microsoft/vcpkg](https://github.com/Microsoft/vcpkg) (Optional)
* [Microsoft/FFmpegInterop](https://github.com/Microsoft/FFmpegInterop) (Have been included in this project)

### Build
Since I've included [Microsoft/FFmpegInterop](https://github.com/Microsoft/FFmpegInterop) in this project, what we need to do is to compile the ffmpeg libraries which are required by [Microsoft/FFmpegInterop](https://github.com/Microsoft/FFmpegInterop).

* Choice 1 **(Recommanded)** : Compiling libraries by using [Microsoft/vcpkg](https://github.com/Microsoft/vcpkg)  
  * Step 1 : Clone [Microsoft/vcpkg](https://github.com/Microsoft/vcpkg) to local
  
    ```
    > git clone https://github.com/Microsoft/vcpkg
    > cd vcpkg
    ```
  * Step 2 : Bootstrap
  
    ```
    PS> .\bootstrap-vcpkg.bat
    Ubuntu:~/$ ./bootstrap-vcpkg.sh
    ```
  * Step 3 : Hook up user-wide integration, run (note: requires admin on first use)
  
    ```
    PS> .\vcpkg integrate install
    Ubuntu:~/$ ./vcpkg integrate install
    ```
  * Step 4 : Install ffmpeg (x86, x64) package with  
    ***Note: ARM version somehow cannot be built correctly.***
    ```
    PS> .\vcpkg install ffmpeg:x86-uwp ffmpeg:x64-uwp
    Ubuntu:~/$ ./vcpkg install ffmpeg:x86-uwp ffmpeg:x64-uwp
    ```
    ***This process may takes a really long time, please be patient.***
  * Step 5 : After the libraries compilation complete, just go to Visual Studio and build it. Good luck!!
         
  
* Choice 2 : Compile libraries by the method built in [Microsoft/FFmpegInterop](https://github.com/Microsoft/FFmpegInterop).  
    For detail, please see [here](https://github.com/Microsoft/FFmpegInterop#prerequisites)  
    Please follow the instruction and compile all the libraries required by [Microsoft/FFmpegInterop](https://github.com/Microsoft/FFmpegInterop).  
    Good luck!!
  
  
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
| [Microsoft/FFmpegInterop](https://github.com/Microsoft/FFmpegInterop) | [Apach-2.0](https://github.com/Microsoft/FFmpegInterop/blob/master/LICENSE) |
| [FFmpeg/FFmpeg](https://github.com/FFmpeg/FFmpeg) | [LGPL(partially GPL)](https://github.com/FFmpeg/FFmpeg/blob/master/LICENSE.md) |

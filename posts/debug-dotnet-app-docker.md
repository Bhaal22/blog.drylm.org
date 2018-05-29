<!--
.. title: Debug dotnet applications inside a docker container
.. slug: debug-dotnet-applications-inside-a-docker-container
.. date: 2018-02-10 09:26:25 UTC
.. tags: docker, csharp, .net, powershell, c#, fuslogvw
.. category: devops
.. link: 
.. description: Debug dotnet applications inside a docker container
.. type: text
-->

From time to time, as a dotnet developer we have to face this specific error **Could not load file or assembly 'Assembly Strong Name' or one of its dependencies**.

To help debugging this particular situation, Microsoft provides a tool in the .net SDK called: *fuslogvw*.

<!-- TEASER_END -->

This is a graphical tool located in: **%Microsoft SDKs%\Windows\%SDKVersion\bin\%NETFXTools%**.

On my system, the path is: **C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools**.

![fuslogvw](/galleries/fuslogvw/fuslogvw.png)

From this user interface run as an administrator (otherwise you cannot configure anything), you can:
 
  * enable / disable assembly loading tracing
  * enable various flavors of tracing such as: bind failures, alll binds, ...
  * configure a custom path for the logs to be written on disk

This is perfect and really usefull.

After some research, it appears this tool only sets some registry keys to enable/disable the features.

```
reg add "HKLM\Software\Microsoft\Fusion" /v LogPath /t REG_SZ /d "c:\\your_log_path"
reg add "HKLM\Software\Microsoft\Fusion" /v LogFailures /t REG_DWORD /d 0x1
reg add "HKLM\Software\Microsoft\Fusion" /v ForceLog /t REG_DWORD /d 0x1
```

Since I do not really want to set/unset those values by hand everytime, I thought it could be useful to have a docker container image with those params set so that we could debug assembly loading from inside a docker container and generating the logs on a mounted volume.

You can checkout and clone this [github repository](https://github.com/Bhaal22/fuslogvw-docker)
Dockerfile is really simple:

```
FROM microsoft/windowsservercore
LABEL maintainer="muller.john@gmail.com"

RUN reg add "HKLM\Software\Microsoft\Fusion" /v LogPath /t REG_SZ /d "c:\\fuslogvw" && \
    reg add "HKLM\Software\Microsoft\Fusion" /v LogFailures /t REG_DWORD /d 0x1 && \
    reg add "HKLM\Software\Microsoft\Fusion" /v ForceLog /t REG_DWORD /d 0x1
```

The way to build and run the image

```
docker build --rm -t fuslog:latest .
docker run --rm -v $FolderOfYourProject:c:\PathToYourProject -v $pwd\logs:c:\fuslogvw \
        -ti fuslog:latest c:\PathToYourProject\your\executable

docker run --rm -v $pwd\example:c:\example -v $pwd\logs:c:\fuslogvw \
        -ti fuslog:latest c:\example\SimpleJsonConverter.exe
```

As soon as you run your container, you will get outputs in your $pwd\logs folder and you can inspect them and search for the root cause issue.

<!--
.. title: Rabbitmq and Erlang/OTP Windows Containers
.. slug: rabbitmq_windows_container
.. date: 2018-05-29 18:49:32 UTC
.. tags: docker, dockerfile, .net, powershell
.. category: devops
.. link: 
.. description: Windows Docker images for erlang and rabbitmq
.. type: text
-->

Here at [Gsx Solutions](http://www.gsx.com) for our full product suite, we highly rely on [RabbitMQ](https://www.rabbitmq.com/). Until last release we rely on version 3.5.4 we delivered to our customers. Since, this version has been recently been deprecated, we decided to upgrade our final package to [v3.7.4](https://github.com/rabbitmq/rabbitmq-server/releases/tag/v3.7.4).

<!-- TEASER_END -->

Then, started internal non regression tests. We are fully windows based, then we installed v3.5.4, ran a couple of micro services around the BUS and then upgrade to v3.7.4. We have found a [regression related to 3.5.4 -> 3.7.4 upgrade](https://github.com/rabbitmq/rabbitmq-server/issues/1568). But this process was a bit painful. That's why I decided to build rabbitmq docker images running in [Windows Containers](https://docs.docker.com/docker-for-windows/)

Let's have a look at the anatomy of the docker files. First, I wanted to prepare base images for [erlang-otp](http://www.erlang.org/).
I host erlang-otp dockerimages on this [github repository](https://github.com/gsx-solutions/erlang-otp-win)

As you may know, there are 2 flavors of windows base docker images. You cannot start your image *from scratch* like on linux system.

  * windowsservercore
  * nanoserver
  
The main difference between those 2 images is mostly the weigth of running and pulling such an image.
In the edition 1803:

  * windowsservercore: 1.58GB compressed / 3.61GB on disk
  * nanoserver       : < 100 MB compressed and on disk
  
One of the main removal between windowsservercore and nanoserver is powershell.
This is the main reason why the images I will show you are [multi stages docker images](https://docs.docker.com/develop/develop-images/multistage-build/) in order to bootstrap image creation in a *windowsservercore* with all administration capabilities and end up in *nanoserver* to be the lighter we can.

Here is the *erlang-otp 18.0* base dockerfile definition:

```dockerfile
FROM microsoft/windowsservercore:1709 as root
LABEL maintainer="muller.john@gmail.com"

ENV erlang_download_url="http://erlang.org/download/otp_win64_18.0.exe" \
    ERLANG_HOME=c:\\erlang

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# download and install erlang using silent install option, and remove installer when done
RUN Invoke-WebRequest -Uri $env:erlang_download_url -OutFile erlang_install.exe ; \
    Start-Process -Wait -FilePath .\erlang_install.exe -ArgumentList /S, /D=$env:ERLANG_HOME ; \
Remove-Item -Force erlang_install.exe

FROM microsoft/nanoserver:1709
LABEL maintainer="muller.john@gmail.com"

ENV ERLANG_HOME=c:\\erlang
COPY --from=root c:\\erlang c:\\erlang
COPY .erlang.cookie c:\\windows

RUN icacls c:\windows\.erlang.cookie /grant %USERNAME%:F

COPY --from=root C:\\windows\\system32\\msvcp100.dll C:\\windows\\system32
COPY --from=root C:\\windows\\system32\\msvcr100.dll C:\\windows\\system32
COPY --from=root C:\\windows\\system32\\vcomp100.dll C:\\windows\\system32
```

The version for *erlang-otp 20.3* is very similar, the only difference is in the url to download the otp msi and v20.0 does not need c++ runtime v100 from Visual Studio 2010.

```dockerfile
FROM microsoft/windowsservercore:1709 as root
LABEL maintainer="muller.john@gmail.com"

ENV erlang_download_url="http://erlang.org/download/otp_win64_20.3.exe" \
    ERLANG_HOME=c:\\erlang

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# download and install erlang using silent install option, and remove installer when done
RUN Invoke-WebRequest -Uri $env:erlang_download_url -OutFile erlang_install.exe ; \
    Start-Process -Wait -FilePath .\erlang_install.exe -ArgumentList /S, /D=$env:ERLANG_HOME ; \
    Remove-Item -Force erlang_install.exe


FROM microsoft/nanoserver:1709
LABEL maintainer="muller.john@gmail.com"

ENV HOMEDRIVE=c:\\ \
    HOMEPATH=Users\\ContainerUser \
    ERLANG_HOME=c:\\erlang

COPY --from=root c:\\erlang c:\\erlang
COPY .erlang.cookie c:\\windows

RUN icacls c:\windows\.erlang.cookie /grant %USERNAME%:F
```

Please note, when we build the final container using the instruction **FROM microsoft/nanoserver:1709**, all the **COPY** inbstructions refer to the step0 image specifying **--form=root** where *root* is the name of the first stage.

And then to build those images:

```
docker build --rm -t otp-nano:18.0 -f .\nanoserver\Dockerfile.18.0 .\nanoserver\
docker build --rm -t otp-nano:20.3 -f .\nanoserver\Dockerfile.20.3 .\nanoserver\
```

And now, you have 2 images called:
  * otp-nano:18.0
  * otp-nano:20.3

So now, based on those 2 base images we will build *rabbitmq* docker images. erlang-base images could be reused for something else after all :).
Nothing so fency for those *rabbitmq* images:
For v3.5.4:

```dockerfile
FROM microsoft/windowsservercore:1709 as root
LABEL maintainer="muller.john@gmail.com"

ENV RMQ_VERSION="3.5.4" \
    rabbit_download_url="https://www.rabbitmq.com/releases/rabbitmq-server/v3.5.4/rabbitmq-server-windows-3.5.4.zip"

# setup powershell options for RUN commands
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# download and extract rabbitmq, and remove zip file when done
RUN Invoke-WebRequest -Uri $env:rabbit_download_url -OutFile rabbitmq.zip ; \
        Expand-Archive -Path .\rabbitmq.zip -DestinationPath "c:\\" ; \
        Remove-Item -Force rabbitmq.zip; \
        Rename-Item c:\rabbitmq_server-$env:RMQ_VERSION c:\rabbitmq

FROM gsxsolutions/erlang-otp-nano:18.0
LABEL maintainer="muller.john@gmail.com"

COPY --from=root c:\\rabbitmq c:\\rabbitmq

# tell rabbitmq where to find our custom config file
ENV RABBITMQ_CONFIG_FILE="c:\rabbitmq" \
    RABBITMQ_BASE="c:\\rmq-data"

VOLUME ${RABBITMQ_BASE}

COPY start.cmd c:\\scripts\\start.cmd

RUN ["cmd", "/c", "echo [{rabbit, [{loopback_users, []}]}].> c:\\rabbitmq.config"]

RUN mkdir c:\Users\%USERNAME%\\AppData\\Roaming\\RabbitMQ

# Ports
# 4369: epmd, a peer discovery service used by RabbitMQ nodes and CLI tools
# 5672: used by AMQP 0-9-1 and 1.0 clients without TLS
# 5671: used by AMQP 0-9-1 and 1.0 clients with TLS
# 25672: used by Erlang distribution for inter-node and CLI tools communication and is allocated from a dynamic range (limited to a single port by default, computed as AMQP port + 20000).
# 15672: HTTP API clients and rabbitmqadmin (only if the management plugin is enabled)
EXPOSE 4369 5671 5672 25672 15672

# run server when container starts - container will shutdown when this process ends
CMD Scripts\\start.cmd
```

and for v3.7.4 and v3.7.5 (only difference is in the url where to download the package)

```dockerfile
FROM microsoft/windowsservercore:1709 as root
LABEL maintainer="muller.john@gmail.com"

ENV RMQ_VERSION="3.7.5" \
    rabbit_download_url="https://github.com/rabbitmq/rabbitmq-server/releases/download/v3.7.5/rabbitmq-server-windows-3.7.5.zip"

# setup powershell options for RUN commands
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# download and extract rabbitmq, and remove zip file when done
RUN [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; \
    Invoke-WebRequest -Uri $env:rabbit_download_url -OutFile rabbitmq.zip ; \
    Expand-Archive -Path .\rabbitmq.zip -DestinationPath "c:\\" ; \
    Remove-Item -Force rabbitmq.zip; \
    Rename-Item c:\rabbitmq_server-$env:RMQ_VERSION c:\rabbitmq

FROM gsxsolutions/erlang-otp-nano:20.3
LABEL maintainer="muller.john@gmail.com"

COPY --from=root c:\\rabbitmq c:\\rabbitmq

# tell rabbitmq where to find our custom config file
ENV RABBITMQ_CONFIG_FILE="c:\rabbitmq" \
    RABBITMQ_BASE="c:\\rmq-data"

VOLUME ${RABBITMQ_BASE}

COPY start.cmd c:\\scripts\\start.cmd

RUN ["cmd", "/c", "echo [{rabbit, [{loopback_users, []}]}].> c:\\rabbitmq.config"]

RUN mkdir c:\Users\%USERNAME%\\AppData\\Roaming\\RabbitMQ

# Ports
# 4369: epmd, a peer discovery service used by RabbitMQ nodes and CLI tools
# 5672: used by AMQP 0-9-1 and 1.0 clients without TLS
# 5671: used by AMQP 0-9-1 and 1.0 clients with TLS
# 25672: used by Erlang distribution for inter-node and CLI tools communication and is allocated from a dynamic range (limited to a single port by default, computed as AMQP port + 20000).
# 15672: HTTP API clients and rabbitmqadmin (only if the management plugin is enabled)
EXPOSE 4369 5671 5672 25672 15672

# run server when container starts - container will shutdown when this process ends
CMD Scripts\\start.cmd
```

In those images, we use the same technic of multi stage build. so that in the initial *From* instruction, we have access to powershell and all fancy commands like : 
  * Invoke-WebRequest
  * Expand-Archive
  * Start-Process
  
As you may notice, for v3.7.4 and v3.7.5, the archives are stored on github. PoerShell fully relies on .net and the .net framework installed on those images does not accept any Tls1.2 connection by default that's why you first must enable it by running:

```powershell
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12;
```

In the final stat, we enable a volume by declaring it:

```
VOLUME ${RABBITMQ_BASE}
```

Then a default ocnfiguration file is created and we boostrap a script when the container bootstraps:

```bat
@echo off

echo "my-cookie" > c:\Windows\.erlang.cookie
if not exist %RABBITMQ_BASE%\enabled_plugins (
    call c:\rabbitmq\sbin\rabbitmq-plugins.bat enable rabbitmq_management --offline
)

call c:\rabbitmq\sbin\rabbitmq-server.bat
```

This script enables the management-plugin and start rabbitmq server.

```
docker build -t rmq:3.7.5 -f .\Dockerfile.3.7.5 .

docker volume create rmq-data

docker run --rm -h my-rabbit1 -v rmq-data:c:\rmq-data -ti rmq:3.7.5
```

Here we are we have now a beautiful *rabbitmq* server running on demand and totally isolated.
Now, we a named volume, we can easily make upgrade scenarios since the data will be persisted accross the container instances.


Stay tuned!



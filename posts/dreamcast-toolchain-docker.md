<!-- 
.. title: Dreamcker: Dreamcast development environment in a docker container
.. slug: dreamcast-toolchain-docker
.. date: 2016-11-01 22:10:43 UTC
.. tags: dreamcast, kallisti os, kos, docker, container, homebrew, dreamcker
.. category: programing
.. link: 
.. description: Dreamcker provides all dreamcast development related tools to build homebrew games on linux
.. type: text
-->

Introduction
===

The motivation behind this little project is to provide a unified way to get the Dreamcast toolchain and development tools.
<!-- TEASER_END -->

  * gcc toolchain : gcc + binutils for SH4 and ARM
  * [Kallisti OS](http://cadcdev.sourceforge.net/softprj/kos/)
  * [KOS Ports](https://sourceforge.net/p/cadcdev/kos-ports/ci/master/tree/)
  * [DC Load Ip](https://sourceforge.net/p/cadcdev/dcload-ip/ci/master/tree/)
  * [DC Load Serial](https://sourceforge.net/p/cadcdev/dcload-serial/ci/master/tree/)


To unify this build, I created a repository on Github with the related submodules : [Dreamcker](https://github.com/Drylm/dc-dev)


Docker Images
===

This project is 3 different docker images to make the full build :

  * dc-toolchain : Build the gcc toolchains for sh4-elf and arm
  * dc-kos : Build KallistiOS / KOS Ports and dcload-tools
  * dc-dev : Contain all binaries to compile dreamcast binaries and related tools

You can build the image by yourself or use the docker image from [Docker Hub](https://hub.docker.com/r/drylm/dc-dev/) or do

> docker pull drylm/dc-dev


Then you can run your container :

> docker run -v path/to/your/source:/src/your_project -it drylm/dc-dev:latest

Then you have your source project mounted under **/src/your_project** folder. Lets do your homebrew projects !

Feel free to submit pull requests !

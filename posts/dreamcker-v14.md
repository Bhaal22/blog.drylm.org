<!--
.. title: dreamcker v1.4
.. slug: dreamcast-toolchain-docker-dreamcker-v14
.. date: 2020-05-27 21:57:53 UTC
.. tags: dreamcast, kallisti os, kos, docker, container, homebrew, dreamcker
.. category: programming
.. link: 
.. description: Dreamcker provides all dreamcast development related tools to build homebrew games on linux
.. type: text
-->

Introduction
===

Here is the [previous article](/posts/dreamcast-toolchain-docker/) I wrote about Dreamcker.

[Dreamcker](https://github.com/Drylm/dc-dev) ([Docker Hub](https://hub.docker.com/repository/docker/drylm/dc-dev)) is a docker container with all required tools to build dreamcast games.
<!-- TEASER_END -->

```bash
docker pull drylm/dc-dev:1.4
docker run --rm  -v /path/to/your/project:/src/project -it drylm/dc-dev:1.4 /bin/bash

#inside container
root@ba9049d7f5f6:/src/project# make
```

What's new
===

In the last weeks dreamcker has been slightly improved:

  * kos has been updated to the last version ([commit](https://github.com/KallistiOS/KallistiOS/commit/eb77357a703e07af08ba538f20b3d2fe8252c3f4))
  * libraries contain an old school libkglx issued from this [project](https://github.com/Drylm/CubicVR)
  * makefile rules have been added to prepare cdi image. If user wants to load his project in an emulator such a [Reicast](https://github.com/reicast/reicast-emulator)

```Makefile
prepare-burn:
	sh-elf-objcopy -R .stack -O binary $(TARGET) $(STRIP)
	mkdir -p $(BURN_FOLDER)/pre
	scramble $(STRIP) $(BURN_FOLDER)/pre/$(SCRAMBLE)
	genisoimage -C 0,11702 -V $(PROJECT) -G $(IP) -r -J -l -o $(BURN_FOLDER)/$(ISO) $(BURN_FOLDER)/pre
	cdi4dc $(BURN_FOLDER)/$(ISO) $(BURN_FOLDER)/$(CDI)
```

**IP.BIN** is not included in this release for rights reasons.


docker image may be run in 2 different ways:
  
  * in interactive shell

```bash
docker run --rm  -v /path/to/your/project:/src/project -it drylm/dc-dev:1.4 /bin/bash
```

  * like an executable

```bash
docker run --rm  -v /path/to/your/project:/src/project -it drylm/dc-dev:1.4

# calls automatically the scripts: /src/build.sh
```

[Source code](https://github.com/Drylm/dc-dev/blob/master/build-resources/runtime/build.sh):

```bash
#!/bin/bash

ROOT="/usr/local/dc"

. ${ROOT}/environ_runtime.sh

pushd /src/project

make all

popd
```

I do have a pet project: [DCSI](https://github.com/Drylm/dcsi) which compiles and run~ish (heavy refactoring at the moment).


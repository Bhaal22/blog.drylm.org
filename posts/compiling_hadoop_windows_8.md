<!-- 
.. title: How-to compile Hadoop on Windows 8
.. slug: how-to-compile-hadoop-on-windows-height
.. date: 2015-02-02 00:00:00 UTC
.. tags: howto, hadoop, java, big data, windows 8
.. link: 
.. description: How to compile hadoop on windows 8
.. type: text
-->


### Steps ###

 * Have at least Visual Studio 2010 installed
 * Install [JDK 1.7](http://www.oracle.com/technetwork/java/javase/downloads/jdk7-downloads-1880260.html) or [OpenJDK](http://openjdk.java.net/install/)
 * Install Maven 3.0+
 * Grab last [Hadoop 2.6 source distribution](http://apache.crihan.fr/dist/hadoop/common/hadoop-2.6.0/)

<!-- TEASER_END -->

###Java Installation###
After launching executable installation file just set **JAVA_HOME** environment variable :

> JAVA_HOME=$java_installation_dir
###Build Hadoop Distribution###

 * add maven binary directory into your **PATH** environment variable
 In a Visual Studio command prompt :
 >cd hadoop-2.6.0-src
 >mvn package -Pdist,native-win -DskipTests -Dtar

 The last command compiles java libraries **AND** binary dynamic libraries. Thats why maven command **MUST** be launches in a Visual Studio compiler shell.
Then you should have some issues like this one :

>[ERROR] Failed to execute goal org.apache.hadoop:hadoop-maven-plugins:2.4.0:protoc (compile-protoc) on project hadoop-common: org.apache.maven.plugin.MojoExecutionException: "protoc --version" did not return a version -> [Help 1]

 Indeed some dependencies are missing :
 > Protoc : https://developers.google.com/protocol-buffers/docs/downloads
Hadoop 2.6.0 is dependant of 2.5.0 version. You must download the [exact version](https://protobuf.googlecode.com/files/protoc-2.5.0-win32.zip)

Then Hadoop native build is based on [CMake](http://www.cmake.org/). Personally I installed it via [Chocolatey](https://chocolatey.org/)
> choco install cmake

 Then I relaunched maven command line and everything built perfectly.




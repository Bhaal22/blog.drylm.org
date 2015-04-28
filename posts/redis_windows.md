<!-- 
.. title: Redis running on Windows
.. slug: redis_on_windows
.. date: 2015-04-26 00:00:00 UTC
.. tags: devops,redis,windows,nosql
.. link: 
.. description:How to run and build Redis on Windows
.. type: text
-->

###Introduction###
[Redis](http://redis.io) is the famous NoSQL Key-Value store with a very simple interface :

 * Get / Set : on simple key/values
 * HGet / HSet : on Hashtable

You can find the full list of commands located [here](http://redis.io/commands).
Then you can really try easily the command line interface through a [website](http://try.redis.io/).

<!-- TEASER_END -->

Redis is running natively on Linux or other *Nix systems but there is a group at Microsoft working on porting Redis on Windows and mainly on Microsoft Azure. Microsoft added recently the distributed cache service on their Azure platform with Redis.

### Compile ###

 * Have at least Visual Studio 2010 installed
 * Build Debug / Build Release

### Differences Linux / Windows ###

The main difference in running Redis on Windows, it that by design, Windows does not have native ***fork*** process API. This process is used when dumping Redis content to file system when performing dumps. Regarding this [issue](https://github.com/MSOpenTech/redis/issues/83) on Github :

  *Redis uses the fork() UNIX system API to create a point-in-time snapshot of the data store for storage to disk. This impacts several features on Redis: AOF/RDB backup, master-slave synchronization, and clustering. Windows does not have a fork-like API available, so we have had to simulate this behavior by placing the Redis heap in a memory mapped file that can be shared with a child(quasi-forked) process. By default we set the size of this file to be equal to the size of physical memory. In order to control the size of this file we have added a maxheap flag.* 

So redis on Windows is generating a file :

 - RedisQFork.dat : 500 Mb for a 32 bits version
 - RedisQFork.dat : Size of RAM for a 64 bits version

On a particular development at [GSX](http://gsx.com) we used [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis) c# client library and we encountered some little strange behaviors on Redis commands execution timeout occuring some times. After researchs, we found out it was due to the time of dumping memory on disk while the fork process.

This process does not seem linear with the amount of memory. Check out this question I opened : 
https://github.com/MSOpenTech/redis/issues/233

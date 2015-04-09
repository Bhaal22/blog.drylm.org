<!-- 
.. title: Redis running on Windows
.. slug: redis_on_windows
.. date: 2015-04-01 00:00:00 UTC
.. tags: devops,redis,windows
.. link: 
.. description:
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
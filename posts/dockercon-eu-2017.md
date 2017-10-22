<!--
.. title: DockerCon EU 2017
.. slug: dockercon-eu-2017
.. date: 2017-10-22 15:35:13 UTC
.. tags: docker, devops, conference, container
.. category: conference
.. link: 
.. description: Brief feedback from DockerCon2017@Copenhagen 
.. type: text
-->

DockerCon EU 2017
==

I was quite excited by going to DockerCon 2017 EU and following in depth technical talks about Containers / Orchestration / Networking / Firewalls and Micro Services.
And to be honest, I have been really satisfied by meeting smart people doing really nice things.

All videos are available at this adress: https://dockercon.docker.com/

Day 1
----

The Keynote starts and then we fully understand where does Docker wants to go: *Leading the Legacy Application run cycle* with what they call the *MTA* program: 
  * Modernize Traditional Application
  
The demo basically takes an **already** tomcat developped legacy application, and transform it as a docker container using the MTA CLI integrated in docker-ee platform.
Well ... ok ... not really crazy to be honest ... any web application can already be easily dockerized ...

The second part of the keynote is THE announcement from **Hykes Solomon** and is still related to  docker-ee platform: End user will have choice between **Swarm** and **Kubernetes** aka **k8s** in
the docker stack orchestration solution. I have to say this is really a great announcement. **k8s** is a bit complex to deploy initially, it will definitely improves its adoption by the community. 
With the recent work from Google with the **istio** project based on top of **k8s**, I see this orchestration solution as the more pronising for the next years.

When Keynote has been over, I have been attending the black belt talks for almost the whole day:
  * What have syscalls done for you from Liz Rice
  * LinuxKit DeepDive from Rolf Neugebauer & Justin Cormack
  * Container-relevant Uptream Kernel developments from Tycho Andersen
  * Modernizing .Net Apps from Elton Stoneman and Iris Classon
  
In fact I am really interested by what docker team had to say about Modernizing legacy application. They never spoke about limitations due to the underlying system on which the application 
itself is built on. I am involved in Microsoft Windows containers introduction in my company. Our technology stack is quite old and is based on technology which will never been implemented in
any **windowsservercore** or **nanoservercore**:
  * MSMQ
  * .net Remoting
  
I am prety sure I am not the only one in this situation and the modernization for free will never work for any of us. But docker team never mentionned that.
MSSQL server dockerization is moreover only available from SQL Server 2017 CTP1 on both linux and windows container technology.


Day 2
---

The keynote on Day 2 was about partnership ... and to be honest, was boring ... docker team continued the speech about Security and **MTA** via the docker-ee platform.
Regaridng the sessions I have been following:
  * Kernel Native Security & DDOS Mitigation for Microservices with BPF from Cynthia Thomas
  * Deeper Dive in docker Overlay Networks from Laurent Bernaille
  * Lookging under the hood: Containerd from Scott Coulton
  * 



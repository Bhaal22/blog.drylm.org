<!-- 
.. title: Developpement Dreamcast part1.
.. slug: developpement-dreamcast-part1
.. date: 2014-10-17 19:57:36 UTC
.. tags: dreamcast, c, c++, linux, gcc, toolchain
.. link: 
.. description: 
.. type: text
-->


Mise en place et &eacute;tude du d&eacute;veloppement Dreamcast
==

*Jonathan Muller* - muller.john@gmail.com</p>

C'est lors d'une install party &agrave; Metz que j'ai rencontr&eacute; une personne qui lan&ccedil;ait un rendu de Tux sur un &eacute;cran g&eacute;ant &agrave; partir de sa Dreamcast. C'est de l&agrave; que m'est venue l'id&eacute;e de d&eacute;velopper pour cette console que j'adore.

## 1. Introduction &agrave; la Dreamcast##

<p class="normal">Qui ne conna&icirc;t pas des titres comme SoulCalibur, Skies of Arcadia, Grandia 2, Sonic adventure, ou encore le magnifique Shenmue .. Vous me suivez, je vais vous parler de la Dreamcast.
Derni&egrave;re console en titre de Sega(tm), cette merveilleuse console &agrave; la mort pr&eacute;matur&eacute;e fait encore parler d'elle au sein d'une communaut&eacute; tr&egrave;s active sur Internet.
En effet, de nombreuses personnes ont d&eacute;cid&eacute; de programmer des jeux, des utilitaires, des &eacute;mulateurs sur cette console. Je pourrais citer le tr&egrave;s c&eacute;l&egrave;bre ScummVM
disponible &eacute;galement sur PC et qui permet de lancer la plupart des jeux de Lucas Art(tm) (Monkey Island, Day of the tantacle, Sam &amp; Max ...), ou encore des &eacute;mulateurs (cpc, megadrive,
n&eacute;o-g&eacute;o en cours de d&eacute;veloppement).<br /> Parmi les jeux <span class="menu">Homebrew</span>, pour ma part, deux se d&eacute;tachent du lot : Feet of fury [1] et Alice Dreams [2] .
On peut dire que la communaut&eacute; est vraiment productive et motiv&eacute;e.
J'en arrive au but de cet article : mettre en place un environnement de compilation/d&eacute;veloppement pour Dreamcast au sein d'un syst&egrave;me GNU/Linux. Des packs windows existent pour l'environnement
de d&eacute;veloppement dev-cpp, mais je n'aborderai pas ce type d'installation.<br />Avant toute explication concernant l'installation de l'environnement,
passons en revue quelques informations techniques de la console.</p>

##2. Historique et Hardware##

<p class="normal"><img src="Dreamcast.png">///insertion image : Dreamcast.png///</p>
<p class="legende">Figure 1&nbsp;: La Dreamcast, sa manette et la carte m&eacute;moire</p>

<p class="normal">Sortie en novembre 1998 au Japon, la Dreamcast est disponible en Europe et aux Etats-unis &agrave; partir de No&euml;l 1999. Sega(tm) frappe fort avec sa nouvelle console 128 bits (contre 32
pour la playstation).</p>

 Composant                       | Description|
 -----------------               | ---------------------------- | ------------------
 `'Micro-Processeur'`            | RISC Hitachi SH4 128 bits 200 Mhz, 360 MIPS, 1,4 GFLOPS |
 `Processeur graphique`          | NEC PowerVR (plus de 3 millions de polygones par secondes)|
 `Processeur sonore`             | Yamaha RISC 32 bits (64 canaux ADPCM) |
 `M&eacute;moire`                | 16 Mo (64Mbit SDRAMx2) |
 `Syst&egrave;me d'exploitation` | Version optimis&eacute;e de Windows CE |
 `Lecteur CD`                    | 12x |
 `Couleurs`                      | 16,7 millions |
 `Carte m&eacute;moire`          | V.M.S. |
 `Taille`                        | 190 x 195 x 78 mm |


<p class="normal">A ce sujet, il existe un tr&egrave;s bon article paru en 2004, disponible sur le site de jeux videos : jeuxvideos.com [3].</p>



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

##3. Introduction##

<p class="normal">Le d&eacute;veloppement amateur pour les diff&eacute;rentes consoles s'appelle : le <span class="menu">Homebrew</span>.
Il est bien &eacute;videmment possible de programmer sur Dreamcast &agrave; condition de disposer des bons outils : </p>

<p class="normal">* un kit de d&eacute;veloppement ; </p>
<p class="normal">* un moyen de transf&eacute;rer les&nbsp;donn&eacute;es du PC vers la console afin de tester les programmes que nous aurons &eacute;crits.</p>

<p class="normal"> Les diff&eacute;rentes manipulations seront d&eacute;taill&eacute;es ci-apr&egrave;s. Fort heureusement, il existe de nombreux logiciels disponibles sur Dreamcast. Le m&eacute;canisme de cr&eacute;ation d'un logiciel est le suivant : </p>

<p class="normal"> * compiler son projet avec la biblioth&egrave;que KOS (Kallisti OS) [4] &eacute;crite par Dan Potter. Des snapshots Subversion sont disponibles ici [5] (cf la proc&eacute;dure d'installation est d&eacute;taill&eacute;e dans le chapitre suivant) ;</p>
<p class="normal"> * 3 choix s'offrent &agrave; nous pour tester notre programme :</p>
<p class="normal">&nbsp;&nbsp;&nbsp; * envoyer le programme via le port s&eacute;rie de la Dreamcast. Pour ce faire, il faudra se fabriquer ou s'acheter un <span class="menu">coder cable</span> ;</p>
<p class="normal">&nbsp;&nbsp;&nbsp; * rendre le binaire "bootable" au format Dreamcast, le graver et mettre le CD dans le lecteur de la console ;</p>
<p class="normal">&nbsp;&nbsp;&nbsp; * rendre le binaire "bootable" et le tester sur un &eacute;mulateur. Malheureusement, il existe peu d'&eacute;mulateurs. Un seul est disponible : Chankast [6] qui apparemment fonctionnerait plut&ocirc;t bien, l'inconv&eacute;nient majeur &eacute;tant qu'il n'est disponible que sous Windows. J'ai bien essay&eacute; de le lancer via Wine [7], mais sans r&eacute;el succ&egrave;s.</p>

<p class="normal">Avant de passer &agrave; la partie technique de l'article, quelques informations p&eacute;cuni&egrave;res pour ceux qui souhaitent se lancer dans le d&eacute;veloppement Dreamcast : </p>

<p class="normal"> - une console d'occasion (manette incluse) se trouve pour une cinquantaine d'euros chez n'importe quel revendeur ;</p>
<p class="normal"> - le <span class="menu">coder cable</span> se trouve sur diff&eacute;rents sites sp&eacute;cialis&eacute;s pour vingt, vingt-cinq euros environ. </p>


<p class="normal">Au final, on peut se construire une plateforme de d&eacute;veloppement on ne peut plus int&eacute;ressante pour soixante dix euros environ. Passons maintenant aux choses s&eacute;rieuses : l'installation de l'environnement compilation/d&eacute;veloppement sur notre GNU/Linux pr&eacute;f&eacute;r&eacute;.</p>



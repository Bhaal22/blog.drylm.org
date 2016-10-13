<!-- 
.. title: Developpement Dreamcast part1.
.. slug: developpement-dreamcast-part1
.. date: 2014-10-17 19:57:36 UTC
.. tags: dreamcast, gcc, toolchain, linux, C++, kallistios, kos, kallisti os
.. category: programming
.. link: 
.. description: Tutoriel de la mise en place d'un environnement de programmation dreamcast 
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
n&eacute;o-g&eacute;o en cours de d&eacute;veloppement).<br /> Parmi les jeux <span class="menu">Homebrew</span>, pour ma part, deux se d&eacute;tachent du lot : Feet of fury et Alice Dreams.
On peut dire que la communaut&eacute; est vraiment productive et motiv&eacute;e.
J'en arrive au but de cet article : mettre en place un environnement de compilation/d&eacute;veloppement pour Dreamcast au sein d'un syst&egrave;me GNU/Linux. Des packs windows existent pour l'environnement
de d&eacute;veloppement dev-cpp, mais je n'aborderai pas ce type d'installation.<br />Avant toute explication concernant l'installation de l'environnement,
passons en revue quelques informations techniques de la console.</p>

<!-- TEASER_END -->

##2. Historique et Hardware##

<img src="/galleries/dreamcast/dreamcast.png" />
<p class="legende">Figure 1&nbsp;: La Dreamcast, sa manette et la carte m&eacute;moire</p>

<p class="normal">Sortie en novembre 1998 au Japon, la Dreamcast est disponible en Europe et aux Etats-unis &agrave; partir de No&euml;l 1999. Sega(tm) frappe fort avec sa nouvelle console 128 bits (contre 32
pour la playstation).</p>

| Composant                       | Description|
|:-----------------               |:----------------------------
| `'Micro-Processeur'`            | RISC Hitachi SH4 128 bits 200 Mhz, 360 MIPS, 1,4 GFLOPS |
| `Processeur graphique`          | NEC PowerVR (plus de 3 millions de polygones par secondes)|
| `Processeur sonore`             | Yamaha RISC 32 bits (64 canaux ADPCM) |
| `M&eacute;moire`                | 16 Mo (64Mbit SDRAMx2) |
| `Syst&egrave;me d'exploitation` | Version optimis&eacute;e de Windows CE |
| `Lecteur CD`                    | 12x |
| `Couleurs`                      | 16,7 millions |
| `Carte m&eacute;moire`          | V.M.S. |
| `Taille`                        | 190 x 195 x 78 mm |


<p class="normal">A ce sujet, il existe un tr&egrave;s bon article paru en 2004, disponible sur le site de jeux videos : jeuxvideos.com.</p>

##3. Introduction##

Le d&eacute;veloppement amateur pour les diff&eacute;rentes consoles s'appelle : le <strong>Homebrew</strong>. Il est bien &eacute;videmment possible de programmer sur Dreamcast &agrave; condition de disposer des bons outils :

- un kit de d&eacute;veloppement ; </p>
- un moyen de transf&eacute;rer les&nbsp;donn&eacute;es du PC vers la console afin de tester les programmes que nous aurons &eacute;crits.</p>

Les diff&eacute;rentes manipulations seront d&eacute;taill&eacute;es ci-apr&egrave;s. Fort heureusement, il existe de nombreux logiciels disponibles sur Dreamcast. Le m&eacute;canisme de cr&eacute;ation d'un logiciel est le suivant :

- compiler son projet avec la biblioth&egrave;que KOS (Kallisti OS) &eacute;crite par Dan Potter. Des snapshots Subversion sont disponibles ici (cf la proc&eacute;dure d'installation est d&eacute;taill&eacute;e dans le chapitre suivant) ;
- 3 choix s'offrent &agrave; nous pour tester notre programme :
 - envoyer le programme via le port s&eacute;rie de la Dreamcast. Pour ce faire, il faudra se fabriquer ou s'acheter un <span class="menu">coder cable</span> ;
 - rendre le binaire "bootable" au format Dreamcast, le graver et mettre le CD dans le lecteur de la console ;
 - rendre le binaire "bootable" et le tester sur un &eacute;mulateur. Malheureusement, il existe peu d'&eacute;mulateurs. Un seul est disponible : Chankast qui apparemment fonctionnerait plut&ocirc;t bien, l'inconv&eacute;nient majeur &eacute;tant qu'il n'est disponible que sous Windows. J'ai bien essay&eacute; de le lancer via Wine, mais sans r&eacute;el succ&egrave;s.</p>

<p class="normal">Avant de passer &agrave; la partie technique de l'article, quelques informations p&eacute;cuni&egrave;res pour ceux qui souhaitent se lancer dans le d&eacute;veloppement Dreamcast : </p>

 - une console d'occasion (manette incluse) se trouve pour une cinquantaine d'euros chez n'importe quel revendeur ;</p>
 - le <span class="menu">coder cable</span> se trouve sur diff&eacute;rents sites sp&eacute;cialis&eacute;s pour vingt, vingt-cinq euros environ. </p>


Au final, on peut se construire une plateforme de d&eacute;veloppement on ne peut plus int&eacute;ressante pour soixante dix euros environ. Passons maintenant aux choses s&eacute;rieuses : <strong>l'installation de l'environnement compilation/d&eacute;veloppement sur notre GNU/Linux pr&eacute;f&eacute;r&eacute;</strong>.

##4. Kallisti Operating system (KOS)

<strong>KOS</strong> est l'acronyme de Kallisti Operating System qui sera d&eacute;sign&eacute; ainsi dans la suite de l'article. Ce syst&egrave;me d'exploitation est consid&eacute;r&eacute; comme &eacute;tant <span class="menu">la</span> r&eacute;f&eacute;rence pour le d&eacute;veloppement Dreamcast. C'est un pseudo syst&egrave;me temps r&eacute;el pour les consoles de jeux vid&eacute;os :

 - GBA (Game Boy Advance)</p>
 - Ps2
 - Dreamcast

Son &eacute;tat d'avancement &eacute;tant le plus important, principalement gr&acirc;ce au grand nombre de gens investis sur cette console, le port Dreamcast est le plus utilis&eacute;. Il peut &ecirc;tre assimil&eacute; &agrave; un noyau, comme Linux ou BSD, que nous allons utiliser via une biblioth&egrave;que (libkallisti.a) qui sera li&eacute;e &agrave; nos propres programmes. Il est aussi possible d'&eacute;crire de nouveaux modules qui seront li&eacute;s dynamiquement &agrave; notre programme. Nous pouvons alors envisager d'ajouter de nouveaux syst&egrave;mes de fichiers, des pilotes de p&eacute;riph&eacute;riques, etc. Les possibilit&eacute;s sont infinies.
KOS &eacute;tant un pseudo syst&egrave;me temps r&eacute;el, il est &eacute;vident qu'il poss&egrave;de des syst&egrave;mes de fichiers particuliers. En effet, on peut : 

 - lire sur le cd-rom (acc&egrave;s par /cd) ;
 - &eacute;crire sur l'ordinateur reli&eacute; par le <span class="menu">coder cable</span> (acc&egrave;s par /pc ; pratique pour faire des impressions d'&eacute;cran) ;
 - lire sur le romdisk (acc&egrave;s par /rd). Ce syst&egrave;me de fichiers est directement li&eacute; au binaire du programme lors de la compilation (sa taille est limit&eacute;e &agrave; 16 Mo).

KOS dispose aussi de pilotes de p&eacute;riph&eacute;riques pour le clavier, la souris, les manettes et la carte r&eacute;seau (10/100Mbits). Assez de th&eacute;orie, passons &agrave; la pratique avec l'installation et l'utilisation sur quelques exemples de cette fameuse biblioth&egrave;que.
Tout d'abord, r&eacute;cup&eacute;rons les sources de KOS : 

 - [kos-ports](http://gamedev.allusion.net/svn/snapshots/kos-ports-snapshot-20050618.tar.bz2) : port de diff&eacute;rentes biblioth&egrave;ques (jpeg, png, ogg, mp3, ...) 
 - [kos-snapshot](http://gamedev.allusion.net/svn/snapshots/kos-snapshot-20050618.tar.bz2) : sources de Kos


Il faut tout d'abord mettre en place un syst&egrave;me de compilation crois&eacute;e (cross-compiler ou toolchain en anglais) afin d'offrir la possibilit&eacute; de compiler sur PC et d'ex&eacute;cuter sur Dreamcast. Deux compilateurs crois&eacute;s seront n&eacute;cessaires :

-  le premier afin de g&eacute;n&eacute;rer du code pour le processeur SH-4 ;
- le second afin de g&eacute;n&eacute;rer du code pour le processeur ARM charg&eacute; de s'occuper du son.

<strong>Attention</strong> :&nbsp;un probl&egrave;me connu des&nbsp;versions de gcc sup&eacute;rieures &agrave; 3.0.4 nous obligent &agrave; utiliser une version 3.0.4 ou inf&eacute;rieure pour le cross-compilateur &agrave; destination du processeur&nbsp;ARM. En ce qui concerne le compilateur crois&eacute; &agrave; destination du processeur SH-4, j'ai utilis&eacute; une version 3.4.3 avec des patchs&nbsp; concernant la gestion des threads (ces derniers seront fournis dans une archive .tar.gz). Une mise en place incorrecte de ces&nbsp;deux compilateurs crois&eacute;s vous assurera de magnifiques <strong>kernel panic</strong>, pour peu que&nbsp;vos programmes utilisent les biblioth&egrave;ques sonores.
Liste des archives &agrave; r&eacute;cup&eacute;rer&nbsp;: 

- gcc-core-3.4.3.tar.bz2
- gcc-g++-3.4.3.tar.bz2
- gcc-core-3.0.4.tar.gz (pour ARM)
- newlib-1.12.0.tar.gz
-  binutils-2.15.tar.bz2
- binutils-2.11.2.tar.gz (pour ARM)
- kos-ports-snapshot-20050618.tar.bz2 kos-snapshot-20050618.tar.bz2

Je vous mets &agrave; disposition un script servant &agrave; construire les compilateurs crois&eacute;s &agrave; partir de ces archives : dc-chain-0.1.tgz (les patchs y sont inclus)

Copier alors ce script dans le r&eacute;pertoire de votre convenance, par&nbsp; exemple&nbsp;<span class="menu">/tmp/testKOS.</span>

<pre>
<strong>tar</strong> xzf dc-chain-0.1.tgz ;
<strong>cd</strong> dc-chain-0.1 ;
copier "gcc-core-3.4.3.tar.bz2" "gcc-g++-3.4.3.tar.bz2" "newlib-1.12.0.tar.gz" "binutils-2.15.tar.bz2" "kos-ports-snapshot-20050618.tar.bz2" "kos-snapshot-20050618.tar.bz2" dans ce r&eacute;pertoire ;
 for in `ls -1 *.bz2`; do tar xjf $i; done;
<strong>tar</strong> xzf newlib-1.12.0.tar.gz
</pre>

###4.1 Modification du Makefile###

Modifier le makefile, notamment les chemins d'installation des compilateurs crois&eacute;s : variables <span class="menu">sh_prefix</span> et <span class="menu">arm_prefix</span>, par exemple : 

- sh_prefix  := /tmp/testKOS/dc-chain-0.1/dc/$(sh_target)
- arm_prefix := /tmp/testKOS/dc-chain-0.1/dc/$(arm_target)

<p class="normal">Renseigner&nbsp;les bonnes versions des logiciels &agrave; compiler pour les variables : <span class="menu">binutils_ver</span>, <span class="menu">gcc_ver</span>, <span class="menu">newlib_ver</span>.<br /> Enfin, mettre &agrave; jour la variable : <span class="menu">kos_root</span> (cette variable repr&eacute;sente le chemin o&ugrave; se trouvent les r&eacute;pertoires de KallistiOS. En l'occurence, si vous avez suivi la manipulation ci-dessus, la valeur de cette variable est : <span class="menu">/tmp/testKOS/dc-chain-0.1</span>.</p>

<strong>Attention</strong> : j'ai not&eacute; des probl&egrave;mes de compilation lorque j'essayais de construire le toolchain en utilisant gcc-4.0 ou sup&eacute;rieur. C'est pour cela que je force l'utlisation de gcc-3.4 et g++-3.4 en assignant les variables CC et CXX dans le shell courant de la mani&egrave;re suivante :</p>

- export CC=/usr/bin/gcc-3.4 ;</p>
- export CXX=/usr/bin/g++-3.4 ;</p>

Je vous laisse le soin d'adapter ces manipulations en fonction de&nbsp;la configuration de votre syst&egrave;me. Une fois ces diff&eacute;rents r&eacute;glages effectu&eacute;s, entrer les commandes suivantes :

- mkdir dc (r&eacute;pertoire qui va accueillir&nbsp;les binaires des compilateurs) ;
- make patch ;
- make build-sh4 (vous pouvez aller vous chercher un petit caf&eacute;)

Pour construire le toolchain ARM, il suffit de changer dans le makefile, les versions de gcc de 3.4.3 vers 3.0.4 et de binutils de 2.15 &agrave; 2.11.2 et de lancer :

- make arm-build (environ 5 minutes sur un Athlon 3000+)</p>

Les deux toolchains sont maintenant construits, nous allons passer &agrave; la compilation proprement dite de KOS (ouf !) :

<pre>
  <strong>cd</strong> kos
  <strong>cp</strong> doc/environ.sh.sample ./environ.sh
  &eacute;diter environ.sh&nbsp;avec votre &eacute;diteur de texte pr&eacute;f&eacute;r&eacute;
     - placer dans la&nbsp;variable KOS_BASE le chemin o&ugrave; est d&eacute;compress&eacute; le snapshot de KOS, par exemple : export KOS_BASE="/tmp/testKOS/dc-chain-0.1/kos/"
    - placer dans la variable KOS_CC_BASE le r&eacute;pertoire d'installation du compilateur sh-4, par exemple : export KOS_CC_BASE="/tmp/testKOS/dc-chain-0.1/dc/sh-elf"
    - KOS_CC_PREFIX= sh-elf</p>
    - DC_ARM_BASE = export DC_ARM_BASE="/tmp/testKOS/dc-chain-0.1/dc/arm-elf"
    - DC_ARM_PREFIX = arm-elf

./environ.sh (inclus toutes ces variables dans le shell courant).
  </pre>

Les variables que nous avons renseign&eacute;es sont utilis&eacute;es par KOS&nbsp;non seulement pour la compilation de la biblioth&egrave;que, mais aussi &agrave; chaque compilation de projets utilisant KOS. Il faudra donc r&eacute;aliser cette manipulation &agrave; chaque compilation,&nbsp; ou alors inclure ces variables dans son shell.


#4.2 Compiler KOS#

Il suffit de taper : make et v&eacute;rifier que tout se passe bien ;)

#4.3 Compiler les biblioth&egrave;ques#

Pour &ecirc;tre standard, l'id&eacute;al est de placer l'arborescence de ports de biblioth&egrave;ques au m&ecirc;me niveau que le r&eacute;pertoire d'installation de KOS. Dans mon exemple, j'ai compil&eacute; KOS dans <span class="menu">/tmp/testKOS/dc-chain-0.1/kos/</span>, j'ai donc plac&eacute; les biblioth&egrave;ques dans <span class="menu">/tmp/testKOS/dc-chain-0.1/kos-port</span>.

<pre>
tar xjf kos-ports-snapshot-20050409.tar.bz2
cd kos-ports
ln -s ../kos/addons/Makefile
make
</pre>

<p class="normal">Apr&egrave;s avoir&nbsp;construit un environnement de d&eacute;veloppement &agrave; destination de la console, nous allons mettre en place les outils de communication PC/Dreamcast.</p>


#4.4 Transfert de fichiers vers la Dreamcast#

Le moyen le plus facile pour transf&eacute;rer des donn&eacute;es entre le PC et la Dreamcast&nbsp;est de se procurer ou fabriquer ce que l'on appelle un <span class="menu">coder cable</span>. Des informations sur sa fabrication sont disponibles sur&nbsp;cette page.

N'ayant aucune connaissance en &eacute;lectronique, je me le suis achet&eacute;. C'est un simple c&acirc;ble s&eacute;rie, nous aurons donc besoin d'outils logiciels de transfert entre le pc et la console, &agrave; savoir : dcload/dc-tool. Le site sur lequel se trouvent ces programmes est un site majeur de l'activit&eacute; autour de notre console bien aim&eacute;e. On y trouve toutes formes de sources, binaires, documentation ayant un proche rapport avec la console. 
La premi&egrave;re phase est de graver un cd-rom et de d&eacute;marrer dessus avec sa Dreamcast. Ce CD-ROM met la console en &eacute;coute sur le port s&eacute;rie. Cette derni&egrave;re s'occupe une fois le transfert termin&eacute;, d'ex&eacute;cuter le programme. La Dreamcast joue alors le r&ocirc;le de serveur et le pc le r&ocirc;le de client. T&eacute;l&eacute;chargeons alors les programmes respectifs :

<h3>4.4.1 C&ocirc;t&eacute; serveur :</h3>

<pre>
  wget http://www.boob.co.uk/files/dcload-1.0.3-1st_read.zip</p>
  unzip dcload-1.0.3-1st_read.zip</p>
  wget http://www.boob.co.uk/files/dcload-1.0.3.tar.gz</p>
  unzip dcload-1.0.3.tar.gz</p>
  cd dcload-1.0.3/make-cd</p>
  ajuster le makefile
</pre>

<strong>Attention</strong>, le fichier Makefile est assez ancien. Pour cr&eacute;er les pistes du cd-rom,&nbsp;une commande bas&eacute;e sur cdrecord est utilis&eacute;e. Il va falloir mettre &agrave; jour ce Makefile, car sur les noyaux de la g&eacute;n&eacute;ration 2.6, l'&eacute;mulation SCSI n'est plus utilis&eacute;e. Personnellement, j'ai modifi&eacute; ainsi : <strong>CDRECORD = cdrecord dev=ATAPI:0,0,0 speed=8</strong></span> et j'ai ajust&eacute; le chemin du fichier <span class="menu">1st_read.bin</span>. N'h&eacute;sitez pas &agrave; lire&nbsp;la section de l'article concernant la gravure de binaires afin de&nbsp;comprendre ce que nous sommes en train de faire ;).</p>

- make (les fichiers .bin sont dans la premi&egrave;re archive pr&eacute;c&eacute;demment t&eacute;l&eacute;charg&eacute;e).</p>

Pour ceux qui n'ont pas envie de faire cette manipulation, je mets &agrave; disposition l'image ISO de ce cd-rom de boot.

<h3>4.4.2 C&ocirc;t&eacute; client :</h3>

<pre>
  wget http://www.boob.co.uk/files/dc-tool-serial-1.0.3-linux.gz;
  gunzip dc-tool-serial-1.0.3-linux.gz
  chmod +x dc-tool-serial-1.0.3-linux
</pre>

Pour&nbsp;utiliser le programme c&ocirc;t&eacute; client : <strong>./dc-tool-linux -x monFichier.elf</strong>
Vous pouvez maintenant&nbsp; transf&eacute;rer des donn&eacute;es du pc vers la Dreamcast.

<strong>Attention</strong> :&nbsp; il faut que l'utilisateur ex&eacute;cutant le programme <span class="menu">dc-tool-serial-1.0.3-linux</span> ait des droits d'&eacute;criture sur le p&eacute;riph&eacute;rique <span class="menu">/dev/ttyS0</span> (p&eacute;riph&eacute;rique utilis&eacute; par d&eacute;faut). Pour voir les param&egrave;tres utilis&eacute;s par <span class="menu">dc-tool-serial-1.0.3-linux</span>, il suffit de le lancer sans aucun argument, ce qui fait appara&icirc;tre&nbsp;ceci :

<pre>
john@Odyssee$ dc-tool-serial-1.0.3-linux 

dc-tool 1.0.3 by <andrewk@napalm-x.com>

-x <filename> Upload and execute <filename>
-u <filename> Upload <filename>
-d <filename> Download to <filename>
-a <address>  Set address to <address> (default: 0x8c010000)
-s <size>     Set size to <size>
-t <device>   Use <device> to communicate with dc (default: /dev/ttyS0)
-b <baudrate> Use <baudrate> (default: 57600)
-e            Try alternate 115200 (must also use -b 115200)
-n            Do not attach console and fileserver
-p            Use dumb terminal rather than console/fileserver
-q            Do not clear screen before download
-c <path>     Chroot to <path> (must be super-user)
-i <isofile>  Enable cdfs redirection using iso image <isofile>
-h            Usage information (you're looking at it)
</pre>


La partie installation de l'environnement est enfin achev&eacute;e. La premi&egrave;re fois, j'en ai eu pour quelques heures,&nbsp;mais quel bonheur lorsque j'ai lanc&eacute; mon premier programme sur la console! Nous allons&nbsp;en cr&eacute;er quelques uns dans la partie suivante. Nous arrivons enfin aux choses passionnantes :).

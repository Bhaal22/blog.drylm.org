<!--
.. title: OpenLDAP + Blowfish hashes
.. slug: openldap_blowfish
.. date: 2019-03-22 23:57:03 UTC
.. tags: openldap, blowfish
.. category: devops
.. link: 
.. description: How to make openldap working with blowfish hashes
.. type: text
-->

I have been recently faced to a problem regarding password encryption and support of the encryption in [OpenLDAP](https://www.openldap.org/). Working on an authentication project migration, we have passwords encrypted using [Blowfish](https://en.wikipedia.org/wiki/Blowfish_(cipher)), we have to migrate them under OpenLdap.

<!-- TEASER_END -->

# Readings
  * [ldap-password-hash](https://www.redpill-linpro.com/techblog/2016/08/16/ldap-password-hash.html)
  * [phc-sf-spec.md](https://github.com/P-H-C/phc-string-format/blob/master/phc-sf-spec.md)


Based on the first article, we understand *OpenLDAP* can perform authentication with blowfish encrypted passwords using the glibc capabilities and after some investigations, it turns out, *slapd* deamon is directly using libcrypt shared object to perform the comparison of the hashed passwords.

By doing some more investigations, we have found orignal *libcrypt* has poor support for password encryption such as Blowfish. Then I found **libxcrypt**
  * https://github.com/besser82/libxcrypt


**libxcrypt** is a modern library for one-way hashing of passwords. It supports a wide variety of both modern and historical hashing methods: yescrypt, gost-yescrypt, scrypt, bcrypt, sha512crypt, sha256crypt, md5crypt, SunMD5, sha1crypt, NT, bsdicrypt, bigcrypt, and descrypt.

On Linux-based systems, by default libxcrypt will be binary backward compatible with the *libcrypt.so.1* shipped as part of the GNU C Library. This means that all existing binary executables linked against glibc’s libcrypt should work unmodified with this library’s libcrypt.so.1.

Then I did think about repolacing symbolic link from libcrypt pointing to libxcrypt instead. To achieve that, I did try the package from the disctribution but it did not work due to **GLIBC_VERSION**. Then, let's compile this library and deploy it:

# Pre-requisites

```shell
apt-get install autotools libtool make
```

```shell
    cd /tmp
    wget https://github.com/besser82/libxcrypt/archive/v4.4.3.tar.gz
    tar xzf v4.4.3.tar.gz
    cd libxcrypt-4.4.3/
    ./bootstrap && ./configure && make
    cp .libs/libcrypt.so.1.1.0 /lib/x86_64-linux-gnu
    rm /lib/x86_64-linux-gnu/libcrypt.so.1 && ln -s /lib/x86_64-linux-gnu/libcrypt.so.1.1.0 /lib/x86_64-linux-gnu/libcrypt.so.1
```

Be careful, if by chance you screw the link, the linux **login** will fail. Means, no more sudo or any other tool required by authentication.
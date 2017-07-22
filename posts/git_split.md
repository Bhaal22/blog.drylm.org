<!-- 
.. title: Import part or full repository to another repository
.. slug: import-part-or-full-repository-to-another-repository
.. date: 2017-07-22 00:00:00 UTC
.. tags: git, split repository
.. link: 
.. description: how to import sub folder of a git repository with full history
.. type: text
-->

Import part of git repository to another repository in order to keep history of the imported files.

<!-- TEASER_END -->

Suppose you have 2 projects (A and B) and you want to move some files from a A to B without losing history of the files.
Prepare the subfolder you are interested to import in B


```
pushd A
git subtree split -P <name-of-folder> -b <name-of-new-branch>
popd
```

Ready to import?

```
pushd /path/to/project/B
mkdir -p folder/to/import/other/repository
git remote add projectA /path/to/project/A
git fetch projectA
git read-tree --prefix=folder/to/import/other/repository -u projectA/$branch_name
git merge -s ours --allow-unrelated-histories --no-commit projectA/$branch_name
 
... review if needed
git commit
```

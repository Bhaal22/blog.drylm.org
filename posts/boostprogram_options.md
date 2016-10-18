<!-- 
.. title: boost::program_options
.. slug: boostprogram_options
.. date: 2014-10-13 04:05:32 UTC
.. tags: C++,boost,tutorial,programming
.. link: 
.. description: Brief tutorial on how to use boost::program_options library 
.. type: text
-->

boost::program_options
===

**boost::program_options** is part of the popular C++ general boost library. This particular library is dedicated to manage command line arguments for your C++ programs.

[boost::program_options](http://www.boost.org/doc/libs/1_56_0/doc/html/program_options.html)

Command line parameters are described at the start of your program and are used like any other command line parameters at Unix-style

<!-- TEASER_END -->
### Example
```
#include <boost/program_options.hpp>

int main (int argc, char **argv)
{
	namespace po = boost::program_options;
	po::options_description description("Hash Generator usage");
	  description.add_options()
	    ("help", "show help")
	    ("level", po::value<int>(), "Recursive level");
	    
	return 0;
}
```

We described 2 paramaters. The well known **help** option, and an integer parameter.
This piece of code used like this :

    ./main --help
    ./main --level 10
    
### Short parameters
If you want to provide short parameters like `./main -h` it is very easy. Just need to modify the previous code with :

```
...
("help,h", "show help")
...
```

###Default value
Of course we can provide default value for a specific flag. Suppose you want to have by default your recursive level set to 5.

```
...
("level", po::value<int>()->default_value(5), "Recursive level")
...
```

###Multiple values
Suppose now for a specific flag, you would like to be able to specify multiple values such as `--flag f1 f2 f3`. Easy ! Just need to specifiy a `std::vector`

```
("file_pattern", po::value<std::string>()->default_value(".*"), "Pattern on which performing checksum")
```
Very easy !

##Description is good ! Is it difficult to use it ?
Answer is No ! Of course.
```
po::variables_map vm;
po::parsed_options parsed =
      po::command_line_parser(argc,argv).options(parameters).allow_unregistered().run();
```

And to check in your program, it a parameter is set or not :

```
int level;
if (vm.count("level"))
{
    level = vm["level"].as<int>();
}
```

That's all !

##Conclusion
Parsing command line arguments is always boring since we started coding in early ages ! 
boost::program_options is then a very elegant library for your C++ programs.

Hope you will use it.

# BoostTestAddin

BoostTestAddin is a Visual Studio 2013 addin that allows to run a test case or a whole test suite written with the boost test framework from inside Visual Studio without using the command line.

Installation
------------

Download *'BoostTest-x.x.x.vsix'*, double click on it and follow the instructions. 

Usage
-----

After installation, a new menu named *'BoostTest'* should appear in the Visual Studio menu bar. 

![screenshot](https://github.com/Abc-Arbitrage/BoostTestAddin/blob/master/img/screenshot.png)

To run a test case, open the source file containing the test, place the cursor inside the unit test and select *'Run current test'*.

The project containing the test will first be set a *'Startup project'*, built and then run with the appropriate command line arguments.

License
-------

The source code is distributed under the [MIT License](http://opensource.org/licenses/MIT).


Credits
-------

- [Famfamfam](http://www.famfamfam.com/) icons
- [nunit](www.nunit.org/)
- [moq](https://github.com/Moq/moq4)

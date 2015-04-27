# Description

Typically in build templates you specify by convention to run your [Machine.Specification](http://github.com/machine/machine.specifications) specs. If you happen to have a mixed processor architecture amongst your assemblies, some of these will fail depending on wether or not you're running with the X64 or X86 version of the runner. 

This fixes that.


# Usage

Put the MSpecRunnerWrapper.exe in the same folder as you have the console runner. Point your build script to this executable instead of any of the others. Any command line arguments will be forwarded to the correct runner. All output will come back to standard output and error for this process, allthough losing colors unfortunately. 
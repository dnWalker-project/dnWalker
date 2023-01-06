# dnWalker
A tool performing concolic execution on a .NET program. The project is based on [MoonWalker](https://fmt.ewi.utwente.nl/tools/moonwalker/), a model checking tool for .NET applications created at Univeristy of Twente. MoonWalker provides the model checking and execution of a .NET program - it basically serves as a .NET runtime executing the IL instructions - a .NET runtime built on top of .NET runtime. The project seems to have been abandonded in 2009. To bring it to the 21st century I removed the Mono dependency and included the incredible `dnLib` library.

## Why?
> Concolic testing (a portmanteau of concrete and symbolic) is a hybrid software verification technique that performs symbolic execution, a classical technique that treats program variables as symbolic variables, along a concrete execution (testing on particular inputs) path. Symbolic execution is used in conjunction with an automated theorem prover or constraint solver based on constraint logic programming to generate new concrete inputs (test cases) with the aim of maximizing code coverage. Its main focus is finding bugs in real-world software, rather than demonstrating program correctness.

## Libs used
* dnlib https://github.com/0xd4d/dnlib
* z3 https://github.com/Z3Prover/z3
* C5 https://github.com/sestoft/C5
* QuikGraph https://github.com/KeRNeLith/QuikGraph

# dnWalker
A tool performing concolic execution on a .NET program. The project is based on [MoonWalker](https://fmt.ewi.utwente.nl/tools/moonwalker/), a model checking tool for .NET applications created at Univeristy of Twente. MoonWalker provides the model checking and execution of a .NET program - it basically serves as a .NET runtime executing the IL instructions - a .NET runtime built on top of .NET runtime. The project seems to have been abandonded in 2009. To bring it to the 21st century I removed the Mono dependency and included the incredible `dnLib` library.

## Why?
> Concolic testing (a portmanteau of concrete and symbolic) is a hybrid software verification technique that performs symbolic execution, a classical technique that treats program variables as symbolic variables, along a concrete execution (testing on particular inputs) path. Symbolic execution is used in conjunction with an automated theorem prover or constraint solver based on constraint logic programming to generate new concrete inputs (test cases) with the aim of maximizing code coverage. Its main focus is finding bugs in real-world software, rather than demonstrating program correctness.

## Libs used
* dnlib https://github.com/0xd4d/dnlib
* z3 https://github.com/Z3Prover/z3
* C5 https://github.com/sestoft/C5
* QuikGraph https://github.com/KeRNeLith/QuikGraph

## Try it out
Proper user friendly interface is not setup. Check out `dnWalker.IntegrationTests` and `dnWalker.Tests.Examples` for features. The script `build-examples.bat` should be run before to ensure all versions of the `Examples` library are built. 

The project `dnWalker.Benchmarks` which can be run using script `run-benchmarks.ps1`, it consumes file `benchamrk_methods.txt` (specifies which methods should be explored). Run the `build-examples.bat` beforehand.

### dnWalker
Runs in interactive and batch mode using the following commands:
* `load assembly <a1> <a2> ...` : loads the assemblies (.dll) specified by paths
* `load models <m1> <m2> ...` : loads the models (.xml) specified by paths, must run after
* `explore <method specifier> <output file>` : specify the method using the following format: `<namespace>[.<declaring type>].<type>::<method name>[(<parameter types>)]`
* `exit` finishes the session

The explore command runs concolic exploration of the specified method and stores the output in the specifed XML file.

### dnWalker.TestWriter
The interface is not ready, checkout the project `dnWalker.Benchmarks` for test generation.

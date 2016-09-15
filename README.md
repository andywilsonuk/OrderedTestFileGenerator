# OrderedTestFileGenerator
A command line application to generate an mstest OrderedTest file based on an mstest assembly DLL file and category sequence.

As part of a CI pipeline, automated regression tests (browser-based or otherwise) are executed to ensure that a check-in hasn't 
broken the build. These tests should stop as soon as one test fails and should be executed from most valuable to least valuable; for
example there is little point in testing the order placement process if the homepage isn't loading.

```
Usage
  -a  The file paths to the assembly DLLs containing the mstests. Required.
  -o  The file path be to used for the generated Ordered Test file. Required.

  Categories (at least one must be specified):
  -c  The sequence of categories for ordered tests (semi-colon separated).
  -x  The file path of a linebreak separated list of categories.

  Optional:
  -p  Specify to append tests which don't match any of the specified 
      categories to the end of the ordered test file.

Examples
1.  OrderedTestFileGenerator.exe -a "C:\TestAssembly.dll" -o "C:\all.orderedtest" -c Smoke;Critical -p
2.  OrderedTestFileGenerator.exe -a "C:\TestAssembly.dll" -a "C:\TestAssembly2.dll" -o "C:\all.orderedtest" -x "C:\categories.txt"
```

The OrderedTest file can be passed mstest to execute, for example:
```
"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\mstest.exe" /testcontainer:"C:\all.orderedtest" /resultsfile:"C:\results.trx"
```

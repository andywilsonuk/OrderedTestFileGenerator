using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OrderedTestFileGenerator
{
    class AssemblyParser
    {
        public IEnumerable<TestDefinition> AllTests(FileSystemInfo assemblyToParse)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyToParse.FullName);
            return AllTests(assembly);
        }

        public IEnumerable<TestDefinition> AllTests(Assembly assemblyToParse)
        {
            FileSystemInfo assemblyPath = assemblyToParse.FilePath();
            return from methodInfo in assemblyToParse.AllTestMethods()
                   where !methodInfo.IsIgnored()
                   select new TestDefinition
                   {
                       QualifiedName = methodInfo.QualifiedName(),
                       MethodName = methodInfo.Name,
                       Categories = methodInfo.Categories(),
                       AssemblyFile = assemblyPath,
                   };
        }
    }
}

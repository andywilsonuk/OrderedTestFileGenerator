using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OrderedTestFileGenerator
{
    class AssemblyParser
    {
        public IEnumerable<TestDefinition> AllTests(FileInfo assemblyToParse)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyToParse.FullName);
            return All2015Tests(assembly).Concat(All2017Tests(assembly));
        }

        public IEnumerable<TestDefinition> All2015Tests(Assembly assembly)
        {
            return new TestAssembly2015().AllTests(assembly);
        }

        public IEnumerable<TestDefinition> All2017Tests(Assembly assembly)
        {
            return new TestAssembly2017().AllTests(assembly);
        }
    }
}

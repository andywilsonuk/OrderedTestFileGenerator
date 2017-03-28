using System.Collections.Generic;
using System.Reflection;

namespace OrderedTestFileGenerator
{
    internal interface TestAssembly
    {
        IEnumerable<TestDefinition> AllTests(Assembly assemblyToParse);
    }
}

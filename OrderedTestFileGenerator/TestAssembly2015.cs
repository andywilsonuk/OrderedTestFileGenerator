extern alias vs2015;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using vs2015.Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrderedTestFileGenerator
{
    internal class TestAssembly2015 : TestAssembly
    {
        public IEnumerable<TestDefinition> AllTests(Assembly assemblyToParse)
        {
            FileSystemInfo assemblyPath = assemblyToParse.FilePath();
            return from methodInfo in assemblyToParse.AllTestMethods<TestMethodAttribute>()
                   where !IsIgnored(methodInfo)
                   select new TestDefinition
                   {
                       QualifiedName = methodInfo.QualifiedName(),
                       MethodName = methodInfo.Name,
                       Categories = Categories(methodInfo),
                       AssemblyFile = assemblyPath,
                       AssemblyBinding = "Microsoft.VisualStudio.TestTools.TestTypes.Unit.UnitTestElement, Microsoft.VisualStudio.QualityTools.Tips.UnitTest.ObjectModel, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
                   };
        }


        private bool IsIgnored(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes<IgnoreAttribute>().Any();
        }

        private IEnumerable<string> Categories(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes<TestCategoryAttribute>().SelectMany(a => a.TestCategories);
        }
    }
}

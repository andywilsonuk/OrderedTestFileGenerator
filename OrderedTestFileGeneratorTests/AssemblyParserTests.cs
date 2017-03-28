using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderedTestFileGenerator;
using System.IO;

namespace OrderedTestFileGeneratorTests
{
    [TestClass]
    public class AssemblyParserTests
    {
        [TestMethod]
        public void AllTests_ExecutingAssemblyContainsTests_AtLeastOneTestReturned()
        {
            var testAssembly = Assembly.GetExecutingAssembly();

            var target = new AssemblyParser();

            var allTests = target.AllTests(new FileInfo(new Uri(testAssembly.CodeBase).LocalPath)).ToList();

            Assert.AreNotEqual(0, allTests.Count);
            Assert.IsTrue(allTests.Any(x => x.MethodName == nameof(AllTests_ExecutingAssemblyContainsTests_AtLeastOneTestReturned)));
        }
    }
}

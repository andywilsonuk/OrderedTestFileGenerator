using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderedTestFileGenerator;

namespace OrderedTestFileGeneratorTests
{
    [TestClass]
    public class OrderedTestFileTests
    {
        [TestMethod]
        public void GenerateInternal_OneTestOutput_XmlMatchesTestDefinition()
        {
            var testDefinitions = new[] { new TestDefinition
            {
                AssemblyFile = new FileInfo(@"c:\tests.dll"),
                Categories = new [] { "Smoke" },
                MethodName = "Test1",
                QualifiedName = "Tests.Tests1",
            } };
            var target = new OrderedTestFile(new FileInfo(@"c:\output.xml"));
            var expected = XDocument.Parse(
@"<OrderedTest name=""output"" storage=""c:\output.xml"" id=""" + target.Id + @""" xmlns=""http://microsoft.com/schemas/VisualStudio/TeamTest/2010"">
  <TestLinks>
    <TestLink id=""2ff92d50-acfb-8f87-aa28-25baf0e02457"" name=""Test1"" storage=""c:\tests.dll"" type=""Microsoft.VisualStudio.TestTools.TestTypes.Unit.UnitTestElement, Microsoft.VisualStudio.QualityTools.Tips.UnitTest.ObjectModel, Version=14.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"" />
  </TestLinks>
</OrderedTest>");

            var actual = target.GenerateInternal(testDefinitions);

            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}

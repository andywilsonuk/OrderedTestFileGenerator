using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderedTestFileGenerator;

namespace OrderedTestFileGeneratorTests
{
    [TestClass]
    public class TestDefinitionTests
    {
        [TestMethod]
        public void Id_GetIdFromQualifiedName_IdMatchesKnownValue()
        {
            Guid expected = new Guid("56a480b5-ceb1-35ed-ad8b-8b87cba5bf60");

            var target = new TestDefinition
            {
                QualifiedName = "OrderedTestFileGeneratorTests.TestDefinitionTests.Id_GetIdFromQualifiedName_IdMatchesKnownValue",
            };

            Guid actual = target.Id;

            Assert.AreEqual(expected, actual);
        }
    }
}

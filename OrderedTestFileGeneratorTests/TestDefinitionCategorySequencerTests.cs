using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderedTestFileGenerator;

namespace OrderedTestFileGeneratorTests
{
    [TestClass]
    public class TestDefinitionCategorySequencerTests
    {
        private List<TestDefinition> testDefinitions = new List<TestDefinition>
        {
            new TestDefinition { Categories = new[] { "Smoke" } },
            new TestDefinition { Categories = new[] { "Critical" } },
            new TestDefinition { Categories = new[] { "Smoke", "Critical" } },
        };

        [TestMethod]
        public void Sequenced_NoOrphans_OnlySmokeTestsReturned()
        {
            List<string> categorySequence = new List<string> { "Critical" };

            var target = new TestDefinitionCategorySequencer(testDefinitions, categorySequence);

            var actual = target.Sequenced.ToList();

            Assert.AreEqual(2, actual.Count);
            Assert.IsTrue(actual[0].Categories.Contains("Critical"));
        }

        [TestMethod]
        public void Sequenced_WithOrphans_AllTestsReturned()
        {
            List<string> categorySequence = new List<string> { "Critical" };

            var target = new TestDefinitionCategorySequencer(testDefinitions, categorySequence);
            target.AppendOrphans = true;

            var actual = target.Sequenced.ToList();

            Assert.AreEqual(3, actual.Count);
            Assert.IsTrue(actual.First().Categories.Contains("Critical"));
            Assert.IsTrue(actual.Last().Categories.Contains("Smoke"));
        }
    }
}

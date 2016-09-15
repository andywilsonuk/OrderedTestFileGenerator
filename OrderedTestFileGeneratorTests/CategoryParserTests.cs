using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderedTestFileGenerator;

namespace OrderedTestFileGeneratorTests
{
    [TestClass]
    public class CategoryParserTests
    {
        [TestMethod]
        public void AddCategories_VerifyAdd_CategoryAdded()
        {
            string[] categories = new[] { "Smoke", "Extended" };

            var target = new CategoryParser();

            target.AddCategories(categories);

            Assert.AreEqual(categories.Length, target.Count);
            Assert.AreEqual(target.Categories.First(), categories[0]);
        }
    }
}

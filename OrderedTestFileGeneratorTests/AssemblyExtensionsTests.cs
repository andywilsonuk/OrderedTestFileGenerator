using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderedTestFileGenerator;

namespace OrderedTestFileGeneratorTests
{
    [TestClass]
    public class AssemblyExtensionsTests
    {
        [TestMethod]
        public void TestClassTypes_ListClassesForExecutingAssembly_AtLeastOneClassAndContainsTestClassAttribute()
        {
            var testAssembly = Assembly.GetExecutingAssembly();

            var actual = testAssembly.TestClassTypes();

            Assert.AreNotEqual(0, actual);
            Assert.IsTrue(actual.First().CustomAttributes.Any(x => x.AttributeType == typeof(TestClassAttribute)));
        }

        [TestMethod]
        public void TestMethods_ListTestMethodsForExecutingClass_AtLeastOneMethodAndContainsTestMethodAttribute()
        {
            var actual = ExecutingClass().TestMethods();

            Assert.AreNotEqual(0, actual);
            Assert.IsTrue(actual.First().CustomAttributes.Any(x => x.AttributeType == typeof(TestMethodAttribute)));
        }

        [TestMethod]
        public void AllTestMethods_ListTestMethodsForExecutingAssembly_AtLeastOneClass()
        {
            var testAssembly = Assembly.GetExecutingAssembly();

            var actual = testAssembly.AllTestMethods();

            Assert.AreNotEqual(0, actual);
        }

        [TestMethod]
        public void IsIgnored_ExecutingMethodNotIgnored_IgnoreIsFalse()
        {
            var testMethods = ExecutingClass().TestMethods();

            var actual = testMethods.First(x => x.Name == nameof(IsIgnored_ExecutingMethodNotIgnored_IgnoreIsFalse));

            Assert.IsFalse(actual.CustomAttributes.Any(x => x.AttributeType == typeof(IgnoreAttribute)));
        }

        [TestMethod]
        public void QualifiedName_GetNameForExecutingTest_NameMatchesExpected()
        {
            string expected = string.Format("{0}.{1}.{2}", nameof(OrderedTestFileGeneratorTests), nameof(AssemblyExtensionsTests), nameof(QualifiedName_GetNameForExecutingTest_NameMatchesExpected));
            var testMethods = ExecutingClass().TestMethods();

            string actual = testMethods.First(x => x.Name == nameof(QualifiedName_GetNameForExecutingTest_NameMatchesExpected)).QualifiedName();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Categories_ExecutingMethodCategoryCount_ZeroCategories()
        {
            var testMethods = ExecutingClass().TestMethods();
            var actual = testMethods.First(x => x.Name == nameof(Categories_ExecutingMethodCategoryCount_ZeroCategories));

            Assert.IsFalse(actual.CustomAttributes.Any(x => x.AttributeType == typeof(TestCategoryAttribute)));
        }

        private static Type ExecutingClass()
        {
            var testAssembly = Assembly.GetExecutingAssembly();
            var executingClass = testAssembly.GetTypes().First(x => x.Name == nameof(AssemblyExtensionsTests));
            return executingClass;
        }
    }
}

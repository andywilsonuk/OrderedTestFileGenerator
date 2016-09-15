using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderedTestFileGenerator;

namespace OrderedTestFileGeneratorTests
{
    [TestClass]
    public class InputOptionsTests
    {
        [TestMethod]
        public void AssemblyFilePaths_AssemblyFilePathsRawValuesAreConvertedToFileInfo_ConvertedFileInfo()
        {
            string[] source = new[] { @"c:\test.dll" };
            var target = new InputOptions();
            target.AssemblyFilePathsRaw = source;

            IList<FileInfo> actual = target.AssemblyFilePaths;

            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(source[0], actual[0].FullName);
        }

        [TestMethod]
        public void AssemblyFilePaths_AssemblyFilePathsRawValuesContainInvalidPath_NullList()
        {
            string[] source = new[] { @"c:\:test.dll" };
            var target = new InputOptions();
            target.AssemblyFilePathsRaw = source;

            IList<FileInfo> actual = target.AssemblyFilePaths;

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void OutputFilePath_OutputFilePathRawValueConvertedToFileInfo_ConvertedFileInfo()
        {
            string source = @"c:\output.orderedtest";
            var target = new InputOptions();
            target.OutputFilePathRaw = source;

            FileInfo actual = target.OutputFilePath;

            Assert.IsNotNull(actual);
            Assert.AreEqual(source, actual.FullName);
        }

        [TestMethod]
        public void CategoriesFilePath_CategoriesFilePathRawConvertedToFileInfo_ConvertedFileInfo()
        {
            string source = @"c:\categories.txt";
            var target = new InputOptions();
            target.CategoriesFilePathRaw = source;

            FileInfo actual = target.CategoriesFilePath;

            Assert.IsNotNull(actual);
            Assert.AreEqual(source, actual.FullName);
        }

        [TestMethod]
        public void TryParseFileInfo_EmptyString_Null()
        {
            var target = new InputOptions();

            FileInfo actual = target.TryParseFileInfo(string.Empty);

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void TryParseFileInfo_InvalidPath_Null()
        {
            var target = new InputOptions();

            FileInfo actual = target.TryParseFileInfo(@"c:\:test.dll");

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void TryParseFileInfo_ValidPath_ConvertedFileInfo()
        {
            string source = @"c:\output.orderedtest";
            var target = new InputOptions();

            FileInfo actual = target.TryParseFileInfo(source);

            Assert.IsNotNull(actual);
            Assert.AreEqual(source, actual.FullName);
        }

        [TestMethod]
        public void GetHelp_HelpText_ExpectedHelpString()
        {
            string expected =
@"Usage
  -a  The file paths to the assembly DLLs containing the mstests. Required.
  -o  The file path be to used for the generated Ordered Test file. Required.

  Categories (at least one must be specified):
  -c  The sequence of categories for ordered tests (semi-colon separated).
  -x  The file path of a linebreak separated list of categories.

  Optional:
  -p  Specify to append tests which don't match any of the specified 
      categories to the end of the ordered test file.

Examples
1.  OrderedTestFileGenerator.exe -a ""C:\TestAssembly.dll"" -o ""C:\all.orderedtest"" -c Smoke;Critical -p
2.  OrderedTestFileGenerator.exe -a ""C:\TestAssembly.dll"" -a ""C:\TestAssembly2.dll"" -o ""C:\all.orderedtest"" -x ""C:\categories.txt""
";
            var target = new InputOptions();

            string actual = target.GetHelp();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetErrors_Valid_EmptyString()
        {
            string testFilePath = TestFilePath();
            var target = new InputOptions
            {
                AssemblyFilePathsRaw = new[] { testFilePath },
                OutputFilePathRaw = testFilePath,
                Categories = new[] { "Smoke" },
            };

            string actual = target.GetErrors();

            Assert.AreEqual(string.Empty, actual);
        }

        [TestMethod]
        public void GetErrors_InvalidAssemblyPath_ErrorStringWithAssemblyPath()
        {
            string testFilePath = TestFilePath();
            var target = new InputOptions
            {
                AssemblyFilePathsRaw = null,
                OutputFilePathRaw = testFilePath,
                Categories = new[] { "Smoke" },
            };
            string expected =
@"Errors:
- Invalid assembly path(s)
";

            string actual = target.GetErrors();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetErrors_InvalidOutputPath_ErrorStringWithOutputPath()
        {
            string testFilePath = TestFilePath();
            var target = new InputOptions
            {
                AssemblyFilePathsRaw = new[] { testFilePath },
                OutputFilePathRaw = null,
                Categories = new[] { "Smoke" },
            };
            string expected = 
@"Errors:
- Invalid output path
";

            string actual = target.GetErrors();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetErrors_InvalidCategoriesPath_ErrorStringWithCategoriesPath()
        {
            string testFilePath = TestFilePath();
            var target = new InputOptions
            {
                AssemblyFilePathsRaw = new[] { testFilePath },
                OutputFilePathRaw = testFilePath,
                CategoriesFilePathRaw = @"c:\testdummy.txt",
            };
            string expected =
@"Errors:
- Invalid categories path
";

            string actual = target.GetErrors();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetErrors_NoCategories_ErrorStringWithCategories()
        {
            string testFilePath = TestFilePath();
            var target = new InputOptions
            {
                AssemblyFilePathsRaw = new[] { testFilePath },
                OutputFilePathRaw = testFilePath,
                CategoriesFilePathRaw = null,
                Categories = null,
            };
            string expected =
@"Errors:
- No categories specified
";

            string actual = target.GetErrors();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsValidAssemblyFilePaths_ValidPath_True()
        {
            string testFilePath = TestFilePath();
            var target = new InputOptions
            {
                AssemblyFilePathsRaw = new[] { testFilePath },
            };

            bool actual = target.IsValidAssemblyFilePaths;

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsValidAssemblyFilePaths_InvalidPathNull_False()
        {
            string testFilePath = TestFilePath();
            var target = new InputOptions
            {
                AssemblyFilePathsRaw = null,
            };

            bool actual = target.IsValidAssemblyFilePaths;

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidAssemblyFilePaths_InvalidPathEmpty_False()
        {
            string testFilePath = TestFilePath();
            var target = new InputOptions
            {
                AssemblyFilePathsRaw = new[] { string.Empty },
            };

            bool actual = target.IsValidAssemblyFilePaths;

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidAssemblyFilePaths_InvalidPathNotExist_False()
        {
            string testFilePath = TestFilePath();
            var target = new InputOptions
            {
                AssemblyFilePathsRaw = new[] { @"c:\test.dll" },
            };

            bool actual = target.IsValidAssemblyFilePaths;

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidOutputFilePath_ValidPath_True()
        {
            string testFilePath = TestFilePath();
            var target = new InputOptions
            {
                OutputFilePathRaw = testFilePath,
            };

            bool actual = target.IsValidOutputFilePath;

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsValidOutputFilePath_InvalidPathNull_False()
        {
            string testFilePath = TestFilePath();
            var target = new InputOptions
            {
                OutputFilePathRaw = null,
            };

            bool actual = target.IsValidOutputFilePath;

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidCategoryFilePath_ValidPath_True()
        {
            string testFilePath = TestFilePath();
            var target = new InputOptions
            {
                CategoriesFilePathRaw = testFilePath,
            };

            bool actual = target.IsValidCategoryFilePath;

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsValidCategoryFilePath_NullPath_True()
        {
            string testFilePath = TestFilePath();
            var target = new InputOptions
            {
                CategoriesFilePathRaw = null,
            };

            bool actual = target.IsValidCategoryFilePath;

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsValidCategoryFilePath_InvalidPathNotExist_False()
        {
            string testFilePath = TestFilePath();
            var target = new InputOptions
            {
                CategoriesFilePathRaw = @"c:\test.txt",
            };

            bool actual = target.IsValidCategoryFilePath;

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidCategoryOption_CategoryFile_True()
        {
            string testFilePath = TestFilePath();
            var target = new InputOptions
            {
                CategoriesFilePathRaw = @"c:\test.txt",
                Categories = null,
            };

            bool actual = target.IsValidCategoryOption;

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsValidCategoryOption_NoCategoriesNull_False()
        {
            string testFilePath = TestFilePath();
            var target = new InputOptions
            {
                CategoriesFilePathRaw = null,
                Categories = null,
            };

            bool actual = target.IsValidCategoryOption;

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidCategoryOption_NoCategoriesEmpty_False()
        {
            string testFilePath = TestFilePath();
            var target = new InputOptions
            {
                CategoriesFilePathRaw = null,
                Categories = new string[0],
            };

            bool actual = target.IsValidCategoryOption;

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidCategoryOption_CategoryList_True()
        {
            string testFilePath = TestFilePath();
            var target = new InputOptions
            {
                CategoriesFilePathRaw = null,
                Categories = new[] { "Smoke" },
            };

            bool actual = target.IsValidCategoryOption;

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsValidCategoryOption_CategoryFileAndCategoryList_True()
        {
            string testFilePath = TestFilePath();
            var target = new InputOptions
            {
                CategoriesFilePathRaw = testFilePath,
                Categories = new[] { "Smoke" },
            };

            bool actual = target.IsValidCategoryOption;

            Assert.IsTrue(actual);
        }

        private string TestFilePath()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return assembly.FilePath().FullName;
        }
    }
}

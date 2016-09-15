using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommandLine;

namespace OrderedTestFileGenerator
{
    class InputOptions
    {
        [OptionArray('a', "assemblypaths", Required = true, HelpText = "The file paths to the assembly DLLs containing the mstests.")]
        public string[] AssemblyFilePathsRaw { get; set; }

        public IList<FileInfo> AssemblyFilePaths
        {
            get
            {
                if (AssemblyFilePathsRaw == null) return null;
                var paths = AssemblyFilePathsRaw.Select(x => TryParseFileInfo(x)).ToList();
                return paths.Any(x => x == null) ? null : paths;
            }
        }

        [Option('o', "outputpath", Required = true, HelpText = "The file path be to used for the generated Ordered Test file.")]
        public string OutputFilePathRaw { get; set; }

        public FileInfo OutputFilePath
        {
            get { return TryParseFileInfo(OutputFilePathRaw); }
        }

        [OptionList('c', "categories", Separator = ';', HelpText = "The sequence of categories for ordered tests (semi-colon separated). Either categories or categoriesfile required.")]
        public IList<string> Categories { get; set; }

        [Option('x', "categoriesfile", HelpText = "The file path of a linebreak separated list of categories.")]
        public string CategoriesFilePathRaw { get; set; }

        public FileInfo CategoriesFilePath
        {
            get { return TryParseFileInfo(CategoriesFilePathRaw); }
        }

        [Option('p', "appendorphans", DefaultValue = false, HelpText = "Specify to append tests which don't match any of the specified categories to the end of the ordered test file.")]
        public bool AppendOrphans { get; set; }

        public string GetHelp()
        {
            return new StringBuilder()
                .AppendLine("Usage:")
                .AppendLine("-a  The file paths to the assembly DLLs containing the mstests. Required.")
                .AppendLine("-o  The file path be to used for the generated Ordered Test file. Required.")
                .AppendLine("Categories (at least one must be specified):")
                .AppendLine("-c  The sequence of categories for ordered tests (semi-colon separated).")
                .AppendLine("-x  The file path of a linebreak separated list of categories.")
                .AppendLine()
                .AppendLine("-p  Specify to append tests which don't match any of the specified categories to the end of the ordered test file.")
                .AppendLine()
                
                .ToString();
        }

        public string GetErrors()
        {
            StringBuilder sb = new StringBuilder();
            if (!IsValidAssemblyFilePaths)
            {
                sb.AppendLine("- Invalid assembly path(s)");
            }
            if (!IsValidOutputFilePath)
            {
                sb.AppendLine("- Invalid output path");
            }
            if (!IsValidCategoryFilePath)
            {
                sb.AppendLine("- Invalid categories path");
            }
            else if (!IsValidCategoryOption)
            {
                sb.AppendLine("- No categories specified");
            }

            if (sb.Length != 0) sb.Insert(0, "Errors:" + Environment.NewLine);
            return sb.ToString();
        }

        internal bool IsValidAssemblyFilePaths
        {
            get { return AssemblyFilePaths != null && AssemblyFilePaths.Count != 0 && AssemblyFilePaths.All(x => x.Exists); }
        }

        internal bool IsValidOutputFilePath
        {
            get { return OutputFilePath != null && OutputFilePath.Exists; }
        }

        internal bool IsValidCategoryFilePath
        {
            get { return CategoriesFilePath == null || CategoriesFilePath.Exists; }
        }

        internal bool IsValidCategoryOption
        {
            get { return CategoriesFilePath != null || (Categories != null && Categories.Count != 0); }
        }

        internal FileInfo TryParseFileInfo(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return null;
            try
            {
                return new FileInfo(fullName);
            }
            catch (NotSupportedException)
            {
                return null;
            }
        }
    }
}

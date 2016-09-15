using System;
using System.Linq;
using System.Reflection;
using CommandLine;

namespace OrderedTestFileGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ordered Test file generator");
            Console.WriteLine();
            InputOptions settings = ParseArguments(args);

            AssemblyParser assemblyParser = new AssemblyParser();
            CategoryParser categoryParser = new CategoryParser();
            categoryParser.AddCategories(settings.Categories);
            if (settings.CategoriesFilePath != null) categoryParser.AddCategoryFile(settings.CategoriesFilePath);
            Console.WriteLine("Categories are: {0}", string.Join(",", categoryParser.Categories));

            var allTests = settings.AssemblyFilePaths.SelectMany(x => assemblyParser.AllTests(x)).ToList();
            var sequencer = new TestDefinitionCategorySequencer(allTests, categoryParser.Categories);
            sequencer.AppendOrphans = settings.AppendOrphans;
            var sequenced = sequencer.Sequenced.ToList();
            Console.WriteLine("Tests found: {0}, tests used: {1}", allTests.Count, sequenced.Count);
            Console.WriteLine();

            var orderedTestFileGenerator = new OrderedTestFile(settings.OutputFilePath);
            orderedTestFileGenerator.Generate(sequenced);
            Console.WriteLine("Generated Ordered Test file at: {0}", orderedTestFileGenerator.File.FullName);
            Console.WriteLine();
            Exit(ExitCodes.OK);
        }

        private static InputOptions ParseArguments(string[] args)
        {
            var settings = new InputOptions();

            if (args.Length == 0)
            {
                Console.Write(settings.GetHelp());
                Exit(ExitCodes.Help);
            }

            bool isValid;

            try
            {
                isValid = Parser.Default.ParseArguments(args, settings);
            }
            catch (TargetInvocationException)
            {
                isValid = false;
            }

            string errors = settings.GetErrors();
            if (errors.Length == 0) return settings;

            Console.Error.WriteLine(settings.GetHelp());
            Console.Error.WriteLine(errors);
            Exit(ExitCodes.InvalidArgs);
            return null;
        }

        private static void Exit(ExitCodes exitCode)
        {
#if DEBUG
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
#endif
            Environment.Exit((int)exitCode);
        }
    }
}

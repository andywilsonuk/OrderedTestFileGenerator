using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrderedTestFileGenerator
{
    class OrderedTestFile
    {
        private FileSystemInfo outputFile;

        public OrderedTestFile(FileSystemInfo outputFile)
        {
            this.outputFile = outputFile;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public void Generate(IEnumerable<TestDefinition> tests)
        {
            var outputXml = GenerateInternal(tests);
            outputXml.Save(outputFile.FullName);
        }

        internal XDocument GenerateInternal(IEnumerable<TestDefinition> tests)
        {
            XNamespace testNamespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";
            XDocument outputXml = new XDocument
                (new XDeclaration("1.0", "UTF-8", null),
                new XElement(testNamespace + "OrderedTest",
                new XAttribute("name", Path.GetFileNameWithoutExtension(outputFile.Name)),
                new XAttribute("storage", outputFile.FullName),
                new XAttribute("id", Id),
                new XElement(testNamespace + "TestLinks",
                from testDefinition in tests
                select new XElement(testNamespace + "TestLink",
                    new XAttribute("id", testDefinition.Id),
                    new XAttribute("name", testDefinition.MethodName),
                    new XAttribute("storage", testDefinition.AssemblyFile.FullName),
                    new XAttribute("type", testDefinition.AssemblyBinding)
                ))));
            return outputXml;
        }

        public FileSystemInfo File
        {
            get { return outputFile; }
        }
    }
}

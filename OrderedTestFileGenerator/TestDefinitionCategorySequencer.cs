using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderedTestFileGenerator
{
    class TestDefinitionCategorySequencer
    {
        private IList<TestDefinition> allTests;
        private IList<TestDefinition> sequencedTests;

        public TestDefinitionCategorySequencer(IEnumerable<TestDefinition> tests, IEnumerable<string> categorySequence)
        {
            allTests = tests.ToList();
            BuildTestSequence(categorySequence);
        }

        public bool AppendOrphans { get; set; }

        public IEnumerable<TestDefinition> Sequenced
        {
            get
            {
                return AppendOrphans ? sequencedTests.Union(allTests.Except(sequencedTests)) : sequencedTests;
            }
        }

        private void BuildTestSequence(IEnumerable<string> categorySequence)
        {
            HashSet<TestDefinition> testSet = new HashSet<TestDefinition>();

            foreach (string category in categorySequence)
            {
                foreach (var test in allTests.Where(i => i.Categories.Contains(category, StringComparer.CurrentCultureIgnoreCase)))
                {
                    testSet.Add(test);
                }
            }

            sequencedTests = testSet.ToList();
        }
    }
}

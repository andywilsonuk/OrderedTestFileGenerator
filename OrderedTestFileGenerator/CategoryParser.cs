using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderedTestFileGenerator
{
    class CategoryParser
    {
        private HashSet<string> allCategories = new HashSet<string>(StringComparer.CurrentCultureIgnoreCase);

        public void AddCategories(IEnumerable<string> categories)
        {
            if (categories == null) return;
            foreach (string category in categories)
            {
                allCategories.Add(category);
            }
        }

        public void AddCategoryFile(FileInfo categoryFilePath)
        {
            var categories = File.ReadAllLines(categoryFilePath.FullName);
            AddCategories(categories);
        }

        public int Count
        {
            get { return allCategories.Count; }
        }

        public IEnumerable<string> Categories
        {
            get { return allCategories; }
        }
    }
}

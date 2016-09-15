using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrderedTestFileGenerator
{
    static class AssemblyExtensions
    {
        public static IEnumerable<Type> TestClassTypes(this Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.GetCustomAttributes<TestClassAttribute>().Any());
        }

        public static IEnumerable<MethodInfo> TestMethods(this Type type)
        {
            return type.GetMethods().Where(m => m.GetCustomAttributes<TestMethodAttribute>().Any());
        }

        public static IEnumerable<MethodInfo> AllTestMethods(this Assembly assembly)
        {
            return assembly.TestClassTypes().SelectMany(x => x.TestMethods());
        }

        public static bool IsIgnored(this MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes<IgnoreAttribute>().Any();
        }

        public static IEnumerable<string> Categories(this MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttributes<TestCategoryAttribute>().SelectMany(a => a.TestCategories);
        }

        public static FileSystemInfo FilePath(this Assembly assembly)
        {
            return new FileInfo(new Uri(assembly.CodeBase).LocalPath);
        }

        public static string QualifiedName(this MethodInfo methodInfo)
        {
            return methodInfo.ReflectedType.FullName + "." + methodInfo.Name;
        }
    }
}

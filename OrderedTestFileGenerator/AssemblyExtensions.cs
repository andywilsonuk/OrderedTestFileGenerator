
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OrderedTestFileGenerator
{
    static class AssemblyExtensions
    { 
        public static IEnumerable<MethodInfo> AllTestMethods<T>(this Assembly assembly) where T : Attribute
        {
            return assembly.GetTypes().SelectMany(x => x.GetMethods()).Where(x => x.GetCustomAttributes<T>().Any());
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

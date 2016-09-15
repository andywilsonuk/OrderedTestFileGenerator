using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OrderedTestFileGenerator
{
    class TestDefinition
    {
        public string MethodName { get; set; }
        public string QualifiedName { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public FileSystemInfo AssemblyFile { get; set; }

        public Guid Id
        {
            get
            {
                SHA1CryptoServiceProvider provider = new SHA1CryptoServiceProvider();
                byte[] id = new byte[16];

                byte[] hash = provider.ComputeHash(Encoding.Unicode.GetBytes(QualifiedName));

                Array.Copy(hash, id, 16);
                return new Guid(id);
            }
        }
    }
}

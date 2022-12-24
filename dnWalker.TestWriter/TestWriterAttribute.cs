using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter
{
    /// <summary>
    /// Specifies some traits of a test writer compoent
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class TestWriterAttribute : Attribute
    {
    }
}

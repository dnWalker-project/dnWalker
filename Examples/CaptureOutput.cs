using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
    public class CaptureOutput
    {
        public static void Capture1()
        {
            Console.Out.WriteLine("X=1");
            Console.Out.WriteLine("Y=1");
            Console.WriteLine("Z=1");
        }

        public static void Capture2()
        {
            var local = 2;
            Console.Out.WriteLine("X=" + local);
            Console.Out.WriteLine($"X={local}");
        }
    }
}

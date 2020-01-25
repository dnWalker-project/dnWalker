using MMC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests
{
    public class TestLoggerOutput : ILoggerOutput
    {
        public void Close()
        {
        }

        public void Flush()
        {
        }

        public void Log(LogPriority lp, string msg)
        {
            System.DateTime now = System.DateTime.Now;
            var message = string.Format("{0:D2}:{1:D2}:{4:D2} [{2,12}] {3}", now.Hour, now.Minute, lp, msg, now.Second);
            System.Console.Out.WriteLine(message);
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
/*
 *   Copyright 2007 University of Twente, Formal Methods and Tools group
 *
 *   Licensed under the Apache License, Version 2.0 (the "License");
 *   you may not use this file except in compliance with the License.
 *   You may obtain a copy of the License at
 *
 *       http://www.apache.org/licenses/LICENSE-2.0
 *
 *   Unless required by applicable law or agreed to in writing, software
 *   distributed under the License is distributed on an "AS IS" BASIS,
 *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *   See the License for the specific language governing permissions and
 *   limitations under the License.
 *
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MMC.Test
{
    /// This is the TestDriver for testing MMC against the BVT tests
    public class TestDriver
    {

        public string Execute(string cmd, string args, string workingDirectory, out int retval)
        {
            using (var p = new Process())
            {
                p.StartInfo.FileName = cmd;
                p.StartInfo.Arguments = args;
                p.StartInfo.WorkingDirectory = workingDirectory;

                Console.Out.WriteLine($"{cmd} {args}");

                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.Start();

                var output = p.StandardError.ReadToEnd();
                output += p.StandardOutput.ReadToEnd();

                p.WaitForExit();
                retval = p.ExitCode;

                return output;
            }
        }


        public string Basename(string filename)
        {
            var dot = filename.LastIndexOf(".");
            var basename = filename.Substring(0, dot);
            return basename;
        }


        public void Compile(string compiler, string args, string filename)
        {
            int retval;

            var basename = Basename(filename);

            if (File.Exists(basename + ".exe"))
            {
                return;
            }

            //filename = "\"" + filename + "\"";
            var output = this.Execute(compiler, args + " " + filename, Path.GetDirectoryName(filename), out retval);

            var sb = new StringBuilder();
            sb.AppendFormat("** COMPILE [{0}] {1} {2} {3} RETURNS {4}\n{5}",
                DateTime.UtcNow.ToLongDateString() + " " + DateTime.UtcNow.ToLongTimeString(),
                compiler,
                args,
                filename,
                retval,
                output);

            writer.WriteLine(sb.ToString());

            cout++;
        }


        public void RunTest(string filename)
        {
            int retval;

            filename = "\"" + filename + "\"";
            var output = this.Execute("mono", "mmc.exe -V -s -a " + filename, Path.GetDirectoryName(filename), out retval);

            var sb = new StringBuilder();
            sb.AppendFormat("** TEST [{0} {1}] {2} RETURNS {3}\n{4}",
                DateTime.UtcNow.ToLongDateString(),
                DateTime.UtcNow.ToLongTimeString(),
                filename,
                retval,
                output);

            writer.WriteLine(sb.ToString());

            if (retval == 0)
                this.passed++;
            else
                this.errors++;
        }


        public List<string> GetFilesRecursively(string root, string criterium)
        {
            var di = new DirectoryInfo(root);
            var retval = new List<string>();

            foreach (var file in di.GetFiles(criterium))
                retval.Add(file.FullName);

            foreach (var din in di.GetDirectories())
                retval.AddRange(this.GetFilesRecursively(din.FullName, criterium));

            return retval;
        }


        public void RunTests(string root, string criterium, string compiler, string flags)
        {
            /* process testfiles */
            var files = GetFilesRecursively(root, criterium);
            var sb = new StringBuilder();
            sb.AppendFormat("Found {0} files using criterium {1}, stand by while processing",
                files.Count,
                criterium);
            Console.WriteLine(sb.ToString());
            for (var i = 0; i < files.Count; i++)
            {
                var file = files[i];

                /* compile */
                Compile(compiler, flags, file);

                /* run test */
                var basename = Basename(file);
                RunTest(basename + ".exe");

                // some feedback to the user
                if ((i % 10) == 0)
                {
                    Console.Write(i);
                }
                Console.Write(".");
            }
            Console.WriteLine("finished processing");
        }


        public TestDriver(string root, string logfile)
        {
            /* init log system */
            if (File.Exists(logfile))
                File.Delete(logfile);
            this.writer = File.CreateText(logfile);

            /* Run tests */
            /// Tests all files with extensions .il, .ifl and .cs
            RunTests(root, "*.il", "ilasm", "/debug");
            RunTests(root, "*.ifl", "ilasm", "/debug");
            RunTests(root, "*.cs", @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe", "-d:DEBUG");


            /* Print stats */
            var sb = new StringBuilder();
            sb.AppendFormat(
@"
Test statistics:
----------------------
Passed:            {0}
Failed:            {1}
Freshly compiled:  {3}

See {2} for logs",
                passed,
                errors,
                logfile,
                cout);
            Console.WriteLine(sb.ToString());
            writer.WriteLine(sb.ToString());
            writer.Close();
        }


        // num of compiled files
        int cout = 0;
        // #errors
        int errors = 0;
        // #passed
        int passed = 0;
        // logfile writer
        TextWriter writer;


        public static void Main(string[] args)
        {
            var logfile = "";
            var root = "";

            /* parse args */
            if (args.Length == 2)
            {
                root = args[0];
                logfile = args[1];
            }
            else
            {
                Console.WriteLine("missing arguments: TestDriver <rootdir of testfiles> <logfile>");
                return;
            }

            /* init data structs */
            var driver = new TestDriver(root, logfile);
        }
    }
}
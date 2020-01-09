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

using C5;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MMC.Test
{
	/// This is the TestDriver for testing MMC against the BVT tests
	public class TestDriver
	{
		
		public string Execute(string cmd, string args, out int retval) {
			using (Process p = new Process()) {
				p.StartInfo.FileName = cmd;
				p.StartInfo.Arguments = args;

				p.StartInfo.UseShellExecute = false;
		        p.StartInfo.RedirectStandardOutput = true;
		        p.StartInfo.RedirectStandardError = true;
		        p.Start();
		        
		        string output = p.StandardError.ReadToEnd();
		        output += p.StandardOutput.ReadToEnd();

		        p.WaitForExit();
		        retval = p.ExitCode;

		        return output;
	        }
		}
		
		
		public string Basename(string filename) {
			int dot = filename.LastIndexOf(".");
			string basename = filename.Substring(0, dot);
			return basename;
		}
		
		
		public void Compile(string compiler, string args, string filename) {
			int retval;
			
			string basename = Basename(filename);
			
			if (File.Exists(basename + ".exe")) {
				return;
			}

			filename = "\"" + filename + "\"";
			string output = this.Execute(compiler, args + " " + filename, out retval);
			
			StringBuilder sb = new StringBuilder();
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
		
		
		public void RunTest(string filename) {
			int retval;

			filename = "\"" + filename + "\"";
			string output = this.Execute("mono", "mmc.exe -V -s -a " + filename, out retval);
			
			StringBuilder sb = new StringBuilder();
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
		
		
		public ArrayList<string> GetFilesRecursively(string root, string criterium) {
			DirectoryInfo di = new DirectoryInfo(root);
			ArrayList<string> retval = new ArrayList<string>();
			
			foreach (FileInfo file in di.GetFiles(criterium)) 
				retval.Add(file.FullName);
			
			foreach (DirectoryInfo din in di.GetDirectories()) 
				retval.AddAll(this.GetFilesRecursively(din.FullName, criterium));

			return retval;
		}
		
		
		public void RunTests(string root, string criterium, string compiler, string flags) {
			/* process testfiles */
			ArrayList<string> files = GetFilesRecursively(root, criterium);
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("Found {0} files using criterium {1}, stand by while processing",
				files.Count,
				criterium);
			Console.WriteLine(sb.ToString());
			for (int i = 0; i < files.Count; i++) {
				string file = files[i];
				
				/* compile */
				Compile(compiler, flags, file);
					
				/* run test */
				string basename = Basename(file);
				RunTest(basename + ".exe");
				
				// some feedback to the user
				if ((i % 10) == 0) {
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
			RunTests(root, "*.cs", "mcs", "-d:DEBUG");
			
		
			/* Print stats */
			StringBuilder sb = new StringBuilder();
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
		

		public static void Main(string[] args) {
			string logfile = "";
			string root = "";
			
			/* parse args */
			if (args.Length == 2) {
				root = args[0];
				logfile = args[1];
			} else {
				Console.WriteLine("missing arguments: TestDriver <rootdir of testfiles> <logfile>");
				return;
			}
			
			/* init data structs */ 
			TestDriver driver = new TestDriver(root, logfile);
		}
	}
}

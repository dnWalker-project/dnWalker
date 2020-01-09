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

namespace MMC {

	using System.IO;
	using System.Collections;
	using System.Diagnostics;
	using System.Reflection;
	using MMC.Data;
	using MMC.State;

	/// Configuration class
	///
	/// This is just a big collection of fields. Because of C# properties, the
	/// functionality behind access to these fields can be transparently
	/// adjusted without breaking source compatibility.
	static class Config {

		public static string AssemblyToCheckFileName = null;
		public static string[] RunTimeParameters = null;
		public static bool ShowStatistics = false;
		public static bool Quiet = false;
		public static bool Interactive = false;

		public static bool UseInstructionCache = true;
		public static bool UseRefCounting = false;
		public static bool UseMarkAndSweep = true;
		public static bool Verbose = false;
		public static bool SymmetryReduction = true;
		public static bool NonStaticSafe = false;
		public static bool MemoisedGC = false;
		public static bool UseDPORCollapser = true;
		public static bool UseObjectEscapePOR = true;
		public static bool UseStatefulDynamicPOR = true;
		public static bool StopOnError = true;
		public static bool TraceOnError = true;
		public static bool OneTraceAndStop = false;

		public static bool ExPostFactoMerging = true;

		public static double MaxExploreInMinutes = double.PositiveInfinity;
		public static double OptimizeStorageAtMegabyte = double.PositiveInfinity;
		public static double MemoryLimit = double.PositiveInfinity;
	}

	/// The main application class.
	class MonoModelChecker {

		/// Print a verbose (debug) message.
		///
		/// Do not use this function as an alternative to the logger, only to
		/// print debug information when actually debugging.
		///
		/// \param msg Message (cf. string.Format).
		/// \param value Values for the message string.
		public static void Message(string msg, params object[] values) {

			if (Config.Verbose) {
				string to_write = (values.Length > 0 ? string.Format(msg, values) : msg);
				System.Console.WriteLine(to_write);
			}
		}

		/// Print a fatal message and die.
		///
		/// \param msg Message to print before quitting.
		public static void Fatal(string msg) {

			Logger.l.CloseAll();
			DotWriter.End();
			System.Console.WriteLine("FATAL: " + msg);
			System.Console.WriteLine("use option \"-h\" for help");
			System.Environment.Exit(2);
		}

		/// Open a file for writing.
		///
		/// \param filename Name of the file to output to.
		static TextWriter TryOpen(string filename) {

			StreamWriter retval = null;
			try {
				retval = File.CreateText(filename);
				retval.AutoFlush = true;
			} catch (System.ArgumentException) {
				Fatal("malformed filename: " + filename);
			} catch (DirectoryNotFoundException) {
				Fatal("directory not found for filename: " + filename);
			} catch (PathTooLongException) {
				Fatal("path name too long for filename: " + filename);
			} catch (IOException) {
				Fatal("i/o exception in opening filename: " + filename);
			}
			return retval;
		}

		/// Parse command line options.
		///
		/// This sets various fields in class Config.
		///
		/// \param args Command-line options as passed to Main.
		static void ParseCommandLineOptions(string[] args) {

			for (int i = 0; i < args.Length; ++i) {
				if (args[i][0] != '-' || args[i].Length <= 1)
					Fatal("malformed argument: " + args[i]);
				char[] flags = args[i].ToCharArray();
				for (int f = 1; f < flags.Length; ++f) {
					switch (flags[f]) {
						case 'A':
							Config.OneTraceAndStop = true;
							break;
						case 'C':
							Config.UseInstructionCache = false;
							break;
						case 'F':
							Config.UseDPORCollapser = false;
							break;
						case 'G':
							Config.UseMarkAndSweep = false;
							break;
						case 'O':
							Config.NonStaticSafe = true;
							break;
						case 'R':
							Config.UseRefCounting = true;
							break;
						case 'S':
							Config.SymmetryReduction = false;
							break;
						case 'T':
							++i;
							if (i < args.Length)
								Config.MaxExploreInMinutes = System.Double.Parse(args[i]);
							else
								Fatal("-T option requires an argument");
							break;

						case 'a':
							++i;
							if (i < args.Length) {
								if (File.Exists(args[i]))
									Config.AssemblyToCheckFileName = args[i];
								else
									Fatal("assembly file not found: " + args[i]);
							} else
								Fatal("-a option requires an argument");
							break;
						case 'c':
							Config.StopOnError = false;
							break;
						case 'd':
							++i;
							if (i < args.Length)
								DotWriter.Begin(TryOpen(args[i]));
							else
								Fatal("-d option requies an argument");
							break;
						case 'f':
							++i;
							if (i < args.Length) {
								if (!Logger.l.ParseAndSetLogFilter(args[i]))
									Fatal("malformed log filter (use -h)");
							} else
								Fatal("-f option requires an argument");
							break;
						case 'h':
							PrintCommandLineUsage();
							System.Environment.Exit(0);
							break;
						case 'i':
							Config.Interactive = true;
							break;
						case 'I':
							Config.MemoisedGC = true;
							break;
						case 'm':
							++i;
							if (i < args.Length) 
								Config.MemoryLimit = System.Int32.Parse(args[i]);
							else
								Fatal("-m option requires an argument");
							break;						
						case 'o':
							++i;
							if (i < args.Length) 
								Config.OptimizeStorageAtMegabyte = System.Int32.Parse(args[i]);
							else
								Fatal("-o option requires an argument");
							break;
						case 'p':
							Config.UseObjectEscapePOR = false;
							break;
						case 'P':
							Config.UseStatefulDynamicPOR = false;
							Config.UseDPORCollapser = false;
							break;
						case 'l':
							++i;
							if (i < args.Length)
								Logger.l.AddOutput(new TextLoggerOutput(TryOpen(args[i])));
							else
								Fatal("-l option requies an argument");
							break;
						case 'r':
							Config.RunTimeParameters = FetchTillNextArgument(args, i + 1);
							i += Config.RunTimeParameters.Length;
							break;
						case 's':
							Config.ShowStatistics = true;
							break;
						case 't':
							Config.TraceOnError = false;
							break;
						case 'q':
							Config.Quiet = true;
							break;
						case 'V':
							Config.Verbose = true;
							break;
						default:
							Fatal("unknown parameter: " + flags[f]);
							break;
					}
				}
			}

			// Fatal combination (or lack of) parameters.
			if (Config.Quiet && Config.Interactive)
				Fatal("you have asked me to be quiet and interactive. make up your mind.");
			if (Config.AssemblyToCheckFileName == null)
				Fatal("no assembly to check specified");

			// Defaults.
			if (!Config.Quiet)
				Logger.l.AddOutput(new TextLoggerOutput(System.Console.Out));
			if (Config.RunTimeParameters == null)
				Config.RunTimeParameters = new string[] { };
		}

		static string[] FetchTillNextArgument(string[] args, int offset) {

			ArrayList toReturn = new ArrayList();
			for (int i = offset; i < args.Length; ++i) {
				if (args[i][0] == '-')
					break;
				toReturn.Add(args[i]);
			}
			return (string[])toReturn.ToArray(typeof(string));
		}

//		static string copyright = @"MMC - Mono Model Checker (version 1.0.0 - 19 December 2007)
//(C) University of Twente, Formal Methods and Tools group";

		static string copyright = @"MoonWalker 1.0.1 (11 April 2008)
(C) University of Twente, Formal Methods and Tools group";

		static void PrintCommandLineUsage() {

			string help_text = @"The following command line parameters are REQUIRED:
  -a  Assembly to check.

The following command line parameters are optional:
  -h  Print this help message and terminate.
  -r  Runtime parameters (can be empty, defaults to empty).
  -l  File to log to.
  -f  Log filter (possible values below, default: mwsf).
  -q  Suppress output to stdout.
  -s  Show statistics after run.
  -d  File to write .dot graph to.
     -D ditto, but produce labels to be typeset by TeX.
  -T  <t> terminate exploration after t minutes
  -m  <m> terminate exploration if memory usage exceeds m Mb 
  -o  <m> perform ex post facto transition merging after m Mb 
  -c  Continue after detection of an error
  -A  Show one trace to an end state and terminate

Log filter is a string of 1+ of the following characters:
   d  Debugging messages, for developers.
   o  Definition lookups (verbose).
   t  Trace of all CIL instructions executed (implies 'l').
   l    Only trace CIL instructions in main assembly.
   i    Only trace CIL instruction names, not local state.
   n  Notice. Mostly non-essential system messages.
   m  Normal message.
   w  Warnings.
   s  Severe errors.
   f  Fatal errors.
Default filter can be modified using +<chars> or -<chars>.  Specifing no plus
or minus sign, or an equal sign sets the filter to exactly the specified
pattern.

Disabling/enabling features:
  -C  Disable caching of instruction executors (default: enabled).
  -G  Disable mark & sweep garbage collection (default: enabled).
  -R  Enable reference couting garbage collection (default: disabled).
  -V  Verbose debugging. This is not the verbose information the user can enable
      in the logging filter; it's really only useful for the developers.  
  -F  Disable the SII collapser (default: enabled)
  -I  Enables the Memoised GC (default: disabled)
  -P  Disable stateful dynamic POR (default: enabled)
  -p  Disable POR using object escape analysis (default: enabled)
  -t  Disable error tracing (default: enabled)";
			System.Console.WriteLine(help_text);
		}

		public static void PrintConfig() {
			System.Type configType = typeof(Config);
			foreach (FieldInfo fld in configType.GetFields()) {
				Logger.l.Notice(
					System.String.Format("Config.{0,-25} = {1,-25}", fld.Name, fld.GetValue(null)).ToString()
				);
			}

#if DEBUG
			Logger.l.Notice("DEBUG is enabled");
#else
			Logger.l.Notice("RELEASE is enabled");
#endif
		}

		/// MMC entry point
		///
		/// First, parse command line arguments. Then create an initial state,
		/// create an explorer, and let it run.
		///
		/// \param args Command-line options to MMC.
		public static void Main(string[] args) {
			System.Console.WriteLine(copyright + "\n");
			ParseCommandLineOptions(args);


			PrintConfig();
			StateSpaceSetup.LoadAssemblies();
			StateSpaceSetup.CreateInitialState();

			Explorer ex = new Explorer();
			TextWriter tw = null;

			try {
				Statistics.s.Start();
				bool noErrors = ex.Run();

				if (!noErrors && Config.StopOnError && Config.TraceOnError) {
					ActiveState.cur.Reset();
					StateSpaceSetup.CreateInitialState();

					string traceFile = Config.AssemblyToCheckFileName + ".trace";
					File.Delete(traceFile);
					tw = File.CreateText(traceFile);

					ex = new TracingExplorer(ex.GetErrorTrace(),tw);
					ex.Run();
					tw.WriteLine(ActiveState.cur.ToString());
				}
			} finally {

				Statistics.s.Stop();
				// Done, show statistics
				if (Config.ShowStatistics)
					Logger.l.Message("statistics: {0}", Statistics.s.ToString());
				
				if (tw != null) {
					tw.Flush();
					tw.Close();
					Logger.l.Message("Trace written to " + Config.AssemblyToCheckFileName + ".trace");
				}

				Logger.l.CloseAll();
				DotWriter.End();
			}

			// to wake me (the user) up after a (long) exploration :)
			//System.Console.Write("\a");
		}

	}
}
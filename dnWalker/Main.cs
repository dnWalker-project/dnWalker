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

using System.ComponentModel;

namespace MMC
{

    using System.IO;
    using System.Collections;
    using System.Reflection;
    using MMC.State;
    using System;
    using System.Linq;
    using MMC.Data;

    public interface IConfig
    {
        string[] RunTimeParameters { get; set; }
        bool Verbose { get; set; }
        bool SymmetryReduction { get; set; }
        bool UseRefCounting { get; set; }
        bool NonStaticSafe { get; set; }
        bool UseMarkAndSweep { get; set; }
        bool UseDPORCollapser { get; set; }
        bool UseInstructionCache { get; set; }
        bool OneTraceAndStop { get; set; }
        double MaxExploreInMinutes { get; set; }
        string AssemblyToCheckFileName { get; set; }
        bool StopOnError { get; set; }
        bool Interactive { get; set; }
        bool MemoisedGC { get; set; }
        double MemoryLimit { get; set; }
        bool UseStatefulDynamicPOR { get; set; }
        bool UseObjectEscapePOR { get; set; }
        bool ShowStatistics { get; set; }
        bool TraceOnError { get; set; }
        bool Quiet { get; set; }
        double OptimizeStorageAtMegabyte { get; set; }
        LogPriority LogFilter { get; }
        string LogFileName { get; }

        int StateStorageSize { get; }

        void SetCustomSetting(string key, object value);
        T GetCustomSetting<T>(string key);
    }

    /// <summary>
    /// <para>Configuration class</para>
    /// <para>
    /// This is just a big collection of fields. Because of C# properties, the
    /// functionality behind access to these fields can be transparently
    /// adjusted without breaking source compatibility.
    /// </para>
    /// </summary>
    public class Config : IConfig
    {
        public string AssemblyToCheckFileName { get; set; }
        public string[] RunTimeParameters { get; set; } = new string[] { };
        public bool ShowStatistics { get; set; }
        public bool Quiet { get; set; }
        public bool Interactive { get; set; }
        public LogPriority LogFilter { get; set; }
        public bool UseInstructionCache { get; set; } = true;
        public bool UseRefCounting { get; set; }
        public bool UseMarkAndSweep { get; set; } = true;
        public bool Verbose { get; set; }
        public bool SymmetryReduction { get; set; } = true;
        public bool NonStaticSafe { get; set; }
        public bool MemoisedGC { get; set; }
        public bool UseDPORCollapser { get; set; } = true;
        public bool UseObjectEscapePOR { get; set; } = true;
        public bool UseStatefulDynamicPOR { get; set; } = true;
        public bool StopOnError { get; set; } = true;
        public bool TraceOnError { get; set; } = true;
        public bool OneTraceAndStop { get; set; }
        public bool ExPostFactoMerging { get; set; } = true;
        public double MaxExploreInMinutes { get; set; } = double.PositiveInfinity;
        public double OptimizeStorageAtMegabyte { get; set; } = double.PositiveInfinity;
        public double MemoryLimit { get; set; } = double.PositiveInfinity;
        public string LogFileName { get; set; }
        public int StateStorageSize { get; set; } = 20;

        private readonly System.Collections.Generic.IDictionary<string, object> _custom =
            new System.Collections.Generic.Dictionary<string, object>();
        public void SetCustomSetting(string key, object value)
        {
            _custom[key] = value;
        }

        public T GetCustomSetting<T>(string key)
        {
            if (!_custom.TryGetValue(key, out var value))
            {
                return default(T);
            }

            if (value is IConvertible convertible)
            {
                return (T) Convert.ChangeType(convertible, typeof(T));
            }

            return (T) value;

        }
    }

    /// The main application class.
    class MonoModelChecker
    {
        private Logger _logger;

        /// <summary>
        /// Print a verbose (debug) message.
        /// </summary>
        /// <remarks>
        /// Do not use this function as an alternative to the logger, only to
        /// print debug information when actually debugging.
        /// </remarks>
        /// <param name="msg">Message (cf. string.Format).</param>
        /// <param name="value">Values for the message string.</param>
        public void Message(string msg, params object[] values)
        {
            //if (Config.Instance.Verbose)
            {
                string to_write = (values.Length > 0 ? string.Format(msg, values) : msg);
                System.Console.WriteLine(to_write);
            }
        }

        /// <summary>
        /// Print a fatal message and die.
        /// </summary>
        /// <param name="msg">Message to print before quitting.</param>
        public void Fatal(string msg)
        {
            _logger?.CloseAll();
            DotWriter.End();
            System.Console.WriteLine("FATAL: " + msg);
            System.Console.WriteLine("use option \"-h\" for help");
            System.Environment.Exit(2);
        }

        /// <summary>
        /// Open a file for writing.
        /// </summary>
        /// <param name="filename">Name of the file to output to.</param>
        TextWriter TryOpen(string filename)
        {
            StreamWriter retval = null;
            try
            {
                retval = File.CreateText(filename);
                retval.AutoFlush = true;
            }
            catch (System.ArgumentException)
            {
                Fatal("malformed filename: " + filename);
            }
            catch (DirectoryNotFoundException)
            {
                Fatal("directory not found for filename: " + filename);
            }
            catch (PathTooLongException)
            {
                Fatal("path name too long for filename: " + filename);
            }
            catch (IOException)
            {
                Fatal("i/o exception in opening filename: " + filename);
            }
            return retval;
        }

        /// <summary>
        /// Parse command line options.
        /// </summary>
        /// <remarks>
        /// This sets various fields in class Config.
        /// </remarks>
        /// <param name="args">Command-line options as passed to Main.</param>
        public IConfig GetConfigFromCommandLine(string[] args)
        {
            var config = new Config();
            for (int i = 0; i < args.Length; ++i)
            {
                if (args[i][0] != '-' || args[i].Length <= 1)
                {
                    Fatal("malformed argument: " + args[i]);
                }
                char[] flags = args[i].ToCharArray();
                for (int f = 1; f < flags.Length; ++f)
                {
                    switch (flags[f])
                    {
                        case 'A':
                            config.OneTraceAndStop = true;
                            break;
                        case 'C':
                            config.UseInstructionCache = false;
                            break;
                        case 'F':
                            config.UseDPORCollapser = false;
                            break;
                        case 'G':
                            config.UseMarkAndSweep = false;
                            break;
                        case 'O':
                            config.NonStaticSafe = true;
                            break;
                        case 'R':
                            config.UseRefCounting = true;
                            break;
                        case 'S':
                            config.SymmetryReduction = false;
                            break;
                        case 'T':
                            ++i;
                            if (i < args.Length)
                                config.MaxExploreInMinutes = System.Double.Parse(args[i]);
                            else
                                Fatal("-T option requires an argument");
                            break;

                        case 'a':
                            ++i;
                            if (i < args.Length)
                            {
                                if (File.Exists(args[i]))
                                    config.AssemblyToCheckFileName = args[i];
                                else
                                    Fatal("assembly file not found: " + args[i]);
                            }
                            else
                                Fatal("-a option requires an argument");
                            break;
                        case 'c':
                            config.StopOnError = false;
                            break;
                        case 'f':
                            ++i;
                            if (i < args.Length)
                            {
                                if (!Logger.TryParseLogFilter(args[i], out var logFilter))
                                {
                                    Fatal("malformed log filter (use -h)");
                                }
                                config.LogFilter = logFilter;
                            }
                            else
                                Fatal("-f option requires an argument");
                            break;
                        case 'h':
                            PrintCommandLineUsage();
                            System.Environment.Exit(0);
                            break;
                        case 'i':
                            config.Interactive = true;
                            break;
                        case 'I':
                            config.MemoisedGC = true;
                            break;
                        case 'm':
                            ++i;
                            if (i < args.Length)
                                config.MemoryLimit = System.Int32.Parse(args[i]);
                            else
                                Fatal("-m option requires an argument");
                            break;
                        case 'o':
                            ++i;
                            if (i < args.Length)
                                config.OptimizeStorageAtMegabyte = System.Int32.Parse(args[i]);
                            else
                                Fatal("-o option requires an argument");
                            break;
                        case 'p':
                            config.UseObjectEscapePOR = false;
                            break;
                        case 'P':
                            config.UseStatefulDynamicPOR = false;
                            config.UseDPORCollapser = false;
                            break;
                        case 'l':
                            ++i;
                            if (i < args.Length)
                            {
                                //logger.AddOutput(new TextLoggerOutput(TryOpen(args[i])));
                            }
                            else
                                Fatal("-l option requies an argument");
                            break;
                        case 'r':
                            config.RunTimeParameters = FetchTillNextArgument(args, i + 1);
                            i += config.RunTimeParameters.Length;
                            break;
                        case 's':
                            config.ShowStatistics = true;
                            break;
                        case 't':
                            config.TraceOnError = false;
                            break;
                        case 'q':
                            config.Quiet = true;
                            break;
                        case 'V':
                            config.Verbose = true;
                            break;
                        default:
                            Fatal("unknown parameter: " + flags[f]);
                            break;
                    }
                }
            }

            // Fatal combination (or lack of) parameters.
            if (config.Quiet && config.Interactive)
                Fatal("you have asked me to be quiet and interactive. make up your mind.");
            if (config.AssemblyToCheckFileName == null)
                Fatal("no assembly to check specified");

            if (config.RunTimeParameters == null)
                config.RunTimeParameters = new string[] { };

            return config;
        }

        static string[] FetchTillNextArgument(string[] args, int offset)
        {
            ArrayList toReturn = new ArrayList();
            for (int i = offset; i < args.Length; ++i)
            {
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
        
        static void PrintCommandLineUsage()
        {

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

        public static void PrintConfig(IConfig config, Logger logger)
        {
            var configType = typeof(IConfig);
            foreach (FieldInfo fld in configType.GetFields())
            {
                logger.Notice(string.Format("Config.{0,-25} = {1,-25}", fld.Name, fld.GetValue(config)));
            }

#if DEBUG
            logger.Notice("DEBUG is enabled");
#else
			logger.Notice("RELEASE is enabled");
#endif
        }

        /// <summary>
        /// MMC entry point
        /// </summary>
        /// <remarks>
        /// First, parse command line arguments. Then create an initial state,
        /// create an explorer, and let it run.
        /// </remarks>
        /// <param name="args">Command-line options to MMC.</param>
        public static void Main(string[] args)
        {
            new MonoModelChecker().Go(args);
        }

        public void Go(string[] args)
        {
            Console.WriteLine(copyright + "\n");

            var config = GetConfigFromCommandLine(args);

            var logger = new Logger(config.LogFilter);
            _logger = logger;

            string dotFile = config.AssemblyToCheckFileName + ".dot";
            File.Delete(dotFile);
            DotWriter.Begin(TryOpen(dotFile));

            // Defaults.
            if (!config.Quiet)
            {
                logger.AddOutput(new TextLoggerOutput(Console.Out));
            }

            PrintConfig(config, logger);

            var assemblyLoader = new dnWalker.AssemblyLoader();
            assemblyLoader.GetModuleDef(File.ReadAllBytes(config.AssemblyToCheckFileName));

            var definitionProvider = DefinitionProvider.Create(assemblyLoader);

            var stateSpaceSetup = new StateSpaceSetup(definitionProvider, config, logger);

            var methodArgs = config.RunTimeParameters.Select(a => new ConstantString(a)).Cast<IDataElement>().ToArray();

            var cur = stateSpaceSetup.CreateInitialState(assemblyLoader.GetModule().EntryPoint, methodArgs);
            var statistics = new SimpleStatistics();

            Explorer ex = new Explorer(
                cur,
                statistics,
                logger,
                config);

            TextWriter tw = null;
            try
            {
                statistics.Start();
                bool noErrors = ex.Run();

                if (!noErrors && config.StopOnError && config.TraceOnError)
                {
                    cur.Reset();
                    cur = stateSpaceSetup.CreateInitialState(assemblyLoader.GetModule().EntryPoint, methodArgs);

                    string traceFile = config.AssemblyToCheckFileName + ".trace";
                    File.Delete(traceFile);
                    tw = File.CreateText(traceFile);

                    ex = new TracingExplorer(cur, statistics, ex.GetErrorTrace(), tw, logger, config);
                    ex.Run();
                    tw.WriteLine(cur.ToString());
                }
            }
            finally
            {
                statistics.Stop();
                // Done, show statistics
                if (config.ShowStatistics)
                {
                    logger.Message("statistics: {0}", statistics.ToString());
                }

                if (tw != null)
                {
                    tw.Flush();
                    tw.Close();
                    logger.Message("Trace written to " + config.AssemblyToCheckFileName + ".trace");
                }

                logger.CloseAll();
                DotWriter.End();
            }

            // to wake me (the user) up after a (long) exploration :)
            //System.Console.Write("\a");
        }

    }
}
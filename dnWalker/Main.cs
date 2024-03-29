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
    using dnWalker;
    using dnWalker.TypeSystem;
    using dnWalker.Graphs.ControlFlow;
    using dnWalker.Traversal;
    using dnWalker.Configuration;


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
            //if (IConfiguration.Instance.Verbose)
            {
                var to_write = (values.Length > 0 ? string.Format(msg, values) : msg);
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
        /// This sets various fields in class IConfiguration.
        /// </remarks>
        /// <param name="args">Command-line options as passed to Main.</param>
        public IConfigurationBuilder GetConfigurationFromCommandLine(string[] args, IConfigurationBuilder configuration)
        {

            for (var i = 0; i < args.Length; ++i)
            {
                if (args[i][0] != '-' || args[i].Length <= 1)
                {
                    Fatal("malformed argument: " + args[i]);
                }
                var flags = args[i].ToCharArray();
                for (var f = 1; f < flags.Length; ++f)
                {
                    switch (flags[f])
                    {
                        case 'A':
                            configuration.SetOneTraceAndStop(true);
                            break;
                        case 'C':
                            configuration.SetUseInstructionCache(false);
                            break;
                        case 'F':
                            configuration.SetUseDPORCollapser(false);
                            break;
                        case 'G':
                            configuration.SetUseMarkAndSweep(false);
                            break;
                        case 'O':
                            configuration.SetNonStaticSafe(true);
                            break;
                        case 'R':
                            configuration.SetUseRefCounting(true);
                            break;
                        case 'S':
                            configuration.SetSymmetryReduction(false);
                            break;
                        case 'T':
                            ++i;
                            if (i < args.Length)
                                configuration.SetMaxExploreInMinutes(System.Double.Parse(args[i]));
                            else
                                Fatal("-T option requires an argument");
                            break;

                        case 'a':
                            ++i;
                            if (i < args.Length)
                            {
                                if (File.Exists(args[i]))
                                    configuration.SetAssemblyToCheckFileName(args[i]);
                                else
                                    Fatal("assembly file not found: " + args[i]);
                            }
                            else
                                Fatal("-a option requires an argument");
                            break;
                        case 'c':
                            configuration.SetStopOnError(false);
                            break;
                        case 'f':
                            ++i;
                            if (i < args.Length)
                            {
                                if (!Logger.TryParseLogFilter(args[i], out var logFilter))
                                {
                                    Fatal("malformed log filter (use -h)");
                                }
                                configuration.SetLogFilter(logFilter);
                            }
                            else
                                Fatal("-f option requires an argument");
                            break;
                        case 'h':
                            PrintCommandLineUsage();
                            System.Environment.Exit(0);
                            break;
                        case 'i':
                            configuration.SetInteractive(true);
                            break;
                        case 'I':
                            configuration.SetMemoisedGC(true);
                            break;
                        case 'm':
                            ++i;
                            if (i < args.Length)
                                configuration.SetMemoryLimit(System.Int32.Parse(args[i]));
                            else
                                Fatal("-m option requires an argument");
                            break;
                        case 'o':
                            ++i;
                            if (i < args.Length)
                                configuration.SetOptimizeStorageAtMegabyte(System.Int32.Parse(args[i]));
                            else
                                Fatal("-o option requires an argument");
                            break;
                        case 'p':
                            configuration.SetUseObjectEscapePOR(false);
                            break;
                        case 'P':
                            configuration.SetUseStatefulDynamicPOR(false);
                            configuration.SetUseDPORCollapser(false);
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
                            configuration.SetRunTimeParameters(FetchTillNextArgument(args, i + 1));
                            i += configuration.RunTimeParameters().Length;
                            break;
                        case 's':
                            configuration.SetShowStatistics(true);
                            break;
                        case 't':
                            configuration.SetTraceOnError(false);
                            break;
                        case 'q':
                            configuration.SetQuiet(true);
                            break;
                        case 'V':
                            configuration.SetVerbose(true);
                            break;
                        default:
                            Fatal("unknown parameter: " + flags[f]);
                            break;
                    }
                }
            }

            // Fatal combination (or lack of) parameters.
            if (configuration.Quiet() && configuration.Interactive())
                Fatal("you have asked me to be quiet and interactive. make up your mind.");
            if (configuration.AssemblyToCheckFileName == null)
                Fatal("no assembly to check specified");

            if (configuration.RunTimeParameters() == null)
                configuration.SetRunTimeParameters(new string[] { });

            return configuration;
        }

        static string[] FetchTillNextArgument(string[] args, int offset)
        {
            var toReturn = new ArrayList();
            for (var i = offset; i < args.Length; ++i)
            {
                if (args[i][0] == '-')
                    break;
                toReturn.Add(args[i]);
            }
            return (string[])toReturn.ToArray(typeof(string));
        }

        static string copyright = @"dnSpy + MoonWalker ((C) University of Twente, Formal Methods and Tools group)";
        
        static void PrintCommandLineUsage()
        {

            var help_text = @"The following command line parameters are REQUIRED:
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

//        public static void PrintIConfiguration(IConfiguration config, Logger logger)
//        {
//            var configType = typeof(IConfiguration);
//            foreach (var fld in configType.GetFields())
//            {
//                logger.Notice(string.Format("IConfiguration.{0,-25} = {1,-25}", fld.Name, fld.GetValue(config)));
//            }

//#if DEBUG
//            logger.Notice("DEBUG is enabled");
//#else
//			logger.Notice("RELEASE is enabled");
//#endif
//        }

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
            static IDataElement[] StrArrToDataElements(string[] strArr)
            {
                IDataElement[] dataElements = new IDataElement[strArr.Length];

                for (int i = 0; i < strArr.Length; ++i)
                {
                    dataElements[i] = new ConstantString(strArr[i]);
                }

                return dataElements;
            }

            Console.WriteLine(copyright + "\n");

            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .InitializeDefaults();
                
            GetConfigurationFromCommandLine(args, configBuilder);

            IConfiguration config = configBuilder.Build();


            Logger logger = new Logger(config.LogFilter());
            _logger = logger;

            var dotFile = config.AssemblyToCheckFileName() + ".dot";
            File.Delete(dotFile);
            DotWriter.Begin(TryOpen(dotFile));

            // Defaults.
            if (!config.Quiet())
            {
                logger.AddOutput(new TextLoggerOutput(Console.Out));
            }

            //PrintIConfiguration(config, logger);

            IDefinitionProvider definitionProvider = new DefinitionProvider(Domain.LoadFromFile(config.AssemblyToCheckFileName()));

            StateSpaceSetup stateSpaceSetup = new StateSpaceSetup(definitionProvider, config, logger);

            IDataElement[] methodArgs = StrArrToDataElements(config.RunTimeParameters());

            ExplicitActiveState cur = stateSpaceSetup.CreateInitialState(definitionProvider.Context.MainModule.EntryPoint, methodArgs);
            SimpleStatistics statistics = new SimpleStatistics();            

            Explorer ex = new Explorer(
                cur,
                statistics,
                logger,
                config,
                new dnWalker.Traversal.PathStore(definitionProvider.Context.MainModule.EntryPoint, new ControlFlowGraphProvider()));

            TextWriter tw = null;
            try
            {
                statistics.Start();
                bool noErrors = ex.Run();

                if (!noErrors && config.StopOnError() && config.TraceOnError())
                {
                    cur.Reset();
                    cur = stateSpaceSetup.CreateInitialState(definitionProvider.Context.MainModule.EntryPoint, methodArgs);

                    var traceFile = config.AssemblyToCheckFileName() + ".trace";
                    File.Delete(traceFile);
                    tw = File.CreateText(traceFile);

                    ex = new TracingExplorer(cur, statistics, ex.GetErrorTrace(), tw, logger, config, new PathStore(definitionProvider.Context.MainModule.EntryPoint, new ControlFlowGraphProvider()));
                    ex.Run();
                    tw.WriteLine(cur.ToString());
                }
            }
            finally
            {
                statistics.Stop();
                // Done, show statistics
                if (config.ShowStatistics())
                {
                    logger.Message("statistics: {0}", statistics.ToString());
                }

                if (tw != null)
                {
                    tw.Flush();
                    tw.Close();
                    logger.Message("Trace written to " + config.AssemblyToCheckFileName() + ".trace");
                }

                logger.CloseAll();
                DotWriter.End();
            }

            // to wake me (the user) up after a (long) exploration :)
            //System.Console.Write("\a");
            System.Console.ReadLine();
        }

    }
}
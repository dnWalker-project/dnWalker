using System;

namespace dnWalker.Interface
{
    internal class BatchModeRunner : RunnerBase
    {
        public BatchModeRunner(Options options) : base(options)
        {
        }

        public override int Run()
        {
            string scriptFile = Options.Script;
            if (string.IsNullOrWhiteSpace(scriptFile)) 
            {
                Console.WriteLine("Commands not specified. The options -s --script must contain path to the command file.");
                return -1;
            }
            if (!System.IO.File.Exists(scriptFile)) 
            {
                Console.WriteLine("Commands file does not exists.");
                return -1;
            }

            try
            {
                return RunCommands(System.IO.File.ReadAllLines(scriptFile));
            }
            catch (Exception e) // which exception? IO etc...
            {
                Console.WriteLine($"Error during execution: '{e}'");
                return -1;
            }
        }
    }
}
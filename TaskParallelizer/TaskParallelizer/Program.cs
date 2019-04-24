using System;
using System.Collections.Generic;

namespace VGA.Parallel
{
    class Program
    {
        const int MAX_PARALLEL_THREADS = 8;

        static int Main(string[] args)
        {
            // Parse the command line args.
            // Expected usage is either one or more -c "command" and an optional -f "file"
            // The file is expected to contain a list of commands to be executed in parallel.
            List<string> commands = new List<string>();
            int exitCode = 0;
            
            if (args.Length < 2 || !ProcessArgs(args, ref commands))
            {
                PrintUsage();
                exitCode = -1;
            }
            else
            {
                ParallelCore parallelCore = new ParallelCore(MAX_PARALLEL_THREADS, commands, new ParallelCore.RecordMessage(Console.WriteLine));
                parallelCore.BeforeProcessSingleItem += ParallelCore_BeforeProcessSingleItem;
                parallelCore.AfterProcessSingleItem += ParallelCore_AfterProcessSingleItem;
                parallelCore.Start();
                parallelCore.WaitForCompletion();
            }
            return exitCode;
        }

        private static void ParallelCore_AfterProcessSingleItem(Tools.ProducerConsumer.IProcessingStage<string, int> sender, string item, int processedItem)
        {
            Console.WriteLine($"== Command {item} exited with code {processedItem} ==");
        }

        private static void ParallelCore_BeforeProcessSingleItem(Tools.ProducerConsumer.IProcessingStage<string, int> sender, string item)
        {
            Console.WriteLine($"Running command '{item}'");
        }

        private static bool ProcessArgs(string[] args, ref List<string> commands)
        {
            bool status = true;
            for (int ind = 0; ind < args.Length; ind++)
            {
                string arg = args[ind];
                switch (arg)
                {
                    case "-c":
                        ind++;
                        if (ind < args.Length)
                        {
                            commands.Add(args[ind]);
                        }
                        else
                        {
                            status = false;
                        }
                        break;
                    case "-f":
                        ind++;
                        if (ind < args.Length)
                        {
                            using (var file = System.IO.File.OpenRead(args[ind]))
                            {
                                using (System.IO.StreamReader sr = new System.IO.StreamReader(file))
                                {
                                    while (!sr.EndOfStream)
                                    {
                                        commands.Add(sr.ReadLine());
                                    }
                                }
                            }
                        }
                        else
                        {
                            status = false;
                        }
                        break;
                    default:
                        // Unknown switch
                        Console.WriteLine($"Unrecognized option '{arg}' specified");
                        status = false;
                        break;
                }
                if (!status)
                {
                    break;
                }
            }
            return status;
        }

        private static void PrintUsage()
        {
            string selfName = "dotnet Parallel.dll";
            if (System.Reflection.Assembly.GetEntryAssembly().GetName().EscapedCodeBase.EndsWith("Parallel.exe"))
            {
                selfName = "Parallel.exe";
            }
            Console.WriteLine($"Usage: {selfName} -c \"<COMMAND>\" [-f COMMAND_INPUT_FILE]");
            Console.WriteLine("  -c    Specifies a command as COMMAND, that can be executed from the command line.");
            Console.WriteLine("  -f    Specifies a file (COMMAND_INPUT_FILE) containing a list of commands, each command being on a separate line.");
            Console.WriteLine("");
            Console.WriteLine("Psst: Prefix any line in the command file with [e] to indicate an executable being called.");
            Console.WriteLine("Anything without a prefix will be wrapped in call to cmd.exe, prefixing with '[e]' will tell the parallelizer to avoid that wrapping.");
        }
    }
}

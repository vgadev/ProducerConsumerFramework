using System;
using System.Collections.Generic;
using System.Text;
using VGA.Tools.ProducerConsumer;

namespace VGA.Parallel
{
    public class ParallelCore : PipelineStage<string, int>
    {
        public delegate void RecordMessage(string message);
        private RecordMessage mRecorder;

        public ParallelCore(int threadCount, IEnumerable<string> inputEnumerator, RecordMessage recorder) : base("ParallelCore", threadCount, inputEnumerator, true, false)
        {
            mRecorder = recorder;
        }

        public override int ProcessSingleItem(string inputCommand)
        {
            bool exe = false;
            string inputItem = inputCommand;
            string exe_name = "cmd.exe";
            if (inputItem.StartsWith("[e]"))
            {
                exe = true;
                inputItem = inputItem.Substring(3);
                inputItem = inputItem.TrimStart(' ');
            }
            System.Diagnostics.ProcessStartInfo pinfo = new System.Diagnostics.ProcessStartInfo();
            pinfo.RedirectStandardOutput = true;
            pinfo.RedirectStandardError = true;
            pinfo.UseShellExecute = false;
            if (exe)
            {
                int space_index = inputItem.IndexOf(' ');
                exe_name = inputItem.Substring(0, space_index);
                inputItem = inputItem.Substring(space_index + 1);
                pinfo.Arguments = inputItem;
            }
            else
            {
                pinfo.Arguments = "/c " + inputItem;
            }
            pinfo.FileName = exe_name;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = pinfo;

            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_OutputDataReceived;

            process.Start();

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            process.WaitForExit();
            return process.ExitCode;
        }

        private void Process_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            WriteOutput("> " + e.Data);
        }

        private void WriteOutput(string message)
        {
            if(mRecorder != null)
            {
                mRecorder(message);
            }
        }

    }
}

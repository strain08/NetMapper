using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetDriveManager.Services.Helpers
{
    public class ShellCommand
    {
        const int NETUSE_STATUS = 0;
        const int NETUSE_LOCAL = 1;
        const int NETUSE_REMOTE = 2;
        private readonly ProcessStartInfo? _startInfo;
        private readonly Process _process;

        public string NetUseStatus(string driveLetter)
        {

            IEnumerable<String> output = ExecuteCommandSync("net use")
                                                        .Split('\n')
                                                        .Where(x => x.Contains(':'));

            foreach (string line in output)
            {
                string[] status = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (status[NETUSE_LOCAL][0] == driveLetter[0])
                {
                    return status[NETUSE_STATUS];
                }
            }
            return string.Empty;
        }


        public ShellCommand()
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            _process = new Process
            {
                StartInfo = procStartInfo
            };
        }

        private string ExecuteCommandSync(string command)
        {
            try
            {
                _process.StartInfo.FileName = "cmd ";
                _process.StartInfo.Arguments = "/c " + command;
                _process.Start();
                return _process.StandardOutput.ReadToEnd();

            }
            catch (Exception ex)
            {
                // Log the exception
                Debug.WriteLine("ExecuteCommandSync failed" + ex.Message);
                return string.Empty;
            }
        }
    }
}

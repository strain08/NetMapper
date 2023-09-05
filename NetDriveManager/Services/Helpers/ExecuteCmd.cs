using NetDriveManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetDriveManager.Services.Helpers
{
    public static class ExecuteCmd
    {
        const int NETUSE_STATUS = 0;
        const int NETUSE_LOCAL = 1;
        const int NETUSE_REMOTE = 2;

        public static string NetUseStatus(string driveLetter)
        {  
            var output = ExecuteCmd.ExecuteCommandSync("net use");            
            
            string[] lines = output.Split('\n');
            foreach (string line in lines)
            {
                if (line.Contains(':'))
                {
                    string[] cols = line.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    //Console.WriteLine($"Drive {cols[NETUSE_LOCAL]} mapped to {cols[NETUSE_REMOTE]} has status {cols[NETUSE_STATUS]}.");
                    // Status = 13 char
                    // Local = 10 char
                    // Remote = 26 char
                    if (cols[NETUSE_LOCAL][0] == driveLetter[0])
                    {
                        Debug.WriteLine(cols[NETUSE_STATUS] + cols[NETUSE_REMOTE]);
                        return cols[NETUSE_STATUS];
                        
                    }

                }
            }
            Debug.WriteLine("EMPTY");
            return string.Empty;
        }
        /// <summary>
        /// Executes a shell command synchronously.
        /// </summary>
        /// <param name="command">string command</param>
        /// <returns>string, as output of the command.</returns>
        public static string ExecuteCommandSync(object command)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run, and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows, and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);
                // The following commands are needed to redirect the standard output. 
                //This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();

                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();

                return result;
            }
            catch (Exception objException)
            {
                // Log the exception
                Console.WriteLine("ExecuteCommandSync failed" + objException.Message);
                return string.Empty;
            }
        }
    }
}

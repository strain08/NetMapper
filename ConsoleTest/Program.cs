// See https://aka.ms/new-console-template for more information

namespace WMISample
{
    public static class Program
    {

        public static void Main(string[] args)
        {
            var result = ExecuteCmd.ExecuteCommandSync("net use");
            const int STATUS_COL = 0;
            const int LOCAL_COL = 1;
            const int REMOTE_COL = 2;
            //Console.WriteLine(result);
            string[] lines = result.Split('\n');
            foreach (string line in lines)
            {
                if (line.Contains(":"))
                {
                    string[] cols = line.Split(" ",StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    Console.WriteLine($"Drive {cols[LOCAL_COL]} mapped to {cols[REMOTE_COL]} has status {cols[STATUS_COL]}.");
                    // Status = 13 char
                    // Local = 10 char
                    // Remote = 26 char

                }
            }

            Console.ReadLine();
        }
    }
    public static class ExecuteCmd
    {
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



using System;
using System.Diagnostics;

namespace NetMapper.Services;
public class ShellCmdBase
{
    private readonly Process _process;
    private readonly ProcessStartInfo _startInfo;
    //CTOR
    public ShellCmdBase()
    {
        _startInfo = new ProcessStartInfo
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        _process = new Process
        {
            StartInfo = _startInfo
        };
    }
    protected string ExecuteCmdSync(string command)
    {
        try
        {
            _process.StartInfo.FileName = "cmd ";
            _process.StartInfo.Arguments = "/c " + command; // execute and exit - /c 
            _process.Start();
            return _process.StandardOutput.ReadToEnd();

        }
        catch (Exception ex)
        {
            // Log the exception
            Debug.WriteLine("ExecuteCommandSync failed: " + ex.Message);
            return string.Empty;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace NetMapper.Services;
public enum NetUseStatus
{
    OK,
    Reconnecting,
    Disconnected,
    NotPresent,
    Error,
    Undefined
}
public class CmdNetUse : ShellCmdBase
{
    private const int
        NETUSE_STATUS = 0,
        NETUSE_LOCAL = 1;


    public NetUseStatus DriveStatus(char driveLetter)
    {
        IEnumerable<string>? output;
        // return only those strings containing "\\"
        // UNC path unusuable due to the way command output splits strings
        output = ExecuteCmdSync("net use").Split('\n').Where(x => x.Contains(@"\\"));

        // possible status: OK, Reconnecting, Disconnected
        if (output is null) return NetUseStatus.Error;
        if (!output.Any()) return NetUseStatus.Error;

        foreach (string line in output)
        {
            string[] status = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (status[NETUSE_LOCAL][0] == driveLetter)
                return status[NETUSE_STATUS] switch
                {
                    "OK" => NetUseStatus.OK,
                    "Reconnecting" => NetUseStatus.Reconnecting,
                    "Disconnected" => NetUseStatus.Disconnected,
                    _ => NetUseStatus.Undefined,
                };

        }
        return NetUseStatus.NotPresent;
    }

}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDriveManager.Enums
{
    public enum ConnectionState
    {
        Undefined,
        Degraded,
        Connected,
        Disconnected,
        Connecting,
        Disconnecting
    }
}

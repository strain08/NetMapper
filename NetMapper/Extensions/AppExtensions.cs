using System;
using System.Diagnostics;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Threading;

namespace NetMapper.Extensions;

public static class AppExtensions
{
    public static unsafe void EnableEfficiencyMode()
    {
        if (!OperatingSystem.IsWindowsVersionAtLeast(8)) return;

        var processHandle = Process.GetCurrentProcess().SafeHandle;
        var handle = new HANDLE(processHandle.DangerousGetHandle());

        PInvoke.SetPriorityClass(handle, PROCESS_CREATION_FLAGS.IDLE_PRIORITY_CLASS);

        PROCESS_POWER_THROTTLING_STATE state = new()
        {
            Version = PInvoke.PROCESS_POWER_THROTTLING_CURRENT_VERSION,
            ControlMask = PInvoke.PROCESS_POWER_THROTTLING_EXECUTION_SPEED,
            StateMask = PInvoke.PROCESS_POWER_THROTTLING_EXECUTION_SPEED,
        };

        PInvoke.SetProcessInformation(
            handle,
            PROCESS_INFORMATION_CLASS.ProcessPowerThrottling,
            &state,
            (uint)sizeof(PROCESS_POWER_THROTTLING_STATE)
        );
    }
}

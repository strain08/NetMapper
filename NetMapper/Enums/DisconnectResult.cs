using System.ComponentModel;

namespace NetMapper.Enums;

public enum DisconnectResult
{
    /// <summary>
    /// The connection was successfully removed.
    /// </summary>
    [Description("DisconnectResult_Success")]
    Success = 0,
    /// <summary>
    /// An unexpected failure occurred - no enum constant was defined for
    /// this case.
    /// </summary>
    [Description("DisconnectResult_UnknownFailure")]
    UnknownDisconnectionFailure = -1,
    /// <summary>
    /// The user profile is in an incorrect format.
    /// </summary>
    [Description("DisconnectResult_BadProfile")]
    BadProfile = 1206,
    /// <summary>
    /// The system is unable to open the user profile to process persistent connections.
    /// </summary>
    [Description("DisconnectResult_CannotOpenProfile")]
    CannotOpenProfile = 1205,
    /// <summary>
    /// The device is in use by an active process and cannot be disconnected.
    /// </summary>
    [Description("DisconnectResult_DeviceInUse")]
    DeviceInUse = 2404,
    /// <summary>
    /// A network-specific error occurred. To obtain a description of the error,
    /// call the WNetGetLastError function.
    /// </summary>
    [Description("DisconnectResult_ExtendedError")]
    ExtendedError = 1208,
    /// <summary>
    /// The name specified by the lpName parameter is not a redirected device,
    /// or the system is not currently connected to the device specified by
    /// the parameter.
    /// </summary>
    [Description("DisconnectResult_NotConnected")]
    NotConnected = 2250,
    /// <summary>
    /// Files of the share are currently in use and disconnection was not
    /// forced.
    /// </summary>
    [Description("DisconnectResult_OpenFiles")]
    OpenFiles = 2401
}
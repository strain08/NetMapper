public enum GetConnectionStatus
{
    /// <summary>
    /// The operation completed successfully.
    /// </summary>
    Success = 0,
    /// <summary>
    /// The string pointed to by the lpLocalName parameter is invalid.
    /// </summary>
    BadDevice = 1200,
    /// <summary>
    /// The device is not currently connected, but it is a persistent connection.
    /// For more information, see the following Remarks section.
    /// </summary>
    ConnectionUnavailable = 1201,
    /// <summary>
    /// A network-specific error occurred. To obtain a description of the error,
    /// call the WNetGetLastError function.
    /// </summary>
    ExtendedError = 1208,
    /// <summary>
    /// The buffer that was submitted to the function was too small.
    /// The lpnLength parameter points to a variable that contains the
    /// required buffer size. More entries are available with subsequent calls.
    /// </summary>
    MoreData = 234,
    /// <summary>
    /// Invalid operation.
    /// </summary>
    NotSupported = 50,
    /// <summary>
    /// None of the providers recognize the local name as having a connection.
    /// However, the network is not available for at least one provider to whom
    /// the connection may belong.
    /// </summary>
    NotNetworkOrBadPath = 1203,
    /// <summary>
    /// The network is unavailable.
    /// </summary>
    NoNetwork = 1222,
    /// <summary>
    /// The device specified by lpLocalName is not a redirected device.
    /// For more information, see the following Remarks section.
    /// </summary>
    NotConnected = 2250

}
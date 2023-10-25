using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NetMapper.Enums
{
    //const int CONNECT_PROMPT = 0x00000010;
    //const int CONNECT_INTERACTIVE = 0x00000008;

    [StructLayout(LayoutKind.Sequential)]
    struct NETRESOURCE
    {
        public ResourceScope oResourceScope;
        public ResourceType oResourceType;
        public ResourceDisplayType oDisplayType;
        public ResourceUsage oResourceUsage;
        public string sLocalName;
        public string sRemoteName;
        public string sComments;
        public string sProvider;
    }
    public enum CancelConnection
    {
        DISCONNECT_SUCCESS = 0,
        DISCONNECT_FAILURE = 2250
    }
    public enum ResourceScope
    {
        RESOURCE_CONNECTED = 1,
        RESOURCE_GLOBALNET,
        RESOURCE_REMEMBERED,
        RESOURCE_RECENT,
        RESOURCE_CONTEXT
    }
    [Flags]
    public enum AddConnectionOptions
    {
        CONNECT_UPDATE_PROFILE = 0x00000001,
        CONNECT_UPDATE_RECENT = 0x00000002,
        CONNECT_TEMPORARY = 0x00000004,
        CONNECT_INTERACTIVE = 0x00000008,
        CONNECT_PROMPT = 0x00000010,
        CONNECT_NEED_DRIVE = 0x00000020,
        CONNECT_REFCOUNT = 0x00000040,
        CONNECT_REDIRECT = 0x00000080,
        CONNECT_LOCALDRIVE = 0x00000100,
        CONNECT_CURRENT_MEDIA = 0x00000200,
        CONNECT_DEFERRED = 0x00000400,
        CONNECT_RESERVED = unchecked((int)0xFF000000),
        CONNECT_COMMANDLINE = 0x00000800,
        CONNECT_CMD_SAVECRED = 0x00001000,
        CONNECT_CRED_RESET = 0x00002000
    }
    public enum ResourceType
    {
        RESOURCETYPE_ANY,
        RESOURCETYPE_DISK,
        RESOURCETYPE_PRINT,
        RESOURCETYPE_RESERVED
    }
    public enum ResourceUsage
    {
        RESOURCEUSAGE_CONNECTABLE = 0x00000001,
        RESOURCEUSAGE_CONTAINER = 0x00000002,
        RESOURCEUSAGE_NOLOCALDEVICE = 0x00000004,
        RESOURCEUSAGE_SIBLING = 0x00000008,
        RESOURCEUSAGE_ATTACHED = 0x00000010
    }
    public enum ResourceDisplayType
    {
        RESOURCEDISPLAYTYPE_GENERIC,
        RESOURCEDISPLAYTYPE_DOMAIN,
        RESOURCEDISPLAYTYPE_SERVER,
        RESOURCEDISPLAYTYPE_SHARE,
        RESOURCEDISPLAYTYPE_FILE,
        RESOURCEDISPLAYTYPE_GROUP,
        RESOURCEDISPLAYTYPE_NETWORK,
        RESOURCEDISPLAYTYPE_ROOT,
        RESOURCEDISPLAYTYPE_SHAREADMIN,
        RESOURCEDISPLAYTYPE_DIRECTORY,
        RESOURCEDISPLAYTYPE_TREE,
        RESOURCEDISPLAYTYPE_NDSCONTAINER
    }
    public enum ConnectResult
    {
        /// <summary>
        /// The connection was successfully created.
        /// </summary>
        [LocDescription("ConnectResult_Success")]
        Success = 0,
        /// <summary>
        /// The connection was lost. This is not a system event number, but
        /// set if a previously established connection was lost.
        /// </summary>
        [LocDescription("ConnectResult_Lost")]
        Lost = -1,
        /// <summary>
        /// An unexpected failure occurred - no enum constant was defined for
        /// this case.
        /// </summary>
        [LocDescription("ConnectResult_UnknownConnectionFailure")]
        UnknownConnectionFailure = -2,
        [LocDescription("ConnectResult_Degraded")]
        Degraded = -3,
        /// <summary>
        /// The submitted user ID is unknown, or the caller does not
        /// have access to the network resource. 
        /// </summary>
        [LocDescription("ConnectResult_UnknownUserOrAccessDenied")]
        UnknownUserOrAccessDenied = 5,
        /// <summary>
        /// A device attached to the system is not functioning.
        /// </summary>
        [LocDescription("ConnectResult_GeneralNetworkFailure")]
        GeneralNetworkFailure = 31,
        /// <summary>
        /// The local device specified by the lpLocalName member
        /// is already connected to a network resource.
        /// </summary>
        [LocDescription("ConnectResult_DriveLetterAlreadyAssigned")]
        DriveLetterAlreadyAssigned = 85,
        /// <summary>
        /// The type of local device and the type of network
        /// resource do not match.
        /// </summary>
        [LocDescription("ConnectResult_BadDeviceType")]
        BadDeviceType = 66,
        /// <summary>
        /// The value specified by lpLocalName is invalid.
        /// </summary>
        [LocDescription("ConnectResult_BadDevice")]
        BadDevice = 1200,
        /// <summary>
        /// The server (host) address could not be resolved.
        /// </summary>
        [LocDescription("ConnectResult_HostNotFound")]
        HostNotFound = 53,
        /// <summary>
        /// The specified share was not found.
        /// </summary>
        [LocDescription("ConnectResult_ShareNotFound")]
        ShareNotFound = 55,
        /// <summary>
        /// The specified server cannot perform the requested operation.
        /// </summary>
        [LocDescription("ConnectResult_InvalidServerOperation")]
        InvalidServerOperation = 58,
        /// <summary>
        /// The value specified by the lpRemoteName member
        /// is not acceptable to any network resource provider,
        /// either because the resource name is invalid, or because
        /// the named resource cannot be located.
        /// </summary>
        [LocDescription("ConnectResult_InvalidNetworkAddressFormat")]
        InvalidNetworkAddressFormat = 67,
        /// <summary>
        /// The specified user name was invalid (invalid format).
        /// </summary>
        [LocDescription("ConnectResult_BadUserNameFormat")]
        BadUserNameFormat = 2202,
        /// <summary>
        /// The user profile is in an incorrect format.
        /// </summary>
        [LocDescription("ConnectResult_BadProfile")]
        BadProfile = 1206,
        /// <summary>
        /// The value specified by the lpProvider member does not
        /// match any provider.
        /// </summary>
        [LocDescription("ConnectResult_BadProvider")]
        BadProvider = 1204,
        /// <summary>
        /// The router or provider is busy, possibly initializing.
        /// The caller should retry.
        /// </summary>
        [LocDescription("ConnectResult_NetworkBusy")]
        NetworkBusy = 170,
        /// <summary>
        /// The attempt to make the connection was canceled by the user
        /// through a dialog box from one of the network resource providers,
        /// or by a called resource.
        /// </summary>
        [LocDescription("ConnectResult_Cancelled")]
        Cancelled = 1223,
        /// <summary>
        /// Multiple connections to a server or shared resource by the same user,
        /// using more than one user name, are not allowed.
        /// Disconnect all previous connections to the server or shared resource
        /// and try again.
        /// </summary>
        [LocDescription("ConnectResult_SessionCredentialConflict")]
        SessionCredentialConflict = 1219,
        /// <summary>
        /// The system is unable to open the user profile to process
        /// persistent connections.
        /// </summary>
        [LocDescription("ConnectResult_CannotOpenProfile")]
        CannotOpenProfile = 1205,
        /// <summary>
        /// The requested drive letter is already registered
        /// for another connection.
        /// </summary>
        [LocDescription("ConnectResult_DriveLetterAlreadyRegistered")]
        DriveLetterAlreadyRegistered = 1202,
        /// <summary>
        /// A network-specific error occurred. Call the WNetGetLastError
        /// function to obtain a description of the error.
        /// </summary>
        [LocDescription("ConnectResult_ExtendedError")]
        ExtendedError = 1208,
        /// <summary>
        /// The specified password is invalid and the CONNECT_INTERACTIVE flag is not set.
        /// Also returned in case of an invalid user ID if a standard UNC connection is
        /// made.<br/>
        /// From a user's point of view, <see cref="LoginFailure"/> and <see cref="InvalidCredentials"/>
        /// are interchangeable.
        /// </summary>
        [LocDescription("ConnectResult_InvalidCredentials")]
        InvalidCredentials = 86,
        /// <summary>
        /// Logon failure: unknown user name or bad password. This error appears to be
        /// used if a drive is mapped (as opposite to just creating a UNC connection).<br/>
        /// From a user's point of view, <see cref="LoginFailure"/> and <see cref="InvalidCredentials"/>
        /// are interchangeable.
        /// </summary>
        [LocDescription("ConnectResult_LoginFailure")]
        LoginFailure = 1326,
        /// <summary>
        /// The operation cannot be performed because a network component is not started
        /// or because a specified name cannot be used.
        /// </summary>
        [LocDescription("ConnectResult_NoNetworkOrInvalidNetworkAddress")]
        NoNetworkOrInvalidNetworkAddress = 1203,
        /// <summary>
        /// The network is unavailable.
        /// </summary>
        [LocDescription("ConnectResult_NetworkUnavailable")]
        NetworkUnavailable = 1222,
        /// <summary>
        /// The operation being requested was not performed because
        /// the user has not been authenticated.
        /// </summary>
        [LocDescription("ConnectResult_UserNotAuthenticated")]
        UserNotAuthenticated = 1244
    }

    internal class LocDescriptionAttribute : Attribute
    {
        public LocDescriptionAttribute(string a)
        {
            Debug.Print(a);
        }
    }
}

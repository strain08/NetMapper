﻿using System;
using System.Runtime.InteropServices;

namespace NetMapper.Enums;
//const int CONNECT_PROMPT = 0x00000010;
//const int CONNECT_INTERACTIVE = 0x00000008;

[StructLayout(LayoutKind.Sequential)]
public struct NETRESOURCE
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
namespace NetMapper.Enums;

public enum ShareState
{
    Undefined,
    Available,
    Unavailable
}

public enum MappingState
{
    Undefined,
    Mapped,
    Unmapped,
    LetterUnavailable
}

public enum ToastActionsDisconnect
{
    Force,
    Retry,
    ShowWindow
}

public enum ToastActionsSimple
{
    ShowWindow
}
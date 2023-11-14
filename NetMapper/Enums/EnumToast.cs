using NetMapper.Models;

namespace NetMapper.Enums;

public record ToastArgsRecord(ToastType ToastType, MapModel model, ToastActions Action = ToastActions.ToastClicked);
public record ToastText(string Text = "", bool Update = false);

public enum ToastLines
{
    LINE1,
    LINE2
}
public enum ToastArgs
{
    TOAST_TYPE,
    MODEL_ID,
    TOAST_ACTION
}
public enum ToastActions
{
    Force,
    Retry,
    ToastClicked
}
public enum ToastType
{
    INF_DISCONNECT,
    INF_CONNECT,
    DLG_CAN_NOT_DISCONNECT,
    INF_LOGIN_FAILURE,
    INF_CUSTOM
}

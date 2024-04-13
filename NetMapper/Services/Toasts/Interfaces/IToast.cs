using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Enums;

namespace NetMapper.Services.Toasts.Interfaces;

public interface IToast
{
    ToastText TextLine1 { get; set; }
    ToastText TextLine2 { get; set; }
    public string Tag { get; init; }
    public ToastArgsRecord Arguments { get; init; }
    public ToastContentBuilder GetToastContent();
}

using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Enums;
using NetMapper.Services.Toasts.Interfaces;

namespace NetMapper.Services.Toasts;

public class RetryForceToast : IToast
{
    public string Tag { get; init; }
    public ToastText TextLine1 { get; set; } = new();
    public ToastText TextLine2 { get; set; } = new();
    public ToastArgsRecord Arguments { get; init; }
    public RetryForceToast(string Tag, ToastArgsRecord args)
    {
        this.Tag = Tag; Arguments = args;
    }
    public ToastContentBuilder GetToastContent()
    {
        return new ToastContentBuilder()
            .AddVisualChild(new AdaptiveText
            {
                Text = new BindableString(ToastLines.LINE1.ToString())
            })
            .AddVisualChild(new AdaptiveText
            {
                Text = new BindableString(ToastLines.LINE2.ToString())
            })
            .AddButton(new ToastButton()
                .SetContent("Retry")
                .AddArgument(ToastArgs.TOAST_ACTION.ToString(), ToastActions.Retry))
            .AddButton(new ToastButton()
                .SetContent("Force")
                .AddArgument(ToastArgs.TOAST_ACTION.ToString(), ToastActions.Force))
            .AddButton(new ToastButtonDismiss())
            .AddArgument(ToastArgs.TOAST_ACTION.ToString(), ToastActions.ToastClicked)
            .AddArgument(ToastArgs.TOAST_TYPE.ToString(), Arguments.ToastType)
            .AddArgument(ToastArgs.MODEL_ID.ToString(), Arguments.model.DriveLetter)
            .SetToastScenario(ToastScenario.Reminder);
    }

}
public class CanNotDisconnect : RetryForceToast
{
    public CanNotDisconnect(string Tag, ToastArgsRecord args) : base(Tag, args)
    {
        TextLine1 = new($"Cannot remove network drive {Arguments.model.DriveLetterColon}", false);
        TextLine2 = new($"Close all files in use on drive {Arguments.model.DriveLetterColon} and retry.", false);
    }
}

using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Enums;
using NetMapper.Services.Toasts.Interfaces;

namespace NetMapper.Services.Toasts;

public class InfoToast : IToast
{
    public string Tag { get; init; }
    public ToastArgsRecord Arguments { get; init; }
    public ToastText TextLine1 { get; set; } = new();
    public ToastText TextLine2 { get; set; } = new();
    public InfoToast(string Tag, ToastArgsRecord args)
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
        .AddArgument(ToastArgs.TOAST_ACTION.ToString(), ToastActions.ToastClicked)
        .AddArgument(ToastArgs.TOAST_TYPE.ToString(), Arguments.ToastType)
        .AddArgument(ToastArgs.MODEL_ID.ToString(), Arguments.model.ID)
        .SetToastScenario(ToastScenario.Default);
    }
}

public class DriveConnected : InfoToast
{
    public DriveConnected(string Tag, ToastArgsRecord args) : base(Tag, args)
    {
        TextLine1 = new("New connection");
        TextLine2 = new($"{args.model.DriveLetterColon} [ {args.model.VolumeLabel} ] connected.", true);
    }
}
public class DriveDisconnected : InfoToast
{

    public DriveDisconnected(string Tag, ToastArgsRecord args) : base(Tag, args)
    {
        TextLine1 = new("Drive disconnected");
        TextLine2 = new($"{args.model.DriveLetterColon} [ {args.model.VolumeLabel} ] disconnected.", true);
    }
}
public class LoginFailure : InfoToast
{
    public LoginFailure(string Tag, ToastArgsRecord args) : base(Tag, args)
    {
        TextLine1 = new($"Login failure connecting to {args.model.NetworkPath}.", false);
        TextLine2 = new("Please connect the share in windows or delete the mapping.", false);
    }
}
public class CustomToast : InfoToast
{
    public CustomToast(string Tag, ToastArgsRecord args, string line1, string line2) : base(Tag, args)
    {
        TextLine1 = new(line1, false);
        TextLine2 = new(line2, false);
    }
}


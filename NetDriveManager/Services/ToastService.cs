using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Models;
using System;

namespace NetMapper.Services
{
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

    public delegate void ToastAnswerDelegate<T>(MapModel m, T answer) where T:struct, Enum;

    public class ToastService
    {
        ToastAnswerDelegate<ToastActionsDisconnect>? _onToastActionsDisconnect;
        ToastAnswerDelegate<ToastActionsSimple>? _onToastActionsSimple;
        MapModel? _mapModel;

        const string ACTION_DISCONNECT = "1";
        const string ACTION_SIMPLE = "2";

        // CTOR
        public ToastService()
        {
            ToastNotificationManagerCompat.OnActivated += (e) =>
            {
                if (_mapModel == null) return;
                ToastArguments args = ToastArguments.Parse(e.Argument);

                if (args.Contains(ACTION_DISCONNECT))
                {
                    _onToastActionsDisconnect?.Invoke(_mapModel, args.GetEnum<ToastActionsDisconnect>(ACTION_DISCONNECT));
                }
                if (args.Contains(ACTION_SIMPLE))
                {
                    _onToastActionsSimple?.Invoke(_mapModel, args.GetEnum<ToastActionsSimple>(ACTION_SIMPLE));
                }

            };

        }

        // PUBLIC TOAST CALLS
        public void ToastDriveConnected(MapModel m, ToastAnswerDelegate<ToastActionsSimple> del)
        {
            _onToastActionsSimple = del;
            _mapModel = m;
            var t = new ToastContentBuilder()
                .AddArgument(ACTION_SIMPLE, ToastActionsDisconnect.ShowWindow)
                .AddText($"Drive {m.DriveLetterColon} connected.")
                .SetToastScenario(ToastScenario.Reminder);

            t.Show();
        }

        public void ToastDriveDisconnected(MapModel m, ToastAnswerDelegate<ToastActionsSimple> del)
        {
            _onToastActionsSimple = del;
            _mapModel = m;
            var t = new ToastContentBuilder()
                .AddArgument(ACTION_SIMPLE, ToastActionsDisconnect.ShowWindow)
                .AddText($"Drive {m.DriveLetterColon} disconnected.")
                .SetToastScenario(ToastScenario.Reminder);
            t.Show();
        }

        public void ToastBadLogin(MapModel m, ToastAnswerDelegate<ToastActionsSimple> del)
        {
            _onToastActionsSimple = del;
            _mapModel = m;
            var t = new ToastContentBuilder()
                .AddArgument(ACTION_SIMPLE, ToastActionsDisconnect.ShowWindow)
                .AddText($"Login failure connecting to {m.NetworkPath}.")
                .AddText($"Please connect the share in windows or delete the mapping.")
                .SetToastScenario(ToastScenario.Reminder);

            t.Show();
        }
        public void ToastDriveDisconnectError(MapModel m, ToastAnswerDelegate<ToastActionsDisconnect> del)
        {
            _onToastActionsDisconnect = del;
            _mapModel = m;

            var t = new ToastContentBuilder()
                .AddText($"Cannot remove network drive {m.DriveLetterColon}")
                .AddText($"Close all files in use on drive {m.DriveLetterColon} and retry.")
                .AddButton(new ToastButton()
                            .SetContent("Retry")
                            .AddArgument(ACTION_DISCONNECT, ToastActionsDisconnect.Retry))
                .AddButton(new ToastButton()
                            .SetContent("Force")
                            .AddArgument(ACTION_DISCONNECT, ToastActionsDisconnect.Force))
                .AddButton(new ToastButtonDismiss())
                .AddArgument(ACTION_DISCONNECT, ToastActionsDisconnect.ShowWindow)
                .SetToastScenario(ToastScenario.Reminder);

            t.Show();
        }

    }
}

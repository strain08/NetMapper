using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Models;
using NetMapper.Enums;

namespace NetMapper.Services
{ 

    public delegate void RemoveDriveAnswerDelegate(MappingModel mappingModel, DisconnectDriveAnswer toast);
    public delegate void AddRemoveAnswerDelegate(MappingModel mappingModel, AddRemoveAnswer toast);

    public class NotificationService
    {
        RemoveDriveAnswerDelegate? _onRemoveDriveToast;
        AddRemoveAnswerDelegate? _onAddRemoveDriveToast;
        MappingModel? _mappingModel;

        const string ACTION_REMOVEDRIVE = "1";
        const string ACTION_ADDREMOVE = "2";

        // CTOR
        public NotificationService()
        {
            ToastNotificationManagerCompat.OnActivated += ToastAction;
        }

        // PUBLIC TOAST CALLS
        public void ToastCanNotRemoveDrive(MappingModel m, RemoveDriveAnswerDelegate del)
        {
            _onRemoveDriveToast = del;
            _mappingModel = m;     

            var t = new ToastContentBuilder()
                .AddText($"Cannot remove network drive {m.DriveLetterColon}")
                .AddText($"Close all files in use on drive {m.DriveLetterColon} and retry.")
                .AddButton(new ToastButton()
                            .SetContent("Retry")
                            .AddArgument(ACTION_REMOVEDRIVE, DisconnectDriveAnswer.Retry))
                .AddButton(new ToastButton()
                            .SetContent("Force")
                            .AddArgument(ACTION_REMOVEDRIVE, DisconnectDriveAnswer.Force))
                .AddButton(new ToastButtonDismiss())
                .AddArgument(ACTION_REMOVEDRIVE, DisconnectDriveAnswer.ShowWindow)
                .SetToastScenario(ToastScenario.Reminder);
            
            t.Show();

            //var notif = new ToastNotification(t.GetXml()) { Tag = "remove"};
            //ToastNotificationManagerCompat.CreateToastNotifier().Show(notif);
        }


        public void ToastDriveConnected(MappingModel model, AddRemoveAnswerDelegate del)
        {
            _onAddRemoveDriveToast = del;
            _mappingModel = model;
            var t = new ToastContentBuilder()
                .AddArgument(ACTION_ADDREMOVE, DisconnectDriveAnswer.ShowWindow)
                .AddText($"Drive {model.DriveLetterColon} connected.")
                .SetToastScenario(ToastScenario.Reminder);            
            
            t.Show();
        }

        public void ToastDriveDisconnected(MappingModel model, AddRemoveAnswerDelegate del)
        {
            _onAddRemoveDriveToast = del;
            _mappingModel = model;
            var t = new ToastContentBuilder()
                .AddArgument(ACTION_ADDREMOVE, DisconnectDriveAnswer.ShowWindow)
                .AddText($"Drive {model.DriveLetterColon} disconnected.")
                .SetToastScenario(ToastScenario.Reminder);

            t.Show();
        }

        // TOAST ACTION SELECTOR
        private void ToastAction(ToastNotificationActivatedEventArgsCompat e)
        {
            if (_mappingModel == null) return;

            ToastArguments args = ToastArguments.Parse(e.Argument);

            if (args.Contains(ACTION_REMOVEDRIVE))
            {       
                // callback with user answer
                _onRemoveDriveToast?.Invoke(_mappingModel, args.GetEnum<DisconnectDriveAnswer>(ACTION_REMOVEDRIVE));
            }

            if (args.Contains(ACTION_ADDREMOVE))
            {                
                // callback with user answer
                _onAddRemoveDriveToast?.Invoke(_mappingModel, args.GetEnum<AddRemoveAnswer>(ACTION_ADDREMOVE));
            }
        }
        

    }
}

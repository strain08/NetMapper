using Microsoft.Toolkit.Uwp.Notifications;
using NetDriveManager.Models;

namespace NetDriveManager.Services
{
    public enum DisconnectDriveAnswer
    {
        Force,
        Retry,
        ShowWindow
    }
    public enum AddRemoveAnswer
    {
        ShowWindow
    }


    public delegate void RemoveDriveAnswerDelegate(MappingModel mappingModel, DisconnectDriveAnswer toast);
    public delegate void AddRemoveAnswerDelegate(MappingModel mappingModel, AddRemoveAnswer toast);

    public class NotificationService
    {
        RemoveDriveAnswerDelegate? _onRemoveDriveToast;
        AddRemoveAnswerDelegate? _onAddRemoveDriveToast;
        MappingModel? _mappingModel;

        const string ACTION_REMOVEDRIVE = "removedrive";
        const string ACTION_ADDREMOVE = "addremove";

        // CTOR
        public NotificationService()
        {
            ToastNotificationManagerCompat.OnActivated += ToastAnswer;
        }

        // PUBLIC TOAST CALLS
        public void NotifyCanNotRemoveDrive(MappingModel model, RemoveDriveAnswerDelegate del)
        {
            _onRemoveDriveToast = del;
            _mappingModel = model;     

            var t = new ToastContentBuilder()
                .AddText("Cannot remove network drive " + model.DriveLetterColon)
                .AddText($"Close all files in use on drive {model.DriveLetterColon} and retry.")
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


        public void DriveConnectedToast(MappingModel model, AddRemoveAnswerDelegate del)
        {
            _onAddRemoveDriveToast = del;
            _mappingModel = model;
            var t = new ToastContentBuilder()
                .AddArgument(ACTION_ADDREMOVE, DisconnectDriveAnswer.ShowWindow)
                .AddText($"Drive {model.DriveLetterColon} connected.")
                .SetToastScenario(ToastScenario.Reminder);            
            
            t.Show();
        }

        public void DriveDisconnectedToast(MappingModel model, AddRemoveAnswerDelegate del)
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
        private void ToastAnswer(ToastNotificationActivatedEventArgsCompat e)
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

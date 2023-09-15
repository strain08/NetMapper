using Microsoft.Toolkit.Uwp.Notifications;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using NetDriveManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using System.Linq;

namespace NetDriveManager.Services
{
    public enum RemoveDriveAnswer
    {
        Force,
        Retry,
        ShowWindow,

    }
    public enum AddRemoveAnswer
    {
        ShowWindow
    }


    public delegate void RemoveDriveAnswerDelegate(DriveModel mappingModel, RemoveDriveAnswer toast);
    public delegate void AddRemoveAnswerDelegate(DriveModel mappingModel, AddRemoveAnswer toast);

    public class NotificationService
    {
        RemoveDriveAnswerDelegate? _onRemoveDriveToast;
        AddRemoveAnswerDelegate? _onAddRemoveDriveToast;
        DriveModel? _mappingModel;

        const string ACTION_REMOVEDRIVE = "removedrive";
        const string ACTION_ADDREMOVE = "addremove";

        // CTOR
        public NotificationService()
        {
            ToastNotificationManagerCompat.OnActivated += ToastAnswer;
        }

        // PUBLIC TOAST CALLS
        public void ToastCanNotRemoveDrive(DriveModel model, RemoveDriveAnswerDelegate del)
        {
            _onRemoveDriveToast = del;
            _mappingModel = model;

            var toastButton1 = new ToastButton()
                            .SetContent("Retry")
                            .AddArgument(ACTION_REMOVEDRIVE, RemoveDriveAnswer.Retry);
            var toastButton2 = new ToastButton()
                            .SetContent("Force")
                            .AddArgument(ACTION_REMOVEDRIVE, RemoveDriveAnswer.Force);
            var toastButton3 = new ToastButtonDismiss();

            var t = new ToastContentBuilder();

            t.AddArgument(ACTION_REMOVEDRIVE, RemoveDriveAnswer.ShowWindow);
            t.AddText("Cannot remove network drive " + model.DriveLetterColon);
            t.AddText($"Close all files in use on drive {model.DriveLetterColon} and retry.");
            t.AddButton(toastButton1);
            t.AddButton(toastButton2);
            t.AddButton(toastButton3);

            t.SetToastScenario(ToastScenario.Reminder);
            t.Show();

            //var notif = new ToastNotification(t.GetXml()) { Tag = "remove"};
            //ToastNotificationManagerCompat.CreateToastNotifier().Show(notif);


        }
        public async Task TestMessage()
        {

            
            var box = MessageBoxManager
            .GetMessageBoxStandard("Title", "Message", ButtonEnum.YesNoCancel);
            
            var result = await box.ShowAsync();
            Debug.WriteLine(result);
        }

        public void ToastDriveAdded(DriveModel model, AddRemoveAnswerDelegate del)
        {
            _onAddRemoveDriveToast = del;
            var t = new ToastContentBuilder();
            t.AddArgument(ACTION_ADDREMOVE, RemoveDriveAnswer.ShowWindow);
            t.AddText($"Drive {model.DriveLetterColon} connected.");
            t.SetToastScenario(ToastScenario.Reminder);

            t.Show();
        }

        public void ToastDriveRemoved(DriveModel model, AddRemoveAnswerDelegate del)
        {
            _onAddRemoveDriveToast = del;
            var t = new ToastContentBuilder();
            t.AddArgument(ACTION_ADDREMOVE, RemoveDriveAnswer.ShowWindow);
            t.AddText($"Drive {model.DriveLetterColon} disconnected.");
            t.SetToastScenario(ToastScenario.Reminder);

            t.Show();
        }

        // TOAST ACTION SELECTOR
        private void ToastAnswer(ToastNotificationActivatedEventArgsCompat e)
        {
            if (_mappingModel == null) return;

            ToastArguments args = ToastArguments.Parse(e.Argument);

            if (args.Contains(ACTION_REMOVEDRIVE))
            {
                RemoveDriveAnswer answer;
                answer = args.GetEnum<RemoveDriveAnswer>(ACTION_REMOVEDRIVE);
                _onRemoveDriveToast?.Invoke(_mappingModel, answer);
            }

            if (args.Contains(ACTION_ADDREMOVE))
            {
                AddRemoveAnswer answer;
                answer = args.GetEnum<AddRemoveAnswer>(ACTION_ADDREMOVE);
                _onAddRemoveDriveToast?.Invoke(_mappingModel, answer);
            }
        }



    }
}

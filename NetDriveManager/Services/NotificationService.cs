using Microsoft.Toolkit.Uwp.Notifications;
using NetDriveManager.Models;
using System.Collections.Generic;

namespace NetDriveManager.Services
{
    public enum RemoveDriveAnswer
    {
        Force,
        Retry,
        ShowWindow,
        Cancel
    }
    public enum AddRemoveAnswer
    {
        ShowWindow
    }


    public delegate void RemoveDriveAnswerDelegate(MappingModel mappingModel, RemoveDriveAnswer toast);
    public delegate void AddRemoveAnswerDelegate(MappingModel mappingModel, AddRemoveAnswer toast);

    public class NotificationService
    {
        private RemoveDriveAnswerDelegate? _onRemoveDriveToast;
        private AddRemoveAnswerDelegate? _onAddRemoveDriveToast;
        private MappingModel? _mappingModel;

        const string ACTION_REMOVEDRIVE = "removedrive";
        const string ACTION_ADDREMOVE = "addremove";

        // CTOR
        public NotificationService()
        {
            ToastNotificationManagerCompat.OnActivated += ToastAnswer;
        }

        // PUBLIC TOAST CALLS
        public void ToastCanNotRemoveDrive(MappingModel model, RemoveDriveAnswerDelegate del)
        {
            _onRemoveDriveToast = del;
            _mappingModel = model;

            var toastButton1 = new ToastButton()
                            .SetContent("Retry")
                            .AddArgument(ACTION_REMOVEDRIVE, RemoveDriveAnswer.Retry);
            var toastButton2 = new ToastButton()
                            .SetContent("Force")
                            .AddArgument(ACTION_REMOVEDRIVE, RemoveDriveAnswer.Force);
            var toastButton3 = new ToastButton()
                            .SetContent("Do nothing")
                            .AddArgument(ACTION_REMOVEDRIVE, RemoveDriveAnswer.Cancel);

            var t = new ToastContentBuilder();

            t.AddArgument(ACTION_REMOVEDRIVE, RemoveDriveAnswer.ShowWindow);
            t.AddText("Cannot remove network drive " + model.DriveLetter);
            t.AddText($"Close all files in use on drive {model.DriveLetter} and retry.");
            t.AddButton(toastButton1);
            t.AddButton(toastButton2);
            t.AddButton(toastButton3);
            t.SetToastScenario(ToastScenario.Reminder);

            t.Show();
        }

        public void ToastDriveAdded(MappingModel model, AddRemoveAnswerDelegate del)
        {
            _onAddRemoveDriveToast = del;
            var t = new ToastContentBuilder();
            t.AddArgument(ACTION_ADDREMOVE, RemoveDriveAnswer.ShowWindow);
            t.AddText($"Drive {model.DriveLetter} connected.");
            t.SetToastScenario(ToastScenario.Reminder);

            t.Show();
        }

        public void ToastDriveRemoved(MappingModel model, AddRemoveAnswerDelegate del)
        {
            _onAddRemoveDriveToast = del;
            var t = new ToastContentBuilder();
            t.AddArgument(ACTION_ADDREMOVE, RemoveDriveAnswer.ShowWindow);
            t.AddText($"Drive {model.DriveLetter} disconnected.");
            t.SetToastScenario(ToastScenario.Reminder);

            t.Show();
        }

        // TOAST ACTION SELECTOR
        private void ToastAnswer(ToastNotificationActivatedEventArgsCompat e)
        {
            if (_mappingModel == null) return;

            ToastArguments args = ToastArguments.Parse(e.Argument);

            try
            {
                RemoveDriveAnswer answer;
                answer = args.GetEnum<RemoveDriveAnswer>(ACTION_REMOVEDRIVE);
                if (_onRemoveDriveToast != null) _onRemoveDriveToast(_mappingModel, answer);
            }
            catch (KeyNotFoundException)
            {
            }

            try
            {
                AddRemoveAnswer answer;
                answer = args.GetEnum<AddRemoveAnswer>(ACTION_ADDREMOVE);
                if (_onAddRemoveDriveToast != null) _onAddRemoveDriveToast(_mappingModel, answer);
            }
            catch (KeyNotFoundException)
            {
            }

        }



    }
}

using Microsoft.Toolkit.Uwp.Notifications;
using NetDriveManager.Models;
using System.Diagnostics;

namespace NetDriveManager.Services
{
    public enum ToastRemoveDriveAction
    {
        Force,
        Retry,
        Cancel
    }

    public delegate void RemoveDriveToastDelegate(MappingModel mappingModel, out ToastRemoveDriveAction toast);


    public class MyToastService
    {
        private RemoveDriveToastDelegate OnRemoveDriveToast;

        const string ACTION_REMOVEDRIVE= "removedrive";
        const string ANSWER_REMOVEDRIVE_FORCE = "ForceArg";
        const string ANSWER_REMOVEDRIVE_RETRY = "RetryArg";
        const string ANSWER_REMOVEDRIVE_CANCEL = "CancelArg";

        // CTOR
        public MyToastService(RemoveDriveToastDelegate onRemoveDriveToast)
        {
            
            ToastNotificationManagerCompat.OnActivated += ToastAnswer;
            OnRemoveDriveToast = onRemoveDriveToast;
        }

        // TOAST ACTION SELECTOR
        private void ToastAnswer(ToastNotificationActivatedEventArgsCompat e)
        {

            // Obtain the arguments from the notification
            ToastArguments args = ToastArguments.Parse(e.Argument);

            // Obtain any user input (text boxes, menu selections) from the notification
            //ValueSet userInput = e.UserInput;

            // TODO: Show the corresponding content
            Debug.WriteLine("Toast activated. Args: " + args.Get(ACTION_REMOVEDRIVE));
        }

        // PUBLIC TOAST CALLS
        public void ToastCanNotRemoveDrive(string driveLetter)
        {
            var toastButton1 = new ToastButton()
                            .SetContent("Retry")
                            .AddArgument(ACTION_REMOVEDRIVE, ANSWER_REMOVEDRIVE_RETRY);
            var toastButton2 = new ToastButton()
                            .SetContent("Force")
                            .AddArgument(ACTION_REMOVEDRIVE, ANSWER_REMOVEDRIVE_FORCE);
            var toastButton3 = new ToastButton()
                            .SetContent("Do nothing")
                            .AddArgument(ACTION_REMOVEDRIVE, ANSWER_REMOVEDRIVE_CANCEL);

            var t = new ToastContentBuilder();

            t.AddArgument(ACTION_REMOVEDRIVE, "ShowDriveList");
            t.AddText("Can not remove network drive " + driveLetter);
            t.AddText($"Close all files in use on drive {driveLetter} and retry.");
            t.AddButton(toastButton1);
            t.AddButton(toastButton2);
            t.AddButton(toastButton3);
            t.SetToastScenario(ToastScenario.Reminder);

            t.Show();
        }

    }
}

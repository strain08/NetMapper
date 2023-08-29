using Microsoft.Toolkit.Uwp.Notifications;
using NetDriveManager.Models;
using System;
using System.Diagnostics;

namespace NetDriveManager.Services
{    
    public class RemoveDriveEventArgs : EventArgs
    {
        public NDModel Model { get; set; } = new NDModel();
    }

    public class MyToastService
    {
        // EVENTS

        // CAN NOT REMOVE DRIVE
        // user must close files, force remove drive or do nothing

        // retry
        public event EventHandler<RemoveDriveEventArgs>? 
            RetryRemoveDrive;
        protected virtual void OnRetryRemoveDrive(NDModel model)
        {
            RetryRemoveDrive?.Invoke(this, new RemoveDriveEventArgs() { Model = model });
        }
        
        // force
        public event EventHandler<RemoveDriveEventArgs>? 
            ForceRemoveDrive;
        protected virtual void OnForceRemoveDrive(NDModel model)
        {
            ForceRemoveDrive?.Invoke(this, new RemoveDriveEventArgs() { Model = model });
        }
        
        // do nothing
        public event EventHandler<RemoveDriveEventArgs>? 
            DoNothing;
        protected virtual void OnDoNothing(NDModel model)
        {
            DoNothing?.Invoke(this, new RemoveDriveEventArgs() { Model = model });
        }
        
        // CTOR
        public MyToastService()
        {
            ToastNotificationManagerCompat.OnActivated += ToastAnswer;
        }

        // TOAST ACTION SELECTOR
        private void ToastAnswer(ToastNotificationActivatedEventArgsCompat e)
        {

            // Obtain the arguments from the notification
            ToastArguments args = ToastArguments.Parse(e.Argument);

            // Obtain any user input (text boxes, menu selections) from the notification
            //ValueSet userInput = e.UserInput;

            // TODO: Show the corresponding content
            Debug.WriteLine("Toast activated. Args: " + args.Get("action"));
        }
        
        // PUBLIC TOAST CALLS
        public void ToastCanNotRemoveDrive(string driveLetter)
        {
            var toastButton1 = new ToastButton()
                            .SetContent("Retry")
                            .AddArgument("action", "ReplyArg");
            var toastButton2 = new ToastButton()
                .SetContent("Force")
                .AddArgument("action", "ForceArg");
            var toastButton3 = new ToastButton()
                .SetContent("Do nothing")
                .AddArgument("action", "CancelArg");
            var t = new ToastContentBuilder();
            t.AddArgument("action", "ShowDriveList");
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

using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Enums;
using NetMapper.Interfaces;
using NetMapper.Services.Toasts.Interfaces;
using Serilog;
using System;
using System.Linq;

namespace NetMapper.Services.Toasts.Implementations
{

    public class ToastActivatedMessenger : IToastActivatedMessenger
    {
        private readonly IDriveListService listService;

        public ToastActivatedMessenger(IDriveListService listService)
        {
            this.listService = listService;
        }
        public void CreateMessage(ToastArguments args)
        {
            ToastActions toastAction;
            ToastType toastType;
            string modelID;
            try
            {
                toastType = args.GetEnum<ToastType>(ToastArgs.TOAST_TYPE.ToString());
                modelID = args.Get(ToastArgs.MODEL_ID.ToString());
                toastAction = args.GetEnum<ToastActions>(ToastArgs.TOAST_ACTION.ToString());

                var model = listService.DriveCollection.Where((m) => m.ID == modelID).First();
                var toastAnswerData = new ToastArgsRecord(toastType, model, toastAction);
                WeakReferenceMessenger.Default.Send(toastAnswerData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception decoding toast args.", args);
            }
            { }
        }
    }
}

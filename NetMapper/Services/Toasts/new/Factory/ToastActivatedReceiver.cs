using Avalonia.Threading;
using CommunityToolkit.Mvvm.Messaging;
using NetMapper.Enums;
using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.Services.Toasts.Interfaces;
using NetMapper.ViewModels;
using System;
using System.Diagnostics;

namespace NetMapper.Services.Toasts.Implementations
{
    public class ToastActivatedReceiver : IRecipient<ToastArgsRecord>
    {
        private readonly IToastFactory _toastFactory;
        private readonly IDriveListService _listService;
        private readonly IConnectService _connectService;
        private readonly INavService navService;
        private readonly IInterop interop;

        public ToastActivatedReceiver(IToastFactory toastFactory,
                                      IDriveListService listService,
                                      IConnectService connectService,
                                      INavService navService,
                                      IInterop interop)
        {
            _toastFactory = toastFactory;
            _listService = listService;
            _connectService = connectService;
            this.navService = navService;
            this.interop = interop;
            WeakReferenceMessenger.Default.Register(this);
        }

        public void Receive(ToastArgsRecord message)
        {
            switch (message.ToastType)
            {
                case ToastType.INF_CONNECT:
                    INF_Connect(message);
                    break;
                case ToastType.INF_DISCONNECT:
                    INF_Disconnect(message);
                    break;
                case ToastType.DLG_CAN_NOT_DISCONNECT:
                    DLG_Can_Not_Disconnect(message);
                    break;
                case ToastType.INF_LOGIN_FAILURE:
                    break;
            }
        }

        private void DLG_Can_Not_Disconnect(ToastArgsRecord message)
        {
            switch (message.Action)
            {
                case ToastActions.Retry:
                    _connectService.Disconnect(message.model);
                    break;

                case ToastActions.Force:
                    _connectService.Disconnect(message.model, forceDisconnect: true);
                    break;

                case ToastActions.ToastClicked:
                    ShowMainWindow();
                    break;
            }
        }

        private void INF_Disconnect(ToastArgsRecord message)
        {
            switch (message.Action)
            {
                case ToastActions.ToastClicked:
                    ShowMainWindow();
                    break;
            }
        }

        private void INF_Connect(ToastArgsRecord message)
        {
            switch (message.Action)
            {
                case ToastActions.ToastClicked:
                    interop.OpenFolderInExplorer(message.model.DriveLetterColon);
                    break;
            }
        }        

        private void ShowMainWindow()
        {
            Dispatcher.UIThread.Post(() =>
            {
                navService.GetViewModel<ApplicationViewModel>()
                    .ShowMainWindow();
            });
        }
    }
}

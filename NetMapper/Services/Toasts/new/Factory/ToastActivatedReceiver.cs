using CommunityToolkit.Mvvm.Messaging;
using NetMapper.Enums;
using NetMapper.Interfaces;
using NetMapper.Services.Toasts.Interfaces;
using System;
using System.Diagnostics;

namespace NetMapper.Services.Toasts.Implementations
{
    public class ToastActivatedReceiver : IRecipient<ToastArgsRecord>
    {
        private readonly IToastFactory _toastFactory;
        private readonly IDriveListService _listService;
        private readonly IConnectService _connectService;

        public ToastActivatedReceiver(IToastFactory toastFactory, IDriveListService listService, IConnectService connectService)
        {
            _toastFactory = toastFactory;
            _listService = listService;
            _connectService = connectService;
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
                    break;
                case ToastType.CAN_NOT_DISCONNECT:
                    break;
                case ToastType.LOGIN_FAILURE:
                    break;
            }
        }

        private void INF_Connect(ToastArgsRecord message)
        {
            switch (message.Action)
            {
                case ToastActions.Force:
                    break;
                case ToastActions.Retry:
                    break;
                case ToastActions.ToastClicked:
                    break;
            }
        }
    }
}

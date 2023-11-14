using NetMapper.Enums;
using NetMapper.Services.Toasts.Interfaces;
using System.Collections.Generic;

namespace NetMapper.Services.Toasts.Implementations;

public class ToastFactory : IToastFactory
{
    public IToastPresenter CreateToast()
    {
        return new ToastPresenter();
    }
    public IToastType CreateToastType(string tag, ToastArgsRecord answerData)
    {

        return answerData.ToastType switch
        {
            ToastType.INF_DISCONNECT => new DriveDisconnected(tag, answerData),
            ToastType.INF_CONNECT => new DriveConnected(tag, answerData),
            ToastType.CAN_NOT_DISCONNECT => new CanNotDisconnect(tag, answerData),
            ToastType.LOGIN_FAILURE => new LoginFailure(tag, answerData),
            _ => throw new KeyNotFoundException(),
        };
    }
}

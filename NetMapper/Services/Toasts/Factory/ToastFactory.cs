using NetMapper.Enums;
using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.Services.Toasts.Interfaces;
using System.Collections.Generic;

namespace NetMapper.Services.Toasts.Factory;

public class ToastFactory : IToastFactory
{
    private ISettingsService settingsService;

    public ToastFactory(ISettingsService settingsService)
    {
        this.settingsService = settingsService;
    }

    public IToastPresenter CreateToastPresenter()
    {
        return new ToastPresenter(settingsService);
    }
    public IToast CreateToast(string tag, ToastType toastType, MapModel m)
    {
        var answerData = new ToastArgsRecord(toastType, m);

        return answerData.ToastType switch
        {
            ToastType.INF_DISCONNECT => new DriveDisconnected(tag, answerData),
            ToastType.INF_CONNECT => new DriveConnected(tag, answerData),
            ToastType.DLG_CAN_NOT_DISCONNECT => new CanNotDisconnect(tag, answerData),
            ToastType.INF_LOGIN_FAILURE => new LoginFailure(tag, answerData),
            _ => throw new KeyNotFoundException(),
        };
    }
    public IToast CreateToast(string tag, ToastType toastType, MapModel m, string Line1, string Line2)
    {
        var answerData = new ToastArgsRecord(toastType, m);
        return new CustomToast(tag, answerData, Line1, Line2);

    }
}

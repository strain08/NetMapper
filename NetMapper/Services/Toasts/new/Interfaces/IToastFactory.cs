using NetMapper.Enums;

namespace NetMapper.Services.Toasts.Interfaces;

public interface IToastFactory
{
    IToastPresenter CreateToast();
    IToastType CreateToastType(string tag, ToastArgsRecord answerData);
}
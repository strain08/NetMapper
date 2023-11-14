using NetMapper.Enums;
using NetMapper.Models;

namespace NetMapper.Services.Toasts.Interfaces;

public interface IToastFactory
{
    IToastPresenter CreateToastPresenter();    
    IToast CreateToast(string tag, ToastType toastType, MapModel m);
}
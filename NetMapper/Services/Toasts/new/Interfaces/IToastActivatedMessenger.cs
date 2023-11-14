using Microsoft.Toolkit.Uwp.Notifications;

namespace NetMapper.Services.Toasts.Interfaces
{
    public interface IToastActivatedMessenger
    {
        public void CreateMessage(ToastArguments args);
    }
}

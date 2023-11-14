using Microsoft.Toolkit.Uwp.Notifications;
using NetMapper.Services.Toasts.Interfaces;

namespace NetMapper.Services.Toasts.Implementations
{
    public class ToastActivatedHandler
    {
        private readonly IToastActivatedMessenger _answerMessenger;
        public ToastActivatedHandler(IToastActivatedMessenger answerMessenger)
        {
            _answerMessenger = answerMessenger;
            ToastNotificationManagerCompat.OnActivated += Activated;
        }
        private void Activated(ToastNotificationActivatedEventArgsCompat e)
        {
            ToastArguments args = ToastArguments.Parse(e.Argument);
            _answerMessenger.CreateMessage(args);
        }
    }
}

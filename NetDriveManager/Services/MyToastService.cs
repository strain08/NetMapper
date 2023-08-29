using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Collections.ObjectModel;


namespace NetDriveManager.Services
{
    public class MyToastService
    {
        public MyToastService()
        {
            ToastNotificationManagerCompat.OnActivated += Test;
        }

        private void Test(ToastNotificationActivatedEventArgsCompat e)
        {
            
            throw new NotImplementedException();
        }
    }
}

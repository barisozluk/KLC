using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace AYP.Helpers.Notifications
{
    public class NotificationManager
    {
        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.BottomCenter,
                offsetX: 30,
                offsetY: 30);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });

        public void ShowSuccessMessage(string message)
        {
            notifier.ShowSuccess(message);
        }

        public void ShowErrorMessage(string message)
        {
            notifier.ShowError(message);
        }

        public void ShowWarningMessage(string message)
        {
            notifier.ShowWarning(message);
        }
    }
}

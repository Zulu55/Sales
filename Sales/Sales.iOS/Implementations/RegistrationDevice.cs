[assembly: Xamarin.Forms.Dependency(typeof(Sales.iOS.Implementations.RegistrationDevice))]

namespace Sales.iOS.Implementations
{
    using Foundation;
    using Interfaces;
    using UIKit;

    public class RegistrationDevice : IRegisterDevice
    {
        #region Methods
        public void RegisterDevice()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert |
                    UIUserNotificationType.Badge |
                    UIUserNotificationType.Sound,
                    new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes =
                    UIRemoteNotificationType.Alert |
                    UIRemoteNotificationType.Badge |
                    UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
        }
        #endregion
    }
}
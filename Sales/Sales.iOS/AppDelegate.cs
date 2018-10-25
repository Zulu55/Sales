namespace Sales.iOS
{
    using System;
    using System.Collections.Generic;
    using Foundation;
    using ImageCircle.Forms.Plugin.iOS;
    using UIKit;
    using ViewModels;
    using WindowsAzure.Messaging;

    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public SBNotificationHub Hub { get; set; }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            ImageCircleRenderer.Init();
            Xamarin.FormsMaps.Init();
            LoadApplication(new App());

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                       UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                       new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }

            return base.FinishedLaunching(app, options);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Hub = new SBNotificationHub(Constants.ConnectionString, Constants.NotificationHubPath);

            Hub.UnregisterAllAsync(deviceToken, (error) =>
            {
                if (error != null)
                {
                    Console.WriteLine("Error calling Unregister: {0}", error.ToString());
                    return;
                }

                var tags_list = new List<string>() { };
                var mainviewModel = MainViewModel.GetInstance();
                if (mainviewModel.UserASP != null)
                {
                    var userId = mainviewModel.UserASP.Id;
                    tags_list.Add("userId:" + userId);
                }

                var tags = new NSSet(tags_list.ToArray());
                Hub.RegisterNativeAsync(deviceToken, tags, (errorCallback) =>
                {
                    if (errorCallback != null)
                        Console.WriteLine("RegisterNativeAsync error: " + errorCallback.ToString());
                });
            });
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            ProcessNotification(userInfo, false);
        }

        void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            if (null != options && options.ContainsKey(new NSString("aps")))
            {
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;

                string alert = string.Empty;
                string type = string.Empty;
                string notification = string.Empty;
                if (aps.ContainsKey(new NSString("alert")))
                {
                    alert = (aps[new NSString("alert")] as NSString).ToString();
                }

                //type = (aps[new NSString("Type")] as NSString).ToString();

                if (!fromFinishedLaunching)
                {
                    //notification = (aps[new NSString("Notification")] as NSString).ToString();
                    var avAlert = new UIAlertView("Sales App", alert, null, "Ok", null);
                    avAlert.Show();
                }
            }
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            new UIAlertView("Error registering push notifications", error.LocalizedDescription, null, "OK", null).Show();
        }
    }
}
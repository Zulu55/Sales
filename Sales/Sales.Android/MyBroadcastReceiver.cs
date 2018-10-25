[assembly: Android.App.Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: Android.App.UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: Android.App.UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]
[assembly: Android.App.UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: Android.App.UsesPermission(Name = "android.permission.INTERNET")]
[assembly: Android.App.UsesPermission(Name = "android.permission.WAKE_LOCK")]

namespace Sales.Droid
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Android.App;
    using Android.Content;
    using Android.Util;
    using Gcm.Client;
    using ViewModels;
    using WindowsAzure.Messaging;

    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]

    public class MyBroadcastReceiver : GcmBroadcastReceiverBase<PushHandlerService>
    {
        public static string[] SENDER_IDS = new string[] { Constants.SenderID };

        public const string TAG = "MyBroadcastReceiver-GCM";
    }

    [Service]
    public class PushHandlerService : GcmServiceBase
    {
        #region Properties
        public NotificationHub Hub { get; set; }
        public static string RegistrationID { get; private set; }
        #endregion

        #region Methods
        public PushHandlerService() : base(Constants.SenderID)
        {
            Log.Info(MyBroadcastReceiver.TAG, "PushHandlerService() constructor");
        }

        protected override void OnMessage(Context context, Intent intent)
        {
            Log.Info(MyBroadcastReceiver.TAG, "GCM Message Received!");

            var msg = new StringBuilder();

            if (intent != null && intent.Extras != null)
            {
                foreach (var key in intent.Extras.KeySet())
                    msg.AppendLine(key + "=" + intent.Extras.Get(key).ToString());
            }

            var message = intent.Extras.GetString("Message");
            var type = intent.Extras.GetString("Type");

            if (!string.IsNullOrEmpty(message))
            {
                var notification = intent.Extras.GetString("Notification");
                createNotification("Sales App", message);
            }
        }

        protected override bool OnRecoverableError(Context context, string errorId)
        {
            Log.Warn(MyBroadcastReceiver.TAG, "Recoverable Error: " + errorId);

            return base.OnRecoverableError(context, errorId);
        }

        protected override void OnError(Context context, string errorId)
        {
            Log.Error(MyBroadcastReceiver.TAG, "GCM Error: " + errorId);
        }

        protected override void OnRegistered(Context context, string registrationId)
        {
            Log.Verbose(MyBroadcastReceiver.TAG, "GCM Registered: " + registrationId);
            RegistrationID = registrationId;

            Hub = new NotificationHub(Constants.NotificationHubName, Constants.ListenConnectionString, context);

            try
            {
                Hub.UnregisterAll(registrationId);
            }
            catch (Exception ex)
            {
                Log.Error(MyBroadcastReceiver.TAG, ex.Message);
            }

            var tags = new List<string>() { };

            var mainviewModel = MainViewModel.GetInstance();
            if (mainviewModel.UserASP != null)
            {
                var userId = mainviewModel.UserASP.Id;
                tags.Add("userId:" + userId);
            }

            try
            {
                var hubRegistration = Hub.Register(registrationId, tags.ToArray());
            }
            catch (Exception ex)
            {
                Log.Error(MyBroadcastReceiver.TAG, ex.Message);
            }
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {
            Log.Verbose(MyBroadcastReceiver.TAG, "GCM Unregistered: " + registrationId);

            createNotification("Sales App", "The device has been unregistered!");
        }

        void createNotification(string title, string desc)
        {
            //Create notification
            var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            //Create an intent to show UI
            var uiIntent = new Intent(this, typeof(MainActivity));

            //Create the notification
            var notification = new Notification(Android.Resource.Drawable.SymActionEmail, title);

            //Auto-cancel will remove the notification once the user touches it
            notification.Flags = NotificationFlags.AutoCancel;

            //Set the notification info
            //we use the pending intent, passing our ui intent over, which will get called
            //when the notification is tapped.
            notification.SetLatestEventInfo(this, title, desc, PendingIntent.GetActivity(this, 0, uiIntent, 0));

            //Show the notification
            notificationManager.Notify(1, notification);
            dialogNotify(title, desc);
        }

        protected void dialogNotify(String title, String message)
        {
            var mainActivity = MainActivity.GetInstance();
            mainActivity.RunOnUiThread(() =>
            {
                AlertDialog.Builder dlg = new AlertDialog.Builder(mainActivity);
                AlertDialog alert = dlg.Create();
                alert.SetTitle(title);
                alert.SetButton("Accept", delegate
                {
                    alert.Dismiss();
                });
                alert.SetIcon(Resource.Drawable.ic_launcher);
                alert.SetMessage(message);
                alert.Show();
            });
        }
        #endregion
    }
}
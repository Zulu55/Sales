﻿namespace Sales.Droid
{
    using Android.App;
    using Android.OS;

    [Activity(
        Theme = "@style/Theme.Splash", //Indicates the theme to use for this activity
        MainLauncher = true, //Set it as boot activity
        NoHistory = true)] //Doesn't place it in back stack
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            System.Threading.Thread.Sleep(1800); //Let's wait awhile...
            this.StartActivity(typeof(MainActivity));
        }
    }
}
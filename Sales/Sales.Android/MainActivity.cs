namespace Sales.Droid
{
    using Android.App;
    using Android.Content.PM;
    using Android.OS;
    using Android.Runtime;
    using ImageCircle.Forms.Plugin.Droid;
    using Plugin.CurrentActivity;
    using Plugin.Permissions;

    [Activity(
        Label = "Sales", 
        Icon = "@drawable/ic_launcher", 
        Theme = "@style/MainTheme", 
        MainLauncher = false, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        #region Singleton
        private static MainActivity instance;

        public static MainActivity GetInstance()
        {
            if (instance == null)
            {
                instance = new MainActivity();
            }

            return instance;
        }
        #endregion

        #region Methods
        protected override void OnCreate(Bundle savedInstanceState)
        {
            instance = this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.FormsMaps.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            ImageCircleRenderer.Init();

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(
            int requestCode,
            string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(
                requestCode,
                permissions,
                grantResults);
        }
        #endregion
    }
}
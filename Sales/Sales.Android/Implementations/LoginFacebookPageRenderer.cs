[assembly: Xamarin.Forms.ExportRenderer(
    typeof(Sales.Views.LoginFacebookPage),
    typeof(Sales.Droid.Implementations.LoginFacebookPageRenderer))]

namespace Sales.Droid.Implementations
{
    using System;
    using System.Threading.Tasks;
    using Android.App;
    using Common.Models;
    using Services;
    using Xamarin.Auth;
    using Xamarin.Forms.Platform.Android;

    public class LoginFacebookPageRenderer : PageRenderer
    {
        public LoginFacebookPageRenderer()
        {
            var activity = this.Context as Activity;

            var facebookAppID = Xamarin.Forms.Application.Current.Resources["FacebookAppID"].ToString();
            var facebookAuthURL = Xamarin.Forms.Application.Current.Resources["FacebookAuthURL"].ToString();
            var facebookRedirectURL = Xamarin.Forms.Application.Current.Resources["FacebookRedirectURL"].ToString();
            var facebookScope = Xamarin.Forms.Application.Current.Resources["FacebookScope"].ToString();

            var auth = new OAuth2Authenticator(
                clientId: facebookAppID,
                scope: facebookScope,
                authorizeUrl: new Uri(facebookAuthURL),
                redirectUrl: new Uri(facebookRedirectURL));

            auth.Completed += async (sender, eventArgs) =>
            {
                if (eventArgs.IsAuthenticated)
                {
                    var accessToken = eventArgs.Account.Properties["access_token"].ToString();
                    var token = await GetFacebookProfileAsync(accessToken);
                    await App.NavigateToProfile(token);
                }
                else
                {
                    App.HideLoginView();
                }
            };

            activity.StartActivity(auth.GetUI(activity));
        }

        private async Task<TokenResponse> GetFacebookProfileAsync(string accessToken)
        {
            var url = Xamarin.Forms.Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Xamarin.Forms.Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Xamarin.Forms.Application.Current.Resources["UrlUsersController"].ToString();
            var apiService = new ApiService();
            var facebookResponse = await apiService.GetFacebook(accessToken);
            var token = await apiService.LoginFacebook(
                url,
                prefix,
                $"{controller}/LoginFacebook",
                facebookResponse);
            return token;
        }
    }
}

using KChristmas.Core.Helpers;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace KChristmas.Core
{
    public partial class App : Application
    {
        private static readonly Version CURRENT_VERSION = new Version(6, 1, 0);

        public NavigationPage Navigation = new NavigationPage();

        public App()
        {
            InitializeComponent();

            if (Settings.LastSeenVersion < CURRENT_VERSION)
            {
                Settings.GiftAccepted = false;
                Settings.GiftHintsV2 = null;
                Settings.PinkieSeenCount = 0;
            }
            Settings.LastSeenVersion = CURRENT_VERSION;

            NetworkService networkService = new NetworkService();
            MainPage = Navigation;
            Navigation.PushAsync(new MainPage(networkService));
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

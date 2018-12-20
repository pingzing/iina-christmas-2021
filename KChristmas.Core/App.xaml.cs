using KChristmas.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace KChristmas.Core
{
    public partial class App : Application
    {
        private static readonly Version CURRENT_VERSION = new Version(3, 0, 0);

        public NavigationPage Navigation = new NavigationPage();

        public App()
        {
            InitializeComponent();

            if (Settings.LastSeenVersion < CURRENT_VERSION)
            {
                Settings.GiftAccepted = false;
                Settings.GiftHints = null;
                Settings.PinkieSeenCount = 0;
                Settings.GiftRedeemed = false;
                Settings.IntroComplete = false;
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

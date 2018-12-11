using KChristmas2016.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace KChristmas2016.Core
{
    public partial class App : Application
    {
        private static readonly Version CURRENT_VERSION = new Version(2, 0, 0);

        public NavigationPage Navigation = new NavigationPage();

        public App()
        {
            InitializeComponent();

            if (Settings.LastSeenVersion < CURRENT_VERSION)
            {
                Settings.GiftAccepted = false;
                Settings.GiftHints = null;
                Settings.GiftRedeemed = false;
                Settings.IntroComplete = false;
            }
            Settings.LastSeenVersion = CURRENT_VERSION;

            MainPage = Navigation;
            Navigation.PushAsync(new MainPage());
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

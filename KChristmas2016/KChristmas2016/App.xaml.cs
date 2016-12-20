using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace KChristmas2016
{
    public partial class App : Application
    {
        public NavigationPage Navigation = new NavigationPage();

        public App()
        {
            InitializeComponent();

            MainPage = Navigation;
            Navigation.PushAsync(new KChristmas2016.MainPage());
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

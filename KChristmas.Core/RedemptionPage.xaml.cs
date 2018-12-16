using KChristmas.Core.Helpers;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace KChristmas.Core
{
    public partial class RedemptionPage : ContentPage
    {        
        public RedemptionPage()
        {
            InitializeComponent();
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            if (!Settings.GiftAccepted)
            {                                
                Settings.GiftAccepted = true;
            }

            await ShowCompletePanel();
        }      

        private async Task ShowCompletePanel()
        {
            await Task.Delay(1000);

            await Task.WhenAll(
                AcceptGiftPanelTopCaption.FadeTo(1, 2000),
                AcceptGiftPanelTopCaption.ScaleTo(1, 2000));

            await Task.WhenAll(
                AcceptGiftImage.ScaleTo(1, 2000),
                AcceptGiftImage.FadeTo(1, 2000));

            await Task.WhenAll(
                AcceptGiftPanelCaption.ScaleTo(1, 2000),
                AcceptGiftPanelCaption.FadeTo(1, 2000));
        }
    }
}

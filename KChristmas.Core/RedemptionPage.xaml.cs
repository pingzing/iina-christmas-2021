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
                AcceptGiftPanelCaption1.FadeTo(1, 2000),
                AcceptGiftPanelCaption1.ScaleTo(1, 2000));

            await Task.Delay(3000);

            await Task.WhenAll(
                AcceptGiftPanelCaption2.FadeTo(1, 2000),
                AcceptGiftPanelCaption2.ScaleTo(1, 2000));

            await Task.Delay(2000);

            await Task.WhenAll(
                AcceptGiftPanelCaption3.ScaleTo(1, 2000),
                AcceptGiftPanelCaption3.FadeTo(1, 2000));

            await Task.Delay(4000);

            await Task.WhenAll(
                AcceptGiftPanelCaption4.ScaleTo(1, 2000),
                AcceptGiftPanelCaption4.FadeTo(1, 2000));

            await Task.Delay(3000);

            await Task.WhenAll(
                AcceptGiftPanelCaption5.ScaleTo(1, 2000),
                AcceptGiftPanelCaption5.FadeTo(1, 2000));
        }
    }
}

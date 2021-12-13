using IinaChristmas.Core.Helpers;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace IinaChristmas.Core
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

            // 1
            await Task.WhenAll(
                AcceptGiftPanelCaption1.FadeTo(1, 2000),
                AcceptGiftPanelCaption1.ScaleTo(1, 2000));

            await Task.Delay(3000);

            // 2
            await Task.WhenAll(
                AcceptGiftPanelCaption2.FadeTo(1, 2000),
                AcceptGiftPanelCaption2.ScaleTo(1, 2000));

            await Task.Delay(2000);

            // 3
            await Task.WhenAll(
                AcceptGiftPanelCaption3.ScaleTo(1, 2000),
                AcceptGiftPanelCaption3.FadeTo(1, 2000));

            await Task.Delay(3000);
            
            // 4
            await Task.WhenAll(
                AcceptGiftPanelCaption4.ScaleTo(1, 2000),
                AcceptGiftPanelCaption4.FadeTo(1, 2000));

            await Task.Delay(3000);

            // 5
            await Task.WhenAll(
                AcceptGiftPanelCaption5.ScaleTo(1, 2000),
                AcceptGiftPanelCaption5.FadeTo(1, 2000));

            await Task.Delay(2000);

            // 6
            await Task.WhenAll(
                AcceptGiftPanelCaption6.ScaleTo(1, 2000),
                AcceptGiftPanelCaption6.FadeTo(1, 2000));

            await Task.Delay(3000);

            // 7
            await Task.WhenAll(
                AcceptGiftPanelCaption7.ScaleTo(1, 2000),
                AcceptGiftPanelCaption7.FadeTo(1, 2000));

            await Task.Delay(3000);

            // 8
            await Task.WhenAll(
                AcceptGiftPanelCaption8.ScaleTo(1, 2000),
                AcceptGiftPanelCaption8.FadeTo(1, 2000));

            await Task.Delay(2000);

            // 9
            await Task.WhenAll(
                AcceptGiftPanelCaption9.ScaleTo(1, 2000),
                AcceptGiftPanelCaption9.FadeTo(1, 2000));

            await Task.Delay(5000);

            // 10
            await Task.WhenAll(
                AcceptGiftPanelCaption10.ScaleTo(1, 2000),
                AcceptGiftPanelCaption10.FadeTo(1, 2000));

            await Task.Delay(2000);

            await Task.WhenAll(
                AcceptGiftPanelCaption11.ScaleTo(1, 2000),
                AcceptGiftPanelCaption11.FadeTo(1, 2000));
        }
    }
}

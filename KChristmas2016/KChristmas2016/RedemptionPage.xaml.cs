using KChristmas2016.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace KChristmas2016
{
    public partial class RedemptionPage : ContentPage
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string AcceptGiftUrl = "https://kc2016.azurewebsites.net/api/AcceptGift?code=XVpBRAihB0s7L7e0JsclN5Ca8RDMpOkTKEAeZFA6a62HtNK0xOG7bg==";
        private const string RedeemGiftUrl = "https://kc2016.azurewebsites.net/api/RedeemGift?code=7c5RrOfucfopvE0g1woo10kMHU/pz4v5MHd8Njo0m00s8TuN1PvAfA==";

        public RedemptionPage()
        {
            InitializeComponent();
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            if (!Settings.GiftAccepted)
            {                
                var response = await _httpClient.GetAsync(AcceptGiftUrl);
                if (!response.IsSuccessStatusCode)
                {
                    AcceptButton.IsVisible = true;
                    AcceptGiftPanelCaption.Text = "Aww! We couldn't weren't able to reach the North Pole's dispatch center. You can try again though!";
                }
                else
                {
                    AcceptGiftPanel.IsVisible = false;
                    RedeemGiftPanel.IsVisible = true;
                    Settings.GiftAccepted = true;
                }
            }
            else if (Settings.GiftAccepted && !Settings.GiftRedeemed)
            {
                AcceptGiftPanel.IsVisible = false;
                RedeemGiftPanel.IsVisible = true;
                CompletePanel.IsVisible = false;                
            }
            else if (Settings.GiftRedeemed)
            {
                ShowCompletePanel();
            }
        }

        private async void AcceptButton_Clicked(object sender, EventArgs e)
        {
            AcceptGiftPanelCaption.Text = "Trying to accept again...";
            var response = await _httpClient.GetAsync(AcceptGiftUrl);
            if (!response.IsSuccessStatusCode)
            {
                AcceptButton.IsVisible = true;
                AcceptGiftPanelCaption.Text = "Aww! We STILL weren't able to reach the North Pole's dispatch center. You can try again though!";
            }
            else
            {
                AcceptGiftPanel.IsVisible = false;
                RedeemGiftPanel.IsVisible = true;
                this.Title = "Redeem gift";
                Settings.GiftAccepted = true;
            }
        }

        private async void RedeemButton_Clicked(object sender, EventArgs e)
        {
            RedeemGiftPanelCaption.Text = "Redemption in progress!";
            var response = await _httpClient.GetAsync(RedeemGiftUrl);
            if(!response.IsSuccessStatusCode)
            {
                if((int)response.StatusCode == 418)
                {
                    Settings.GiftRedeemed = true;
                    ShowCompletePanel();
                }
                else
                {
                    RedeemGiftPanelCaption.Text = "Aww! We couldn't weren't able to reach the North Pole's dispatch center. You can try again though!";
                }
            }
            else
            {
                ShowCompletePanel();
            }
        }

        private void ShowCompletePanel()
        {
            AcceptGiftPanel.IsVisible = false;
            RedeemGiftPanel.IsVisible = false;
            CompletePanel.IsVisible = true;
            this.Title = "Awwww yeah glasses time";
        }
    }
}

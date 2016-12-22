using KChristmas2016.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KChristmas2016
{
    public partial class MainPage : ContentPage
    {
        private readonly bool IsInDebug = false;
        private readonly DateTime ChristmasDate = new DateTime(2016, 12, 24, 16, 0, 0);
        private readonly HttpClient _httpClient = new HttpClient();
        private const string GetGiftHintsUrl = "https://kc2016.azurewebsites.net/api/GetGiftHints?code=7c5RrOfucfopvE0g1woo10kMHU/pz4v5MHd8Njo0m00s8TuN1PvAfA==";

        private List<string> _giftHints = new List<string>();
        private List<string> _seenHints = new List<string>();
        private Random rand = new Random();

        private enum PanelState
        {
            None,
            Panel1,
            Panel2,
            Panel3,
            Panel4,
            Panel5,
            Panel6,
            End
        }

        private PanelState _currentState;

        public MainPage()
        {
#if DEBUG
            IsInDebug = false;
#endif
            InitializeComponent();
            InitHints(Settings.GiftHints);
        }

        private void InitHints(string unbrokenHintString)
        {
            if (Settings.GiftHints != null)
            {
                _giftHints = unbrokenHintString.Split('|').ToList();
            }
        }

        private string GetHint()
        {            
            if(_giftHints.Count <= 0 && _seenHints.Count > 0)
            {
                Debug.WriteLine("SeenHints emptied, GiftHints refilled.");
                _giftHints = new List<string>(_seenHints);
                _seenHints.Clear();
            }
            if(_giftHints.Count > 0)
            {
                int hintIndex = rand.Next() % _giftHints.Count;
                string hint = _giftHints[hintIndex];
                _giftHints.RemoveAt(hintIndex);
                _seenHints.Add(hint);
                return hint;
            }
            else
            {
                return "No hints here. Sorry!";
            }
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            if(Settings.IntroComplete)
            {
                await ((App)(Application.Current)).Navigation.PushAsync(new RedemptionPage());
                ((App)App.Current).Navigation.Navigation.RemovePage(this);
                return;
            }
            else if (DateTime.Now < ChristmasDate && !IsInDebug)
            {
                //Set up panel state
                await Task.Delay(1000);
                TooEarlyPanel.Opacity = 0;
                TooEarlyPanel.IsVisible = true;
                await TooEarlyPanel.FadeTo(1, 500);
                _currentState = PanelState.None;

                //Set up countdown timer
                Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    var timeTillChristmas = ChristmasDate - DateTime.Now;
                    if(timeTillChristmas > TimeSpan.Zero)
                    {
                        TimerLabel.Text = $"{timeTillChristmas.Days}d {timeTillChristmas.Hours}h {timeTillChristmas.Minutes}m {timeTillChristmas.Seconds}s";
                        return true;
                    }
                    else
                    {
                        TimerLabel.Text = "0d 0h 0m 0s";
                        TooEaryPanelNextButton.InputTransparent = false;
                        Task.WhenAll(
                            TooEarlyLabel1.FadeTo(0, 2000),
                            TooEarlyLabel2.FadeTo(0, 2000),
                            TimerLabel.TranslateTo(0, 50, 4000),
                            TimerLabel.ScaleTo(2, 4000),
                            TooEaryPanelNextButton.FadeTo(1, 4000)
                        );
                        return false;
                    }
                });

                //Set up gift box hints
                try
                {
                    string response = await _httpClient.GetStringAsync(GetGiftHintsUrl);
                    if(String.IsNullOrWhiteSpace(response))
                    {
                        return;
                    }
                    Settings.GiftHints = response.Trim('"');
                    InitHints(Settings.GiftHints);
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Failed to get gift hints list.");
                }
            }
            else
            {
                await Task.Delay(1000);
                Panel1.Opacity = 0;
                Panel1.IsVisible = true;
                await Panel1.FadeTo(1, 500);
                _currentState = PanelState.Panel1;
            }
        }

        bool showingHint = false;
        private async void TooEarlyPresentButton_Clicked(object sender, EventArgs e)
        {
            if(showingHint)
            {
                return;
            }
            showingHint = true;

            var storyboard = new Animation();
            var shakeUpHigh = new Animation(v => TooEarlyPresentButton.TranslationY = v, 0, 20, Easing.SpringIn);
            var fromHighToLow = new Animation(v => TooEarlyPresentButton.TranslationY = v, 20, -20, Easing.SpringIn);
            var fromLowToSmallHigh = new Animation(v => TooEarlyPresentButton.TranslationY = v, -20, 10, Easing.SpringIn);
            var fromSmallHighToSmallLow = new Animation(v => TooEarlyPresentButton.TranslationY = v, 10, -10, Easing.SpringIn);
            var fromSmallLowToTinyHigh = new Animation(v => TooEarlyPresentButton.TranslationY = v, -10, 5, Easing.SpringIn);
            var fromTinyHighToTinyLow = new Animation(v => TooEarlyPresentButton.TranslationY = v, 5, -5, Easing.SpringIn);
            var fromTinyLowToComplete = new Animation(v => TooEarlyPresentButton.TranslationY = v, -5, 0, Easing.SpringOut);
            storyboard.Add(0.0, 0.1, shakeUpHigh);
            storyboard.Add(0.1, 0.2, fromHighToLow);
            storyboard.Add(0.2, 0.3, fromLowToSmallHigh);
            storyboard.Add(0.3, 0.4, fromSmallHighToSmallLow);
            storyboard.Add(0.4, 0.5, fromSmallLowToTinyHigh);
            storyboard.Add(0.5, 0.6, fromTinyHighToTinyLow);
            storyboard.Add(0.6, 1.0, fromTinyLowToComplete);

            storyboard.Commit(TooEarlyPresentButton, "ShakeAnimation", 16, 1000);
            HintCaption.Text = GetHint();
            await HintCaption.FadeTo(1, 100);
            await Task.WhenAll(                
                HintCaption.FadeTo(0, 5000),
                HintCaption.TranslateTo(0, -230, 5000)
            );
            HintCaption.TranslationY = -130;

            showingHint = false;
        }        

        private async void TooEaryPanelNextButton_Clicked(object sender, EventArgs e)
        {
            _currentState = await ChangePanelState(TooEarlyPanel, Panel1, _currentState);
        }

        private async void Panel1NextButton_Clicked(object sender, EventArgs e)
        {
            _currentState = await ChangePanelState(Panel1, Panel2, _currentState);
        }

        private async void Panel2Gift_Clicked(object sender, EventArgs e)
        {
            await Task.WhenAll(
                Panel2_GiftTop.FadeTo(0, 1000),
                Panel2_GiftTop.TranslateTo(0, -100, 1000),
                Panel2_InstructionText.FadeTo(0, 1000)
            );

            Panel2NextButton.Opacity = 0;
            Panel2NextButton.IsVisible = true;
            Panel2NextButton.InputTransparent = false;
            await Panel2_TeaserText.FadeTo(1, 800);
            await Panel2NextButton.FadeTo(1, 300);            
        }

        private async void Panel2NextButton_Clicked(object sender, EventArgs e)
        {
            _currentState = await ChangePanelState(Panel2, Panel3, _currentState);
            await Panel3_Caption1.ScaleTo(1.0, 300, Easing.BounceIn);
        }

        private async void Panel3_NextButton1_Clicked(object sender, EventArgs e)
        {
            await Panel3_Caption1.FadeTo(0, 100);
            await Panel3_NextButton1.FadeTo(0, 100);
            await Task.Delay(500);
            await Panel3_Caption2Line1.FadeTo(1, 250);
            await Task.Delay(1000);
            await Panel3_Caption2Line2.FadeTo(1, 1000);
            await Task.Delay(1000);
            Panel3_NextButton1.IsVisible = false;
            Panel3_NextButton2.IsVisible = true;            
            await Panel3_NextButton2.FadeTo(1, 1000);
        }

        private async void Panel3_NextButton2_Clicked(object sender, EventArgs e)
        {
            _currentState = await ChangePanelState(Panel3, Panel4, _currentState);

            await Panel4_Caption1.ScaleTo(1, 300, Easing.BounceIn);
            await Task.Delay(2500);
            Panel4_Caption1.Opacity = 0;
            Panel4_Caption2Line1.Opacity = 1;
            await Task.Delay(1000);
            Panel4_Caption2Line2.Opacity = 1;
            await Task.Delay(1000);
            Panel4_Caption2Line3.Opacity = 1;

            await Task.Delay(2000);

            await Task.WhenAll(
                Panel4_Caption2Line1.FadeTo(0, 500),
                Panel4_Caption2Line2.FadeTo(0, 500),
                Panel4_Caption2Line3.FadeTo(0, 500)
            );

            await Panel4_Caption3Line1.FadeTo(1, 1000);
            await Task.Delay(500);
            await Panel4_Caption3Line2.FadeTo(1, 1000);
            await Panel4_NextButton.FadeTo(1, 1000);
            Panel4_NextButton.InputTransparent = false;
        }

        private async void Panel4_NextButton_Clicked(object sender, EventArgs e)
        {
            _currentState = await ChangePanelState(Panel4, Panel5, _currentState);

            await Panel5_Caption1.ScaleTo(1, 300, Easing.BounceIn);
            await Task.Delay(2000);
            await Task.WhenAll(
                Panel5_Caption2.ScaleTo(1, 300, Easing.BounceIn),
                Panel5_Caption2.TranslateTo(0, 0, 300, Easing.CubicIn)
            );
            await Task.Delay(500);
            await Task.WhenAll(
                Panel5_Caption3.ScaleTo(1, 300, Easing.BounceIn),
                Panel5_Caption3.TranslateTo(0, 0, 300, Easing.CubicIn)
            );
            await Task.Delay(500);
            await Task.WhenAll(
                Panel5_Caption4.ScaleTo(1, 300, Easing.BounceIn),
                Panel5_Caption4.TranslateTo(0, 0, 300, Easing.CubicIn)
            );
            await Task.Delay(500);
            await Panel5_NextButton.ScaleTo(1, 300, Easing.BounceIn);
        }

        bool pulseButton = true;
        private async void Panel5_NextButton_Clicked(object sender, EventArgs e)
        {
            _currentState = await ChangePanelState(Panel5, Panel6, _currentState);
            await Task.Delay(1000);
            await Panel6_Caption.FadeTo(1, 1000);
            await Task.Delay(1500);
            await Panel6_NextButton.FadeTo(1, 1000);
            Panel6_NextButton.InputTransparent = false;

            while (pulseButton)
            {
                await Panel6_NextButton.ScaleTo(1.3, 500);
                if(!pulseButton)
                {
                    break;
                }
                await Panel6_NextButton.ScaleTo(1, 500);
            }                        
        }


        private void Panel6_NextButton_Clicked(object sender, EventArgs e)
        {
            pulseButton = false;
            ((App)App.Current).Navigation.PushAsync(new RedemptionPage());
            ((App)App.Current).Navigation.Navigation.RemovePage(this);
            Settings.IntroComplete = true;
            _httpClient.Dispose();             
        }

        private static async Task<PanelState> ChangePanelState(Grid fromPanel, Grid toPanel, PanelState fromState)
        {
            var fromContent = fromPanel.Children.OfType<Layout>().FirstOrDefault();
            await Task.WhenAll(
                fromContent.TranslateTo(-200, 0, 250, Easing.CubicInOut),
                fromPanel.FadeTo(0)
            );
            fromPanel.IsVisible = false;

            var toContent = toPanel.Children.OfType<Layout>().FirstOrDefault();
            toPanel.Opacity = 0;
            toPanel.IsVisible = true;
            toContent.TranslationX = 200;
            await Task.WhenAll(
                toContent.TranslateTo(0, 0, 250, Easing.CubicInOut),
                toPanel.FadeTo(1)
            );

            return fromState + 1;
        }
    }
}

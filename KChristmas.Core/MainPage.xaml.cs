using KChristmas.Core.Helpers;
using KChristmas.Core.XamlExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KChristmas.Core
{
    public partial class MainPage : ContentPage
    {
        private const uint StartingSpecialEventCooldown = 50;
        private readonly bool SkipCountdown = false;
        private readonly DateTime ChristmasDate = new DateTime(2018, 12, 24, 18, 0, 0);
        private readonly HttpClient _httpClient = new HttpClient();
        private const string GetGiftHintsUrl = "https://kc2016.azurewebsites.net/api/GetGiftHints?code=7c5RrOfucfopvE0g1woo10kMHU/pz4v5MHd8Njo0m00s8TuN1PvAfA==";

        private List<string> _giftHints = new List<string>();
        private List<string> _seenHints = new List<string>();
        private Random rand = new Random();
        private uint CurrentSpecialEventCooldown = 15;

        public MainPage()
        {
#if !DEBUG
            SkipCountdown = false;
#endif
            InitializeComponent();            

            //Init with locally-cached hints
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
            //Set up panel state
            await Task.Delay(1000);
            TooEarlyPanel.Opacity = 0;
            TooEarlyPanel.IsVisible = true;
            await TooEarlyPanel.FadeTo(1, 500);           

            //Set up gift box hints
            try
            {
                string response = await _httpClient.GetStringAsync(GetGiftHintsUrl);
                if (String.IsNullOrWhiteSpace(response))
                {
                    return;
                }

                // Update local cache
                Settings.GiftHints = response.Trim('"');
                InitHints(Settings.GiftHints);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get gift hints list.");
            }

            if (DateTime.Now < ChristmasDate && !SkipCountdown)
            {
                //Set up countdown timer
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
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
                        NextButton.IsVisible = true;
                        NextButton.InputTransparent = false;
                        Task.WhenAll(                            
                            TooEarlyLabel1.FadeTo(0, 2000),
                            TooEarlyLabel2.FadeTo(0, 2000),
                            TimerLabel.TranslateTo(0, 50, 4000),
                            TimerLabel.ScaleTo(2, 4000),
                            NextButton.FadeTo(1, 4000)
                        );
                        return false;
                    }
                });                
            }
            else
            {
                NextButton.Opacity = 1;
                NextButton.InputTransparent = false;
            }
        }
        
        private async void Gift_Clicked(object sender, EventArgs e)
        {            
            var storyboard = new Animation();
            var shakeUpHigh = new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, 0, 20, Easing.SpringIn);
            var fromHighToLow = new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, 20, -20, Easing.SpringIn);
            var fromLowToSmallHigh = new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, -20, 10, Easing.SpringIn);
            var fromSmallHighToSmallLow = new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, 10, -10, Easing.SpringIn);
            var fromSmallLowToTinyHigh = new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, -10, 5, Easing.SpringIn);
            var fromTinyHighToTinyLow = new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, 5, -5, Easing.SpringIn);
            var fromTinyLowToComplete = new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, -5, 0, Easing.SpringOut);
            storyboard.Add(0.0, 0.1, shakeUpHigh);
            storyboard.Add(0.1, 0.2, fromHighToLow);
            storyboard.Add(0.2, 0.3, fromLowToSmallHigh);
            storyboard.Add(0.3, 0.4, fromSmallHighToSmallLow);
            storyboard.Add(0.4, 0.5, fromSmallLowToTinyHigh);
            storyboard.Add(0.5, 0.6, fromTinyHighToTinyLow);
            storyboard.Add(0.6, 1.0, fromTinyLowToComplete);

            storyboard.Commit(GiftBase, "ShakeAnimation", 16, 1000);

            if (CurrentSpecialEventCooldown == 0 && rand.Next() % 15 == 0)
            {
                await RandomSpecialEvent();
                CurrentSpecialEventCooldown = StartingSpecialEventCooldown;
            }
            else
            {
                await ShowFloatingText(GetHint());
                if (CurrentSpecialEventCooldown > 0)
                {
                    CurrentSpecialEventCooldown -= 1;
                }
            }                                    
        }

        private async Task RandomSpecialEvent()
        {
            // For now, we only have one. In the future, we can do some randomness here.
            Rectangle baseRect = GiftBase.Bounds;
            double xMid = baseRect.X + baseRect.Width / 2;
            double pinkieHeight = 94;
            double pinkieWidth = 94;

            Image pinkieImage = new Image
            {
                HeightRequest = 94,
                WidthRequest = 94,
                Source = ImageExtension.GetPlatformIndependentPath("pinkie_woundup_1.png")
            };
            double pinkieMid = pinkieImage.WidthRequest / 2;
            pinkieImage.Rotation = 270;

            AbsoluteLayout.SetLayoutBounds(pinkieImage, new Rectangle(xMid - pinkieMid, baseRect.Y - 40, pinkieWidth, pinkieHeight));
            SpecialEventCanvas.Children.Add(pinkieImage);

            double totalMillis = 3000.0;
            double tick = 25.0 / totalMillis;
            var boxShudderSlamOpen = new Animation
            {
                { 0.0, tick * 1, new Animation(v => { GiftBase.TranslationX = v; GiftTop.TranslationX = v; }, 0, 10) },
                { tick * 1, tick * 2, new Animation(v => { GiftBase.TranslationX = v; GiftTop.TranslationX = v; }, 10, -10) },
                { tick * 2, tick * 3, new Animation(v => { GiftBase.TranslationX = v; GiftTop.TranslationX = v; }, 10, -10) },
                { tick * 3, tick * 4, new Animation(v => { GiftBase.TranslationX = v; GiftTop.TranslationX = v; }, -10, 0) },
                { tick * 4, tick * 5, new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, 0, 10) },
                { tick * 5, tick * 6, new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, 10, -10) },
                { tick * 6, tick * 7, new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, -10, 0) },

                { tick * 7, tick * 8, new Animation(v => { GiftBase.TranslationX = v; GiftTop.TranslationX = v; }, 10, -10) },
                { tick * 8, tick * 9, new Animation(v => { GiftBase.TranslationX = v; GiftTop.TranslationX = v; }, 10, -10) },
                { tick * 9, tick * 10, new Animation(v => { GiftBase.TranslationX = v; GiftTop.TranslationX = v; }, -10, 0) },
                { tick * 10, tick * 11, new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, 0, 10) },
                { tick * 11, tick * 12, new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, 10, -10) },
                { tick * 12, tick * 13, new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, -10, 0) },

                { tick * 13, tick * 14, new Animation(v => { GiftBase.TranslationX = v; GiftTop.TranslationX = v; }, 10, -10) },
                { tick * 14, tick * 15, new Animation(v => { GiftBase.TranslationX = v; GiftTop.TranslationX = v; }, 10, -10) },
                { tick * 15, tick * 16, new Animation(v => { GiftBase.TranslationX = v; GiftTop.TranslationX = v; }, -10, 0) },
                { tick * 16, tick * 17, new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, 0, 10) },
                { tick * 17, tick * 18, new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, 10, -10) },
                { tick * 18, tick * 19, new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, -10, 0) },

                { tick * 19, tick * 20, new Animation(v => { GiftBase.TranslationX = v; GiftTop.TranslationX = v; }, 10, -10) },
                { tick * 20, tick * 21, new Animation(v => { GiftBase.TranslationX = v; GiftTop.TranslationX = v; }, 10, -10) },
                { tick * 21, tick * 22, new Animation(v => { GiftBase.TranslationX = v; GiftTop.TranslationX = v; }, -10, 0) },
                { tick * 22, tick * 23, new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, 0, 10) },
                { tick * 23, tick * 24, new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, 10, -10) },
                { tick * 24, tick * 25, new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, -10, 0) },

                { tick * 25, tick * 26, new Animation(v => { GiftBase.TranslationX = v; GiftTop.TranslationX = v; }, 10, -10) },
                { tick * 26, tick * 27, new Animation(v => { GiftBase.TranslationX = v; GiftTop.TranslationX = v; }, 10, -10) },
                { tick * 27, tick * 28, new Animation(v => { GiftBase.TranslationX = v; GiftTop.TranslationX = v; }, -10, 0) },
                { tick * 28, tick * 29, new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, 0, 10) },
                { tick * 29, tick * 30, new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, 10, -10) },
                { tick * 30, tick * 31, new Animation(v => { GiftBase.TranslationY = v; GiftTop.TranslationY = v; }, -10, 0) },

                { 0.95, 0.97, new Animation(v => {pinkieImage.Source = ImageExtension.GetPlatformIndependentPath("pinkie_woundup_2.png"); }, 0, 0) },
                { 0.95, 0.98, new Animation(v => {GiftBase.TranslationY = v; }, 0, -30, Easing.SpringOut) },
                { 0.97, 1.0, new Animation(v => {pinkieImage.Source = ImageExtension.GetPlatformIndependentPath("pinkie_woundup_3.png"); }, 0, 0) },
                { 0.98, 1.0, new Animation(v => {GiftBase.TranslationY = v; }, -30, 0, Easing.SpringOut) },
                { 0.95, 1.0, new Animation(v => { GiftTop.TranslationY = v; }, 0, -300, Easing.CubicOut) }
            };

            boxShudderSlamOpen.Commit(GiftBase, "BoxShduder", 16, (uint)totalMillis);
            await Task.Delay((int)totalMillis);

            await Task.Delay(750);
            pinkieImage.Source = ImageExtension.GetPlatformIndependentPath("pinkie_woundup_2.png");
            await Task.Delay(750);
            await pinkieImage.TranslateTo(0, 20, 1500);

            pinkieImage.Rotation = 0;
            pinkieImage.Source = ImageExtension.GetPlatformIndependentPath("pinkie_bounce_up_3.png");
            await pinkieImage.TranslateTo(0, -20, 300, Easing.SpringOut);

            ShowFloatingText("Hi!", Color.HotPink);
            await Task.Delay(2000);

            pinkieImage.Source = ImageExtension.GetPlatformIndependentPath("pinkie_confused.png");
            ShowFloatingText("Hey, wait a minute...", Color.HotPink);
            await Task.Delay(4000);

            ShowFloatingText("...this isn't the Hearth's Warming Eve party!", Color.HotPink);
            await Task.Delay(4000);

            pinkieImage.Source = ImageExtension.GetPlatformIndependentPath("pinkie_bounce_up_3.png");
            ShowFloatingText("Ohmigosh, I gotta get going!", Color.HotPink);
            await Task.Delay(4000);

            ShowFloatingText("I hope you're having lots and lots of fun though!", Color.HotPink);
            await Task.Delay(4000);

            await ShowFloatingText("Byeeeee!", Color.HotPink);
            await pinkieImage.TranslateTo(0, 40, 300, Easing.SpringIn);
            pinkieImage = null;
            SpecialEventCanvas.Children.Clear();
            await GiftTop.TranslateTo(0, 0, 300, Easing.SpringIn);
        }

        private async Task ShowFloatingText(string text, Color? textColor = null)
        {
            Label floatingHintLabel = new Label
            {                
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                VerticalOptions = LayoutOptions.Center,
                TranslationY = -130,
                HorizontalTextAlignment = TextAlignment.Center,
                Opacity = 0
            };
            if (textColor != null)
            {
                floatingHintLabel.TextColor = textColor.Value;
            }

            Grid.SetRow(floatingHintLabel, 0);
            TooEarlyPanel.Children.Add(floatingHintLabel);

            floatingHintLabel.Text = text;
            await floatingHintLabel.FadeTo(1, 100);
            await Task.WhenAll(
                floatingHintLabel.FadeTo(0, 5000),
                floatingHintLabel.TranslateTo(0, -230, 5000)
            );

            TooEarlyPanel.Children.Remove(floatingHintLabel);
        }

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            await Task.WhenAll(
                GiftTop.FadeTo(0, 1000),
                GiftTop.TranslateTo(0, -100, 1000));

            await NavigateToRedemptionPage();
        }
        
        private async Task NavigateToRedemptionPage()
        {
            await ((App)App.Current).Navigation.PushAsync(new RedemptionPage());
            ((App)App.Current).Navigation.Navigation.RemovePage(this);
            _httpClient.Dispose();
        }
    }
}

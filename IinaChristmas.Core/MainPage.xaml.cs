using IinaChristmas.Core.Helpers;
using IinaChristmas.Core.SpecialEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading;

namespace IinaChristmas.Core
{
    public partial class MainPage : ContentPage
    {
        private const uint StartingSpecialEventCooldown = 40;
        private readonly bool SkipCountdown = true;
        private readonly DateTime ChristmasDate = new DateTime(2021, 12, 24, 18, 0, 0);

        private bool _isSpecialEventInProgress = false;
        private NetworkService _networkService;
        private List<string> _giftHints = new List<string>();
        private List<string> _seenHints = new List<string>();
        private Random rand = new Random();
        private uint CurrentSpecialEventCooldown = 5;
        private PinkieEventService _pinkieService;
        private SensorSpeed _sensorSpeed = SensorSpeed.Game;
        private DateTimeOffset? _lastShownHintTime = null;

        public MainPage(NetworkService networkService)
        {
#if !DEBUG
            SkipCountdown = false;
#endif
            InitializeComponent();

            _networkService = networkService;
            //Init with locally-cached hints
            InitHints(Settings.GiftHintsV2);
            _pinkieService = new PinkieEventService(this, networkService);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //Set up panel state
            await Task.Delay(1000);
            TooEarlyPanel.Opacity = 0;
            TooEarlyPanel.IsVisible = true;

            await TooEarlyPanel.FadeTo(1, 500);

            // Undo possible navigation to next page
            await Task.WhenAll(
                GiftTop.TranslateTo(0, 0, 500),
                GiftTop.FadeTo(1, 500));

            if (DateTime.Now < ChristmasDate && !SkipCountdown)
            {
                //Set up countdown timer
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {                    
                    var timeTillChristmas = ChristmasDate - DateTime.Now;
                    if (timeTillChristmas > TimeSpan.Zero)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            TimerLabel.Text = $"{timeTillChristmas.Days}d {timeTillChristmas.Hours}h {timeTillChristmas.Minutes}m {timeTillChristmas.Seconds}s";
                        });
                        return true;
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
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
                        });

                        return false;
                    }
                });
            }
            else
            {
                NextButton.Opacity = 1;
                NextButton.InputTransparent = false;
            }

            //Set up gift box hints                  
            string? response = await _networkService.GetGiftHints();
            if (String.IsNullOrWhiteSpace(response))
            {
                return;
            }

            // Update local cache
            Settings.GiftHintsV2 = response;
            InitHints(Settings.GiftHintsV2);
            await _pinkieService.UpdateEventsFromRemote();

            Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
            try
            {
                if (!Accelerometer.IsMonitoring)
                {
                    Accelerometer.Start(_sensorSpeed);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to start listening to Accelerometer: {ex}");
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Accelerometer.ShakeDetected -= Accelerometer_ShakeDetected;
            try
            {
                if (Accelerometer.IsMonitoring)
                {
                    Accelerometer.Stop();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to stop listening to Accelerometer: {ex}");
            }
        }

        private void InitHints(string? hintStringJson)
        {
            if (Settings.GiftHintsV2 != null)
            {
                _giftHints = JsonConvert.DeserializeObject<string[]>(hintStringJson).ToList();
            }
        }

        private string GetHint()
        {
            if (_giftHints.Count <= 0 && _seenHints.Count > 0)
            {
                Debug.WriteLine("SeenHints emptied, GiftHints refilled.");
                _giftHints = new List<string>(_seenHints);
                _seenHints.Clear();
            }
            if (_giftHints.Count > 0)
            {
                int hintIndex = rand.Next() % _giftHints.Count;
                string hint = _giftHints[hintIndex];
                _giftHints.RemoveAt(hintIndex);
                _seenHints.Add(hint);
                return hint;
            }
            else
            {
                return "No hints here. Sorry! Maybe try restarting the app?";
            }
        }

        private async void Gift_Clicked(object sender, EventArgs e)
        {
            await ShowHint();
        }

        private SemaphoreSlim _shakeLock = new SemaphoreSlim(1);
        private async void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            if (await _shakeLock.WaitAsync(100))
            {
                await ShowHint();
                await Task.Delay(1000);
                _shakeLock.Release();
            }
        }

        private async Task ShowHint()
        {
            if (_isSpecialEventInProgress)
            {
                return;
            }

            // A half-second cooldown on hints, so users can't accidentally spam themselves.
            var now = DateTimeOffset.UtcNow;
            if (_lastShownHintTime != null && now - _lastShownHintTime < TimeSpan.FromMilliseconds(500))
            {
                return;
            }

            _lastShownHintTime = now;

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

            if (CurrentSpecialEventCooldown == 0 && rand.Next() % 7 == 0)
            {
                await RandomSpecialEvent();
                CurrentSpecialEventCooldown = StartingSpecialEventCooldown;
            }
            else
            {
                Debug.WriteLine($"Current Special Event Cooldown counter is: {CurrentSpecialEventCooldown}.");
                if (CurrentSpecialEventCooldown > 0)
                {
                    CurrentSpecialEventCooldown -= 1;
                }
                await ShowFloatingText(GetHint());
            }
        }

        private async Task RandomSpecialEvent()
        {
            _isSpecialEventInProgress = true;
            GiftBase.InputTransparent = true;
            GiftTop.InputTransparent = true;

            // For now, we only have one. In the future, we can do some randomness here.            
            await _pinkieService.Run(GiftBase, GiftTop, SpecialEventCanvas);

            GiftBase.InputTransparent = false;
            GiftTop.InputTransparent = false;
            _isSpecialEventInProgress = false;
        }

        public async Task ShowFloatingText(string text, Color? textColor = null)
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

            await TooEarlyPanel.FadeTo(0, 1000);

            await ((App)App.Current).Navigation.PushAsync(new RedemptionPage());
        }
    }
}

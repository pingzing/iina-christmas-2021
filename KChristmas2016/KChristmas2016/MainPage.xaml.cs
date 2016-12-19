using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace KChristmas2016
{
    public partial class MainPage : ContentPage
    {
        private readonly bool IsInDebug = false;

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
            IsInDebug = true;
#endif
            InitializeComponent();
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            if (DateTime.Now <= new DateTime(2016, 12, 24) && !IsInDebug)
            {
                await Task.Delay(1000);
                TooEarlyPanel.Opacity = 0;
                TooEarlyPanel.IsVisible = true;
                await TooEarlyPanel.FadeTo(1, 500);
                _currentState = PanelState.None;
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

        private async void Panel5_NextButton_Clicked(object sender, EventArgs e)
        {
            _currentState = await ChangePanelState(Panel5, Panel6, _currentState);
            await Task.Delay(1000);
            await Panel6_Caption.FadeTo(1, 1000);
            await Task.Delay(1500);
            await Panel6_NextButton.FadeTo(1, 1000);

            Animation buttonPulseAnimation = new Animation();
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

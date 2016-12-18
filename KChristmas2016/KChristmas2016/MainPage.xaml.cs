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
        private enum PanelState
        {
            None,
            Panel1,
            Panel2,
            End
        }

        private PanelState _currentState;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {            
            await Task.Delay(1000);
            Panel1.Opacity = 0;
            Panel1.IsVisible = true;
            await Panel1.FadeTo(1, 500);
            _currentState = PanelState.Panel1;
        }

        private async void Panel1NextButton_Clicked(object sender, EventArgs e)
        {
            _currentState = await ChangePanelState(Panel1, Panel2, _currentState);
        }        

        private static async Task<PanelState> ChangePanelState(Grid fromPanel, Grid toPanel, PanelState fromState)
        {
            var fromContent = fromPanel.Children.OfType<StackLayout>().FirstOrDefault();
            await Task.WhenAll(
                fromContent.TranslateTo(-200, 0, 250, Easing.CubicInOut),
                fromPanel.FadeTo(0)
            );
            fromPanel.IsVisible = false;

            var toContent = toPanel.Children.OfType<StackLayout>().FirstOrDefault();
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

using GoFish.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GoFish
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        private GameStateViewModel gameState = new GameStateViewModel();

        public Settings()
        {
            this.InitializeComponent();
        }

        private void DarkRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            gameState.UsingLightTheme = false;
        }

        private void LightRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            gameState.UsingLightTheme = true;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            string json = JsonConvert.SerializeObject(gameState);
            ApplicationData.Current.LocalSettings.Values["gameState"] = json;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("gameState"))
            {
                lightRadioBtn.IsChecked = true;
                string json = ApplicationData.Current.LocalSettings.Values["gameState"] as string;
                gameState = JsonConvert.DeserializeObject<GameStateViewModel>(json);
                if (!gameState.UsingLightTheme)
                {
                    darkRadioBtn.IsChecked = true;
                }
                if (!gameState.UsingWoodlandDeck)
                {
                    woodlandImg.Opacity = 0.5;
                    seaImg.Opacity = 1;
                }
            }
        }

        private void SeaImg_Tapped(object sender, TappedRoutedEventArgs e)
        {
            gameState.UsingWoodlandDeck = false;
            woodlandImg.Opacity = 0.5;
            seaImg.Opacity = 1;
        }

        private void WoodlandImg_Tapped(object sender, TappedRoutedEventArgs e)
        {
            gameState.UsingWoodlandDeck = true;
            woodlandImg.Opacity = 1;
            seaImg.Opacity = 0.5;
        }
    }
}

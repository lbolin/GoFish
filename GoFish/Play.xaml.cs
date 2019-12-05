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
    public sealed partial class Play : Page
    {
        private GameStateViewModel gameState = new GameStateViewModel();

        public Play()
        {
            this.InitializeComponent();
        }

        private void PlayGameBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(gamePlay));
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
                string json = ApplicationData.Current.LocalSettings.Values["gameState"] as string;
                gameState = JsonConvert.DeserializeObject<GameStateViewModel>(json);
                if (gameState.AIDifficulty == 1)
                {
                    MediumBtn_Tapped(null, null);
                }
                else if (gameState.AIDifficulty == 2)
                {
                    HardBtn_Tapped(null, null);
                }
            }
        }

        private void HardBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            hardBtn.BorderThickness = new Thickness(3);
            mediumBtn.BorderThickness = new Thickness(0);
            easyBtn.BorderThickness = new Thickness(0);
            gameState.AIDifficulty = 2;
        }

        private void MediumBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            hardBtn.BorderThickness = new Thickness(0);
            mediumBtn.BorderThickness = new Thickness(3);
            easyBtn.BorderThickness = new Thickness(0);
            gameState.AIDifficulty = 1;
        }

        private void EasyBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            hardBtn.BorderThickness = new Thickness(0);
            mediumBtn.BorderThickness = new Thickness(0);
            easyBtn.BorderThickness = new Thickness(3);
            gameState.AIDifficulty = 0;
        }
    }
}

using GoFish.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GoFish
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class gamePlay : Page
    {
        private GameViewModel game = new GameViewModel();

        private int selectedCard = -1;

        public gamePlay()
        {
            this.InitializeComponent();

            var img = NewImage("Assets/images/back1.png");
            img.Width = 100;
            img.Height = 100;
            mainGrid.Children.Add(img);

            Draw();
        }

        public void Draw()
        {
            myCards.Children.Clear();
            opponentCards.Children.Clear();

            foreach (var card in game.Hands[1].Cards)
            {
                string cardNumber = card.Number.ToString();
                if (cardNumber.Length == 1) cardNumber = "0" + cardNumber;
                                
                var newCard = NewImage("Assets/Woodland/" + cardNumber + "-" + card.Suit + ".png");
                if (card.Number == selectedCard / 10 && card.Suit == selectedCard % 10)
                {
                    newCard.Width = 100;
                    newCard.Height = 140;
                }
                else
                {
                    newCard.Width = 80;
                    newCard.Height = 110;
                }
                newCard.Margin = new Thickness(2, 0, 2, 0);
                newCard.Tapped += SelectCard;
                newCard.Tag = card;
                myCards.Children.Add(newCard);
            }

            foreach (var card in game.Hands[2].Cards)
            {
                var newCard = NewImage("Assets/images/back1.png");
                newCard.Width = 80;
                newCard.Height = 110;
                newCard.Margin = new Thickness(2, 0, 2, 0);
                opponentCards.Children.Add(newCard);
            }
        }

        private void SelectCard(object sender, TappedRoutedEventArgs e)
        {
            var card = ((sender as Image)?.Tag as CardViewModel);
            selectedCard = card.Number * 10 + card.Suit;
            Draw();
        }

        public Image NewImage(string path)
        {
            //https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.image
            Image img = new Image();
            BitmapImage bitmapImage = new BitmapImage();
            img.Width = bitmapImage.DecodePixelWidth = 80;
            bitmapImage.UriSource = new Uri("ms-appx:///" + path);
            img.Source = bitmapImage;
            return img;
        }
    }
}

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
            
            Draw();
        }

        public void Draw()
        {
            myScore.Text = game.Scores[0].ToString();
            opponentScore.Text = game.Scores[1].ToString();

            myCards.Children.Clear();
            opponentCards.Children.Clear();
            deck.Children.Clear();

            foreach (var card in game.Hands[1].Cards)
            {
                string cardNumber = card.Number.ToString();
                if (cardNumber.Length == 1) cardNumber = "0" + cardNumber;
                                
                var newCard = NewImage("Assets/Woodland/" + cardNumber + "-" + card.Suit + ".png");
                if (card.Number == selectedCard / 10 && card.Suit == selectedCard % 10)
                {
                    newCard.Width = 80;
                    newCard.Height = 110;
                }
                else
                {
                    newCard.Width = 65;
                    newCard.Height = 90;
                }
                newCard.Margin = new Thickness(2, 0, 2, 0);
                newCard.Tapped += SelectCard;
                newCard.Tag = card;
                myCards.Children.Add(newCard);
            }

            foreach (var card in game.Hands[2].Cards)
            {
                var newCard = NewImage("Assets/images/back1.png");
                newCard.Width = 65;
                newCard.Height = 90;
                newCard.Tapped += RequestCard;
                newCard.Margin = new Thickness(2, 0, 2, 0);
                opponentCards.Children.Add(newCard);
            }

            if (game.Hands[0].Cards.Count() != 0)
            {
                var img = NewImage("Assets/images/back1.png");
                img.Width = 100;
                img.Height = 100;
                deck.Children.Add(img);
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
        
        public void RequestCard(object sender, TappedRoutedEventArgs e)
        {
            if (game.Hands[2].HasMatch(selectedCard / 10)){
                //got'm
                var cardsToSwap = new List<CardViewModel>();
                foreach (CardViewModel c in game.Hands[2].Cards)
                {
                    if (c.Number == selectedCard / 10)
                    {
                        cardsToSwap.Add(c);
                    }
                }

                foreach (var c in cardsToSwap)
                {
                    game.Hands[1].Cards.Add(c);
                    game.Hands[2].Cards.Remove(c);
                }
            }
            else
            {
                //Go Fish!
                if (game.Hands[0].Cards.Count > 0)
                {
                    game.Hands[1].Cards.Add(game.Hands[0].Cards[0]);
                    game.Hands[0].Cards.Remove(game.Hands[0].Cards[0]);                    
                }

                MakeOpponentMove();
            }
            game.PostMoveRefresh();
            Draw();
        }

        public void MakeOpponentMove()
        {
            Random random = new Random();
            int numberAskingFor = game.Hands[2].Cards[random.Next(0, game.Hands[2].Cards.Count() - 1)].Number;

            if (game.Hands[1].HasMatch(numberAskingFor))
            {
                //got'm
                var cardsToSwap = new List<CardViewModel>();
                foreach (CardViewModel c in game.Hands[1].Cards)
                {
                    if (c.Number == numberAskingFor)
                    {
                        cardsToSwap.Add(c);
                    }
                }

                foreach (var c in cardsToSwap)
                {
                    game.Hands[2].Cards.Add(c);
                    game.Hands[1].Cards.Remove(c);
                }

                game.PostMoveRefresh();
                MakeOpponentMove();
            }
            else
            {
                //Go Fish!
                if (game.Hands[0].Cards.Count > 0)
                {
                    game.Hands[2].Cards.Add(game.Hands[0].Cards[0]);
                    game.Hands[0].Cards.Remove(game.Hands[0].Cards[0]);
                }
            }
            game.PostMoveRefresh();
        }
    }
}

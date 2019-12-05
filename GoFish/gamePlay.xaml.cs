using GoFish.Models;
using GoFish.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
        private GameStateViewModel gameState = new GameStateViewModel();
        private GameViewModel game = new GameViewModel();

        private int opponentSelectedCardIndex = -1;
        private int selectedCard = -1;
        private bool selectionLocked = false;

        //https://www.c-sharpcorner.com/uploadfile/kirtan007/measure-execution-time-of-code-in-C-Sharp/
        Stopwatch sw = new Stopwatch();

        public gamePlay()
        {
            this.InitializeComponent();
            
            Draw();

            sw.Start();
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

            for (int i = 0; i < game.Hands[2].Cards.Count(); i++)
            {
                var newCard = NewImage("Assets/images/back1.png");
                
                if (i == opponentSelectedCardIndex)
                {
                    string number = game.Hands[2].Cards[i].Number.ToString();
                    if (number.Length == 1) number = "0" + number;
                    newCard = NewImage("Assets/Woodland/" + number + "-" + game.Hands[2].Cards[i].Suit.ToString() + ".png");
                }
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
            if (!selectionLocked)
            {
                var card = ((sender as Image)?.Tag as CardViewModel);
                selectedCard = card.Number * 10 + card.Suit;
                Draw();
            }
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

        public async void RequestCard(object sender, TappedRoutedEventArgs e)
        {
            if (selectionLocked || selectedCard == -1) return;
            await Task.Run(() => CardRequestSequenceMessages());            
        }

        public int GetAIMove(int mode)
        {
            Random random = new Random();
            if (mode == 0)
            {
                return random.Next(0, game.Hands[2].Cards.Count() - 1);
            }
            else if (mode == 1)
            {
                var cardItems = new List<int> ();
                for (int i = 0; i < 13; i++) cardItems.Add(0);
                foreach (var c in game.Hands[2].Cards) ++cardItems[c.Number];

                int total = 0;
                for (int i = 0; i < cardItems.Count(); i++)
                {
                    cardItems[i] *= cardItems[i];
                    total += cardItems[i];
                }
                int value = random.Next(0, total - 1);
                int current = 0;
                int index = 0;
                while (current <= value)
                {
                    current += cardItems[index];
                    if (current <= value)
                    {
                        index++;
                    }
                }

                int actualIndex = -1;
                for (var i = 0; i < game.Hands[2].Cards.Count() && actualIndex == -1; i++)
                {
                    if (game.Hands[2].Cards[i].Number == index) actualIndex = i;
                }
                return actualIndex;
            }
            else if (mode == 2)
            {
                //Todo hard mode
                return 0;
            }
            else
            {
                int index = -1;
                for (int i = 0; i < game.Hands[2].Cards.Count(); i++)
                {
                    foreach (var theirCard in game.Hands[1].Cards)
                    {
                        if (index == -1 && game.Hands[2].Cards[i].Number == theirCard.Number) index = i;
                    }
                }
                if (index == -1)
                {
                    index = random.Next(0, game.Hands[2].Cards.Count() - 1);
                }
                return index;
            }
        }

        private async void HandleEndOfMoveSequence()
        {
            game.PostMoveRefresh();
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                Draw();
            });
            selectionLocked = false;

            if (game.GameOver)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                    sw.Stop();
                    opponentChat.Text = "GAME OVER";
                    string text = "You tied!";
                    if (game.Scores[0] > game.Scores[1])
                    {
                        text = "You won!";
                    }
                    else if (game.Scores[0] < game.Scores[1])
                    {
                        text = "You lost!";
                    }
                    myChat.Text = text;

                    timeLabel.Text += ((int)(sw.ElapsedMilliseconds / 1000)).ToString();

                    nameLabel.Visibility = Visibility.Visible;
                    nameTxt.Visibility = Visibility.Visible;
                    timeLabel.Visibility = Visibility.Visible;
                    submitBtn.Visibility = Visibility.Visible;                    
                });
            }
        }

        private async void CardRequestSequenceMessages()
        {
            selectionLocked = true;
            //https://social.msdn.microsoft.com/Forums/windowsapps/en-US/4342b343-fc45-4638-a183-f5b77e1063ff/uwp-when-am-i-in-the-main-ui-thread
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                myChat.Text = "Do you have this card?";
            });

            Thread.Sleep(2000);

            if (game.Hands[2].HasMatch(selectedCard / 10))
            {
                //got'm
                var cardsToSwap = new List<CardViewModel>();
                foreach (CardViewModel c in game.Hands[2].Cards)
                {
                    if (c.Number == selectedCard / 10)
                    {
                        cardsToSwap.Add(c);
                    }
                }
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                   opponentChat.Text = "I have "+cardsToSwap.Count()+" of them.";
                });

                Thread.Sleep(1000);

                foreach (var c in cardsToSwap)
                {
                    game.Hands[1].Cards.Add(c);
                    game.Hands[2].Cards.Remove(c);
                }
                selectedCard = -1;
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                    Draw();
                    opponentChat.Text = "";
                    myChat.Text = "";
                });

                HandleEndOfMoveSequence();
            }
            else
            {
                //Go Fish!
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                    opponentChat.Text = "Go Fish! ";
                });

                Thread.Sleep(1000);

                if (game.Hands[0].Cards.Count > 0)
                {
                    game.Hands[1].Cards.Add(game.Hands[0].Cards[0]);
                    game.Hands[0].Cards.Remove(game.Hands[0].Cards[0]);
                }
                selectedCard = -1;
                game.PostMoveRefresh();
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                    Draw();
                    opponentChat.Text = "";
                    myChat.Text = "";
                });

                Thread.Sleep(2000);

                MakeOpponentMove();
            }
        }

        public async void MakeOpponentMove()
        {
            int cardId = GetAIMove(3);
            int numberAskingFor = game.Hands[2].Cards[cardId].Number;
            opponentSelectedCardIndex = cardId;

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                opponentChat.Text = "Do you have this card?";
                Draw();
            });

            Thread.Sleep(2000);
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
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                    myChat.Text = "I have " + cardsToSwap.Count() + " of them.";
                });

                Thread.Sleep(1000);

                foreach (var c in cardsToSwap)
                {
                    game.Hands[2].Cards.Add(c);
                    game.Hands[1].Cards.Remove(c);
                }
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                    myChat.Text = "";
                    opponentChat.Text = "";
                    opponentSelectedCardIndex = -1;
                    Draw();
                });
                
                Thread.Sleep(2000);

                HandleEndOfMoveSequence();

                if (game.Hands[2].Cards.Count() > 0)
                {
                    MakeOpponentMove();
                }
            }
            else
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                    myChat.Text = "Go Fish! ";
                });

                Thread.Sleep(1000);

                //Go Fish!
                if (game.Hands[0].Cards.Count > 0)
                {
                    game.Hands[2].Cards.Add(game.Hands[0].Cards[0]);
                    game.Hands[0].Cards.Remove(game.Hands[0].Cards[0]);
                }
                game.PostMoveRefresh();
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                    myChat.Text = "";
                    opponentChat.Text = "";
                    opponentSelectedCardIndex = -1;
                    Draw();
                });

                HandleEndOfMoveSequence();
            }
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
            }
        }

        private void SubmitBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (nameTxt.Text != "")
            {
                gameState.HighScores.Add(new HighScoreViewModel(new Model.HighScore(nameTxt.Text, game.Scores[0], (int)(sw.ElapsedMilliseconds / 1000))));

                bool done = false;
                int currentIndex = gameState.HighScores.Count() - 1;
                while (!done)
                {
                    if (currentIndex == 0)
                    {
                        done = true;
                    }
                    else if (gameState.HighScores[currentIndex].Books > gameState.HighScores[currentIndex - 1].Books ||
                        (gameState.HighScores[currentIndex].Books == gameState.HighScores[currentIndex - 1].Books &&
                        gameState.HighScores[currentIndex].Time < gameState.HighScores[currentIndex - 1].Time))
                    {
                        var temp = gameState.HighScores[currentIndex];
                        gameState.HighScores[currentIndex] = gameState.HighScores[currentIndex - 1];
                        gameState.HighScores[currentIndex - 1] = temp;
                        currentIndex--;
                    }
                    else
                    {
                        done = true;
                    }
                }
                while (gameState.HighScores.Count > 10)
                {
                    gameState.HighScores.RemoveAt(10);
                }

                nameTxt.IsEnabled = false;
                submitBtn.IsEnabled = false;
            }
        }

        private void HelpBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(Help));
        }
    }
}

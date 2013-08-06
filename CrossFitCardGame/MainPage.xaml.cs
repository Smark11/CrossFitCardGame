using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CrossFitCardGame.Resources;
using System.Collections.ObjectModel;
using CrossFitCardGame.DataObjects;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;
using System.Globalization;
using Common.Licencing;
using Microsoft.Phone.Tasks;

namespace CrossFitCardGame
{
    public partial class MainPage : PhoneApplicationPage, System.ComponentModel.INotifyPropertyChanged, IDisposable
    {
        private bool _clockTickShouldBeRunning = true;
        private object _lockObject = new object();
        MarketplaceDetailTask _marketPlaceDetailTask = new MarketplaceDetailTask();
        public static MainPage _mainPageInstance;

        // Constructor
        public MainPage()
        {
            try
            {
                InitializeComponent();
                BuildLocalizedApplicationBar();
                _mainPageInstance = this;
                ClockTicked += MainPage_ClockTicked;
                BlackCards = new ObservableCollection<Card>();
                RedCards = new ObservableCollection<Card>();

                AdControl.ErrorOccurred += AdControl_ErrorOccurred;
                AdControl.CountryOrRegion = RegionInfo.CurrentRegion.TwoLetterISORegionName;
            

                if ((Application.Current as App).IsTrial)
                {
                    Trial.SaveStartDateOfTrial();

                    if (Trial.IsTrialExpired())
                    {
                        MessageBox.Show(AppResources.TrialExpired);
                        _marketPlaceDetailTask.Show();
                        GoToScreen(Screen.Trial);
                    }
                    else
                    {
                        MessageBox.Show(AppResources.YouHave + Trial.GetDaysLeftInTrial() + AppResources.DaysLeftInTrial);
                        GoToScreen(Screen.Main);
                    }
                }
                else
                {
                    GoToScreen(Screen.Main);
                }



                ShuffledDeck = new ObservableCollection<Card>();

                LoadCards();

                ShuffleDeck();
            }
            catch (Exception ex)
            {

            }
            ElapsedSeconds = 0;
            DataContext = this;

            StartTickingClock();
        }

        private void AdControl_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
            
        }

        private void LoadCards()
        {
            Card twoClubs = new Card(2, "/Cards/Clubs/2c.png", CardColor.Black, 1);
            Card threeClubs = new Card(3, "/Cards/Clubs/3c.png", CardColor.Black, 2);
            Card fourClubs = new Card(4, "/Cards/Clubs/4c.png", CardColor.Black, 3);
            Card fiveClubs = new Card(5, "/Cards/Clubs/5c.png", CardColor.Black, 4);
            Card sixClubs = new Card(6, "/Cards/Clubs/6c.png", CardColor.Black, 5);
            Card sevenClubs = new Card(7, "/Cards/Clubs/7c.png", CardColor.Black, 6);
            Card eightClubs = new Card(8, "/Cards/Clubs/8c.png", CardColor.Black, 7);
            Card nineClubs = new Card(9, "/Cards/Clubs/9c.png", CardColor.Black, 8);
            Card tenClubs = new Card(10, "/Cards/Clubs/10c.png", CardColor.Black, 9);
            Card jackClubs = new Card(11, "/Cards/Clubs/JC.png", CardColor.Black, 10);
            Card queenClubs = new Card(12, "/Cards/Clubs/QC.png", CardColor.Black, 11);
            Card kingClubs = new Card(13, "/Cards/Clubs/KC.png", CardColor.Black, 12);
            Card aceClubs = new Card(14, "/Cards/Clubs/AC.png", CardColor.Black, 13);

            BlackCards.Add(twoClubs);
            BlackCards.Add(threeClubs);
            BlackCards.Add(fourClubs);
            BlackCards.Add(fiveClubs);
            BlackCards.Add(sixClubs);
            BlackCards.Add(sevenClubs);
            BlackCards.Add(eightClubs);
            BlackCards.Add(nineClubs);
            BlackCards.Add(tenClubs);
            BlackCards.Add(jackClubs);
            BlackCards.Add(queenClubs);
            BlackCards.Add(kingClubs);
            BlackCards.Add(aceClubs);

            Card twoDiamonds = new Card(2, "/Cards/Diamonds/2d.png", CardColor.Red, 1);
            Card threeDiamonds = new Card(3, "/Cards/Diamonds/3d.png", CardColor.Red, 2);
            Card fourDiamonds = new Card(4, "/Cards/Diamonds/4d.png", CardColor.Red, 3);
            Card fiveDiamonds = new Card(5, "/Cards/Diamonds/5d.png", CardColor.Red, 4);
            Card sixDiamonds = new Card(6, "/Cards/Diamonds/6d.png", CardColor.Red, 5);
            Card sevenDiamonds = new Card(7, "/Cards/Diamonds/7d.png", CardColor.Red, 6);
            Card eightDiamonds = new Card(8, "/Cards/Diamonds/8d.png", CardColor.Red, 7);
            Card nineDiamonds = new Card(9, "/Cards/Diamonds/9d.png", CardColor.Red, 8);
            Card tenDiamonds = new Card(10, "/Cards/Diamonds/10d.png", CardColor.Red, 9);
            Card jackDiamonds = new Card(11, "/Cards/Diamonds/JD.png", CardColor.Red, 10);
            Card queenDiamonds = new Card(12, "/Cards/Diamonds/QD.png", CardColor.Red, 11);
            Card kingDiamonds = new Card(13, "/Cards/Diamonds/KD.png", CardColor.Red, 12);
            Card aceDiamonds = new Card(14, "/Cards/Diamonds/AD.png", CardColor.Red, 13);

            RedCards.Add(twoDiamonds);
            RedCards.Add(threeDiamonds);
            RedCards.Add(fourDiamonds);
            RedCards.Add(fiveDiamonds);
            RedCards.Add(sixDiamonds);
            RedCards.Add(sevenDiamonds);
            RedCards.Add(eightDiamonds);
            RedCards.Add(nineDiamonds);
            RedCards.Add(tenDiamonds);
            RedCards.Add(jackDiamonds);
            RedCards.Add(queenDiamonds);
            RedCards.Add(kingDiamonds);
            RedCards.Add(aceDiamonds);

            Card twoSpades = new Card(2, "/Cards/Spades/2s.png", CardColor.Black, 14);
            Card threeSpades = new Card(3, "/Cards/Spades/3s.png", CardColor.Black, 15);
            Card fourSpades = new Card(4, "/Cards/Spades/4s.png", CardColor.Black, 16);
            Card fiveSpades = new Card(5, "/Cards/Spades/5s.png", CardColor.Black, 17);
            Card sixSpades = new Card(6, "/Cards/Spades/6s.png", CardColor.Black, 18);
            Card sevenSpades = new Card(7, "/Cards/Spades/7s.png", CardColor.Black, 19);
            Card eightSpades = new Card(8, "/Cards/Spades/8s.png", CardColor.Black, 20);
            Card nineSpades = new Card(9, "/Cards/Spades/9s.png", CardColor.Black, 21);
            Card tenSpades = new Card(10, "/Cards/Spades/10s.png", CardColor.Black, 22);
            Card jackSpades = new Card(11, "/Cards/Spades/JS.png", CardColor.Black, 23);
            Card queenSpades = new Card(12, "/Cards/Spades/QS.png", CardColor.Black, 24);
            Card kingSpades = new Card(13, "/Cards/Spades/KS.png", CardColor.Black, 25);
            Card aceSpades = new Card(14, "/Cards/Spades/AS.png", CardColor.Black, 26);

            BlackCards.Add(twoSpades);
            BlackCards.Add(threeSpades);
            BlackCards.Add(fourSpades);
            BlackCards.Add(fiveSpades);
            BlackCards.Add(sixSpades);
            BlackCards.Add(sevenSpades);
            BlackCards.Add(eightSpades);
            BlackCards.Add(nineSpades);
            BlackCards.Add(tenSpades);
            BlackCards.Add(jackSpades);
            BlackCards.Add(queenSpades);
            BlackCards.Add(kingSpades);
            BlackCards.Add(aceSpades);

            Card twoHearts = new Card(2, "/Cards/Hearts/2h.png", CardColor.Red, 14);
            Card threeHearts = new Card(3, "/Cards/Hearts/3h.png", CardColor.Red, 15);
            Card fourHearts = new Card(4, "/Cards/Hearts/4h.png", CardColor.Red, 16);
            Card fiveHearts = new Card(5, "/Cards/Hearts/5h.png", CardColor.Red, 17);
            Card sixHearts = new Card(6, "/Cards/Hearts/6h.png", CardColor.Red, 18);
            Card sevenHearts = new Card(7, "/Cards/Hearts/7h.png", CardColor.Red, 19);
            Card eightHearts = new Card(8, "/Cards/Hearts/8h.png", CardColor.Red, 20);
            Card nineHearts = new Card(9, "/Cards/Hearts/9h.png", CardColor.Red, 21);
            Card tenHearts = new Card(10, "/Cards/Hearts/10h.png", CardColor.Red, 22);
            Card jackHearts = new Card(11, "/Cards/Hearts/JH.png", CardColor.Red, 23);
            Card queenHearts = new Card(12, "/Cards/Hearts/QH.png", CardColor.Red, 24);
            Card kingHearts = new Card(13, "/Cards/Hearts/KH.png", CardColor.Red, 25);
            Card aceHearts = new Card(14, "/Cards/Hearts/AH.png", CardColor.Red, 26);

            RedCards.Add(twoHearts);
            RedCards.Add(threeHearts);
            RedCards.Add(fourHearts);
            RedCards.Add(fiveHearts);
            RedCards.Add(sixHearts);
            RedCards.Add(sevenHearts);
            RedCards.Add(eightHearts);
            RedCards.Add(nineHearts);
            RedCards.Add(tenHearts);
            RedCards.Add(jackHearts);
            RedCards.Add(queenHearts);
            RedCards.Add(kingHearts);
            RedCards.Add(aceHearts);
        }

        public void GoToScreen(Screen screen)
        {

            switch (screen)
            {
                case Screen.Main:
                    CardVisibility = System.Windows.Visibility.Visible;
                    DoneVisibility = System.Windows.Visibility.Collapsed;
                    InfoGridVisibility = System.Windows.Visibility.Collapsed;
                    StatsVisibility = System.Windows.Visibility.Visible;
                    TrialScreenVisibility = System.Windows.Visibility.Collapsed;
                    break;
                case Screen.Trial:
                    CardVisibility = System.Windows.Visibility.Collapsed;
                    DoneVisibility = System.Windows.Visibility.Collapsed;
                    InfoGridVisibility = System.Windows.Visibility.Collapsed;
                    StatsVisibility = System.Windows.Visibility.Collapsed;
                    TrialScreenVisibility = System.Windows.Visibility.Visible;
                    break;
                case Screen.Info:
                    CardVisibility = System.Windows.Visibility.Collapsed;
                    DoneVisibility = System.Windows.Visibility.Collapsed;
                    InfoGridVisibility = System.Windows.Visibility.Visible;
                    StatsVisibility = System.Windows.Visibility.Collapsed;
                    TrialScreenVisibility = System.Windows.Visibility.Collapsed;
                    break;
                case Screen.Done:
                    CardVisibility = System.Windows.Visibility.Collapsed;
                    DoneVisibility = System.Windows.Visibility.Visible;
                    InfoGridVisibility = System.Windows.Visibility.Collapsed;
                    StatsVisibility = System.Windows.Visibility.Collapsed;
                    TrialScreenVisibility = System.Windows.Visibility.Collapsed;
                    break;
            }
        }

        #region ticking clock

        public delegate void RaiseClockTick(object sender, EventArgs e);
        public event RaiseClockTick ClockTicked;

        private void ClockTickProcess()
        {
            while (_clockTickShouldBeRunning)
            {
                if (ClockTicked != null)
                {
                    ClockTicked(this, new EventArgs());
                }
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }

        private void MainPage_ClockTicked(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                ElapsedSeconds = ElapsedSeconds + 1;
            }
            );
        }

        private void StartTickingClock()
        {
            _clockTickShouldBeRunning = true;
            Task.Factory.StartNew(() => ClockTickProcess());
        }

        #endregion ticking clock

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

        #region properties

        private ObservableCollection<Card> _shuffledDeck;
        public ObservableCollection<Card> ShuffledDeck
        {
            get { return _shuffledDeck; }
            set { _shuffledDeck = value; RaisePropertyChanged("ShuffledDeck"); }
        }

        private ObservableCollection<Card> _redCards;
        public ObservableCollection<Card> RedCards
        {
            get { return _redCards; }
            set { _redCards = value; RaisePropertyChanged("RedCards"); }
        }

        private ObservableCollection<Card> _blackCards;
        public ObservableCollection<Card> BlackCards
        {
            get { return _blackCards; }
            set { _blackCards = value; RaisePropertyChanged("BlackCards"); }
        }

        private Card _selectedCard;
        public Card SelectedCard
        {
            get { return _selectedCard; }
            set
            {
                _selectedCard = value;
                RaisePropertyChanged("SelectedCard");
            }
        }

        private int _redCardsComplete;
        public int RedCardsComplete
        {
            get { return _redCardsComplete; }
            set { _redCardsComplete = value; RaisePropertyChanged("RedCardsComplete"); }
        }

        private int _blackRepsComplete;
        public int BlackRepsComplete
        {
            get { return _blackRepsComplete; }
            set { _blackRepsComplete = value; RaisePropertyChanged("BlackRepsComplete"); }
        }

        private int _redRepsComplete;
        public int RedRepsComplete
        {
            get { return _redRepsComplete; }
            set { _redRepsComplete = value; RaisePropertyChanged("RedRepsComplete"); }
        }

        private int _totalRepsComplete;
        public int TotalRepsComplete
        {
            get { return _totalRepsComplete; }
            set { _totalRepsComplete = value; RaisePropertyChanged("TotalRepsComplete"); }
        }

        private int _blackCardsComplete;
        public int BlackCardsComplete
        {
            get { return _blackCardsComplete; }
            set { _blackCardsComplete = value; RaisePropertyChanged("BlackCardsComplete"); }
        }

        private int _totalCardsComplete;
        public int TotalCardsComplete
        {
            get { return _totalCardsComplete; }
            set { _totalCardsComplete = value; RaisePropertyChanged("TotalCardsComplete"); }
        }

        private int _elapsedSeconds;
        public int ElapsedSeconds
        {
            get { return _elapsedSeconds; }
            set { _elapsedSeconds = value; RaisePropertyChanged("ElapsedSeconds"); }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }

        private Visibility _doneVisibility;
        public Visibility DoneVisibility
        {
            get { return _doneVisibility; }
            set { _doneVisibility = value; RaisePropertyChanged("DoneVisibility"); }
        }

        private Visibility _cardVisibility;
        public Visibility CardVisibility
        {
            get { return _cardVisibility; }
            set { _cardVisibility = value; ; RaisePropertyChanged("CardVisibility"); }
        }

        private Visibility _trialScreenVisibility;
        public Visibility TrialScreenVisibility
        {
            get { return _trialScreenVisibility; }
            set { _trialScreenVisibility = value; RaisePropertyChanged("TrialScreenVisibility"); }
        }
        

        private Visibility _infoGridVisibility;
        public Visibility InfoGridVisibility
        {
            get { return _infoGridVisibility; }
            set { _infoGridVisibility = value; ; RaisePropertyChanged("InfoGridVisibility"); }
        }

        private Visibility _statsVisibility;
        public Visibility StatsVisibility
        {
            get { return _statsVisibility; }
            set { _statsVisibility = value; ; RaisePropertyChanged("StatsVisibility"); }
        }

        private Visibility _appBarVisibility;
        public Visibility AppBarVisibility
        {
            get { return _appBarVisibility; }
            set { _appBarVisibility = value; ; RaisePropertyChanged("AppBarVisibility"); }
        }

        #endregion properties

        private void ShuffleDeck()
        {
            Random random = new Random();
            int first = random.Next(1, 3);

            ShuffledDeck.Clear();

            for (int i = 0; i < 26; i++)
            {
                try
                {
                    List<int> listOfRedCardNumbers = new List<int>();
                    List<int> listOfBlackNumbers = new List<int>();
                    Card redCardToAdd = new Card();
                    Card blackCardToAdd = new Card();

                    foreach (var row in ShuffledDeck)
                    {
                        if (row.Color == CardColor.Black)
                        {
                            listOfBlackNumbers.Add(row.CardKey);
                        }
                        else
                        {
                            listOfRedCardNumbers.Add(row.CardKey);
                        }
                    }

                    //red
                    do
                    {
                        redCardToAdd = GetRandomCard(CardColor.Red);

                    }
                    while (listOfRedCardNumbers.Contains(redCardToAdd.CardKey));

                    //black
                    do
                    {
                        blackCardToAdd = GetRandomCard(CardColor.Black);

                    }
                    while (listOfBlackNumbers.Contains(blackCardToAdd.CardKey));

                    if (first == 1)
                    {
                        ShuffledDeck.Add(redCardToAdd);
                        ShuffledDeck.Add(blackCardToAdd);
                    }
                    else
                    {
                        ShuffledDeck.Add(blackCardToAdd);
                        ShuffledDeck.Add(redCardToAdd);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        Random _random = new Random();

        private Card GetRandomCard(CardColor color)
        {
            Card returnValue = new Card();
            int randomNumber = _random.Next(0, 26);
            Console.WriteLine(randomNumber);
            try
            {
                if (color == CardColor.Red)
                {
                    returnValue = RedCards[randomNumber];
                }
                else
                {
                    returnValue = BlackCards[randomNumber];
                }
            }
            catch (Exception ex)
            {
            }


            return returnValue;
        }

        private void PivotChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            int panoramaIndex = (int)CardPivot.SelectedIndex;
            ReloadTotals(panoramaIndex);
        }

        private void ReloadTotals(int startingCard)
        {
            var totalCompletedCards = from x in ShuffledDeck
                                      where x.Completed
                                      select x;

            int totalComplete = totalCompletedCards.Count();

            if (totalComplete == 52)
            {
                GoToScreen(Screen.Done);
                lock (_lockObject)
                {
                    _clockTickShouldBeRunning = false;
                }
            }
            else
            {
                foreach (var row in ShuffledDeck)
                {
                    row.Completed = false;
                }

                for (int i = 0; i <= startingCard; i++)
                {
                    ShuffledDeck[i].Completed = true;
                }

                var redCardsCompleted = from x in ShuffledDeck
                                        where x.Completed && x.Color == CardColor.Red
                                        select x;

                var blackCardsCompleted = from x in ShuffledDeck
                                          where x.Completed && x.Color == CardColor.Black
                                          select x;

                RedRepsComplete = redCardsCompleted.Select(c => c.NumberOfReps).Sum();
                BlackRepsComplete = blackCardsCompleted.Select(c => c.NumberOfReps).Sum();

                RedCardsComplete = redCardsCompleted.ToList().Count();
                BlackCardsComplete = blackCardsCompleted.ToList().Count();

                TotalRepsComplete = RedRepsComplete + BlackRepsComplete;
                TotalCardsComplete = RedCardsComplete + BlackCardsComplete;
            }
        }

        public void Dispose()
        {
            lock (_lockObject)
            {
                _clockTickShouldBeRunning = false;
            }
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
            ClockTicked -= MainPage_ClockTicked;
        }

        private void ResetClickedHandler(object sender, EventArgs e)
        {
            GoToScreen(Screen.Main);

            foreach (var card in ShuffledDeck)
            {
                card.Completed = false;
            }
            CardPivot.SelectedIndex = 0;
            lock (_lockObject)
            {
                ElapsedSeconds = 0;

                if (!_clockTickShouldBeRunning)
                {
                    _clockTickShouldBeRunning = true;
                    StartTickingClock();
                }
            }
            ReloadTotals(0);
        }

        private bool _aboutHasBeenClicked;
        private void AboutClicked(object sender, EventArgs e)
        {
            _aboutHasBeenClicked = true;
            GoToScreen(Screen.Info);
            ApplicationBar.IsVisible = false;
        }

        private void BackKeyPressed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_aboutHasBeenClicked)
            {
                if (!Trial.IsTrialExpired())
                {
                    GoToScreen(Screen.Main);
                    _aboutHasBeenClicked = false;
                    ApplicationBar.IsVisible = true;
                }
                else
                {
                    GoToScreen(Screen.Trial);
                    _aboutHasBeenClicked = false;
                }
                e.Cancel = true;
            }
        }

        private void PauseClickedHandler(object sender, EventArgs e)
        {
            _clockTickShouldBeRunning = false;
        }

        private void PlayClickedHandler(object sender, EventArgs e)
        {
            if (!_clockTickShouldBeRunning)
            {
                _clockTickShouldBeRunning = true;
                StartTickingClock();
            }
        }

        public void AppHasComeBackToLife()
        {
            if ((Application.Current as App).IsTrial)
            {
                if (Trial.IsTrialExpired())
                {
                    GoToScreen(Screen.Trial);
                }
            }
            else
            {
                GoToScreen(Screen.Main);
            }
        }
  
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/refresh.png", UriKind.Relative));
            appBarButton.Text = AppResources.Reset;
            ApplicationBar.Buttons.Add(appBarButton);
            appBarButton.Click += new EventHandler(ResetClickedHandler);

            ApplicationBarIconButton appBarButton2 = new ApplicationBarIconButton(new Uri("/Assets/AppBar/transport.pause.png", UriKind.Relative));
            appBarButton2.Text = AppResources.PauseClock;
            ApplicationBar.Buttons.Add(appBarButton2);
            appBarButton2.Click += new EventHandler(PauseClickedHandler);

            ApplicationBarIconButton appBarButton3 = new ApplicationBarIconButton(new Uri("/Assets/AppBar/transport.play.png", UriKind.Relative));
            appBarButton3.Text = AppResources.StartClock;
            ApplicationBar.Buttons.Add(appBarButton3);
            appBarButton3.Click += new EventHandler(PlayClickedHandler);

            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.About);
            ApplicationBar.MenuItems.Add(appBarMenuItem);
            appBarMenuItem.Click += new EventHandler(AboutClicked);
        }

    }
}
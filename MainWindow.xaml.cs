using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PomoImmerse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> 
    public partial class MainWindow : Window
    {
        public static int MainInterval
        {
            get => _mainInterval;
            set
            {
                if (_nextInterval == MainInterval) _nextInterval = value;
                _mainInterval = value;
            }
        }

        public static int BreakInterval
        {
            get => _breakInterval;
            set
            {
                if (_nextInterval == BreakInterval) _nextInterval = value;
                _breakInterval = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _backgroundLink = "Assets/background.jpg";
        public string BackgroundLink
        {
            get => _backgroundLink;
            set
            {
                if (_backgroundLink == value) return;
                _backgroundLink = value;
                OnPropertyChanged();
                UpdateBackgroundImage();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private static int _mainInterval = 25;
        private static int _breakInterval = 5;
        private bool _timerInit;
        private TimeSpan _countdown;
        private static int _nextInterval;
        private int _lastWholeSeconds = int.MaxValue;
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly Stopwatch _sw = new Stopwatch();


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            PomoTime.Content = $"{MainInterval}:00";
            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Tick += OnTick;
            _nextInterval = BreakInterval;
        }

        private void PauseTimer()
        {
            _sw.Stop();
            _timer.IsEnabled = false;
            _countdown -= _sw.Elapsed;
            StartText.Text = "Resume";
        }

        private void UpdateBackgroundImage()
        {
            try
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = new Uri(_backgroundLink, UriKind.RelativeOrAbsolute);
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.CreateOptions = BitmapCreateOptions.None;
                bmp.EndInit();

                Background = new ImageBrush(bmp)
                {
                    Stretch = Stretch.UniformToFill
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Failed to update background: " + ex);
                // optional: keep old background or fall back to a default
            }
        }

        private void ResumeTimer()
        {
            _sw.Restart();
            _timer.IsEnabled = true;
            StartText.Text = "Pause";
        }

        private void StartTimer(int length)
        {
            _countdown = TimeSpan.FromMinutes(length);
            _nextInterval = (length == BreakInterval) ? MainInterval : BreakInterval;
            _lastWholeSeconds = int.MaxValue;
            _sw.Restart();



            if (!_timer.IsEnabled) _timer.Start();
            StartText.Text = "Pause";
            _timerInit = true;
            PomoTime.Content = _countdown.ToString("mm\\:ss");
        }
        private void OnTick(object? sender, EventArgs e)
        {
            var currTime = _countdown - _sw.Elapsed;
            if (currTime <= TimeSpan.Zero)
            {
                PomoTime.Content = "00:00";
                StartTimer(_nextInterval);
                return;
            }

            var whole = (int)Math.Ceiling(currTime.TotalSeconds);
            if (whole != _lastWholeSeconds)
            {
                _lastWholeSeconds = whole;
                // format from whole seconds to mm:ss
                var mmss = TimeSpan.FromSeconds(whole).ToString("mm\\:ss");
                PomoTime.Content = mmss;
            }
        }

        public void ResetTimer(bool onlySegment = false)
        {
            var interval = MainInterval;
            if (onlySegment && _nextInterval == MainInterval)
                interval = BreakInterval;
            else _nextInterval = BreakInterval;
            _timer.IsEnabled = false;
            StartText.Text = "Start";
            PomoTime.Content = $"{interval}:00";
            _countdown = TimeSpan.FromMinutes(interval);
        }

        public void SkipInterval()
        {
            _timerInit = true;
            _timer.IsEnabled = false;
            StartText.Text = "Start";
            PomoTime.Content = $"{_nextInterval}:00";
            _countdown = TimeSpan.FromMinutes(_nextInterval);
            _nextInterval = _nextInterval == MainInterval ? BreakInterval : MainInterval;
        }

        public void ChangeBackground()
        {

        }

        private void StartBtnPress()
        {
            if (!_timerInit) StartTimer(MainInterval);
            else if (_timer.IsEnabled) PauseTimer();
            else ResumeTimer();
        }
        private void StartBtn_OnClick(object sender, RoutedEventArgs e)
        {
            StartBtnPress();
        }

        private void ResetBtn_OnClick(object sender, RoutedEventArgs e)
        {
            ResetPopup.IsOpen = true;
            GreyOutBox.Visibility = Visibility.Visible;
            PauseTimer();
        }

        public void CloseResetPopup()
        {
            ResetPopup.IsOpen = false;
            GreyOutBox.Visibility = Visibility.Collapsed;
        }
        public void CloseInfoPopup()
        {
            InfoPopup.IsOpen = false;
            GreyOutBox.Visibility = Visibility.Collapsed;
        }
        public void CloseSkipPopup()
        {
            SkipPopup.IsOpen = false;
            GreyOutBox.Visibility = Visibility.Collapsed;
        }

        public void CloseSettingsPopup()
        {
            SettingsPopup.IsOpen = false;
            GreyOutBox.Visibility = Visibility.Collapsed;
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                StartBtnPress();
            }
        }

        private void InfoBtn_OnClick(object sender, RoutedEventArgs e)
        {
            InfoPopup.IsOpen = true;
            GreyOutBox.Visibility = Visibility.Visible;
        }

        private void SkipBtn_OnClick(object sender, RoutedEventArgs e)
        {
            SkipPopup.IsOpen = true;
            GreyOutBox.Visibility = Visibility.Visible;
            PauseTimer();
        }

        private void SettingsBtn_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsPopup.IsOpen = true;
            GreyOutBox.Visibility = Visibility.Visible;
        }
    }
}
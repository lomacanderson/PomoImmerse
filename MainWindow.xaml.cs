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
        private bool _timerInit;
        private TimeSpan _countdown;
        private DateTime _startTime;
        private readonly int _mainInterval = 1;
        private readonly int _breakInterval = 5;
        private int _nextInterval;
        private DispatcherTimer _timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            PomoTime.Content = $"{_mainInterval}:00";
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += OnTick;
        }

        private void PauseTimer()
        {
            _timer.IsEnabled = false;
            _countdown = (_countdown - (DateTime.Now - _startTime));
            StartText.Text = "Resume";
        }

        private void ResumeTimer()
        {
            _timer.IsEnabled = true;
            _startTime = DateTime.Now;
            StartText.Text = "Pause";
        }

        private void StartTimer(int length)
        {
            _startTime = DateTime.Now;
            _countdown = TimeSpan.FromMinutes(length);
            _nextInterval = (length == _breakInterval) ? _mainInterval : _breakInterval;


            if (!_timer.IsEnabled) _timer.Start();
            StartText.Text = "Pause";
            _timerInit = true;
            PomoTime.Content = _countdown.ToString("mm\\:ss");
        }
        private void OnTick(object? sender, EventArgs e)
        {
            var currTime = _countdown - (DateTime.Now - _startTime);
            if (currTime <= TimeSpan.Zero)
            {
                PomoTime.Content = "00:00";
                StartTimer(_nextInterval);
                return;
            }

            PomoTime.Content = currTime.ToString("mm\\:ss");
        }
        private void StartBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_timerInit) StartTimer(_mainInterval);
            else if (_timer.IsEnabled) PauseTimer();
            else ResumeTimer();
        }

        private void ResetBtn_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.IsEnabled = false;
            StartText.Text = "Start";
            _timerInit = false;
            PomoTime.Content = $"{_mainInterval}:00";
            _startTime = DateTime.Now;
            _countdown = TimeSpan.FromMinutes(_mainInterval);
        }
    }
}
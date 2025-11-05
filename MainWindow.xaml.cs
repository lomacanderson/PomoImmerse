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
        public DispatcherTimer Timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void PauseTimer()
        {
            Timer.IsEnabled = false;
            _countdown = (_countdown - (DateTime.Now - _startTime));
            StartText.Text = "Resume";
        }

        private void ResumeTimer()
        {
            Timer.IsEnabled = true;
            _startTime = DateTime.Now;
            StartText.Text = "Pause";
        }

        private void StartTimer(int length)
        {
            Timer = new DispatcherTimer();
            _startTime = DateTime.Now;
            _countdown = TimeSpan.FromMinutes(length);
            var nextInterval = 15;
            if (length == 15) nextInterval = 30;


                Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += (obj, args) =>
            {
                var currTime = (_countdown - (DateTime.Now - _startTime));
                PomoTime.Content = currTime.ToString("mm\\:ss");
                if (currTime <= TimeSpan.Zero) StartTimer(nextInterval);
            };
            StartText.Text = "Pause";
            Timer.IsEnabled = true;
            _timerInit = true;
        }
        private void StartBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_timerInit) StartTimer(30);
            else if (Timer.IsEnabled) PauseTimer();
            else ResumeTimer();
        }

        private void ResetBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Timer.IsEnabled = false;
            PomoTime.Content = "30:00";
            _startTime = DateTime.Now;
            _countdown = TimeSpan.FromMinutes(30);
        }
    }
}
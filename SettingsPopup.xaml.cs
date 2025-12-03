using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PomoImmerse
{
    /// <summary>
    /// Interaction logic for SettingsPopup.xaml
    /// </summary>
    public partial class SettingsPopup : UserControl
    {
        private int MainInterval => MainWindow.MainInterval;
        private int BreakInterval => MainWindow.BreakInterval;
        public SettingsPopup()
        {
            InitializeComponent();
            MainIntervalBox.Text = MainInterval.ToString();
            BreakIntervalBox.Text = BreakInterval.ToString();
        }

        private void ExitBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current?.MainWindow is MainWindow mw)
            {
                mw.Dispatcher.Invoke(() => mw.CloseSettingsPopup());
            }
        }

        private void MainIntervalBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MainIntervalBox.Text, out _))
            {
                MainWindow.MainInterval = int.Parse(MainIntervalBox.Text);
            }
            else
            {
                MainIntervalBox.Text = MainWindow.MainInterval.ToString();
            }
        }

        private void BreakIntervalBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(BreakIntervalBox.Text, out _))
            {
                MainWindow.BreakInterval = int.Parse(BreakIntervalBox.Text);
            }
            else
            {
                BreakIntervalBox.Text = MainWindow.BreakInterval.ToString();
            }
        }

        private void ImageQueryBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}

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
    /// Interaction logic for ResetPopup.xaml
    /// </summary>
    public partial class ResetPopup : UserControl
    {
        public ResetPopup()
        {
            InitializeComponent();
        }

        private void ResetSegmentBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current?.MainWindow is MainWindow mw)
            {
                mw.Dispatcher.Invoke(() => mw.ResetTimer(true));
                mw.Dispatcher.Invoke(() => mw.CloseResetPopup());
            }
        }

        private void ResetSessionBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current?.MainWindow is MainWindow mw)
            {
                mw.Dispatcher.Invoke(() => mw.ResetTimer());
                mw.Dispatcher.Invoke(() => mw.CloseResetPopup());
            }
        }

        private void ExitBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current?.MainWindow is MainWindow mw)
            {
                mw.Dispatcher.Invoke(() => mw.CloseResetPopup());
            }
        }
    }
}

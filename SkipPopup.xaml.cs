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
    /// Interaction logic for SkipPopup.xaml
    /// </summary>
    public partial class SkipPopup : UserControl
    {
        public SkipPopup()
        {
            InitializeComponent();
        }

        private void SkipSegmentBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current?.MainWindow is MainWindow mw)
            {
                mw.Dispatcher.Invoke(() => mw.SkipInterval());
                mw.Dispatcher.Invoke(() => mw.CloseSkipPopup());
            }
        }

        private void ExitBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current?.MainWindow is MainWindow mw)
            {
                mw.Dispatcher.Invoke(() => mw.CloseSkipPopup());
            }
        }
    }
}

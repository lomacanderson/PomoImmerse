using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Net.Http;
using System.Text.Json;
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

        private async void ImageQueryBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var query = ImageQueryBox.Text;
            if (string.IsNullOrWhiteSpace(query))
                return;

            try
            {
                await GetImage(query);
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine("HTTP ERROR:");
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                MessageBox.Show("Could not contact image service. Check that it is running.",
                                "Network error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("UNEXPECTED ERROR:");
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        async Task GetImage(String query)
        {
            using var client = new HttpClient();

            var url = "http://localhost:8000/images";

            var response = await client.GetAsync($"{url}?query={query}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                using JsonDocument doc = JsonDocument.Parse(jsonString);

                string imageLink = doc.RootElement
                    .GetProperty("images")[0]
                    .GetProperty("url")
                    .GetString();

                var main = (MainWindow)Application.Current.MainWindow;
                main.Dispatcher.Invoke(() =>
                {
                    main.BackgroundLink = imageLink;
                });

                Console.WriteLine($"Received image");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        
    }
}

using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace TourAgency
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Tour> _tours = new ObservableCollection<Tour>();

        public MainWindow()
        {
            InitializeComponent();
            ToursDataGrid.ItemsSource = _tours; // Прив'язуємо один раз!
            ShowAllTours();
        }

        // Додає новий тур
        private void AddTour_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                if (DaysInput.Text != null && TourNameInput.Text != null && CountryInput.Text != null && DepartureDateInput.SelectedDate != null && CostInput.Text != null)
                {
                    var tour = new Tour
                    {
                        TourName = TourNameInput.Text,
                        Country = CountryInput.Text,
                        DepartureDate = DepartureDateInput.SelectedDate?.DateTime ?? DateTime.Now,
                        NumberOfDays = int.Parse(DaysInput.Text),
                        Cost = decimal.Parse(CostInput.Text),
                        HasNightTransfers = NightTransfersInput.IsChecked ?? false
                    };

                    _tours.Add(tour);

                    // Зберігаємо у файл усі тури
                    FileHandler.WriteTours(_tours.ToList());
                }

                MessageBox("Тур успішно додано!");
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox($"Помилка: {ex.Message}");
            }
        }

        // Показати всі тури (оновлює список і DataGrid)
        private void ShowAllTours()
        {
            _tours.Clear();
            foreach (var tour in FileHandler.ReadTours())
                _tours.Add(tour);

            DisplayToursInTextBox(_tours);
        }

        // Кнопка "Показати всі тури"
        private void ViewAllTours_Click(object? sender, RoutedEventArgs e)
        {
            ShowAllTours();
        }

        // Кнопка "Показати тури до Чехії без нічних переїздів"
        private void FilterTours_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                var filtered = FileHandler.ReadTours()
                    .Where(t =>
                        t.Country.Equals("Чехія", StringComparison.OrdinalIgnoreCase) &&
                        !t.HasNightTransfers)
                    .ToList();

                _tours.Clear();
                foreach (var tour in filtered)
                    _tours.Add(tour);

                DisplayToursInTextBox(filtered);
            }
            catch (Exception ex)
            {
                MessageBox($"Помилка: {ex.Message}");
            }
        }

        // Очищення полів введення
        private void ClearInputFields()
        {
            TourNameInput.Text = string.Empty;
            CountryInput.Text = string.Empty;
            DepartureDateInput.SelectedDate = null;
            DaysInput.Text = string.Empty;
            CostInput.Text = string.Empty;
            NightTransfersInput.IsChecked = false;
        }

        // Відображення повідомлення з кнопкою Закрити
        private void MessageBox(string message)
        {
            var msgBox = new Window
            {
                Width = 300,
                Height = 180,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var panel = new StackPanel
            {
                Margin = new Thickness(10)
            };

            var text = new TextBlock
            {
                Text = message,
                Margin = new Thickness(0, 0, 0, 15)
            };

            var closeButton = new Button
            {
                Content = "Закрити",
                Width = 80,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            closeButton.Click += (_, __) => msgBox.Close();

            panel.Children.Add(text);
            panel.Children.Add(closeButton);

            msgBox.Content = panel;
            msgBox.ShowDialog(this);
        }

        // Вивід турів у текстовий бокс
        private void DisplayToursInTextBox(IEnumerable<Tour> tours)
        {
            if (this.FindControl<TextBox>("ToursTextBox") is { } tb)
            {
                tb.Text = string.Join(Environment.NewLine, tours.Select(t =>
                    $"Назва: {t.TourName}, Країна: {t.Country}, Відправлення: {t.DepartureDate:dd.MM.yyyy}, Днів: {t.NumberOfDays}, Вартість: {t.Cost}, Нічні переїзди: {(t.HasNightTransfers ? "Так" : "Ні")}"));
            }
        }
    }
}
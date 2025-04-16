using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TourAgency
{
    public partial class MainWindow : Window
    {
        private List<Tour> _tours = new List<Tour>();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Додавання нового туру
        private void AddTour_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(DaysInput.Text) &&
                    !string.IsNullOrWhiteSpace(TourNameInput.Text) &&
                    !string.IsNullOrWhiteSpace(CountryInput.Text) &&
                    DepartureDateInput.SelectedDate != null &&
                    !string.IsNullOrWhiteSpace(CostInput.Text))
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
                    FileHandler.WriteTours(_tours);

                    // Оновлення таблиці після додавання
                    ViewAllTours_Click(null, null);

                    MessageBox("Тур успішно додано!");
                    ClearInputFields();
                }
                else
                {
                    MessageBox("Будь ласка, заповніть усі поля!");
                }
            }
            catch (Exception ex)
            {
                MessageBox($"Помилка: {ex.Message}");
            }
        }

        // Перегляд усіх турів
        private void ViewAllTours_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                _tours = FileHandler.ReadTours();

                if (_tours == null || !_tours.Any())
                {
                    MessageBox("Список турів порожній або не вдалося завантажити дані.");
                    return;
                }

                ToursDataGrid.ItemsSource = null;
                ToursDataGrid.ItemsSource = _tours;

                DisplayToursInTextBox(_tours);
            }
            catch (Exception ex)
            {
                MessageBox($"Помилка: {ex.Message}");
            }
        }

        // Фільтрація турів (Чехія, без нічних переїздів)
        private void FilterTours_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                var filteredTours = _tours.FindAll(t =>
                    t.Country.Equals("Чехія", StringComparison.OrdinalIgnoreCase) &&
                    !t.HasNightTransfers);

                ToursDataGrid.ItemsSource = null;
                ToursDataGrid.ItemsSource = filteredTours;

                DisplayToursInTextBox(filteredTours);
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
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;

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
                }

                FileHandler.WriteTours(_tours);

                MessageBox("Тур успішно додано!");
                ClearInputFields();
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
                ToursDataGrid.ItemsSource = _tours;
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

                ToursDataGrid.ItemsSource = filteredTours;
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

        // Відображення повідомлення
        private void MessageBox(string message)
        {
            var msgBox = new Window
            {
                Content = new TextBlock { Text = message, Margin = new Avalonia.Thickness(10) },
                Width = 300,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            msgBox.ShowDialog(this);
        }
    }
}
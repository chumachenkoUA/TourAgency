using System;
using System.Collections.Generic;
using System.IO;

namespace TourAgency
{
    public static class FileHandler
    {
        private const string FileName = "tours.bin";

        public static void WriteTours(List<Tour> tours)
        {
            // Перевірка існування файлу. Якщо файл не існує, створюємо його.
            if (!File.Exists(FileName))
            {
                InitializeFileWithDefaultTours();
            }

            // Пишемо дані у файл
            using (var writer = new BinaryWriter(File.Open(FileName, FileMode.Create)))
            {
                writer.Write(tours.Count);
                foreach (var tour in tours)
                {
                    writer.Write(tour.TourName);
                    writer.Write(tour.Country);
                    writer.Write(tour.DepartureDate.ToBinary());
                    writer.Write(tour.NumberOfDays);
                    writer.Write(tour.Cost);
                    writer.Write(tour.HasNightTransfers);
                }
            }
        }

        public static List<Tour> ReadTours()
        {
            // Якщо файл не існує, створюємо його з початковими турами
            if (!File.Exists(FileName))
            {
                InitializeFileWithDefaultTours();
            }

            var tours = new List<Tour>();
            using (var reader = new BinaryReader(File.Open(FileName, FileMode.Open)))
            {
                var count = reader.ReadInt32();
                for (var i = 0; i < count; i++)
                {
                    tours.Add(new Tour
                    {
                        TourName = reader.ReadString(),
                        Country = reader.ReadString(),
                        DepartureDate = DateTime.FromBinary(reader.ReadInt64()),
                        NumberOfDays = reader.ReadInt32(),
                        Cost = reader.ReadDecimal(),
                        HasNightTransfers = reader.ReadBoolean()
                    });
                }
            }

            return tours;
        }

        private static void InitializeFileWithDefaultTours()
        {
            var defaultTours = new List<Tour>
            {
                new Tour
                {
                    TourName = "Прага вихідного дня",
                    Country = "Чехія",
                    DepartureDate = new DateTime(2025, 5, 15),
                    NumberOfDays = 4,
                    Cost = 300.00m,
                    HasNightTransfers = false
                },
                new Tour
                {
                    TourName = "Екскурсійна Європа",
                    Country = "Чехія",
                    DepartureDate = new DateTime(2025, 6, 20),
                    NumberOfDays = 7,
                    Cost = 550.00m,
                    HasNightTransfers = true
                },
                new Tour
                {
                    TourName = "Відень і Прага",
                    Country = "Австрія",
                    DepartureDate = new DateTime(2025, 7, 10),
                    NumberOfDays = 5,
                    Cost = 400.00m,
                    HasNightTransfers = false
                },
                new Tour
                {
                    TourName = "Смак Чехії",
                    Country = "Чехія",
                    DepartureDate = new DateTime(2025, 8, 1),
                    NumberOfDays = 3,
                    Cost = 250.00m,
                    HasNightTransfers = false
                }
            };

            using (var writer = new BinaryWriter(File.Open(FileName, FileMode.Create)))
            {
                writer.Write(defaultTours.Count);
                foreach (var tour in defaultTours)
                {
                    writer.Write(tour.TourName);
                    writer.Write(tour.Country);
                    writer.Write(tour.DepartureDate.ToBinary());
                    writer.Write(tour.NumberOfDays);
                    writer.Write(tour.Cost);
                    writer.Write(tour.HasNightTransfers);
                }
            }
        }
    }
}
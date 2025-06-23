using CourseA4.Helpers;
using CourseA4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CourseA4.Presenters
{
    public class StarVisibilityPresenter // клас реалізований як презентер для функціоналу розрахунку видимості зірок
    {
        private readonly StarRepository _starRepository; // екземпляр StarRepository для доступу до всіх зірок.
        private readonly NumericUpDown _numericUpDownLatitude;
        private readonly NumericUpDown _numericUpDownLongitude; // посилання на UI-елементи для отримання координат та часу спостереження
        private readonly DateTimePicker _dateTimePickerObservation;
        private readonly DataGridView _dataGridViewVisibility;

        public StarVisibilityPresenter(StarRepository starRepository,
                                       NumericUpDown numericUpDownLatitude, NumericUpDown numericUpDownLongitude,
                                       DateTimePicker dateTimePickerObservation, DataGridView dataGridViewVisibility)
        {                                   // ініціалізує всі поля посиланнями на репозиторій та елементи UI
            _starRepository = starRepository; 
            _numericUpDownLatitude = numericUpDownLatitude;
            _numericUpDownLongitude = numericUpDownLongitude;
            _dateTimePickerObservation = dateTimePickerObservation;
            _dataGridViewVisibility = dataGridViewVisibility;
        }

        public void CalculateAndDisplayVisibility() // отримує значення широти, довготи та часу спостереження з відповідних UI-елементів
        {
            double latitude = (double)_numericUpDownLatitude.Value;
            double longitude = (double)_numericUpDownLongitude.Value;
            DateTime observationTime = _dateTimePickerObservation.Value;

            _dataGridViewVisibility.Rows.Clear(); // очищаємо _dataGridViewVisibility перед заповненням новими результатами

            HashSet<string> visibleConstellations = new HashSet<string>();
            // збір видимих сузір'їв, ініціалізація для зберігання унікальних назв видимих сузір'їв. HashSet автоматично гарантує унікальність

            foreach (var star in _starRepository.Stars) // перебирає кожну зірку з
            {
                try
                {
                    // Вызов метода расчета из StarCalculator
                    var (altitude, azimuth, isVisible) = StarCalculator.CalculateStarPosition(star, latitude, longitude, observationTime); // для кожної зірки викликає функцію, передаючи зірку, координати спостерігача та час.

                    if (isVisible)
                    {
                        _dataGridViewVisibility.Rows.Add(
                            star.Name,
                            star.Constellation,
                            star.Magnitude,
                            Math.Round(altitude, 2),
                            Math.Round(azimuth, 2),
                            star.Color,
                            "Видима"
                        );
                        visibleConstellations.Add(star.Constellation);
                    }
                }
                catch (FormatException ex)
                {
                    // якщо виникла ошибка парсингу самих координат, пропустимо цю зірку
                    Console.WriteLine($"Ошибка парсинга координат для звезды '{star.Name}': {ex.Message}");
                }
                catch (Exception ex)
                {
                    // логуємо другі помилки
                    Console.WriteLine($"Ошибка при расчете видимости для звезды '{star.Name}': {ex.Message}");
                }
            }

            string message = "Видимі сузір'я: " + (visibleConstellations.Any() ? string.Join(", ", visibleConstellations.OrderBy(c => c)) : "Немає видимих сузір'їв.");
            MessageBox.Show(message, "Видимі сузір'я", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //Після обробки всіх зірок, створює повідомлення MessageBox.Show, що перераховує всі унікальні видимі сузір'я
            // (відсортовані за алфавітом), якщо видимих сузір'їв немає, виводить відповідне повідомлення
        }
    }
}
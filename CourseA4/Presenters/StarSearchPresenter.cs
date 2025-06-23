using CourseA4.Helpers;
using CourseA4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CourseA4.Presenters
{
    public class StarSearchPresenter // цей клас є "презентером" для вкладки "Пошук зірок" у головній формі,
                              // він містить логіку, яка відповідає за обробку запитів пошуку та оновлення інтерфейсу користувача
    {
        // поля
        private readonly StarRepository _starRepository; // екземпляр StarRepository для доступу до даних про зірки
        private readonly ComboBox _comboBoxConstellation;
        private readonly TextBox _textBoxStarName;
        private readonly NumericUpDown _numericUpDownBrightestCount;  // посилання на відповідні елементи керування UI з MainForm'у
        private readonly DataGridView _dataGridViewResults;

        public StarSearchPresenter(StarRepository starRepository, ComboBox comboBoxConstellation,
                                   TextBox textBoxStarName, NumericUpDown numericUpDownBrightestCount,
                                   DataGridView dataGridViewResults) // конструктор ініціалізує поля посиланнями на репозиторій та елементи UI
        {
            _starRepository = starRepository;
            _comboBoxConstellation = comboBoxConstellation;
            _textBoxStarName = textBoxStarName;
            _numericUpDownBrightestCount = numericUpDownBrightestCount;
            _dataGridViewResults = dataGridViewResults;
        }


        public void FillConstellationComboBox() // метод отримує унікальні сузір'я з _comboBoxConstellation
                                      // сортує їх та заповнює _comboBoxConstellation, додаючи також опцію "Всі сузір'я"
        {
            var constellations = _starRepository.Stars.Select(s => s.Constellation).Distinct().OrderBy(c => c).ToList();
            _comboBoxConstellation.Items.Clear();
            _comboBoxConstellation.Items.Add("Всі сузір'я");
            _comboBoxConstellation.Items.AddRange(constellations.ToArray());
            _comboBoxConstellation.SelectedIndex = 0;
        }

        public void PopulateInitialData()
        {
            StarDisplayHelper.DisplayStarsInGrid(_starRepository.Stars.ToList(), _dataGridViewResults);
        }

        public void SearchByConstellation() // метод отримує обране сузір'я з _comboBoxConstellation
                            // фільтрує зірки з _starRepository за вибраним сузір'ям (або показує всі, якщо вибрано "Всі сузір'я").
        {
            string selectedConstellation = _comboBoxConstellation.SelectedItem.ToString();
            List<Star> filteredStars;

            if (selectedConstellation == "Всі сузір'я")
            {
                filteredStars = _starRepository.Stars.ToList();
            }
            else
            {
                filteredStars = _starRepository.Stars.Where(s => s.Constellation == selectedConstellation).ToList();
            }

            StarDisplayHelper.DisplayStarsInGrid(filteredStars, _dataGridViewResults);
        }

        public void SearchByName() // метод отримує текст з _textBoxStarName, фільтрує зірки, назви яких містять введений текст(без урахування регістру)
                                   // відображає результати та виводить повідомлення, якщо зірок не знайдено
        {
            string starName = _textBoxStarName.Text.Trim();
            if (string.IsNullOrEmpty(starName))
            {
                MessageBox.Show("Введіть назву зірки для пошуку", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var filteredStars = _starRepository.Stars.Where(s => s.Name.ToLower().Contains(starName.ToLower())).ToList();
            StarDisplayHelper.DisplayStarsInGrid(filteredStars, _dataGridViewResults);

            if (filteredStars.Count == 0)
            {
                MessageBox.Show("Зірки з такою назвою не знайдено", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void ShowBrightestStars() // метод отримує кількість зірок з _numericUpDownBrightestCount, сортує зірки за Magnitude (за зростанням) та бере вказану кількість
        {                               // відображає ці найяскравіші зірки (brightestStars)
            int count = (int)_numericUpDownBrightestCount.Value;
            var brightestStars = _starRepository.Stars.OrderBy(s => s.Magnitude).Take(count).ToList();
            StarDisplayHelper.DisplayStarsInGrid(brightestStars, _dataGridViewResults);
        }
    }
}
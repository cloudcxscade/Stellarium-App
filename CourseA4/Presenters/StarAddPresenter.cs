using System;
using System.Windows.Forms;
using System.Linq;
using CourseA4.Models;

namespace CourseA4.Presenters
{
    public class StarAddPresenter
    {
        private readonly StarRepository _starRepository; // екземпляр StarRepository для доступу до даних про зірки та їх збереження
        private readonly TextBox _textBoxAddName;
        private readonly TextBox _textBoxAddConstellation;
        private readonly NumericUpDown _numericUpDownAddMagnitude;
        private readonly NumericUpDown _numericUpDownAddDistance; // посилання на відповідні елементи керування UI з вкладки "Додати зірку" у MainForm
        private readonly TextBox _textBoxAddRightAscension;
        private readonly TextBox _textBoxAddDeclination;
        private readonly ComboBox _comboBoxAddColor;

        public StarAddPresenter(StarRepository starRepository,
                                TextBox textBoxAddName, TextBox textBoxAddConstellation,
                                NumericUpDown numericUpDownAddMagnitude, NumericUpDown numericUpDownAddDistance,
                                TextBox textBoxAddRightAscension, TextBox textBoxAddDeclination,
                                ComboBox comboBoxAddColor) // ініціалізує всі поля посиланнями на репозиторій та елементи UI
        {
            _starRepository = starRepository;
            _textBoxAddName = textBoxAddName;
            _textBoxAddConstellation = textBoxAddConstellation;
            _numericUpDownAddMagnitude = numericUpDownAddMagnitude;
            _numericUpDownAddDistance = numericUpDownAddDistance;
            _textBoxAddRightAscension = textBoxAddRightAscension;
            _textBoxAddDeclination = textBoxAddDeclination;
            _comboBoxAddColor = comboBoxAddColor;
        }

        public void AddStar()
        {
            // перевіряє, чи всі обов'язкові текстові поля
            // (Name, Constellation, RightAscension, Declination) не є порожніми або складаються лише з пробілів, якщо ні, показує попередження

            if (string.IsNullOrEmpty(_textBoxAddName.Text.Trim()) ||
                string.IsNullOrEmpty(_textBoxAddConstellation.Text.Trim()) ||
                string.IsNullOrEmpty(_textBoxAddRightAscension.Text.Trim()) ||
                string.IsNullOrEmpty(_textBoxAddDeclination.Text.Trim()) ||
                _comboBoxAddColor.SelectedItem == null) // перевіряє, чи вибраний елемент у _comboBoxAddColor
            {
                MessageBox.Show("Будь ласка, заповніть усі поля", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // використовує _starRepository.Stars.Any, для перевірки, чи зірка з такою назвою вже існує (без урахування регістру), якщо так, показує помилку
            if (_starRepository.Stars.Any(s => s.Name.Equals(_textBoxAddName.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Зірка з такою назвою вже існує", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // якщо всі валідації пройшли успішно, створюється новий об'єкт Star
            Star newStar = new Star
            {
                Name = _textBoxAddName.Text.Trim(),
                Constellation = _textBoxAddConstellation.Text.Trim(),
                Magnitude = (double)_numericUpDownAddMagnitude.Value,
                Distance = (double)_numericUpDownAddDistance.Value,
                RightAscension = _textBoxAddRightAscension.Text.Trim(),
                Declination = _textBoxAddDeclination.Text.Trim(),
                Color = _comboBoxAddColor.SelectedItem.ToString()
            };

            // додаємо створену newStar до колекції _starRepository.Stars
            _starRepository.Stars.Add(newStar);

            // очищає всі поля введення на формі після успішного додавання зірки, щоб користувач міг додати наступну
            _textBoxAddName.Clear();
            _textBoxAddConstellation.Clear();
            _numericUpDownAddMagnitude.Value = 0;
            _numericUpDownAddDistance.Value = 0;
            _textBoxAddRightAscension.Clear();
            _textBoxAddDeclination.Clear();
            _comboBoxAddColor.SelectedIndex = 0;

            MessageBox.Show("Зірку успішно додано до бази даних", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
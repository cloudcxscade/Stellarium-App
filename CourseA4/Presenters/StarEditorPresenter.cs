using CourseA4.Helpers;
using CourseA4.Models;
using CourseA4.Forms;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CourseA4.Presenters
{
    public class StarEditorPresenter // цей клас є презентером для операцій редагування та видалення зірок
    {
        private readonly StarRepository _starRepository; // поля
        private readonly DataGridView _dataGridViewResults; // для оновлення відображення після видалення/редагування

        public StarEditorPresenter(StarRepository starRepository, DataGridView dataGridViewResults) // ініціалізує поля
        {
            _starRepository = starRepository;
            _dataGridViewResults = dataGridViewResults;
        }


        public void DeleteStar(string starName) // приймає назву зірки для видалення
        {
            if (string.IsNullOrEmpty(starName)) // перевіряємо, чи назва не порожня
            {
                MessageBox.Show("Будь ласка, оберіть зірку для видалення.", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var starToRemove = _starRepository.Stars.FirstOrDefault(s => s.Name.Equals(starName, StringComparison.OrdinalIgnoreCase));
                                // знаходить зірку в _starRepository.Stars

            if (starToRemove != null)
            {
                // виводить діалогове вікно підтвердження для користувача
                DialogResult dialogResult = MessageBox.Show($"Ви впевнені, що хочете видалити зірку '{starName}'?", "Підтвердження видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes) // якщо користувач підтверджує, то викликаємо _starRepository.DeleteStar() та _starRepository.SaveData().
                {
                    _starRepository.Stars.Remove(starToRemove);
                    _starRepository.SaveData(); // зберігаємо зміни
                    MessageBox.Show($"Зірку '{starName}' успішно видалено.", "Видалення", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshDataGridView(); // оновлюємо DataGridView (відображення)
                }
            }
            else
            {
                MessageBox.Show($"Зірку '{starName}' не знайдено.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void EditStar(string starName) // форма для редагування даних зірки
        {
            if (string.IsNullOrEmpty(starName))
            {
                MessageBox.Show("Будь ласка, оберіть зірку для редагування.", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var starToEdit = _starRepository.Stars.FirstOrDefault(s => s.Name.Equals(starName, StringComparison.OrdinalIgnoreCase));

            if (starToEdit != null)
            {
                // відкриваємо модальну форму для редагування
                using (var editForm = new EditStarForm(starToEdit))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        // якщо користувач підтвердив зміни, оновлюємо дані в репозиторії
                        _starRepository.SaveData(); // Зберігаємо зміни
                        MessageBox.Show("Зірку успішно оновлено.", "Редагування", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshDataGridView(); // оновлюємо відображення
                    }
                }
            }
            else
            {
                MessageBox.Show($"Зірку '{starName}' не знайдено.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Оновлює DataGridView з поточними даними зірок.
        /// </summary>
        private void RefreshDataGridView()
        {
            StarDisplayHelper.DisplayStarsInGrid(_starRepository.Stars.ToList(), _dataGridViewResults);
        }
    }
}

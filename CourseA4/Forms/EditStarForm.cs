using System;
using System.Windows.Forms;
using System.Globalization;
using CourseA4.Models;

namespace CourseA4.Forms
{
    public partial class EditStarForm : Form
    {
        private Star _starToEdit;

        // ui
        private Label labelName;
        private TextBox textBoxName;
        private Label labelConstellation;
        private TextBox textBoxConstellation;
        private Label labelMagnitude;
        private NumericUpDown numericUpDownMagnitude;
        private Label labelDistance;
        private NumericUpDown numericUpDownDistance;
        private Label labelRightAscension;
        private TextBox textBoxRightAscension;
        private Label labelDeclination;
        private TextBox textBoxDeclination;
        private Label labelColor;
        private ComboBox comboBoxColor;
        private Button buttonSave;
        private Button buttonCancel;

        public EditStarForm(Star star)
        {
            _starToEdit = star;
            InitializeComponent();
            LoadStarData();
        }

        private void InitializeComponent()
        {
            SuspendLayout();

            Text = $"Редагування зірки: {_starToEdit.Name}";
            Size = new Size(400, 450);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            int yPos = 20;
            int xLabel = 20;
            int xControl = 150;
            int height = 25;
            int spacing = 35;
            int controlWidth = 200;

            // ім'я зірки
            labelName = new Label { Text = "Назва:", Location = new Point(xLabel, yPos), Size = new Size(120, height) };
            textBoxName = new TextBox { Location = new Point(xControl, yPos), Size = new Size(controlWidth, height) };
            yPos += spacing;

            // сузір'я
            labelConstellation = new Label { Text = "Сузір'я:", Location = new Point(xLabel, yPos), Size = new Size(120, height) };
            textBoxConstellation = new TextBox { Location = new Point(xControl, yPos), Size = new Size(controlWidth, height) };
            yPos += spacing;

            // зоряна величина
            labelMagnitude = new Label { Text = "Зоряна величина:", Location = new Point(xLabel, yPos), Size = new Size(120, height) };
            numericUpDownMagnitude = new NumericUpDown { Location = new Point(xControl, yPos), Size = new Size(controlWidth, height), Minimum = -30, Maximum = 30, DecimalPlaces = 2, Increment = 0.01m };
            yPos += spacing;

            // дистанція
            labelDistance = new Label { Text = "Відстань (св.р.):", Location = new Point(xLabel, yPos), Size = new Size(120, height) };
            numericUpDownDistance = new NumericUpDown { Location = new Point(xControl, yPos), Size = new Size(controlWidth, height), Minimum = 0, Maximum = 1000000, DecimalPlaces = 2, Increment = 0.1m };
            yPos += spacing;

            // пряме сходження
            labelRightAscension = new Label { Text = "Пряме сходження:", Location = new Point(xLabel, yPos), Size = new Size(120, height) };
            textBoxRightAscension = new TextBox { Location = new Point(xControl, yPos), Size = new Size(controlWidth, height), PlaceholderText = "00h00m00s" };
            yPos += spacing;

            // схилення
            labelDeclination = new Label { Text = "Схилення:", Location = new Point(xLabel, yPos), Size = new Size(120, height) };
            textBoxDeclination = new TextBox { Location = new Point(xControl, yPos), Size = new Size(controlWidth, height), PlaceholderText = "+00°00'00\"" };
            yPos += spacing;

            // колір
            labelColor = new Label { Text = "Колір:", Location = new Point(xLabel, yPos), Size = new Size(120, height) };
            comboBoxColor = new ComboBox { Location = new Point(xControl, yPos), Size = new Size(controlWidth, height), DropDownStyle = ComboBoxStyle.DropDownList };
            comboBoxColor.Items.AddRange(new string[] { "Білий", "Блакитний", "Блакитно-білий", "Жовтий", "Жовто-білий", "Помаранчевий", "Червоний" });
            yPos += spacing + 10;

            // кнопки
            buttonSave = new Button { Text = "Зберегти", Location = new Point(xControl - 50, yPos), Size = new Size(100, 30) };
            buttonSave.Click += new EventHandler(ButtonSave_Click);

            buttonCancel = new Button { Text = "Скасувати", Location = new Point(xControl + 70, yPos), Size = new Size(100, 30) };
            buttonCancel.Click += new EventHandler(ButtonCancel_Click);

            Controls.AddRange(new Control[] {
                labelName, textBoxName,
                labelConstellation, textBoxConstellation,
                labelMagnitude, numericUpDownMagnitude,
                labelDistance, numericUpDownDistance,
                labelRightAscension, textBoxRightAscension,
                labelDeclination, textBoxDeclination,
                labelColor, comboBoxColor,
                buttonSave, buttonCancel
            });

            ResumeLayout(false);
            PerformLayout();
        }

        private void LoadStarData()
        {
            textBoxName.Text = _starToEdit.Name;
            textBoxConstellation.Text = _starToEdit.Constellation;
            numericUpDownMagnitude.Value = (decimal)_starToEdit.Magnitude;
            numericUpDownDistance.Value = (decimal)_starToEdit.Distance;
            textBoxRightAscension.Text = _starToEdit.RightAscension;
            textBoxDeclination.Text = _starToEdit.Declination;
            comboBoxColor.SelectedItem = _starToEdit.Color; // встановлює вибраний елемент
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            // Валідація
            if (string.IsNullOrEmpty(textBoxName.Text.Trim()) ||
                string.IsNullOrEmpty(textBoxConstellation.Text.Trim()) ||
                string.IsNullOrEmpty(textBoxRightAscension.Text.Trim()) ||
                string.IsNullOrEmpty(textBoxDeclination.Text.Trim()) ||
                comboBoxColor.SelectedItem == null)
            {
                MessageBox.Show("Будь ласка, заповніть усі поля.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // оновлення об'єкта Star
            _starToEdit.Name = textBoxName.Text.Trim();
            _starToEdit.Constellation = textBoxConstellation.Text.Trim();
            _starToEdit.Magnitude = (double)numericUpDownMagnitude.Value;
            _starToEdit.Distance = (double)numericUpDownDistance.Value;
            _starToEdit.RightAscension = textBoxRightAscension.Text.Trim();
            _starToEdit.Declination = textBoxDeclination.Text.Trim();
            _starToEdit.Color = comboBoxColor.SelectedItem.ToString();

            DialogResult = DialogResult.OK; // вказуємо, що форма закрилася з успіхом
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel; // вказуємо, що форма закрилася без змін
            Close();
        }
    }
}
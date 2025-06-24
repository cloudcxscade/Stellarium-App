using CourseA4.Models;
using CourseA4.Presenters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CourseA4.Forms
{
    public partial class MainForm : Form
    // відображення користувацького інтерфейсу (вкладки, кнопки, текстові поля, таблиці),
    // збір введення користувача та ініціювання дій. Вона делегує більшість логіки презентерам.
    {
        // --- 1. поля класу (елементи управління UI та екземпляри інших класів)

        private TabControl tabControl;
        private TabPage tabPageSearch;
        private TabPage tabPageAdd;
        private TabPage tabPageVisibility;

        // елементи керування вкладки "Пошук зірок"
        private Label labelSearchTitle;
        private Label labelConstellation;
        private ComboBox comboBoxConstellation;
        private Button buttonSearchConstellation;
        private Label labelStarName;
        private TextBox textBoxStarName;
        private Button buttonSearchName;
        private Label labelBrightestCount;
        private NumericUpDown numericUpDownBrightestCount;
        private Button buttonShowBrightest;
        private DataGridView dataGridViewResults;

        // кнопки для редагування та видалення
        private Button buttonEditStar;
        private Button buttonDeleteStar;

        // елементи керування вкладки "Додати зірку"
        private Label labelAddTitle;
        private Label labelAddName;
        private TextBox textBoxAddName;
        private Label labelAddConstellation;
        private TextBox textBoxAddConstellation;
        private Label labelAddMagnitude;
        private NumericUpDown numericUpDownAddMagnitude;
        private Label labelAddDistance;
        private NumericUpDown numericUpDownAddDistance;
        private Label labelAddRightAscension;
        private TextBox textBoxAddRightAscension;
        private Label labelAddDeclination;
        private TextBox textBoxAddDeclination;
        private Label labelAddColor;
        private ComboBox comboBoxAddColor;
        private Button buttonAddStar;

        // елементи керування вкладки "Видимість"
        private Label labelVisibilityTitle;
        private Label labelLatitude;
        private NumericUpDown numericUpDownLatitude;
        private Label labelLongitude;
        private NumericUpDown numericUpDownLongitude;
        private Label labelDateTime;
        private DateTimePicker dateTimePickerObservation;
        private Button buttonCalculateVisibility;
        private DataGridView dataGridViewVisibility;

        // екземпляри класів для роботи з даними та обчисленнями
        private StarRepository _starRepository;
        private const string DataFilePath = "stars.xml"; // константа для шляху до файлу даних

        // екземпляри презентатора
        private StarSearchPresenter _searchPresenter;
        private StarAddPresenter _addPresenter;
        private StarVisibilityPresenter _visibilityPresenter;
        private StarEditorPresenter _editorPresenter; 

        // --- конструктор мейн форми ---
        public MainForm()
        {
            InitializeComponent();

            _starRepository = new StarRepository(DataFilePath);

            _searchPresenter = new StarSearchPresenter(_starRepository, comboBoxConstellation, textBoxStarName,
                                                     numericUpDownBrightestCount, dataGridViewResults);
            _addPresenter = new StarAddPresenter(_starRepository, textBoxAddName, textBoxAddConstellation,
                                               numericUpDownAddMagnitude, numericUpDownAddDistance,
                                               textBoxAddRightAscension, textBoxAddDeclination, comboBoxAddColor);
            _visibilityPresenter = new StarVisibilityPresenter(_starRepository, numericUpDownLatitude, numericUpDownLongitude,
                                                             dateTimePickerObservation, dataGridViewVisibility);
            _editorPresenter = new StarEditorPresenter(_starRepository, dataGridViewResults); // ініціалізація редактора презентації

            InitializeUIColumns();
            _searchPresenter.FillConstellationComboBox();
            _searchPresenter.PopulateInitialData();
        }

        // --- методи ініціалізації UI ---

        private void InitializeComponent() // створює всі елементи керування UI
        {
            SuspendLayout();

            Text = "База зірок і сузір'їв";
            Size = new Size(800, 600);
            MinimumSize = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;
            FormClosed += new FormClosedEventHandler(MainForm_FormClosed);

            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            tabPageSearch = new TabPage();
            tabPageAdd = new TabPage();
            tabPageVisibility = new TabPage();

            tabControl.Controls.Add(tabPageSearch);
            tabControl.Controls.Add(tabPageAdd);
            tabControl.Controls.Add(tabPageVisibility);

            ConfigureSearchTab();
            ConfigureAddTab();
            ConfigureVisibilityTab();

            Controls.Add(tabControl);

            ResumeLayout(false);
        }

        private void ConfigureSearchTab()
        {
            tabPageSearch.Text = "Пошук зірок";
            tabPageSearch.Padding = new Padding(10);

            labelSearchTitle = new Label();
            labelSearchTitle.Text = "Пошук у базі даних зірок";
            labelSearchTitle.Font = new Font(Font.FontFamily, 14, FontStyle.Bold);
            labelSearchTitle.Location = new Point(10, 10);
            labelSearchTitle.Size = new Size(760, 30);

            labelConstellation = new Label();
            labelConstellation.Text = "Сузір'я:";
            labelConstellation.Location = new Point(10, 50);
            labelConstellation.Size = new Size(100, 20);

            comboBoxConstellation = new ComboBox();
            comboBoxConstellation.Location = new Point(120, 50);
            comboBoxConstellation.Size = new Size(200, 20);
            comboBoxConstellation.DropDownStyle = ComboBoxStyle.DropDownList;

            buttonSearchConstellation = new Button();
            buttonSearchConstellation.Text = "Знайти зірки в сузір'ї";
            buttonSearchConstellation.Location = new Point(330, 50);
            buttonSearchConstellation.Size = new Size(150, 23);
            buttonSearchConstellation.Click += new EventHandler(ButtonSearchConstellation_Click);

            labelStarName = new Label();
            labelStarName.Text = "Назва зірки:";
            labelStarName.Location = new Point(10, 80);
            labelStarName.Size = new Size(100, 20);

            textBoxStarName = new TextBox();
            textBoxStarName.Location = new Point(120, 80);
            textBoxStarName.Size = new Size(200, 20);

            buttonSearchName = new Button();
            buttonSearchName.Text = "Знайти за назвою";
            buttonSearchName.Location = new Point(330, 80);
            buttonSearchName.Size = new Size(150, 23);
            buttonSearchName.Click += new EventHandler(ButtonSearchName_Click);

            labelBrightestCount = new Label();
            labelBrightestCount.Text = "Кількість:";
            labelBrightestCount.Location = new Point(10, 110);
            labelBrightestCount.Size = new Size(100, 20);

            numericUpDownBrightestCount = new NumericUpDown();
            numericUpDownBrightestCount.Location = new Point(120, 110);
            numericUpDownBrightestCount.Size = new Size(60, 20);
            numericUpDownBrightestCount.Minimum = 1;
            numericUpDownBrightestCount.Maximum = 100;
            numericUpDownBrightestCount.Value = 10;

            buttonShowBrightest = new Button();
            buttonShowBrightest.Text = "Показати найяскравіші зірки";
            buttonShowBrightest.Location = new Point(330, 110);
            buttonShowBrightest.Size = new Size(200, 23);
            buttonShowBrightest.Click += new EventHandler(ButtonShowBrightest_Click);

            dataGridViewResults = new DataGridView();
            dataGridViewResults.Location = new Point(10, 150);
            dataGridViewResults.Size = new Size(760, 340); // Adjusted size to make space for new buttons
            dataGridViewResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewResults.AllowUserToAddRows = false;
            dataGridViewResults.ReadOnly = true;
            dataGridViewResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Allow full row selection

            // кнопки редагування та видалення
            buttonEditStar = new Button();
            buttonEditStar.Text = "Редагувати зірку";
            buttonEditStar.Location = new Point(10, 500); // розташування нижче DataGridView
            buttonEditStar.Size = new Size(150, 30);
            buttonEditStar.Click += new EventHandler(ButtonEditStar_Click);

            buttonDeleteStar = new Button();
            buttonDeleteStar.Text = "Видалити зірку";
            buttonDeleteStar.Location = new Point(170, 500); // розташуємо поруч із кнопкою «Редагувати»
            buttonDeleteStar.Size = new Size(150, 30);
            buttonDeleteStar.Click += new EventHandler(ButtonDeleteStar_Click);

            tabPageSearch.Controls.Add(labelSearchTitle);
            tabPageSearch.Controls.Add(labelConstellation);
            tabPageSearch.Controls.Add(comboBoxConstellation);
            tabPageSearch.Controls.Add(buttonSearchConstellation);
            tabPageSearch.Controls.Add(labelStarName);
            tabPageSearch.Controls.Add(textBoxStarName);
            tabPageSearch.Controls.Add(buttonSearchName);
            tabPageSearch.Controls.Add(labelBrightestCount);
            tabPageSearch.Controls.Add(numericUpDownBrightestCount);
            tabPageSearch.Controls.Add(buttonShowBrightest);
            tabPageSearch.Controls.Add(dataGridViewResults);
            tabPageSearch.Controls.Add(buttonEditStar);   // додати кнопку редагування
            tabPageSearch.Controls.Add(buttonDeleteStar); // додати кнопку видалення
        }

        private void ConfigureAddTab()
        {
            tabPageAdd.Text = "Додати зірку";
            tabPageAdd.Padding = new Padding(10);

            labelAddTitle = new Label();
            labelAddTitle.Text = "Додати нову зірку";
            labelAddTitle.Font = new Font(Font.FontFamily, 14, FontStyle.Bold);
            labelAddTitle.Location = new Point(10, 10);
            labelAddTitle.Size = new Size(760, 30);

            labelAddName = new Label();
            labelAddName.Text = "Назва:";
            labelAddName.Location = new Point(10, 50);
            labelAddName.Size = new Size(120, 20);

            textBoxAddName = new TextBox();
            textBoxAddName.Location = new Point(140, 50);
            textBoxAddName.Size = new Size(200, 20);

            labelAddConstellation = new Label();
            labelAddConstellation.Text = "Сузір'я:";
            labelAddConstellation.Location = new Point(10, 80);
            labelAddConstellation.Size = new Size(120, 20);

            textBoxAddConstellation = new TextBox();
            textBoxAddConstellation.Location = new Point(140, 80);
            textBoxAddConstellation.Size = new Size(200, 20);

            labelAddMagnitude = new Label();
            labelAddMagnitude.Text = "Зоряна величина:";
            labelAddMagnitude.Location = new Point(10, 110);
            labelAddMagnitude.Size = new Size(120, 20);

            numericUpDownAddMagnitude = new NumericUpDown();
            numericUpDownAddMagnitude.Location = new Point(140, 110);
            numericUpDownAddMagnitude.Size = new Size(80, 20);
            numericUpDownAddMagnitude.Minimum = -30;
            numericUpDownAddMagnitude.Maximum = 30;
            numericUpDownAddMagnitude.DecimalPlaces = 2;
            numericUpDownAddMagnitude.Increment = 0.01m;
            numericUpDownAddMagnitude.Value = 0;

            labelAddDistance = new Label();
            labelAddDistance.Text = "Відстань (св.р.):";
            labelAddDistance.Location = new Point(10, 140);
            labelAddDistance.Size = new Size(120, 20);

            numericUpDownAddDistance = new NumericUpDown();
            numericUpDownAddDistance.Location = new Point(140, 140);
            numericUpDownAddDistance.Size = new Size(100, 20);
            numericUpDownAddDistance.Minimum = 0;
            numericUpDownAddDistance.Maximum = 1000000;
            numericUpDownAddDistance.DecimalPlaces = 2;
            numericUpDownAddDistance.Increment = 0.1m;
            numericUpDownAddDistance.Value = 0;

            labelAddRightAscension = new Label();
            labelAddRightAscension.Text = "Пряме сходження:";
            labelAddRightAscension.Location = new Point(10, 170);
            labelAddRightAscension.Size = new Size(120, 20);

            textBoxAddRightAscension = new TextBox();
            textBoxAddRightAscension.Location = new Point(140, 170);
            textBoxAddRightAscension.Size = new Size(100, 20);
            textBoxAddRightAscension.PlaceholderText = "00h00m00s";

            labelAddDeclination = new Label();
            labelAddDeclination.Text = "Схилення:";
            labelAddDeclination.Location = new Point(10, 200);
            labelAddDeclination.Size = new Size(120, 20);

            textBoxAddDeclination = new TextBox();
            textBoxAddDeclination.Location = new Point(140, 200);
            textBoxAddDeclination.Size = new Size(100, 20);
            textBoxAddDeclination.PlaceholderText = "+00°00'00\"";

            labelAddColor = new Label();
            labelAddColor.Text = "Колір:";
            labelAddColor.Location = new Point(10, 230);
            labelAddColor.Size = new Size(120, 20);

            comboBoxAddColor = new ComboBox();
            comboBoxAddColor.Location = new Point(140, 230);
            comboBoxAddColor.Size = new Size(120, 20);
            comboBoxAddColor.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxAddColor.Items.AddRange(new string[] {
                "Білий", "Блакитний", "Блакитно-білий", "Жовтий",
                "Жовто-білий", "Помаранчевий", "Червоний"
            });
            comboBoxAddColor.SelectedIndex = 0;

            buttonAddStar = new Button();
            buttonAddStar.Text = "Додати зірку";
            buttonAddStar.Location = new Point(140, 270);
            buttonAddStar.Size = new Size(150, 30);
            buttonAddStar.Click += new EventHandler(ButtonAddStar_Click);

            tabPageAdd.Controls.Add(labelAddTitle);
            tabPageAdd.Controls.Add(labelAddName);
            tabPageAdd.Controls.Add(textBoxAddName);
            tabPageAdd.Controls.Add(labelAddConstellation);
            tabPageAdd.Controls.Add(textBoxAddConstellation);
            tabPageAdd.Controls.Add(labelAddMagnitude);
            tabPageAdd.Controls.Add(numericUpDownAddMagnitude);
            tabPageAdd.Controls.Add(labelAddDistance);
            tabPageAdd.Controls.Add(numericUpDownAddDistance);
            tabPageAdd.Controls.Add(labelAddRightAscension);
            tabPageAdd.Controls.Add(textBoxAddRightAscension);
            tabPageAdd.Controls.Add(labelAddDeclination);
            tabPageAdd.Controls.Add(textBoxAddDeclination);
            tabPageAdd.Controls.Add(labelAddColor);
            tabPageAdd.Controls.Add(comboBoxAddColor);
            tabPageAdd.Controls.Add(buttonAddStar);
        }

        private void ConfigureVisibilityTab()
        {
            tabPageVisibility.Text = "Видимість";
            tabPageVisibility.Padding = new Padding(10);

            labelVisibilityTitle = new Label();
            labelVisibilityTitle.Text = "Розрахувати видимі зірки та сузір'я";
            labelVisibilityTitle.Font = new Font(Font.FontFamily, 14, FontStyle.Bold);
            labelVisibilityTitle.Location = new Point(10, 10);
            labelVisibilityTitle.Size = new Size(760, 30);

            labelLatitude = new Label();
            labelLatitude.Text = "Широта (°):";
            labelLatitude.Location = new Point(10, 50);
            labelLatitude.Size = new Size(100, 20);

            numericUpDownLatitude = new NumericUpDown();
            numericUpDownLatitude.Location = new Point(120, 50);
            numericUpDownLatitude.Size = new Size(80, 20);
            numericUpDownLatitude.Minimum = -90;
            numericUpDownLatitude.Maximum = 90;
            numericUpDownLatitude.DecimalPlaces = 4;
            numericUpDownLatitude.Value = 50.45m;    // Киев, Украина

            labelLongitude = new Label();
            labelLongitude.Text = "Довгота (°):";
            labelLongitude.Location = new Point(10, 80);
            labelLongitude.Size = new Size(100, 20);

            numericUpDownLongitude = new NumericUpDown();
            numericUpDownLongitude.Location = new Point(120, 80);
            numericUpDownLongitude.Size = new Size(80, 20);
            numericUpDownLongitude.Minimum = -180;
            numericUpDownLongitude.Maximum = 180;
            numericUpDownLongitude.DecimalPlaces = 4;
            numericUpDownLongitude.Value = 30.52m;   // Киев, Украина

            labelDateTime = new Label();
            labelDateTime.Text = "Дата і час:";
            labelDateTime.Location = new Point(10, 110);
            labelDateTime.Size = new Size(100, 20);

            dateTimePickerObservation = new DateTimePicker();
            dateTimePickerObservation.Location = new Point(120, 110);
            dateTimePickerObservation.Size = new Size(200, 20);
            dateTimePickerObservation.Format = DateTimePickerFormat.Custom;
            dateTimePickerObservation.CustomFormat = "dd.MM.yyyy HH:mm";
            dateTimePickerObservation.Value = DateTime.Now;

            buttonCalculateVisibility = new Button();
            buttonCalculateVisibility.Text = "Розрахувати видимі зірки";
            buttonCalculateVisibility.Location = new Point(10, 140);
            buttonCalculateVisibility.Size = new Size(200, 30);
            buttonCalculateVisibility.Click += new EventHandler(ButtonCalculateVisibility_Click);

            dataGridViewVisibility = new DataGridView();
            dataGridViewVisibility.Location = new Point(10, 180);
            dataGridViewVisibility.Size = new Size(760, 350);
            dataGridViewVisibility.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewVisibility.AllowUserToAddRows = false;
            dataGridViewVisibility.ReadOnly = true;

            tabPageVisibility.Controls.Add(labelVisibilityTitle);
            tabPageVisibility.Controls.Add(labelLatitude);
            tabPageVisibility.Controls.Add(numericUpDownLatitude);
            tabPageVisibility.Controls.Add(labelLongitude);
            tabPageVisibility.Controls.Add(numericUpDownLongitude);
            tabPageVisibility.Controls.Add(labelDateTime);
            tabPageVisibility.Controls.Add(dateTimePickerObservation);
            tabPageVisibility.Controls.Add(buttonCalculateVisibility);
            tabPageVisibility.Controls.Add(dataGridViewVisibility);
        }

        private void InitializeUIColumns()
        {
            dataGridViewResults.Columns.Clear();
            dataGridViewResults.Columns.Add("Name", "Назва");
            dataGridViewResults.Columns.Add("Constellation", "Сузір'я");
            dataGridViewResults.Columns.Add("Magnitude", "Зоряна величина");
            dataGridViewResults.Columns.Add("Distance", "Відстань (св.р.)");
            dataGridViewResults.Columns.Add("RightAscension", "Пряме сходження");
            dataGridViewResults.Columns.Add("Declination", "Схилення");
            dataGridViewResults.Columns.Add("Color", "Колір");

            dataGridViewVisibility.Columns.Clear();
            dataGridViewVisibility.Columns.Add("Name", "Назва");
            dataGridViewVisibility.Columns.Add("Constellation", "Сузір'я");
            dataGridViewVisibility.Columns.Add("Magnitude", "Зоряна величина");
            dataGridViewVisibility.Columns.Add("Altitude", "Висота над горизонтом (°)");
            dataGridViewVisibility.Columns.Add("Azimuth", "Азимут (°)");
            dataGridViewVisibility.Columns.Add("Color", "Колір");
            dataGridViewVisibility.Columns.Add("Visibility", "Видимість");
        }

        // --- 5. Обработчики событий (Event Handlers) ---

        private void ButtonSearchConstellation_Click(object? sender, EventArgs e)
        {
            _searchPresenter.SearchByConstellation();
        }

        private void ButtonSearchName_Click(object? sender, EventArgs e)
        {
            _searchPresenter.SearchByName();
        }

        private void ButtonShowBrightest_Click(object? sender, EventArgs e)
        {
            _searchPresenter.ShowBrightestStars();
        }

        private void ButtonAddStar_Click(object? sender, EventArgs e)
        {
            _addPresenter.AddStar();
            // після додавання оновляємо результати пошуку та випадаюче поле сузір'їв
            _searchPresenter.FillConstellationComboBox();
            _searchPresenter.PopulateInitialData(); // показ дати
        }

        private void ButtonEditStar_Click(object? sender, EventArgs e)
        {
            if (dataGridViewResults.SelectedRows.Count > 0)
            {
                // отримуємо назву зірки з першої вибраної комірки
                string starName = dataGridViewResults.SelectedRows[0].Cells["Name"].Value?.ToString();
                if (!string.IsNullOrEmpty(starName))
                {
                    _editorPresenter.EditStar(starName);
                    // після редагування, оновлюємо combobox, якщо назва сузір'я могла змінитися
                    _searchPresenter.FillConstellationComboBox();
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, оберіть зірку для редагування.", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ButtonDeleteStar_Click(object sender, EventArgs e)
        {
            if (dataGridViewResults.SelectedRows.Count > 0)
            {
                // отримуємо назву зірки з першої вибраної комірки
                string starName = dataGridViewResults.SelectedRows[0].Cells["Name"].Value?.ToString();
                if (!string.IsNullOrEmpty(starName))
                {
                    _editorPresenter.DeleteStar(starName);
                    // після видалення, оновлюємо combobox, якщо сузір'я більше не існує
                    _searchPresenter.FillConstellationComboBox();
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, оберіть зірку для видалення.", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ButtonCalculateVisibility_Click(object sender, EventArgs e)
        {
            _visibilityPresenter.CalculateAndDisplayVisibility();
        }

        // --- оброблювач подій закриття форми ---
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _starRepository.SaveData(); // Сохраняем данные при закрытии формы
        }
    }

    // --- мейн для того щоб програма хоть якось працювала ---
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CourseA4.Models
{

    public class StarRepository
    {
        private string _filePath; // поля (шлях до хмл файлу, де будуть зберігатися дані)
        private List<Star> _stars;      // приватна колекція, яка зберігає всі об'єкти Star

        public List<Star> Stars => _stars; // властивість тільки для читання для доступу к коллекції зірок

        public StarRepository(string filePath) // приймає шлях до файлу та ініціалізує _stars як новий список
        {
            _filePath = filePath;
            _stars = new List<Star>();
            LoadData(); // Загружаем данные при создании репозитория
        }

        private void LoadData() // метод перевіряє, чи існує файл за шляхом
        {
            if (!File.Exists(_filePath)) // якщо файл не існує, викликаємо CreateDemoData() для створення початкових даних і потім SaveData() для їх збереження
            {
                CreateDemoData(); 
                SaveData(); // зберігаємо демонстраційні дані
            }
            else
            {
                try // якщо файл існує, намагаєтмось десеріалізувати список Star з хмл файлу за допомогою XmlSerializer
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Star>));
                    using (FileStream fs = new FileStream(_filePath, FileMode.Open))
                    {
                        _stars = (List<Star>)serializer.Deserialize(fs);
                    }
                }
                catch (Exception ex) // обробляємо потенційні помилки десеріалізації (наприклад, пошкоджений файл), в цьому випадку також створюємо демонстраційні дані
                {
                    Console.WriteLine($"Ошибка загрузки данных: {ex.Message}");
                    CreateDemoData();
                    SaveData(); // зберігаємо демонстраційні дані
                }
            }
        }

        public void SaveData() // метод серіалізує поточну колекцію _stars у хмл файл за шляхом _filePath за допомогою XmlSerializer
        {
            try 
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Star>));
                using (FileStream fs = new FileStream(_filePath, FileMode.Create)) // використаємо FileStream та FileMode.Create для перезапису файлу
                {
                    serializer.Serialize(fs, _stars);
                }
            }
            catch (Exception ex)
            {
                // В реальном приложении здесь можно логировать ошибку
                Console.WriteLine($"Ошибка сохранения данных: {ex.Message}");
                // Можно также уведомить пользователя через MessageBox в UI, если это уместно
            }
        }


        private void CreateDemoData() // тут ми створюємо початковий набір зірок (демонстраційні (початкові) дані))
        {                           
            _stars = new List<Star>   // і додаємо до колекції
            {
                new Star { Name = "Сіріус", Constellation = "Великий Пес", Magnitude = -1.46, Distance = 8.6, RightAscension = "06h45m08.9s", Declination = "-16°42'58\"", Color = "Блакитно-білий" },
                new Star { Name = "Канопус", Constellation = "Кіль", Magnitude = -0.74, Distance = 310, RightAscension = "06h23m57.1s", Declination = "-52°41'45\"", Color = "Жовто-білий" },
                new Star { Name = "Арктур", Constellation = "Волопас", Magnitude = -0.05, Distance = 37, RightAscension = "14h15m39.7s", Declination = "+19°10'57\"", Color = "Помаранчевий" },
                new Star { Name = "Вега", Constellation = "Ліра", Magnitude = 0.03, Distance = 25, RightAscension = "18h36m56.3s", Declination = "+38°47'01\"", Color = "Блакитно-білий" },
                new Star { Name = "Капелла", Constellation = "Візничий", Magnitude = 0.08, Distance = 42.9, RightAscension = "05h16m41.4s", Declination = "+45°59'53\"", Color = "Жовтий" },
                new Star { Name = "Рігель", Constellation = "Оріон", Magnitude = 0.13, Distance = 860, RightAscension = "05h14m32.3s", Declination = "-08°12'06\"", Color = "Блакитно-білий" },
                new Star { Name = "Проціон", Constellation = "Малий Пес", Magnitude = 0.34, Distance = 11.4, RightAscension = "07h39m18.1s", Declination = "+05°13'30\"", Color = "Жовто-білий" },
                new Star { Name = "Бетельгейзе", Constellation = "Оріон", Magnitude = 0.50, Distance = 640, RightAscension = "05h55m10.3s", Declination = "+07°24'25\"", Color = "Червоний" },
                new Star { Name = "Альтаїр", Constellation = "Орел", Magnitude = 0.76, Distance = 16.7, RightAscension = "19h50m47.0s", Declination = "+08°52'06\"", Color = "Білий" },
                new Star { Name = "Альдебаран", Constellation = "Телець", Magnitude = 0.87, Distance = 65, RightAscension = "04h35m55.2s", Declination = "+16°30'33\"", Color = "Помаранчевий" },
                new Star { Name = "Спіка", Constellation = "Діва", Magnitude = 1.04, Distance = 250, RightAscension = "13h25m11.6s", Declination = "-11°09'41\"", Color = "Блакитно-білий" },
                new Star { Name = "Антарес", Constellation = "Скорпіон", Magnitude = 1.09, Distance = 550, RightAscension = "16h29m24.5s", Declination = "-26°25'55\"", Color = "Червоний" },
                new Star { Name = "Поллукс", Constellation = "Близнюки", Magnitude = 1.15, Distance = 33.7, RightAscension = "07h45m18.9s", Declination = "+28°01'34\"", Color = "Помаранчевий" },
                new Star { Name = "Денеб", Constellation = "Лебідь", Magnitude = 1.25, Distance = 2600, RightAscension = "20h41m25.9s", Declination = "+45°16'49\"", Color = "Білий" },
                new Star { Name = "Регул", Constellation = "Лев", Magnitude = 1.36, Distance = 77, RightAscension = "10h08m22.3s", Declination = "+11°58'02\"", Color = "Блакитно-білий" },
                new Star { Name = "Фомальгаут", Constellation = "Південна Риба", Magnitude = 1.16, Distance = 25, RightAscension = "22h57m39.1s", Declination = "-29°37'20\"", Color = "Білий" },
                new Star { Name = "Полярна", Constellation = "Мала Ведмедиця", Magnitude = 1.97, Distance = 430, RightAscension = "02h31m49.1s", Declination = "+89°15'51\"", Color = "Жовто-білий" },
                new Star { Name = "Мірфак", Constellation = "Персей", Magnitude = 1.79, Distance = 590, RightAscension = "03h24m19.4s", Declination = "+49°51'40\"", Color = "Жовтий" },
                new Star { Name = "Деніб Кайтос", Constellation = "Кит", Magnitude = 2.04, Distance = 96, RightAscension = "01h08m35.4s", Declination = "-17°59'12\"", Color = "Помаранчевий" },
                new Star { Name = "Алголь", Constellation = "Персей", Magnitude = 2.09, Distance = 93, RightAscension = "03h08m10.1s", Declination = "+40°57'20\"", Color = "Блакитно-білий" }
            };
        }

        public bool UpdateStar(Star updatedStar) // оновлює існуючу зірку в колекції зірок
        {
            // Знаходимо зірку за її назвою (припускаємо, що назва унікальна для простоти)
            // В реальному проекті краще використовувати унікальний ID
            var existingStar = _stars.FirstOrDefault(s => s.Name.Equals(updatedStar.Name, StringComparison.OrdinalIgnoreCase));

            if (existingStar != null)
            {
                existingStar.Name = updatedStar.Name;
                existingStar.Constellation = updatedStar.Constellation;
                existingStar.Magnitude = updatedStar.Magnitude;
                existingStar.Distance = updatedStar.Distance;
                existingStar.RightAscension = updatedStar.RightAscension;
                existingStar.Declination = updatedStar.Declination;
                existingStar.Color = updatedStar.Color;
                SaveData(); // зберігаємо зміни
                return true;
            }
            return false;
        }

        public bool DeleteStar(string starName) // видаляє зірку з колекції за її назвою
        {
            var starToRemove = _stars.FirstOrDefault(s => s.Name.Equals(starName, StringComparison.OrdinalIgnoreCase));
            if (starToRemove != null)
            {
                _stars.Remove(starToRemove);
                SaveData(); // зберігаємо зміни
                return true;
            }
            return false;
        }
    }
}

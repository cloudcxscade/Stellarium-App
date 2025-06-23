using System;
using System.Xml.Serialization;

namespace CourseA4.Models
{

    [Serializable] // необхідно для XML-серіалізації
    public class Star
    {
        public string Name { get; set; }
        public string Constellation { get; set; } // Сузір'я
        public double Magnitude { get; set; } // зоряна величина
        public double Distance { get; set; } // відстань
        public string RightAscension { get; set; } // пряме сходження
        public string Declination { get; set; } // склонение
        public string Color { get; set; } // колір зірки

        public Star()
        {
            // ініціалізуємо властивості значеннями за замовчуванням, щоб уникнути null значень при серіалізації/десеріалізації
            Name = "";
            Constellation = "";
            Magnitude = 0;
            Distance = 0;
            RightAscension = "";
            Declination = "";
            Color = "Білий";
        }
    }
}
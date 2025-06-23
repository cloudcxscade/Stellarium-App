using CourseA4.Models;
using System;
using System.Globalization;

namespace CourseA4.Helpers
{
    public static class StarCalculator // цей статичний клас надає методи для виконання астрономічних розрахунків, зокрема для визначення видимості зірок
    {

        public static (double altitude, double azimuth, bool isVisible) CalculateStarPosition(Star star, double latitude, double longitude, DateTime observationTime)
            // основний метод, який приймає об'єкт Star, координати спостерігача (широта, довгота) та час спостереження
        {                                
            // конвертація координат зірок в радіани
            double ra = ParseRightAscension(star.RightAscension); // у часах
            double dec = ParseDeclination(star.Declination);     // у радіанах


            double LST = CalculateLocalSiderealTime(observationTime, longitude); // обчислюємо Місцевий Зоряний Час (LST)

            // Часовой угол (HA) = LST - RA (в часах)
            double HA = LST - ra;

            // перетворення HA на радіани (тобто 1 година = 15 градусів)
            HA = HA * 15.0 * Math.PI / 180.0;

            // конвертація широти спостерігача на радіани
            double lat = latitude * Math.PI / 180.0;

            // розрахунок висоти (altitude)
            double sinAlt = Math.Sin(dec) * Math.Sin(lat) + Math.Cos(dec) * Math.Cos(lat) * Math.Cos(HA);
            double altitude = Math.Asin(sinAlt);

            // розрахунок азимута (azimuth)
            // використання Math.Min/Max для захисту від помилок округлення, які можуть призвести до виходу за межі [-1, 1]
            double cosA = (Math.Sin(dec) - Math.Sin(altitude) * Math.Sin(lat)) / (Math.Cos(altitude) * Math.Cos(lat));
            cosA = Math.Min(Math.Max(cosA, -1.0), 1.0);
            double azimuth = Math.Acos(cosA);

            // коррекція азимута для повного круга (север = 0, восток = 90, юг = 180, запад = 270)
            if (Math.Sin(HA) > 0)
            {
                azimuth = 2 * Math.PI - azimuth;
            }

            // перетворення на градуси
            altitude = altitude * 180.0 / Math.PI;
            azimuth = azimuth * 180.0 / Math.PI;

            // врахування атмосферної рефракції (якось приблизно)
            // додаємо рефракцію тільки якщо зірка над горизонтом
            if (altitude >= 0)
            {
                altitude += 0.0167 / Math.Tan((altitude + 10.3 / (altitude + 5.11)) * Math.PI / 180.0);
            }

            // зірка видима, якщо її висота більше 0 градусів і зіркова величина менше 6.0
            bool isVisible = altitude > 0 && star.Magnitude < 6.0;

            return (altitude, azimuth, isVisible);
        }


        public static double CalculateLocalSiderealTime(DateTime localDateTime, double longitude) // обчислює місцевий зоряний час (Local Sidereal Time – LST)
        {
            // переводимо локальне время в UTC для астрономических расчетов
            DateTime utc = localDateTime.ToUniversalTime();

            // обчислення Юліанської дати для UTC
            double jd = JulianDate(utc);

            // кількість століть від J2000.0

            double T = (jd - 2451545.0) / 36525.0;

            // Greenwich середній зоряний час у градусах
            double GMST = 280.46061837 + 360.98564736629 * (jd - 2451545.0) +
                          0.000387933 * T * T - T * T * T / 38710000.0;

            // нормалізація GMST до діапазону 0-360 градусів
            GMST = GMST % 360.0;
            if (GMST < 0) GMST += 360.0;

            // місцевий зоряний час з урахуванням довготи
            double LST = GMST + longitude;

            // нормалізація LST до діапазону 0-360 градусів
            LST = LST % 360.0;
            if (LST < 0) LST += 360.0;

            // перетворення LST на часии (1 година = 15 градусів)
            return LST / 15.0;
        }

        public static double JulianDate(DateTime utcDateTime)
        {
            // алгоритм розрахунку юліанської дати для заданої дати та часу UTC

            int year = utcDateTime.Year;
            int month = utcDateTime.Month;

            // день включає дрібну частину для години, хвилини, секунди
            double day = utcDateTime.Day + utcDateTime.Hour / 24.0 + utcDateTime.Minute / 1440.0 + utcDateTime.Second / 86400.0 + utcDateTime.Millisecond / 86400000.0;

            if (month <= 2)
            {
                year -= 1;
                month += 12;
            }

            double A = Math.Floor((double)year / 100);
            double B = 2 - A + Math.Floor(A / 4);

            double JD = Math.Floor(365.25 * (year + 4716)) +
                        Math.Floor(30.6001 * (month + 1)) +
                        day + B - 1524.5;

            return JD;
        }

        public static double ParseRightAscension(string raStr) // парсить рядок прямого сходження (RA) (наприклад, "07h45m18.9s") у години
        {
            var parts = raStr.Split(new[] { 'h', 'm', 's' }, StringSplitOptions.RemoveEmptyEntries);
            double hours = 0, minutes = 0, seconds = 0;

            // використовуємо InvariantCulture для надійного парсингу десяткових роздільників
            if (parts.Length >= 1 && double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out hours))
            {
                if (parts.Length >= 2 && double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out minutes))
                {
                    if (parts.Length >= 3 && double.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out seconds))
                    {
                        // усі частини успішно розпарсені
                    }
                }
            }
            return hours + minutes / 60.0 + seconds / 3600.0;
        }

        public static double ParseDeclination(string decStr) // парсить рядок схилення (наприклад, "+28°01'34"") у градуси, а потім конвертує в радіани.
        {
            // замінюємо символи градусів, хвилин та секунд на пробіли для спрощення парсингу

            decStr = decStr.Replace("°", " ").Replace("'", " ").Replace("\"", " ");
            var parts = decStr.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            double sign = decStr.StartsWith("-") ? -1 : 1;
            double degrees = 0, minutes = 0, seconds = 0;

            if (parts.Length >= 1 && double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out degrees))
            {
                if (parts.Length >= 2 && double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out minutes))
                {
                    if (parts.Length >= 3 && double.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out seconds))
                    {
                        // усі частини успішно розпарсені
                    }
                }
            }
            // обчислюємо абсолютне значення градусів, хвилин та секунд
            double result = Math.Abs(degrees) + minutes / 60.0 + seconds / 3600.0;
            // застосовуємо знак і конвертуємо на радіани
            return sign * result * Math.PI / 180.0;
        }
    }
}
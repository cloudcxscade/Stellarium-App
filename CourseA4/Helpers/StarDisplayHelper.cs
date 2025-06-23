using CourseA4.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace CourseA4.Helpers
{

    public static class StarDisplayHelper // цей статичний клас надає допоміжні методи для відображення колекцій об'єктів Star у контролі DataGridView
    {                                     // допомагає у візуалізації даних, відокремлюючи логіку відображення від бізнес-логіки та логіки презентації

        public static void DisplayStarsInGrid(List<Star> starsToDisplay, DataGridView grid) // очищує існуючі рядки у DataGridView
                                                                                            // ітерує по списку зірок starsToDisplay
                                                                          // для кожної зірки додає новий рядок у DataGridView, заповнюючи його властивостями зірки
        {
            grid.Rows.Clear();

            foreach (var star in starsToDisplay)
            {
                grid.Rows.Add(
                    star.Name,
                    star.Constellation,
                    star.Magnitude,
                    star.Distance,
                    star.RightAscension,
                    star.Declination,
                    star.Color
                );
            }

            // сортує DataGridView за стовпцем "Magnitude"(зоряна величина) за зростанням, щоб найяскравіші зірки(з меншою величиною) були нагорі
            var magnitudeColumn = grid.Columns["Magnitude"];
            if (magnitudeColumn != null)
            {
                grid.Sort(magnitudeColumn, ListSortDirection.Ascending);
            }
        }
    }
}
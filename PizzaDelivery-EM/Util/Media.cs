using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaDelivery_EM.Util
{
    static class Media
    {
        public static string Directory { get; } = "pack://application:,,,/Media/";

        public static Dictionary<string, string> PizzaNameToFileName { get; } = new Dictionary<string, string>
        {
            { "Песто", "Pesto" },
            { "Гавайская", "Hawaiian" },
            { "Двойной цыплёнок", "DoubleChicken" },
            { "Ветчина и сыр", "HamNCheese" },
            { "Своя", "Custom" }
        };
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_PizzaDelivery.Util
{
    static class Media
    {
        public static string directory { get; } = "/Data/Media/";

        public static Dictionary<string, string> pizzaNameToFileName { get; } = new Dictionary<string, string>
        {
            { "Песто", "Pesto" },
            { "Гавайская", "Hawaiian" },
            { "Двойной цыплёнок", "DoubleChicken" },
            { "Ветчина и сыр", "HamNCheese" }
        };
    }
}

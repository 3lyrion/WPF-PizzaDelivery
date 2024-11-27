﻿using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using PizzaDelivery.Util;

namespace PizzaDelivery.ViewModel
{
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal)
                return string.Format("{0:C2}", (decimal)value);

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PizzaImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
                return Media.Directory + Media.PizzaNameToFileName[(string)value] + ".png";

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class OrderPartBottomLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Model.OrderPart)
            {
                var orderPart = value as Model.OrderPart;

                return $"{orderPart.PizzaSize.Name} {orderPart.PizzaSize.Size} см, {orderPart.Dough.Name} тесто";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ObjectToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Hidden;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Model.PizzaSize)
            {
                var pizzaSize = value as Model.PizzaSize;

                return $"{pizzaSize.Name} {pizzaSize.Size} см";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DoughConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Model.Dough)
            {
                var dough = value as Model.Dough;

                return dough.Name;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SubmitOrderTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                var quantity = (int)value;

                if (quantity > 0) return "Сохранить";

                return "Добавить";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PizzaToQuantityConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value[0] is Model.Pizza)
            {
                var pizza = value[0] as Model.Pizza;
                var orderParts = value[1] as ObservableCollection<Model.OrderPart>;

                try
                {
                    return orderParts.First(e => e.Pizza == pizza).Quantity;
                }

                catch
                {
                    return 0;
                }
            }

            return value;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            return new object[] { Binding.DoNothing, Binding.DoNothing, Binding.DoNothing };
        }
    }

    public class PizzaQuantityToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                var quantity = (int)value;

                if (quantity > 0) return Visibility.Hidden;
                return Visibility.Visible;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InversePizzaQuantityToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                var quantity = (int)value;

                if (quantity > 0) return Visibility.Visible;
                return Visibility.Hidden;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /*
    public class PizzaQuantityToVisibilityConverter : IValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value[0] is Model.Pizza)
            {
                var pizza = value[0] as Model.Pizza;
                var orderParts = value[1] as ObservableCollection<Model.OrderPart>;

                try
                {
                    var quantity = orderParts.First(e => e.Pizza == pizza).Quantity;

                    if (quantity > 0) return Visibility.Hidden;
                    return Visibility.Visible;
                }

                catch
                {
                    return Visibility.Visible;
                }
            }

            return value;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            return new object[] { Binding.DoNothing, Binding.DoNothing };
        }
    }

    public class InversePizzaQuantityToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value[0] is Model.Pizza)
            {
                var pizza = value[0] as Model.Pizza;
                var orderParts = value[1] as ObservableCollection<Model.OrderPart>;

                try
                {
                    var quantity = orderParts.First(e => e.Pizza == pizza).Quantity;

                    if (quantity > 0) return Visibility.Visible;
                    return Visibility.Hidden;
                }

                catch
                {
                    return Visibility.Hidden;
                }
            }

            return value;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            return new object[] { Binding.DoNothing, Binding.DoNothing };
        }
    }
    */
    public class CurrentDoughToBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value[0] is Model.Dough)
            {
                var dough = value[0] as Model.Dough;
                var op = value[1] as Model.OrderPart;

                if (op != null && op.Dough != null && op.Dough.Name == dough.Name)
                    return true;

                return false;
            }

            return value;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            return new object[] { Binding.DoNothing, Binding.DoNothing };
        }
    }

    public class CurrentSizeToBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value[0] is Model.PizzaSize)
            {
                var pizzaSize = value[0] as Model.PizzaSize;
                var op = value[1] as Model.OrderPart;

                if (op != null && op.PizzaSize != null && op.PizzaSize.Size == pizzaSize.Size)
                    return true;

                return false;
            }

            return value;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            return new object[] { Binding.DoNothing, Binding.DoNothing };
        }
    }
}

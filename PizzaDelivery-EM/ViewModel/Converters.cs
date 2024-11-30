using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using PizzaDelivery_EM.Util;
using DTO = Interfaces.DTO;

namespace PizzaDelivery_EM.ViewModel
{
    public class InvertVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
            {
                var visibility = (Visibility)value;

                if (visibility == Visibility.Visible)
                    return Visibility.Hidden;

                return Visibility.Visible;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class VisibilityToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
                return (Visibility)value == Visibility.Visible;

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InvertBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value)
                    return Visibility.Hidden;

                return Visibility.Visible;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

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

    public class CustomPizzaToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                if ((string)value == "Своя")
                    return Visibility.Visible;

                return Visibility.Hidden;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InvertCustomPizzaToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                if ((string)value == "Своя")
                    return Visibility.Hidden;

                return Visibility.Visible;
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

    public class OrderPartBottomLabelConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value[0] is DTO.PizzaSize)
            {
                var pizzaSize = value[0] as DTO.PizzaSize;
                var dough = value[1] as DTO.Dough;

                return $"{pizzaSize.Name} {pizzaSize.Size} см, {dough.Name} тесто";
            }

            return value;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            return new object[] { Binding.DoNothing, Binding.DoNothing };
        }
    }

    public class OrderHistoryPartBottomLabelConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value[0] is DTO.PizzaSize)
            {
                var pizzaSize = value[0] as DTO.PizzaSize;
                var dough = value[1] as DTO.Dough;
                var quantity = (int)value[2];

                return $"{pizzaSize.Name} {pizzaSize.Size} см, {dough.Name} тесто, x {quantity}";
            }

            return value;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            return new object[] { Binding.DoNothing, Binding.DoNothing, Binding.DoNothing };
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

    public class InvertPizzaQuantityToVisibilityConverter : IValueConverter
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

    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                var dt = (DateTime)value;

                var month = dt.Month.ToString();
                if (month.Length == 1) month = '0' + month;

                return $"{dt.Day}.{month}.{dt.Year} {dt.Hour}:{dt.Minute}";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class OrderStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DTO.OrderStatus)
            {
                var status = (DTO.OrderStatus)value;

                switch (status)
                {
                case DTO.OrderStatus.Cancellation: return "Отменён";
                case DTO.OrderStatus.Delivery: return "Доставляется";
                case DTO.OrderStatus.Preparation: return "Готовится";
                case DTO.OrderStatus.Success: return "Доставлен";
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

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

    public class LoginMenuVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value[0] is bool)
            {
                var regMenuVisible = (bool)value[0];
                var account = value[1];

                if (!regMenuVisible && account == null)
                    return Visibility.Visible;

                return Visibility.Hidden;
            }

            return value;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            return new object[] { Binding.DoNothing, Binding.DoNothing };
        }
    }

    public class RegistrationMenuVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value[0] is bool)
            {
                var regMenuVisible = (bool)value[0];
                var account = value[1];

                if (regMenuVisible && account == null)
                    return Visibility.Visible;

                return Visibility.Hidden;
            }

            return value;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            return new object[] { Binding.DoNothing, Binding.DoNothing, Binding.DoNothing };
        }
    }

    public class CloneDataConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            var objects = new object[targetType.Length];

            for (int i = 0; i < targetType.Length; i++)
                objects.Append(Binding.DoNothing);

            return objects;
        }
    }
}

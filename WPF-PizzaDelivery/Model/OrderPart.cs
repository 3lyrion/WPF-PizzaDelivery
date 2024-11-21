using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PizzaDelivery.Model
{
    public class OrderPart : INotifyPropertyChanged
    {
        public ObservableCollection<string> Models { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        string name;
        decimal cost;
        int quantity;
        string doughName;
        string sizeName;
        int sizeValue;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public decimal Cost
        {
            get { return cost; }
            set
            {
                cost = value;
                OnPropertyChanged("Cost");
            }
        }

        public int Quantity
        {
            get { return quantity; }
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged("Quantity");
                }
            }
        }

        public string DoughName
        {
            get { return doughName; }
            set
            {
                doughName = value;
                OnPropertyChanged("DoughName");
            }
        }

        public string SizeName
        {
            get { return sizeName; }
            set
            {
                sizeName = value;
                OnPropertyChanged("SizeName");
            }
        }

        public int SizeValue
        {
            get { return sizeValue; }
            set
            {
                sizeValue = value;
                OnPropertyChanged("SizeValue");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PD_Employee.Model
{
    public class PizzaSize : INotifyPropertyChanged
    {
        public ObservableCollection<string> Models { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        string name;
        double costMult;
        int size;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public double CostMult
        {
            get { return costMult; }
            set
            {
                costMult = value;
                OnPropertyChanged("CostMult");
            }
        }

        public int Size
        {
            get { return size; }
            set
            {
                size = value;
                OnPropertyChanged("Size");
            }
        }
    }
}

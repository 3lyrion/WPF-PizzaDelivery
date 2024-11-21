using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DTO = Interfaces.DTO;

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
        Model.Pizza pizza;
        DTO.Dough dough;
        DTO.PizzaSize pizzaSize;

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

        public Model.Pizza Pizza
        {
            get { return pizza; }
            set
            {
                pizza = value;
                OnPropertyChanged("Pizza");
            }
        }

        public DTO.Dough Dough
        {
            get { return dough; }
            set
            {
                dough = value;
                OnPropertyChanged("Dough");
            }
        }

        public DTO.PizzaSize PizzaSize
        {
            get { return pizzaSize; }
            set
            {
                pizzaSize = value;
                OnPropertyChanged("PizzaSize");
            }
        }
    }
}

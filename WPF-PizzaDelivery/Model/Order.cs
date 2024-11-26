using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PizzaDelivery.Model
{
    public enum OrderStatus
    {
        Preparation,
        Delivery,
        Success,
        Cancellation
    }

    public class Order : INotifyPropertyChanged
    {
        public ObservableCollection<string> Models { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        string address;
        decimal cost;
        DateTime creationTime;
        OrderStatus status;
        Model.OrderPart selectedPart;
        
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged("Address");
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

        public DateTime CreationTime
        {
            get { return creationTime; }
            set
            {
                creationTime = value;
                OnPropertyChanged("CreationTime");
            }
        }

        public OrderStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        public Model.OrderPart SelectedPart
        {
            get { return selectedPart; }
            set
            {
                selectedPart = value;
                OnPropertyChanged("SelectedPart");
            }
        }
    }
}

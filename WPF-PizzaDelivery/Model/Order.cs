using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DTO = Interfaces.DTO;

namespace PizzaDelivery.Model
{
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
        string recipientName;
        decimal cost;
        DateTime creationTime;
        DTO.OrderStatus status;
        List<Model.OrderPart> parts;
        
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged("Address");
            }
        }

        public string RecipientName
        {
            get { return recipientName; }
            set
            {
                recipientName = value;
                OnPropertyChanged("RecipientName");
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

        public DTO.OrderStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        public List<Model.OrderPart> Parts
        {
            get { return parts; }
            set
            {
                parts = value;
                OnPropertyChanged("Parts");
            }
        }

        public void Clear()
        {
            Address = "";
            Cost = 0.0m;
            CreationTime = new DateTime();
            Status = 0;
            Parts.Clear();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DTO = Interfaces.DTO;

namespace PizzaDelivery_EM.Model
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
        DateTime creationDate;
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

        public DateTime CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;
                OnPropertyChanged("CreationDate");
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
            CreationDate = default;
            Status = 0;
            Parts?.Clear();
        }
    }
}

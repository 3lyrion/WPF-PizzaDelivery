using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Runtime.CompilerServices;

namespace WPF_PizzaDelivery.ViewModel
{
    public class SelectPizzaButton : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        Visibility visibility = Visibility.Visible;
        public Visibility Visibility
        {
            get { return visibility; }
            set
            {
                visibility = value;
                OnPropertyChanged("Visibility");
            }
        }

        private RelayCommand clickCommand;
        public RelayCommand ClickCommand
        {
            get
            {
                return clickCommand ??
                  (clickCommand = new RelayCommand(obj =>
                  {
                      Visibility = Visibility.Hidden;
                  }));
            }
        }
    }
}

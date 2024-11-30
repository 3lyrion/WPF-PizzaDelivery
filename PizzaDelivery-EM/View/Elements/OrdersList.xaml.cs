using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PizzaDelivery_EM.View.Elements
{
    /// <summary>
    /// Interaction logic for OrdersList.xaml
    /// </summary>
    public partial class OrdersList : UserControl
    {
        public OrdersList()
        {
            InitializeComponent();
        }

        public static DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(IEnumerable<Model.Order>), typeof(OrdersList), new PropertyMetadata(null));

        public IEnumerable<Model.Order> Source
        {
            get { return GetValue(SourceProperty) as IEnumerable<Model.Order>; }

            set { SetValue(SourceProperty, value); }
        }
    }
}

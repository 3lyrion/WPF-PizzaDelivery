using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SV = Interfaces.Service;
using Ninject;

namespace WPF_PizzaDelivery
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        static void Main()
        {
            var application = new App();
            application.InitializeComponent();
            application.Run();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var kernel = new StandardKernel(new Util.NinjectRegistrations(), new Util.ReposModule("PizzaDeliveryDB"));

            SV.IClient clientService = kernel.Get<SV.IClient>();
            SV.ICourier courierService = kernel.Get<SV.ICourier>();
            SV.IDough doughService = kernel.Get<SV.IDough>();
            SV.IOrder orderService = kernel.Get<SV.IOrder>();
            SV.IPizza pizzaService = kernel.Get<SV.IPizza>();
            SV.IPizza_Order pizzaOrderService = kernel.Get<SV.IPizza_Order>();
            SV.IPizza_Size pizzaSizeService = kernel.Get<SV.IPizza_Size>();
            SV.IReport reportService = kernel.Get<SV.IReport>();

            var window = new MainWindow
            (
                clientService,
                courierService,
                doughService,
                orderService,
                pizzaService,
                pizzaOrderService,
                pizzaSizeService,
                reportService
            );

            window.Show();
        }
    }
}

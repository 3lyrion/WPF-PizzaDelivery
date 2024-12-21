using System;
using Ninject;
using SV = Interfaces.Service;

namespace PD_Server
{
    class Program
    {
        static void Main(string[] args)
        {
        //    Database.Creator.CreateIfNotExists();
            Database.Creator.CreateMoke();

            var kernel = new StandardKernel(new Util.NinjectRegistrations(), new Util.ReposModule("PizzaDeliveryDB"));

            var orderService = kernel.Get<SV.IOrder>();
            var reportService = kernel.Get<SV.IReport>();

            var app = new App(orderService, reportService);
            app.Start();
        }
    }
}

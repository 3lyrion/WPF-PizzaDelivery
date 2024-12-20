using System;
using Ninject;
using SV = Interfaces.Service;

namespace PizzaDelivery_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.Creator.CreateIfNotExists();

            var kernel = new StandardKernel(new Util.NinjectRegistrations(), new Util.ReposModule("PizzaDeliveryDB"));

            SV.IOrder orderService = kernel.Get<SV.IOrder>();

            var app = new App(orderService);
            app.Start();
        }
    }
}

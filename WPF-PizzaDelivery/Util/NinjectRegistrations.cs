using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Service;
using BLL.Service;

namespace PizzaDelivery.Util
{
    public class NinjectRegistrations : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IClient>().To<Client>();
            Bind<ICourier>().To<Courier>();
            Bind<IDough>().To<Dough>();
            Bind<IOrder>().To<Order>();
            Bind<IPizza>().To<Pizza>();
            Bind<IPizzaOrder>().To<PizzaOrder>();
            Bind<IPizzaSize>().To<PizzaSize>();
            Bind<IReport>().To<Report>();
        }
    }
}

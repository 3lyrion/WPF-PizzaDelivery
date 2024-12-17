using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Service;
using BLL.Service;

namespace PizzaDelivery_Server.Util
{
    public class NinjectRegistrations : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IOrder>().To<Order>();
        }
    }
}

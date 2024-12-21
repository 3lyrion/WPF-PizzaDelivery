using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Service;
using BLL.Service;

namespace PD_Employee.Util
{
    public class NinjectRegistrations : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IClient>().To<Client>();
            Bind<ICook>().To<Cook>();
            Bind<ICourier>().To<Courier>();
            Bind<IDough>().To<Dough>();
            Bind<IIngredient>().To<Ingredient>();
            Bind<IOrder>().To<Order>();
            Bind<IPizza>().To<Pizza>();
            Bind<IPizzaOrder>().To<PizzaOrder>();
            Bind<IPizzaSize>().To<PizzaSize>();
            Bind<IRecipe>().To<Recipe>();
            Bind<IReport>().To<Report>();
        }
    }
}

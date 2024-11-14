using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DM = DomainModel;

namespace Interfaces.Repository
{
    public interface IDbRepos
    {
        IRepository<DM.Client> client { get; }
        IRepository<DM.Cook> cook { get; }
        IRepository<DM.Courier> courier { get; }
        IRepository<DM.Dough> dough { get; }
        IRepository<DM.Ingredient> ingredient { get; }
        IRepository<DM.Order> order { get; }
        IRepository<DM.Order_Status> order_status { get; }
        IRepository<DM.Pizza> pizza { get; }
        IRepository<DM.Pizza_Order> pizza_order { get; }
        IRepository<DM.Pizza_Size> pizza_size { get; }
        IRepository<DM.Recipe> recipe { get; }

        IReportRepository report { get; }

        int save();
    }
}

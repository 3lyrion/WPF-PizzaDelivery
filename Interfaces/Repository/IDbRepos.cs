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
        IRepository<DM.Client> Client { get; }

        IRepository<DM.Cook> Cook { get; }

        IRepository<DM.Courier> Courier { get; }

        IRepository<DM.Dough> Dough { get; }

        IRepository<DM.Ingredient> Ingredient { get; }

        IRepository<DM.Order> Order { get; }

        IRepository<DM.Pizza> Pizza { get; }

        IRepository<DM.Pizza_Order> Pizza_Order { get; }

        IRepository<DM.Pizza_Size> Pizza_Size { get; }

        IRepository<DM.Recipe> Recipe { get; }

        IReportRepository Report { get; }

        ITransactionRepository Transaction { get; }

        int Save();
    }
}

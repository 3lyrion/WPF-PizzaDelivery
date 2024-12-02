using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repository
{
    public interface ITransactionRepository
    {
        void PassOrderToCook(int orderId);

        void PassOrderToCourier(int OrderId);

        void CloseOrder(int id, int status);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repository
{
    public interface IRepository<T> where T : class
    {
        List<T> getList();

        T getItem(int id);

        int create(T item);
        void update(T item);
        void delete(int id);
    }
}

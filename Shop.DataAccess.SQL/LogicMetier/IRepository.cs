using Shop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataAccess.SQL.LogicMetier
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> Collection();
        void DeleteById(int id);
        T FindById(int id);
        void SaveChanges();
        void Update(T t);
        void Insert(T t);
    }
}

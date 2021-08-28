using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenBook.MVVM.Models
{
    interface IRepository<T> : IDisposable where T : class
    {
        void Create(T item);
        void Update(T item);
        void Delete(T item);
        T GetItem(int id);
        ObservableCollection<T> GetItems();

    }
}

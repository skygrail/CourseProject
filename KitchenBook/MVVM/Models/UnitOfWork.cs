using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenBook.MVVM.Models
{
    class UnitOfWork : IDisposable
    {
        private KitchenBookEntities db;

        private UserRepository userRepository = null;
        private RecipesRepository recipesRepository = null;
        

        public UserRepository Users
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new UserRepository(db);
                }
                return userRepository;
            }
        }

        public RecipesRepository Recipes
        {
            get
            {
                if (recipesRepository == null)
                {
                    recipesRepository = new RecipesRepository(db);
                }
                return recipesRepository;
            }
        }

        

        public UnitOfWork()
        {
            db = new KitchenBookEntities();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenBook.MVVM.Models
{
    class RecipesRepository : IRepository<Recipes>
    {
        private KitchenBookEntities db;

        public RecipesRepository(KitchenBookEntities context)
        {
            db = context;
        }

        public void Create(Recipes item)
        {
            if (item != null)
                db.Recipes.Add(item);

        }
        public void Create(Recipe item)
        {
            if (item != null)
                db.Recipe.Add(item);

        }

        public void Delete(Recipes item)
        {
            if (item != null)
            {
                db.Recipes.Remove(item);
            }
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

        public Recipes GetItem(int id)
        {
            return db.Recipes.FirstOrDefault();
        
        }public Recipes GetLast()
        {
            return db.Recipes.OrderByDescending(c => c.ID_recipe).FirstOrDefault();
        }
        public Recipes GetMostPops()
        {
            List<Recipes> list = db.Recipes.ToList();
            list = new List<Recipes>(list.OrderByDescending(c => c.Popularity));
            return list[0];

        }
        public List<Recipes> GetMostInterest(UserFile user)
        {
            List<Recipes> list = db.Recipes.ToList();
            list = new List<Recipes>(list.OrderByDescending(c => c.Popularity));
            if (user != null)
            {
                if (user.Category != null)
                    list = new List<Recipes>(list.Where(c => c.Category == user.Category));
                else
                    list = new List<Recipes>(list.Where(c => c.Category == list[0].Category));
            }
            else
                list = new List<Recipes>(list.Where(c => c.Category == list[0].Category));
            return new List<Recipes> { list[0], list[1], list[2] };

        }

        public ObservableCollection<Recipes> GetItems()
        {
            ObservableCollection<Recipes> temp = new ObservableCollection<Recipes>(db.Recipes.ToList());
            foreach (var item in temp)
            {
                string str = "      ";
                int dotCount = 0;
                char prev = item.Description[0];
                foreach (char ch in item.Description)
                {
                    if (ch == '.')
                        dotCount++;

                    if (char.IsWhiteSpace(ch))
                    {

                        if (!char.IsWhiteSpace(prev))
                        {
                            if (prev == '.' && dotCount > 5)
                            {
                                str += '\n';
                                str += "      ";
                                dotCount = 0;
                            }
                            else
                                str += ' ';
                        }

                    }
                    else
                        str += ch;
                    prev = ch;
                }
                item.Description = str;
                SaveChanges(item);
            }


            return temp;
        }
        private void SaveChanges(Recipes item)
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public ObservableCollection<Recipes> GetItems(string category)
        {
            ObservableCollection<Recipes> temp = new ObservableCollection<Recipes>(db.Recipes.Where(c => c.Category == category).ToList());
            foreach(var item in temp)
            {
                string str = "      ";
                int dotCount = 0;
                char prev = item.Description[0];
                foreach(char ch in item.Description)
                {
                    if (ch == '.')
                        dotCount++;

                    if(char.IsWhiteSpace(ch))
                    {

                        if (!char.IsWhiteSpace(prev))
                        {
                            if (prev == '.' && dotCount > 5)
                            {
                                str += '\n';
                                str += "      ";
                                dotCount = 0;
                            }
                            else
                                str += ' ';
                        }

                    }
                    else
                        str += ch;
                    prev = ch;
                }
                item.Description = str;
            }

            return temp;
        }

        public void Update(Recipes item)
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace KitchenBook.MVVM.Models
{
    class UserRepository : IRepository<UserFile>
    {
        private KitchenBookEntities db;

        public UserRepository(KitchenBookEntities context)
        {
            db = context;

        }

        public void Create(UserFile item)
        {
            Users temp = new Users();
            temp.SecretInfo = new SecretInfo();
            temp.Name = item.Name;
            temp.Surame = item.LastName;
            temp.Email = item.Email;
            temp.SecretInfo.Login = item.Login;
            temp.SecretInfo.Password = item.Password;
            temp.Image = item.Image;
            db.Users.Add(temp);
            temp.SecretInfo.ID_User = temp.ID_User;
            db.SecretInfo.Add(temp.SecretInfo);
        }

        public void CreateAdmin()
        {
            Users temp = new Users();
            temp.SecretInfo = new SecretInfo();
            temp.Name = "Админ";
            temp.Surame = "Админ";
            temp.Email = "admin@mail.ru";
            temp.SecretInfo.Login = "admin";
            temp.Admin = true;
            //temp.SecretInfo.Password = ViewModel.HashingPasswords.HashPassword("12345678");
            //temp.Image = ViewModel.Image.Convert(@"");
            temp.SecretInfo.ID_User = temp.ID_User;
            db.Users.Add(temp);
            db.SecretInfo.Add(temp.SecretInfo);
        }

        public void Delete(UserFile item)
        {
            Users user = db.Users.FirstOrDefault(c => c.SecretInfo.Login == item.Login);
            if (user != null)
            {
                SecretInfo secret = user.SecretInfo;
                db.Users.Remove(user);
                db.SecretInfo.Remove(secret);
            }
        }

        public bool IsAdmin(UserFile item)
        {
            Users temp = db.Users.FirstOrDefault(c => c.ID_User == item.ID_user);
            if (temp.Admin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool SearchAdmin()
        {
            foreach (var item in db.Users)
            {
                if (item.Admin)
                {
                    return true;
                }
            }
            return false;
        }
        public bool SearchFav(int ID_recipe)
        {
            foreach (var item in db.UsersRecipes)
            {
                if(item.ID_User == UserFile.user.ID_user && item.ID_recipe == ID_recipe)
                {
                    return true;
                }
            }
            return false;
        }
        public ObservableCollection<Recipes> GetFavs(UserFile user)
        {
            ObservableCollection<Recipes> temp = new ObservableCollection<Recipes>();
            foreach(var item in db.UsersRecipes)
            {
                if (item.ID_User == user.ID_user)
                    temp.Add(item.Recipes);
            }
            return temp;  
        }
        public ObservableCollection<Recipes> GetMyRecipes(UserFile user)
        {
            return new ObservableCollection<Recipes>(db.Recipes.Where(c => c.ID_author == user.ID_user));
        }
        public bool AddFav(int ID_recipe)
        {
            UsersRecipes temp = db.UsersRecipes.FirstOrDefault(c => c.ID_User == UserFile.user.ID_user && c.ID_recipe == ID_recipe);
            if(temp == null)
            {
                temp = new UsersRecipes();
                temp.ID_recipe = ID_recipe;
                temp.ID_User = UserFile.user.ID_user;

                db.UsersRecipes.Add(temp);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool RemoveFav(int ID_recipe)
        {
            UsersRecipes temp = db.UsersRecipes.FirstOrDefault(c => c.ID_User == UserFile.user.ID_user && c.ID_recipe == ID_recipe);
            if(temp != null)
            {
                
                db.UsersRecipes.Remove(temp);
                return true;
            }
            else
            {
                return false;
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

        public UserFile GetItem(int id)
        {
            Users item = db.Users.FirstOrDefault(s => s.SecretInfo.ID_User == id);
            if (item != null)
            {
                return new UserFile(item.Name, item.Surame, item.Email, item.SecretInfo.Login, item.SecretInfo.Password, item.Image, item.ID_User, item.Category);
            }
            else return null;
        }

        public UserFile GetItem(string login)
        {
            Users item = db.Users.FirstOrDefault(s => s.SecretInfo.Login == login);
            if (item != null)
            {
                return new UserFile(item.Name, item.Surame, item.Email, item.SecretInfo.Login, item.SecretInfo.Password, item.Image, item.ID_User, item.Category);
            }
            else return null;
        }

        public ObservableCollection<UserFile> GetItems()
        {
            ObservableCollection<UserFile> userfiles = new ObservableCollection<UserFile>();
            foreach (var item in db.Users)
            {
                if (!item.Admin)
                    userfiles.Add(new UserFile(item.Name, item.Surame, item.Email, item.SecretInfo.Login, item.SecretInfo.Password, item.Image));
            }
            return userfiles;
        }

        public void Update(UserFile item)
        {
            Users temp = db.Users.FirstOrDefault(c => c.ID_User == item.ID_user);

            temp.Name = item.Name;
            temp.Image = item.Image;
            temp.Surame = item.LastName;
            temp.Email = item.Email;
            temp.Category = item.Category;
            db.Entry(temp).State = System.Data.Entity.EntityState.Modified;
        }
    }
}

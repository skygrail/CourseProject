using KitchenBook.Commands;
using KitchenBook.MVVM.Models;
using KitchenBook.MVVM.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KitchenBook.MVVM.ViewModels
{
    class AdminViewVM : ViewModel
    {
        private UnitOfWork unit;
        private MessageBoxService MessageBoxService;
        public ObservableCollection<Recipes> RecipesAll { get; set; }
        public ObservableCollection<UserFile> Users { get; set; }

        private Recipes selectedrecipe = null;
        public Recipes SelectedRecipe
        {
            get
            {
                return selectedrecipe;
            }
            set
            {
                selectedrecipe = value;
                OnPropertyChanged("SelectedRecipe");
            }
        }
        private UserFile selecteditem = null;
        public UserFile SelectedItem
        {
            get
            {
                return selecteditem;
            }
            set
            {
                selecteditem = value;
                OnPropertyChanged("SelectedItem");
            }
        }


        public AdminViewVM()
        {
            MessageBoxService = new MessageBoxService();
            unit = new UnitOfWork();
            Users = unit.Users.GetItems();

            RecipesAll = unit.Recipes.GetItems();


            
        }


        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                    (deleteCommand = new RelayCommand((obj) =>
                    {
                        bool result = MessageBoxService.ShowMessage("Действительно хотите это сделать?", "Удаление аккаунта", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if(result)
                        {
                            
                            unit.Users.Delete(SelectedItem);
                            unit.Save();
                            Users.Remove(SelectedItem);
                            
                        }
                        SelectedItem = null;

                        

                    }, (o) => SelectedItem != null));
            }
        }
        private RelayCommand delrecCommand;
        public RelayCommand DelRecCommand
        {
            get
            {
                return delrecCommand ??
                    (delrecCommand = new RelayCommand((obj) =>
                    {
                        bool result = MessageBoxService.ShowMessage("Действительно хотите это сделать?", "Удаление рецепта", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if(result)
                        {
                            
                            unit.Recipes.Delete(SelectedRecipe);
                            RecipesAll.Remove(SelectedRecipe);
                            unit.Save();
                            
                        }
                        SelectedRecipe = null;

                        

                    }, (o) => SelectedRecipe != null));
            }
        }
    }
}

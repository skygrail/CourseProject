using KitchenBook.Commands;
using KitchenBook.MVVM.Models;
using KitchenBook.MVVM.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenBook.MVVM.ViewModels
{
    public class ItemsViewModel : ViewModel
    {
        private Recipes selectedrecipe;
        private MainViewModel vm;
        public ObservableCollection<Recipes> RecipesAll { get; set; }
        public RelayCommand SelectionChangedCommand { get; set; }
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
        private UnitOfWork unit;
        public ItemsViewModel(MainViewModel vm)
        {
            unit = new UnitOfWork();
            RecipesAll = unit.Recipes.GetItems();
            unit.Dispose();
            this.vm = vm;
            SelectionChangedCommand = new RelayCommand((obj) =>
            {
                if(SelectedRecipe != null)
                {
                    vm.CurrentView = new RecipeViewModel(SelectedRecipe, vm, this);
                    SelectedRecipe = null;
                }
            });
        }
        public ItemsViewModel(MainViewModel vm, string search)
        {
            unit = new UnitOfWork();
            RecipesAll = unit.Recipes.GetItems();
            unit.Dispose();
            RecipesAll = new ObservableCollection<Recipes>(RecipesAll.Where(c => c.NameRecipe.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0));
            this.vm = vm;
            SelectionChangedCommand = new RelayCommand((obj) =>
            {
                if(SelectedRecipe != null)
                {
                    vm.CurrentView = new RecipeViewModel(SelectedRecipe, vm, this);
                    SelectedRecipe = null;
                }
            });
        }


    }
}

using KitchenBook.Commands;
using KitchenBook.MVVM.Models;
using KitchenBook.MVVM.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenBook.MVVM.ViewModels
{
    public class RecipeViewModel : ViewModel
    {
        public Recipes SelectedRecipe { get; set; }
        private UnitOfWork unit;
        private List<string> state;
        public string Author { get; set; } = "";
        private string favourite;
        public string Favourite
        {
            get
            {
                return favourite;
            }
            set
            {
                favourite = value;
                OnPropertyChanged("Favourite");
            }
        }
        private MainViewModel vm;
        private ViewModel CurrentView;


        public RecipeViewModel(Recipes SelectedItem, MainViewModel vm, ViewModel CurrentView)
        {
            SelectedRecipe = SelectedItem;
            
            
            unit = new UnitOfWork();
            SelectedRecipe.Popularity++;
            unit.Recipes.Update(SelectedRecipe);
            unit.Save();
            if (SelectedRecipe.ID_author != null)
            {
                UserFile temp = unit.Users.GetItem((int)SelectedRecipe.ID_author);
                Author = $"@{temp.Login}";
            }

            this.vm = vm;
            this.CurrentView = CurrentView;
            state = new List<string> { @"D:\VisualStudio\CourseProject (with Admin)\CourseProject\KitchenBook\Images\love.png",
                @"D:\VisualStudio\CourseProject (with Admin)\CourseProject\KitchenBook\Images\lovered.png",
                @"D:\VisualStudio\CourseProject (with Admin)\CourseProject\KitchenBook\Images\cantlike.png" };
            if(UserFile.user == null)
            {
                Favourite = state[2];
            }
            else
            {
                UserFile.user.AddVisit(SelectedRecipe.Category);
                if(unit.Users.SearchFav(SelectedRecipe.ID_recipe))
                    Favourite = state[1];
                else
                    Favourite = state[0];
            }
            

        }
        private RelayCommand backCommand;
        public RelayCommand BackCommand
        {
            get
            {
                return backCommand ??
                    (backCommand = new RelayCommand((obj) =>
                    {
                       
                        if(CurrentView is ItemsViewModel)
                            vm.CurrentView = (ItemsViewModel)CurrentView;
                        else if(CurrentView is HomeViewModel)
                            vm.CurrentView = (HomeViewModel)CurrentView;
                        else if(CurrentView is SearchViewModel)
                            vm.CurrentView = (SearchViewModel)CurrentView;
                        else
                            vm.CurrentView = new CabinetViewModel(vm);

                        unit.Dispose();

                    }));
            }
        }
        private RelayCommand favCommand;
        public RelayCommand FavCommand
        {
            get
            {
                return favCommand ??
                    (favCommand = new RelayCommand((obj) =>
                    {
                        
                        if(Favourite == state[0])
                        {
                            if(unit.Users.AddFav(SelectedRecipe.ID_recipe))
                                Favourite = state[1];
                        }
                        else
                        {
                            if (unit.Users.RemoveFav(SelectedRecipe.ID_recipe))
                                Favourite = state[0];
                        }
                        unit.Save();
                        


                    }, (obj) => UserFile.user != null));
            }
        }
    }
}

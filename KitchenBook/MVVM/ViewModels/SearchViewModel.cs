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
    public class SearchViewModel : ViewModel
    {
        private UnitOfWork unit;
        private Recipes selectedrecipe;
        private MainViewModel vm;
        private ObservableCollection<Recipes> recipes;
        public RelayCommand SelectionChangedCommand { get; set; }
        private string firstIN = "";
        private string secondIN = "";
        private string thirdIN = "";
        public string FirstIN
        {
            get
            {
                return firstIN;
            }
            set
            {
                firstIN = value;
                OnPropertyChanged("FirstIN");
            }
        }
        public string SecondIN
        {
            get
            {
                return secondIN;
            }
            set
            {
                secondIN = value;
                OnPropertyChanged("SecondIN");
            }
        }
        public string ThirdIN
        {
            get
            {
                return thirdIN;
            }
            set
            {
                thirdIN = value;
                OnPropertyChanged("ThirdIN");
            }
        }

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
        public ObservableCollection<Recipes> RecipesCategory
        {
            get
            {
                return recipes;
            }
            set
            {
                recipes = value;
                OnPropertyChanged("RecipesCategory");
            }
        }

        public SearchViewModel(MainViewModel vm)
        {
            this.vm = vm;
            unit = new UnitOfWork();
            RecipesCategory = unit.Recipes.GetItems("Выпечка");
            cat = "Выпечка";

            unit.Dispose();
            SelectionChangedCommand = new RelayCommand((obj) =>
            {
                if (SelectedRecipe != null)
                {
                    vm.CurrentView = new RecipeViewModel(SelectedRecipe, vm, this);
                    SelectedRecipe = null;
                }
            });

        }
        private string cat;


        private RelayCommand changeCommand;
        public RelayCommand ChangeCommand
        {
            get
            {
                return changeCommand ??
                    (changeCommand = new RelayCommand((obj) =>
                    {
                        unit = new UnitOfWork();
                        string category = obj as string;
                        cat = category;
                        FirstIN = "";
                        SecondIN = "";
                        ThirdIN = "";
                        switch (category)
                        {
                            case "Выпечка":
                                RecipesCategory = unit.Recipes.GetItems("Выпечка");
                                break;
                            case "Напитки":
                                RecipesCategory = unit.Recipes.GetItems("Напиток");
                                cat = "Напиток";
                                break;
                            case "Второе":
                                RecipesCategory = unit.Recipes.GetItems("Второе");
                                break;
                            case "Салат":
                                RecipesCategory = unit.Recipes.GetItems("Салат");
                                break;
                            case "Суп":
                                RecipesCategory = unit.Recipes.GetItems("Суп");
                                break;

                        }
                        

                        unit.Dispose();
                    }));
            }
        }
        private RelayCommand searchCommand;
        public RelayCommand SearchCommand
        {
            get
            {
                return searchCommand ??
                    (searchCommand = new RelayCommand((obj) =>
                    {
                        unit = new UnitOfWork();
                        RecipesCategory = unit.Recipes.GetItems(cat);
                        ObservableCollection<Recipes> tempCol;
                        switch (ToSearch())
                        {
                            case 1:

                                tempCol = new ObservableCollection<Recipes>();
                                foreach (var item in RecipesCategory)
                                {
                                    var col = item.Recipe.Select(c => c.Ingredient.ToLower());
                                    if (col.Any(c => c.StartsWith(FirstIN.ToLower())))
                                    {
                                        tempCol.Add(item);
                                    }
                                }
                                RecipesCategory = tempCol;

                                break;
                            case 2:

                                tempCol = new ObservableCollection<Recipes>();
                                foreach (var item in RecipesCategory)
                                {
                                    var col = item.Recipe.Select(c => c.Ingredient.ToLower());

                                    if (col.Any(c => c.StartsWith(FirstIN.ToLower())) && col.Any(c => c.StartsWith(FirstIN.ToLower())))
                                    {
                                        tempCol.Add(item);
                                    }
                                }
                                RecipesCategory = tempCol;

                                break;

                            case 3:
                                tempCol = new ObservableCollection<Recipes>();
                                foreach (var item in RecipesCategory)
                                {
                                    var col = item.Recipe.Select(c => c.Ingredient.ToLower());

                                    if (col.Any(c => c.StartsWith(FirstIN.ToLower())) && col.Any(c => c.StartsWith(FirstIN.ToLower())) && col.Any(c => c.StartsWith(ThirdIN.ToLower())))
                                    {
                                        tempCol.Add(item);
                                    }
                                }
                                RecipesCategory = tempCol;
                                break;
                            default:
                                RecipesCategory = unit.Recipes.GetItems(cat);
                                break;
                        }
                        unit.Dispose();
                        

                    }));
            }
        }



        private int ToSearch()
        {
            int result = 0;
            if (FirstIN != "" && SecondIN != "" && ThirdIN != "")
                result = 3;
            else if (FirstIN != "" && SecondIN != "" && ThirdIN == "")
                result = 2;
            else if (FirstIN != "" && SecondIN == "" && ThirdIN == "")
                result = 1;
            else if (FirstIN == "" && SecondIN != "" && ThirdIN == "")
            {
                result = 1;
                FirstIN = SecondIN;
                SecondIN = "";
            }
            else if (FirstIN == "" && SecondIN == "" && ThirdIN != "")
            {
                result = 1;
                FirstIN = ThirdIN;
                ThirdIN = "";
            }
            else if (FirstIN != "" && SecondIN == "" && ThirdIN != "")
            {
                result = 2;
                SecondIN = ThirdIN;
                ThirdIN = "";
            }
            else if (FirstIN == "" && SecondIN != "" && ThirdIN != "")
            {
                result = 2;
                FirstIN = SecondIN;
                SecondIN = ThirdIN;
                ThirdIN = "";
            }

            return result;
        }
    }
}

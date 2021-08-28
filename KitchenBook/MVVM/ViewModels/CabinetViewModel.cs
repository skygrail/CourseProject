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
    public class CabinetViewModel : ViewModel
    {
        private string image;
        public string Ingr { get; set; } = "";
        public string Massingr { get; set; } = "";
        private string category;
        public string Category
        {
            get
            {
                return category;
            }
            set
            {
                category = value;
                NewRec.Category = category;
                OnPropertyChanged("Category");
            }
        }

        public ObservableCollection<Ingredient> Ingredients { get; set; }
        public RecipeToAdd NewRec { get; set; }
        public RelayCommand SelectionChangedCommand { get; set; }
        private Recipes selectedrecipe;
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
        public bool Edit { get; set; } = true;
        private MessageBoxService MessageBoxService;
        public ObservableCollection<Recipes> RecipesAll { get; set; }
        public ObservableCollection<Recipes> MyRecipes { get; set; }
        public UserFile User { get; set; }
        private UnitOfWork unit;
        public string ImageEdit
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                OnPropertyChanged("ImageEdit");
            }
        }
        private List<string> images;
        public List<string> types { get; set; }

        private MainViewModel vm;
        public CabinetViewModel(MainViewModel vm)
        {
            



            this.vm = vm;
            images = new List<string> { @"D:\VisualStudio\CourseProject (with Admin)\CourseProject\KitchenBook\Images\writing.png",
                @"D:\VisualStudio\CourseProject (with Admin)\CourseProject\KitchenBook\Images\stop.png" };
            ImageEdit = images[0];
            User = vm.User;
            MessageBoxService = new MessageBoxService();

            unit = new UnitOfWork();
            RecipesAll = unit.Users.GetFavs(User);

            MyRecipes = unit.Users.GetMyRecipes(User);
            NewRec = new RecipeToAdd(User.ID_user, User.Login);
            unit.Dispose();

            types = new List<string>();

            foreach(var item in UserFile.user.Visit)
            {
                types.Add(item.Key);
            }
            Ingredients = new ObservableCollection<Ingredient>();


            SelectionChangedCommand = new RelayCommand((obj) =>
            {
                if (SelectedRecipe != null)
                {
                    vm.CurrentView = new RecipeViewModel(SelectedRecipe, vm, this);
                    SelectedRecipe = null;
                }
            });
        }


        private RelayCommand editCommand;
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                    (editCommand = new RelayCommand((obj) =>
                    {
                        if (Edit)
                        {
                            Edit = false;
                            OnPropertyChanged("Edit");
                            ImageEdit = images[1];
                        }
                        else
                        {
                            if (!User.HasErrors)
                            {
                                Edit = true;
                                OnPropertyChanged("Edit");
                                ImageEdit = images[0];
                                unit = new UnitOfWork();
                                unit.Users.Update(User);
                                unit.Save();
                                unit.Dispose();
                            }
                        }
                    }));
            }
        }
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand((obj) =>
                    {
                        Recipes temp = new Recipes();
                        temp.Category = NewRec.Category;
                        temp.Description = NewRec.Description;
                        temp.ID_author = NewRec.ID_author;
                        temp.Image = NewRec.Image;
                        temp.NameRecipe = NewRec.NameRecipe;
                        
                        unit = new UnitOfWork();
                        unit.Recipes.Create(temp);
                        unit.Save();
                        temp = unit.Recipes.GetLast();
                        foreach(var item in NewRec.ings)
                        {
                            Recipe rec = new Recipe();
                            rec.Ingredient = item.Ingrediet;
                            rec.MassIngredient = item.MassIngredient;
                            rec.ID_recipe = temp.ID_recipe;
                            unit.Recipes.Create(rec);
                            unit.Save();
                        }
                        MyRecipes = unit.Users.GetMyRecipes(User);
                        OnPropertyChanged("MyRecipes");
                        NewRec = new RecipeToAdd(User.ID_user, User.Login);
                        OnPropertyChanged("NewRec");
                        unit.Dispose();
                        Ingredients = new ObservableCollection<Ingredient>();
                        OnPropertyChanged("Ingredients");
                        

                    }, (obj) => !NewRec.HasErrors));
            }
        }
        private RelayCommand picCommand;
        public RelayCommand PicCommand
        {
            get
            {
                return picCommand ??
                    (picCommand = new RelayCommand((obj) =>
                    {
                        unit = new UnitOfWork();
                        Image.Open();
                        if (Image.file_image != "")
                            User.Image = Image.Convert(Image.file_image);
                        unit.Users.Update(User);
                        unit.Save();
                        unit.Dispose();

                    }));
            }
        }
        private RelayCommand imageCommand;
        public RelayCommand ImageCommand
        {
            get
            {
                return imageCommand ??
                    (imageCommand = new RelayCommand((obj) =>
                    {
                        Image.Open();
                        if (Image.file_image != "")
                            NewRec.Image = Image.Convert(Image.file_image);


                    }));
            }
        }
        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                    (deleteCommand = new RelayCommand((obj) =>
                    {
                        unit = new UnitOfWork();
                        bool result = MessageBoxService.ShowMessage("Действительно хотите это сделать?", "Удаление аккаунта", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result)
                        {
                            unit.Users.Delete(User);
                            unit.Save();
                            unit.Dispose();
                            vm.User = null;
                        }
                        

                    }, (obj) => Edit));
            }
        }
        private RelayCommand logoutommand;
        public RelayCommand LogOutCommand
        {
            get
            {
                return logoutommand ??
                    (logoutommand = new RelayCommand((obj) =>
                    {
                        vm.User = null;
                        vm.Admin = false;
                        vm.CurrentView = new EnterViewModel(vm);
                        

                    }, (obj) => Edit));
            }
        }
        private RelayCommand ingrcommand;
        public RelayCommand IngrCommand
        {
            get
            {
                return ingrcommand ??
                    (ingrcommand = new RelayCommand((obj) =>
                    {
                        NewRec.ings = new List<Ingredient>();
                        Ingredient temp = new Ingredient { Ingrediet = Ingr, MassIngredient = Massingr };
                        NewRec.ings.Add(temp);
                        Ingredients.Add(temp);
                        Ingr = "";
                        OnPropertyChanged("Ingr");
                        Massingr = "";
                        OnPropertyChanged("Massingr");

                    }, (obj) => Ingr != "" && Massingr != ""));
            }
        }
    }
}

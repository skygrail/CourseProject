using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KitchenBook.MVVM.ViewModels.Base;
using KitchenBook.Commands;
using KitchenBook.MVVM.ViewModels;
using KitchenBook.MVVM.Models;

namespace KitchenBook.MVVM.ViewModels
{
    public class HomeViewModel : ViewModel //обработать отсутствие и присутствие категории у пользователя
    {
        
        public Recipes MostPop { get; set; }
        
        public Recipes Interest1 { get; set; }
        public Recipes Interest2 { get; set; }
        public Recipes Interest3 { get; set; }
        private UnitOfWork unit;
        
        private MainViewModel mainVM;

        public HomeViewModel(MainViewModel mainVM)
        {
            unit = new UnitOfWork();
            
            MostPop = unit.Recipes.GetMostPops();


            List<Recipes> userRec = unit.Recipes.GetMostInterest(UserFile.user);
            Interest1 = userRec[0];
            Interest2 = userRec[1];
            Interest3 = userRec[2];
            unit.Dispose();
            this.mainVM = mainVM;
            
            

            
        }


        private RelayCommand mostPopCommand;
        public RelayCommand MostPopCommand
        {
            get
            {
                return mostPopCommand ??
                    (mostPopCommand = new RelayCommand((obj) =>
                    {
                        mainVM.CurrentView = new RecipeViewModel(MostPop, mainVM, this);

                    }));
            }
        }
        private RelayCommand interestCommand;
        public RelayCommand InterestCommand
        {
            get
            {
                return interestCommand ??
                    (interestCommand = new RelayCommand((obj) =>
                    {
                        string text = obj as string;
                        if(text != null)
                        {
                            switch(text)
                            {
                                case "interest1":
                                    mainVM.CurrentView = new RecipeViewModel(Interest1, mainVM, this);
                                    break;
                                case "interest2":
                                    mainVM.CurrentView = new RecipeViewModel(Interest2, mainVM, this);
                                    break;
                                case "interest3":
                                    mainVM.CurrentView = new RecipeViewModel(Interest3, mainVM, this);
                                    break;
                                default:
                                    break;
                            }
                            
                        }

                    }));
            }
        }
    }
}

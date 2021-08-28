using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KitchenBook.MVVM.ViewModels.Base;
using KitchenBook.Commands;
using KitchenBook.MVVM.Models;
using System.Windows.Controls;

namespace KitchenBook.MVVM.ViewModels
{
    public class MainViewModel: ViewModel
    {
        private UnitOfWork unit;
        private bool admin = false;
        public bool Admin
        {
            get
            {
                return admin;
            }
            set
            {
                admin = value;
                OnPropertyChanged("Admin");
            }
        }
        public RelayCommand HomeViewCommand { get; set; }
        private string search = "";
        public string Search
        {
            get
            {
                return search;
            }
            set
            {
                search = value;
                OnPropertyChanged("Search");
            }
        }
        public RelayCommand ItemsViewCommand { get; set; }
        public RelayCommand SearchViewCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand AdminCommand { get; set; }
        public RelayCommand CabinetViewCommand { get; set; }
        public RelayCommand EnterViewCommand { get; set; }
        private RelayCommand closingCommand;
        public RelayCommand ClosingCommand
        {
            get
            {
                return closingCommand ??
                    (closingCommand = new RelayCommand((obj) =>
                    {
                        CurrentView = null;
                        unit = new UnitOfWork();

                        var result = UserFile.user.Visit.FirstOrDefault(s => s.Value == UserFile.user.Visit.Max(c => c.Value));

                        UserFile.user.Category = result.Key;
                        unit.Users.Update(UserFile.user);
                        unit.Save();

                    },(obj) => UserFile.user != null ));
            }
        }
        public HomeViewModel HomeVM { get; set; }
        public ItemsViewModel ItemsVM { get; set; }
        public SearchViewModel SearchVM { get; set; }
        public CabinetViewModel CabinetVM { get; set; }
        public EnterViewModel EnterVM { get; set; }

        private object _currentView;
        private UserFile user = null;
        public UserFile User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }

        public object CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }
        public MainViewModel()
        {
            EnterVM = new EnterViewModel(this);

            CurrentView = new HomeViewModel(this);

            HomeViewCommand = new RelayCommand(o => 
            {
                CurrentView = new HomeViewModel(this);
            });

            ItemsViewCommand = new RelayCommand(o =>
            {
                CurrentView = new ItemsViewModel(this);
            });

            SearchCommand = new RelayCommand(o =>
            {
                RadioButton but = o as RadioButton;
                if(but != null)
                {
                    but.IsChecked = true;
                    CurrentView = new ItemsViewModel(this, Search);
                    Search = "";

                }
            }, (o) => search != "");

            CabinetViewCommand = new RelayCommand(o =>
            {
                CurrentView = new CabinetViewModel(this);
            }, (o) => User != null);
            SearchViewCommand = new RelayCommand(o =>
            {
                CurrentView = new SearchViewModel(this);
            });

            EnterViewCommand = new RelayCommand(o =>
            {
                CurrentView = EnterVM;
            });
            AdminCommand = new RelayCommand(o =>
            {
                CurrentView = new AdminViewVM();
            });
        }
    }
}

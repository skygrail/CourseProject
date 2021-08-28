using Experimental.System.Messaging;
using KitchenBook.Commands;
using KitchenBook.MVVM.Models;
using KitchenBook.MVVM.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows.Forms;

namespace KitchenBook.MVVM.ViewModels
{
    public class EnterViewModel : ViewModel
    {
        private UnitOfWork unit;
        private MessageBoxService MessageBoxService;
        private MainViewModel vm;
        public UserFile newUser { get; set; }
        private string login = "";
        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }
        private string password = "";
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged("Password");
                
            }
        }
        private RelayCommand enterCommand;
        public RelayCommand EnterCommand
        {
            get
            {
                return enterCommand ??
                  (enterCommand = new RelayCommand(obj =>
                  {
                      unit = new UnitOfWork();
                      if (Login == "" || Password == "0")
                      {
                          MessageBoxService.ShowMessage("Не все поля заполнены!", "Вход", MessageBoxButtons.OK, MessageBoxIcon.Information);
                      }
                      else {
                          UserFile.user = unit.Users.GetItem(Login);
                          if (UserFile.user != null)
                          {
                              if (Password == UserFile.user.Password)
                              {
                                  
                                  MessageBoxService.ShowMessage("Вход выполнен успешно!", "Вход", MessageBoxButtons.OK, MessageBoxIcon.None);
                                  Login = "";
                                  Password = "";
                                  vm.User = UserFile.user;
                                  vm.CurrentView = new HomeViewModel(vm);
                                  if(unit.Users.IsAdmin(UserFile.user))
                                  {
                                      vm.Admin = true;
                                  }
                              }
                              else
                              {
                                  MessageBoxService.ShowMessage("Пароль неверный!", "Вход", MessageBoxButtons.OK, MessageBoxIcon.Information);
                              }
                          }
                          else
                          {
                              MessageBoxService.ShowMessage("Такого пользователя нет!", "Вход", MessageBoxButtons.OK, MessageBoxIcon.Information);
                          }
                      }
                      unit.Dispose();
                  }));

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
                        Image.Open();
                        if (Image.file_image != "")
                            newUser.Image = Image.Convert(Image.file_image);

                    }));
            }
        }
        private RelayCommand regCommand;
        public RelayCommand RegCommand
        {
            get
            {
                return regCommand ??
                    (regCommand = new RelayCommand((obj) =>
                    {
                        unit = new UnitOfWork();
                        TabItem temp = obj as TabItem;
                        unit.Users.Create(newUser);
                        unit.Save();
                        MessageBoxService.ShowMessage("Зарегистрирован!", "Регистрация", MessageBoxButtons.OK, MessageBoxIcon.None);
                        if (temp != null)
                            temp.IsSelected = true;
                        newUser = new UserFile();
                        unit.Dispose();
                    },(obj) => !newUser.HasErrors));
            }
        }



        public EnterViewModel(MainViewModel vm)
        {
            
            MessageBoxService = new MessageBoxService();
            this.vm = vm;
            newUser = new UserFile();
        }

    }
}

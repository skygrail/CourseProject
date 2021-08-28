using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KitchenBook.MVVM.Models
{

    public class UserFile : ValidatableModel
    {
        public static UserFile user = null;
        private string name;
        private string lastName;
        private string email;
        private string login;
        private string password;
        private byte[] image = null;
        public string Category;
        private Dictionary<string, int> visit = new Dictionary<string, int>
        {
            {"Выпечка", 0 },
            {"Напиток", 0 },
            {"Второе", 0 },
            {"Салат", 0 },
            {"Суп", 0 }
        };
        
        public Dictionary<string, int> Visit
        {
            get
            {
                return visit;
            }
        }
        public void AddVisit(string category)
        {
            visit[category] = ++visit[category];
        }

        public int ID_user { get; set; }

        public UserFile() { }

        public UserFile(string _name, string _last_name, string e_mail, string _login, string _password, byte[] _image)
        {
            name = _name;
            lastName = _last_name;
            email = e_mail;
            login = _login;
            password = _password;
            image = _image;
        }

        public UserFile(string _name, string _last_name, string e_mail, string _login, string _password, byte[] _image, int ID_User, string Category)
        {
            name = _name;
            lastName = _last_name;
            email = e_mail;
            login = _login;
            password = _password;
            image = _image;
            ID_user = ID_User;
            this.Category = Category;
        }

        [Required(ErrorMessage = "Это поле обязательно")]
        [RegularExpression(@"^([А-Я]{1})([а-я]*)$", ErrorMessage = "Имя должно начинаться с заглавной!")]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                //Regex regex = new Regex(@"^([А-Я]{1})([а-я]*)$");
                //if (regex.IsMatch(value))
                //{

                    
                //}
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        [Required(ErrorMessage = "Это поле обязательно")]
        [RegularExpression(@"^([А-Я]{1})([а-я]*)$", ErrorMessage = "Фамилия должна начинаться с заглавной!")]
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                //Regex regex = new Regex(@"^([А-Я]{1})([а-я]*)$");
                //if (regex.IsMatch(value))
                //{

                    
                //}
                lastName = value;
                RaisePropertyChanged("LastName");
            }
        }

        [Required(ErrorMessage = "Это поле обязательно")]
        [RegularExpression(@"^\S{2,30}[@]{1}(gmail|mail)(\.ru|\.com)$", ErrorMessage = "Неверный адрес почты!")]
        public string Email
        {
            get
            {
                return email;
            }
            set
            {

                //Regex regex = new Regex(@"^\S{2,30}[@]{1}(gmail|mail)(\.ru|\.com)$");
                //if (regex.IsMatch(value))
                //{
                    
                //}
                email = value;
                RaisePropertyChanged("Email");

            }
        }

        [Required(ErrorMessage = "Это поле обязательно")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Невалидный логин")]
        public string Login
        {
            get
            {
                return login;
            }

            set
            {
                login = value;
                RaisePropertyChanged("Login");
            }
        }

        [Required(ErrorMessage = "Это поле обязательно")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "Пароль не должен содержать русские символы")]
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }

        [Required(ErrorMessage = "Фотография обязательна")]
        public byte[] Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                RaisePropertyChanged("Image");
            }
        }

    }
}

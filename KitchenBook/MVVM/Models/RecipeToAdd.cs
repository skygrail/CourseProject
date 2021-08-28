using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenBook.MVVM.Models
{
    public class Ingredient
    {
        public string Ingrediet { get; set; }
        public string MassIngredient { get; set; }
    }
    public class RecipeToAdd : ValidatableModel
    {
        private string namerecipe;
        private string category;
        private int? id_author;
        private string description;
        private byte[] image;
        private List<Ingredient> ing = null;
        [Required]
        public List<Ingredient> ings
        {
            get
            {
                return ing;
            }
            set
            {
                ing = value;
                RaisePropertyChanged("ings");
            }
        }

        public RecipeToAdd(int? ID_author, string Login)
        {
            this.ID_author = ID_author;
            this.Login = Login;
            
        }
        public string Login { get; set; }
        

        [Required]
        public string NameRecipe
        {
            get
            {
                return namerecipe;
            }
            set
            {
                namerecipe = value;
                RaisePropertyChanged("NameRecipe");
            }
        }
        [Required]
        public string Category
        {
            get
            {
                return category;
            }
            set
            {
                category = value;
                RaisePropertyChanged("Category");
            }
        }
        [Required]
        public int? ID_author
        {
            get
            {
                return id_author;
            }
            set
            {
                id_author = value;
                RaisePropertyChanged("ID_author");
            }
        }
        [Required]
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
        [Required]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                RaisePropertyChanged("Description");
            }
        }
        





    }
}

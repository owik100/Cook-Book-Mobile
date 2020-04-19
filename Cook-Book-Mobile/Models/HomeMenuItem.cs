using System;
using System.Collections.Generic;
using System.Text;

namespace Cook_Book_Mobile.Models
{
    public enum MenuItemType
    {
        Browse,
        About,
        Register,
        Login,
        Recipes,
        RecipePreview,
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}

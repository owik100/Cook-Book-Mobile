using Cook_Book_Shared_Code.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cook_Book_Mobile.Models
{
    public class RecipeAndTitlePage
    {
        public RecipeAndTitlePage(RecipeModelDisplay recipeModelDisplay, string title)
        {
            RecipeModelDisplay = recipeModelDisplay;
            Title = title;
        }

       public RecipeModelDisplay RecipeModelDisplay { get; set; }
       public string Title { get; set; }
    }

}

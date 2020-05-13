using Cook_Book_Shared_Code.Models;

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

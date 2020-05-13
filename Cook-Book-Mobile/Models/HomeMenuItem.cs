namespace Cook_Book_Mobile.Models
{
    public enum MenuItemType
    {
        Browse,
        About,
        Register,
        Login,
        UserRecipes,
        PublicRecipes,
        RecipePreview,
        FavouritesRecipes
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}

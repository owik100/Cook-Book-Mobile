namespace Cook_Book_Mobile.Models.Interfaces
{
    public interface ILoggedUser
    {
        string Email { get; set; }
        string Id { get; set; }
        string Token { get; set; }
        string UserName { get; set; }

        void LogOffUser();
    }
}
using Cook_Book_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Book_Mobile.API.Interfaces
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task<LoggedUser> GetLoggedUserData(string token);
        HttpClient ApiClient { get; }
        Task<bool> Register(RegisterModel registerModel);

        void LogOffUser();
    }
}
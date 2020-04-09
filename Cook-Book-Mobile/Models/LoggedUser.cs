using Cook_Book_Mobile.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cook_Book_Mobile.Models
{
    public class LoggedUser : ILoggedUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }

        public void LogOffUser()
        {
            Id = "";
            Email = "";
            UserName = "";
            Token = "";
        }
    }
}
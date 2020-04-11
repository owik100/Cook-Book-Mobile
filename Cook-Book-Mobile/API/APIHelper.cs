using Cook_Book_Mobile.Helpers;
using Cook_Book_Shared_Code.API;
using Cook_Book_Shared_Code.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Book_Mobile.API
{
    public class APIHelper : IAPIHelper
    {
        private HttpClient _apiClient { get; set; }
        private ILoggedUser _loggedUser { get; set; }

        public APIHelper(ILoggedUser loggedUser)
        {
            InitializeClient();
            _loggedUser = loggedUser;
        }

        public HttpClient ApiClient
        {
            get
            {
                return _apiClient;
            }
        }

        private void InitializeClient()
        {
            try
            {
                string api = AppSettingsManager.Settings["api"];

                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                _apiClient = new HttpClient(handler);
                _apiClient.BaseAddress = new Uri(api);
                _apiClient.DefaultRequestHeaders.Accept.Clear();
                _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            catch (Exception ex)
            {
                //_logger.Error("Got exception", ex);
            }
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            try
            {
                var data = new FormUrlEncodedContent(new[]
           {
                new KeyValuePair<string,string>("grant_type", "password"),
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("password", password),
            });

                using (HttpResponseMessage response = await _apiClient.PostAsync("/Token", data))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                        return result;
                    }
                    else
                    {
                        string message = GetMessage.ErrorMessageFromResponse(response);

                        if (!string.IsNullOrEmpty(message))
                        {
                            Exception ex = new Exception(message);
                          // _logger.Error("Got exception", ex);
                            throw ex;
                        }
                        Exception exc = new Exception(response.ReasonPhrase);
                       // _logger.Error("Got exception", exc);
                        throw exc;
                    }
                }
            }
            catch (Exception ex)
            {
               // _logger.Error("Got exception", ex);
                throw;
            }

        }

        public async Task<LoggedUser> GetLoggedUserData(string token)
        {
            _apiClient.DefaultRequestHeaders.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiClient.DefaultRequestHeaders.Add("Authorization", $"bearer {token} ");

            using (HttpResponseMessage response = await _apiClient.GetAsync("/api/User"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedUser>();
                    _loggedUser.Id = result.Id;
                    _loggedUser.Token = token;
                    _loggedUser.UserName = result.UserName;
                    _loggedUser.Email = result.Email;

                    return result;
                }
                else
                {
                    Exception ex = new Exception(response.ReasonPhrase);
                   // _logger.Error("Got exception", ex);
                    throw ex;
                }
            }
        }

        public async Task<bool> Register(RegisterModel registerModel)
        {
            using (HttpResponseMessage response = await _apiClient.PostAsJsonAsync("/api/Account/register", registerModel))
            {
                if (response.IsSuccessStatusCode)
                {
                    return response.IsSuccessStatusCode;
                }
                else
                {
                    string message = GetMessage.ErrorMessageFromResponse(response);

                    if (!string.IsNullOrEmpty(message))
                    {
                        Exception ex = new Exception(message);
                        //_logger.Error("Got exception", ex);
                        throw ex;
                    }
                    Exception exc = new Exception(response.ReasonPhrase);
                   // _logger.Error("Got exception", exc);
                    throw exc;
                }
            }
        }

        public void LogOffUser()
        {
            try
            {
                _apiClient.DefaultRequestHeaders.Clear();
            }
            catch (Exception ex)
            {
               // _logger.Error("Got exception", ex);
                throw;
            }

        }
    }
}

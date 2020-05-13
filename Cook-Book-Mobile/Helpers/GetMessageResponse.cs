using Newtonsoft.Json;
using System.Net.Http;

namespace Cook_Book_Mobile.Helpers
{
    public static class GetMessageResponse
    {
        public static string ErrorMessageFromResponse(HttpResponseMessage response)
        {
            string output = "";
            try
            {
                var jsonMsg = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
                output = jsonMsg["message"];

            }
            catch (System.Exception ex)
            {
                throw;
            }
            return output;
        }
    }
}

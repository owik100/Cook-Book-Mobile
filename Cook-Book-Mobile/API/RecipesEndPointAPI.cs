using Cook_Book_Mobile.Helpers;
using Cook_Book_Shared_Code.API;
using Cook_Book_Shared_Code.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cook_Book_Mobile.API
{
    class RecipesEndPointAPI : IRecipesEndPointAPI
    {
      //  private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IAPIHelper _apiHelper;
        public RecipesEndPointAPI(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<RecipeModel>> GetRecipesLoggedUser(int PageSize, int PageNumber)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Recipes/CurrentUserRecipes/{PageSize}/{PageNumber}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<RecipeModel>>();
                    return result;
                }
                else
                {
                    Exception ex = new Exception(response.ReasonPhrase);
                    //_logger.Error("Got exception", ex);
                    throw ex;
                }
            }
        }

        public async Task<List<RecipeModel>> GetPublicRecipes(int PageSize, int PageNumber)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Recipes/GetPublicRecipes/{PageSize}/{PageNumber}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<RecipeModel>>();
                    return result;
                }
                else
                {
                    Exception ex = new Exception(response.ReasonPhrase);
                    //_logger.Error("Got exception", ex);
                    throw ex;
                }
            }
        }

        public async Task<List<RecipeModel>> GetFavouritesRecipes(int PageSize, int PageNumber)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Recipes/GetFavouritesRecipes/{PageSize}/{PageNumber}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<RecipeModel>>();
                    return result;
                }
                else
                {
                    Exception ex = new Exception(response.ReasonPhrase);
                    //_logger.Error("Got exception", ex);
                    throw ex;
                }
            }
        }

        public async Task<bool> DownloadImage(string id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Recipes/GetPhoto/{id}"))
            {

                if (response.IsSuccessStatusCode)
                {

                    var result = await response.Content.ReadAsStreamAsync();

                    MemoryStream memoryStream = result as MemoryStream;

                    FileStream file = new FileStream(TempData.GetImagePath(id), FileMode.Create, FileAccess.Write);

                    memoryStream.WriteTo(file);

                    file.Close();
                    file.Dispose();
                    memoryStream.Close();
                    memoryStream.Dispose();
                    result.Close();
                    result.Dispose();
                }
                else
                {
                    Exception ex = new Exception(response.ReasonPhrase);
                    //_logger.Error("Got exception", ex);
                    throw ex;
                }

                return true;
            }
        }

        public async Task<bool> InsertRecipe(RecipeModel recipeModel)
        {
            try
            {
                var multiForm = new MultipartFormDataContent();

                string ingredients = string.Join(";", recipeModel.Ingredients);

                if (!string.IsNullOrEmpty(recipeModel.NameOfImage))
                {
                    FileStream fs = File.OpenRead(recipeModel.NameOfImage);
                    multiForm.Add(new StreamContent(fs), "Image", Path.GetFileName(recipeModel.NameOfImage));
                }

                multiForm.Add(new StringContent(recipeModel.Name), "Name");
                multiForm.Add(new StringContent(ingredients), "Ingredients");
                multiForm.Add(new StringContent(recipeModel.Instruction), "Instruction");
                multiForm.Add(new StringContent(recipeModel.IsPublic.ToString()), "IsPublic");

                using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync("/api/Recipes", multiForm))
                {
                    if (response.IsSuccessStatusCode)
                    {

                        return response.IsSuccessStatusCode;
                    }
                    else
                    {
                        Exception ex = new Exception(response.ReasonPhrase);
                       // _logger.Error("Got exception", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                //_logger.Error("Got exception", ex);
                throw;
            }

        }

        public async Task<bool> DeleteRecipe(string id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync($"api/Recipes/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {

                    return response.IsSuccessStatusCode;
                }
                else
                {
                    Exception ex = new Exception(response.ReasonPhrase);
                    //_logger.Error("Got exception", ex);
                    throw ex;
                }
            }
        }

        public async Task<bool> EditRecipe(RecipeModel recipeModel)
        {
            try
            {
                var multiForm = new MultipartFormDataContent();

                string ingredients = string.Join(";", recipeModel.Ingredients);
                string fileName = Path.GetFileName(recipeModel.NameOfImage);

                if (!string.IsNullOrEmpty(recipeModel.NameOfImage))
                {
                    FileStream fs = File.OpenRead(recipeModel.NameOfImage);
                    multiForm.Add(new StreamContent(fs), "Image", Path.GetFileName(recipeModel.NameOfImage));
                }

                multiForm.Add(new StringContent(recipeModel.Name), "Name");
                multiForm.Add(new StringContent(ingredients), "Ingredients");
                multiForm.Add(new StringContent(recipeModel.Instruction), "Instruction");
                multiForm.Add(new StringContent(recipeModel.RecipeId.ToString()), "RecipeId");
                multiForm.Add(new StringContent(fileName), "NameOfImage");
                multiForm.Add(new StringContent(recipeModel.IsPublic.ToString()), "IsPublic");

                using (HttpResponseMessage response = await _apiHelper.ApiClient.PutAsync($"/api/Recipes/{recipeModel.RecipeId.ToString()}", multiForm))
                {
                    if (response.IsSuccessStatusCode)
                    {

                        return response.IsSuccessStatusCode;
                    }
                    else
                    {
                        Exception ex = new Exception(response.ReasonPhrase);
                        //_logger.Error("Got exception", ex);
                        throw ex;
                    }
                }
            }
            catch (Exception exc)
            {
                //_logger.Error("Got exception", exc);
                throw;
            }
        }
    }
}


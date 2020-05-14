using Cook_Book_Mobile.Helpers;
using Cook_Book_Mobile.Services;
using Cook_Book_Mobile.Views;
using Cook_Book_Shared_Code.API;
using Cook_Book_Shared_Code.Models;
using FormsToolkit;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cook_Book_Mobile.ViewModels
{
    public class AddOrEditViewModel : BaseViewModel
    {
        public ICommand SubmitCommand { get; set; }
        public ICommand AddIngredientCommand { get; set; }
        public ICommand DeleteIngredientCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public ICommand DeleteImageCommand { get; set; }


        private string _recipeName;
        private ObservableCollection<string> _recipeIngredients = new ObservableCollection<string>();
        private string _selectedIngredient;
        private string _ingredientInsert;
        private string _recipeInstructions;
        private string _image;

        private IRecipesEndPointAPI _recipesEndPointAPI;

        private AddOrEdit _addOrEdit = AddOrEdit.Add;
        private string _submitText;
        private int _recipeId;
        private bool _isPublic;

        public AddOrEditViewModel(IRecipesEndPointAPI RecipesEndPointAPI)
        {
            _recipesEndPointAPI = RecipesEndPointAPI;

            SubmitCommand = new Command(async () => await Submit());
            AddIngredientCommand = new Command(async () => await AddIngredient());
            DeleteIngredientCommand = new Command(async () => await DeleteIngredient());
            SelectImageCommand = new Command(async () => await OpenFile());
            DeleteImageCommand = new Command(async () => await DeleteFile());

            Title = "Dodaj";
            ImagePath = ImageConstants.LoadDefaultImage;
            SubmitText = "Dodaj";

            MessagingService.Current.Unsubscribe<RecipeModel>(EventMessages.EditRecipeEvent);

            MessagingService.Current.Subscribe<RecipeModel>(EventMessages.EditRecipeEvent, (sender, arg) => 
            {
                Title = "Edytuj";
                _addOrEdit = AddOrEdit.Edit;
                _recipeId = arg.RecipeId;
                SubmitText = "Zaktualizuj";

                RecipeName = arg.Name;
                RecipeIngredients = new ObservableCollection<string>(arg.Ingredients.ToList());
                RecipeInstructions = arg.Instruction;
                ImagePath = arg.ImagePath;
                IsPublic = arg.IsPublic;

                OnPropertyChanged(nameof(CanDeleteImage));

            });

        }

        #region Props
        public string SubmitText
        {
            get { return _submitText; }
            set
            {
                _submitText = value;
                OnPropertyChanged(nameof(SubmitText));
            }
        }

        public string ImagePath
        {
            get { return _image; }
            set
            {
                _image = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }

        public string RecipeName
        {
            get { return _recipeName; }
            set
            {
                _recipeName = value;
                OnPropertyChanged(nameof(RecipeName));
                OnPropertyChanged(nameof(CanRecipeSubmit));
            }
        }

        public ObservableCollection<string> RecipeIngredients
        {
            get { return _recipeIngredients; }
            set
            {
                _recipeIngredients = value;
                OnPropertyChanged(nameof(RecipeIngredients));
                OnPropertyChanged(nameof(CanRecipeSubmit));
            }
        }

        public string SelectedIngredient
        {
            get { return _selectedIngredient; }
            set
            {
                _selectedIngredient = value;
                OnPropertyChanged(nameof(SelectedIngredient));
                OnPropertyChanged(nameof(RecipeIngredients));
                OnPropertyChanged(nameof(CanDeleteIngredient));
            }
        }

        public string RecipeInstructions
        {
            get { return _recipeInstructions; }
            set
            {
                _recipeInstructions = value;
                OnPropertyChanged(nameof(RecipeInstructions));
                OnPropertyChanged(nameof(CanRecipeSubmit));
            }
        }

        public string IngredientInsert
        {
            get { return _ingredientInsert; }
            set
            {
                _ingredientInsert = value;
                OnPropertyChanged(nameof(IngredientInsert));
                OnPropertyChanged(nameof(CanAddIngredient));
            }
        }

        public bool IsPublic
        {
            get { return _isPublic; }
            set
            {
                _isPublic = value;
                OnPropertyChanged(nameof(IsPublic));
            }
        }

        public bool CanAddIngredient
        {
            get
            {
                bool output = false;

                if (!string.IsNullOrWhiteSpace(IngredientInsert))
                {
                    output = true;
                }

                return output;
            }

        }

        public bool CanDeleteIngredient
        {
            get
            {
                bool output = false;

                if (!string.IsNullOrWhiteSpace(SelectedIngredient))
                {
                    output = true;
                }

                return output;
            }

        }

        public bool CanDeleteImage
        {
            get
            {
                bool output = false;

                if (!string.IsNullOrWhiteSpace(ImagePath) && ImagePath != ImageConstants.LoadDefaultImage)
                {
                    output = true;
                }

                return output;
            }
        }

        public bool CanRecipeSubmit
        {
            get
            {
                bool output = false;

                if (!string.IsNullOrWhiteSpace(RecipeName) && !string.IsNullOrWhiteSpace(RecipeInstructions) && RecipeIngredients.Count > 0 && !IsBusy)
                {
                    output = true;
                }

                return output;
            }
        }

        #endregion

        async Task AddIngredient()
        {
            try
            {
                RecipeIngredients.Add(IngredientInsert);
                IngredientInsert = "";
                OnPropertyChanged(nameof(CanRecipeSubmit));
            }
            catch (Exception ex)
            {
                //_logger.Error("Got exception", ex);
                await Application.Current.MainPage.DisplayAlert("Błąd", ex.Message, "Ok");
            }
        }

        async Task DeleteIngredient()
        {
            try
            {
                RecipeIngredients.Remove(SelectedIngredient);
                OnPropertyChanged(nameof(CanRecipeSubmit));
            }
            catch (Exception ex)
            {
                //_logger.Error("Got exception", ex);
                await Application.Current.MainPage.DisplayAlert("Błąd", ex.Message, "Ok");
            }
        }

        async Task OpenFile()
        {
            try
            {
                PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();

                if (status == PermissionStatus.Granted)
                {
                    string path = await DependencyService.Get<IPhotoPickerService>().GetImagePathAsync();
                    if (path != null)
                    {

                        ImagePath = path;
                        OnPropertyChanged(nameof(ImagePath));
                        OnPropertyChanged(nameof(CanDeleteImage));
                    }
                }
                else
                {
                    status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                }

            }
            catch (Exception ex)
            {
                //_logger.Error("Got exception", ex);
            }
        }

        async Task DeleteFile()
        {
            bool answer;
            try
            {
                answer = await Application.Current.MainPage.DisplayAlert(RecipeName, "Na pewno chcesz usunąć obrazek?", "Tak", "Nie");
                if (answer)
                {
                    ImagePath = ImageConstants.LoadDefaultImage;
                    OnPropertyChanged(nameof(ImagePath));
                    OnPropertyChanged(nameof(CanDeleteImage));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Błąd", ex.Message, "Ok");
            }
        }

        public async Task Submit()
        {
            try
            {
                IsBusy = true;

                RecipeModel recipeModel = new RecipeModel
                {
                    Name = RecipeName,
                    Ingredients = RecipeIngredients.ToList(),
                    Instruction = RecipeInstructions,
                    NameOfImage = ImagePath,
                    IsPublic = IsPublic,
                };

                if (recipeModel.NameOfImage == ImageConstants.LoadDefaultImage)
                {
                    recipeModel.NameOfImage = "";
                }

                if (_addOrEdit == AddOrEdit.Add)
                {
                    await _recipesEndPointAPI.InsertRecipe(recipeModel);
                    DependencyService.Get<IMessage>().LongAlert("Dodano pomyślnie!");
                    MessagingCenter.Send(this, EventMessages.BasicNavigationEvent);
                }
                else if (_addOrEdit == AddOrEdit.Edit)
                {
                    recipeModel.RecipeId = _recipeId;
                    var result = await _recipesEndPointAPI.EditRecipe(recipeModel);

                    if (result)
                    {
                        if (recipeModel.NameOfImage == "")
                        {
                            recipeModel.NameOfImage = ImageConstants.LoadDefaultImage;
                        }

                        OnPropertyChanged(nameof(ImagePath));
                        OnPropertyChanged(nameof(CanDeleteImage));

                        DependencyService.Get<IMessage>().LongAlert("Zaktualizowano pomyślnie!");
                        MessagingCenter.Send(this, EventMessages.BasicNavigationEvent);
                    }
                }
            }
            catch (Exception ex)
            {
                // _logger.Error("Got exception", ex);
                await Application.Current.MainPage.DisplayAlert("Błąd", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

    }

    public enum AddOrEdit
    {
        Add,
        Edit
    }
}

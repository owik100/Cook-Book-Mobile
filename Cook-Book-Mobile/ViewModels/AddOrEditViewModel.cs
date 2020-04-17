using Cook_Book_Mobile.Helpers;
using Cook_Book_Mobile.Services;
using Cook_Book_Mobile.Views;
using Cook_Book_Shared_Code.API;
using Cook_Book_Shared_Code.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
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

        Image image = new Image();

        public AddOrEditViewModel(IRecipesEndPointAPI RecipesEndPointAPI)
        {
            _recipesEndPointAPI = RecipesEndPointAPI;

            SubmitCommand = new Command (async () => await Submit());
            AddIngredientCommand = new Command (async () => await AddIngredient());
            DeleteIngredientCommand = new Command (async () => await DeleteIngredient());
            SelectImageCommand = new Command (async () => await OpenFile());
            DeleteImageCommand = new Command (async () => await DeleteFile());

            Title = "Dodaj";
            ImagePath = "Cook_Book_Mobile.Images.foodtemplate.png";
            SubmitText = "Dodaj";

            MessagingCenter.Subscribe<RecipePreviewPage, RecipeModel>(this, EventMessages.EditRecipeEvent, (sender, arg) =>
            {
                Title = "Edytuj";
                _addOrEdit = AddOrEdit.Edit;
                _recipeId = arg.RecipeId;
                SubmitText = "Zaktualizuj";

                RecipeName = arg.Name;
                RecipeIngredients = new ObservableCollection<string>(arg.Ingredients.ToList());
                RecipeInstructions = arg.Instruction;
                ImagePath = arg.ImagePath;

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

                if (!string.IsNullOrWhiteSpace(ImagePath) && ImagePath != "Cook_Book_Mobile.Images.foodtemplate.png")
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

                if (!string.IsNullOrWhiteSpace(RecipeName) && !string.IsNullOrWhiteSpace(RecipeInstructions) && RecipeIngredients.Count > 0)
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
                Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
                if (stream != null)
                {
                    image.Source = ImageSource.FromStream(() => stream);
                    ImagePath = "123.jpeg";
                    OnPropertyChanged(nameof(ImagePath));
                    OnPropertyChanged(nameof(CanDeleteImage));
                }
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Błąd", ex.Message, "Ok");
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
                    ImagePath = "Cook_Book_Mobile.Images.foodtemplate.png";
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
                RecipeModel recipeModel = new RecipeModel
                {
                    Name = RecipeName,
                    Ingredients = RecipeIngredients.ToList(),
                    Instruction = RecipeInstructions,
                    NameOfImage = ImagePath
                };

                if (recipeModel.NameOfImage == "Cook_Book_Mobile.Images.foodtemplate.png")
                {
                    recipeModel.NameOfImage = "";
                }

                if (_addOrEdit == AddOrEdit.Add)
                {
                    await _recipesEndPointAPI.InsertRecipe(recipeModel);
                    MessagingCenter.Send(this, EventMessages.BasicNavigationEvent);

                    //await _eventAggregator.PublishOnUIThreadAsync(new LogOnEvent(reloadNeeded), new CancellationToken());
                }
                else if (_addOrEdit == AddOrEdit.Edit)
                {
                    recipeModel.RecipeId = _recipeId;
                    var result = await _recipesEndPointAPI.EditRecipe(recipeModel);

                    if (result)
                    {
                        if (recipeModel.NameOfImage == "")
                        {
                            recipeModel.NameOfImage = "Cook_Book_Mobile.Images.foodtemplate.png";
                        }

                        OnPropertyChanged(nameof(ImagePath));
                        OnPropertyChanged(nameof(CanDeleteImage));

                        await Application.Current.MainPage.DisplayAlert("Zaktualizowano pomyślnie!", "Zaktualizowano", "Ok");
                        MessagingCenter.Send(this, EventMessages.BasicNavigationEvent);
                    }
                }
            }
            catch (Exception ex)
            {
              // _logger.Error("Got exception", ex);
                await Application.Current.MainPage.DisplayAlert("Błąd", ex.Message, "Ok");
            }
        }

    }

    public enum AddOrEdit
    {
        Add,
        Edit
    }
}

using CommonServiceLocator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cook_Book_Mobile.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            Bootstrap.Initialize();
        }

        public RegisterViewModel Reg
        {
            get => ServiceLocator.Current.GetInstance<RegisterViewModel>();
        }

        public LoginViewModel Log
        {
            get => ServiceLocator.Current.GetInstance<LoginViewModel>();
        }

        public RecipesViewModel Recipes
        {
            get => ServiceLocator.Current.GetInstance<RecipesViewModel>();
        }

        public MenuViewModel Menu
        {
            get => ServiceLocator.Current.GetInstance<MenuViewModel>();
        }
    }
}

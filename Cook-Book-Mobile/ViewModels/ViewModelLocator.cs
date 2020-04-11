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
            get
            {
                return ServiceLocator.Current.GetInstance<RegisterViewModel>();
            }
        }

        public LoginViewModel Log
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }
    }
}

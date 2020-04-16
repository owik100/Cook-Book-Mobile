using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using Cook_Book_Mobile.API;
using Cook_Book_Mobile.ViewModels;
using Cook_Book_Shared_Code.API;
using Cook_Book_Shared_Code.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cook_Book_Mobile
{
    public sealed class Bootstrap
    {
        public static void Initialize()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RegisterViewModel>().AsSelf();
            builder.RegisterType<LoginViewModel>().AsSelf();
            builder.RegisterType<RecipesViewModel>().AsSelf();
            builder.RegisterType<MenuViewModel>().AsSelf();
            builder.RegisterType<RecipePreviewViewModel>().AsSelf();
            builder.RegisterType<AddOrEditViewModel>().AsSelf();

            builder.RegisterType<LoggedUser>().As<ILoggedUser>().SingleInstance();
            builder.RegisterType<APIHelper>().As<IAPIHelper>().SingleInstance();
            builder.RegisterType<RecipesEndPointAPI>().As<IRecipesEndPointAPI>().SingleInstance();

            IContainer container = builder.Build();

            AutofacServiceLocator asl = new AutofacServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => asl);

        }
    }
}

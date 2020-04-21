using System;
using System.Collections.Generic;
using System.Text;

namespace Cook_Book_Mobile.Helpers
{
    public static class EventMessages
    {
       public static readonly string LogOnEvent = "LogOnEvent";
       public static readonly string BasicNavigationEvent = "BasicNavigationEvent";
       public static readonly string AppStartEvent = "AppStartEvent";
       public static readonly string ReloadUserRecipesEvent = "ReloadUserRecipesEvent";
       public static readonly string ReloadPublicRecipesEvent = "ReloadPublicRecipesEvent";
       public static readonly string RecipesPreviewEvent = "RecipesPreviewEvent";
       public static readonly string EditRecipeEvent = "EditRecipeEvent";
    }
}

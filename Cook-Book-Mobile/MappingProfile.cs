﻿using AutoMapper;
using Cook_Book_Shared_Code.Models;

namespace Cook_Book_Mobile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RecipeModel, RecipeModelDisplay>();
            CreateMap<RecipeModelDisplay, RecipeModel>();
        }
    }
}

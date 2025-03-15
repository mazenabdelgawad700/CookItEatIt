﻿using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.InfrastructureBases;

namespace RecipeApp.Infrastructure.Abstracts
{
    public interface IRecipeLikeRepository : IGenericRepositoryAsync<RecipeLike>
    {
    }
}

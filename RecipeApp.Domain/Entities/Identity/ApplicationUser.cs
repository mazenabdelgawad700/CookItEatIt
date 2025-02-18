using Microsoft.AspNetCore.Identity;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public int CountryId { get; set; }
        public bool IsVerifiedChef { get; set; } = false;
        public bool AcceptNotification { get; set; } = false;
        public string PreferredTheme { get; set; } = null!;
        public string ProfilePictureURL { get; set; } = null!;
        public string Bio { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public Country Country { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
        public ICollection<UserFollower> Followers { get; set; }
        public ICollection<UserFollower> Following { get; set; }
        public ICollection<SavedRecipe> SavedRecipes { get; set; }
        public ICollection<RecipeLike> LikedRecipes { get; set; }
        public ICollection<ChefRecipeRequest> RecipeRequests { get; set; }
        public ICollection<ChefRecipeRequest> ReceivedRequests { get; set; }
    }
}
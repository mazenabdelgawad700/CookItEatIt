using Microsoft.AspNetCore.Identity;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public int? CountryId { get; set; }
        public bool IsVerifiedChef { get; set; } = false;
        public byte PreferredTheme { get; set; }
        public string? ProfilePictureURL { get; set; }
        public string? Bio { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public virtual Country? Country { get; set; }
        public virtual UserPreferences Preferences { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
        public virtual ICollection<UserFollower> Followers { get; set; }
        public virtual ICollection<UserFollower> Following { get; set; }
        public virtual ICollection<SavedRecipe> SavedRecipes { get; set; }
        public virtual ICollection<RecipeLike> LikedRecipes { get; set; }
        public virtual ICollection<LoginAttempt> LoginAttempts { get; set; }
        public virtual ICollection<ChefRecipeRequest> RecipeRequests { get; set; }
        public virtual ICollection<ChefRecipeRequest> ReceivedRequests { get; set; }
        public virtual ICollection<ApplicationUserRefreshToken> ApplicationUserRefreshTokens { get; set; }
    }
}
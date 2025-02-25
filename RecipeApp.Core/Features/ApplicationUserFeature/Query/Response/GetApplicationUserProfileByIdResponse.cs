namespace RecipeApp.Core.Features.ApplicationUserFeature.Query.Response
{
    public class GetApplicationUserProfileByIdResponse
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ProfilePictureURL { get; set; }
        public string? Bio { get; set; }
        public bool IsVerifiedChef { get; set; }
        public string? Country { get; set; }
        public int RecipesCount { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
    }
}
using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Domain.Entities.Models
{
    public class UserFollower
    {
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }
        public DateTime FollowedAt { get; set; }
        public virtual ApplicationUser Following { get; set; }
        public virtual ApplicationUser Follower { get; set; }
    }
}
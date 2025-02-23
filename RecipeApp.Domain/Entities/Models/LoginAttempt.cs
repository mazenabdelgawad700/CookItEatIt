using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Domain.Entities.Models
{
    public class LoginAttempt
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string IpAddress { get; set; }
        public int AttemptCount { get; set; }
        public DateTime LastAttemptTime { get; set; }
        public bool IsBlocked { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
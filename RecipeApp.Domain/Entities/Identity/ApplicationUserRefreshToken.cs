namespace RecipeApp.Domain.Entities.Identity
{
    public class ApplicationUserRefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? RefreshToken { get; set; }
        public string? JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
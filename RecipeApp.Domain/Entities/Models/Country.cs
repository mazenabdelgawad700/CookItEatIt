using RecipeApp.Domain.Entities.Identity;

namespace RecipeApp.Domain.Entities.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}

using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Infrastructure.InfrastructureBases;

namespace RecipeApp.Infrastructure.Abstracts
{
    public interface IApplicationUserRepository : IGenericRepositoryAsync<ApplicationUser>
    {
    }
}

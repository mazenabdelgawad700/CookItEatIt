using RecipeApp.Domain.Entities.Identity;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Infrastructure.InfrastructureBases;

namespace RecipeApp.Infrastructure.Repositories
{
    internal class ApplicationUserRepository : GenericRepositoryAsync<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
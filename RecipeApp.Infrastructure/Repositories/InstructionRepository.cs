using RecipeApp.Domain.Entities.Models;
using RecipeApp.Infrastructure.Abstracts;
using RecipeApp.Infrastructure.Context;
using RecipeApp.Infrastructure.InfrastructureBases;

namespace RecipeApp.Infrastructure.Repositories
{
    internal class InstructionRepository : GenericRepositoryAsync<Instruction>, IInstructionRepository
    {
        public InstructionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeApp.Domain.Entities.Models;

namespace RecipeApp.Infrastructure.Configurations
{
    public class InstructionConfiguration : IEntityTypeConfiguration<Instruction>
    {
        public void Configure(EntityTypeBuilder<Instruction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.InstructionNumber)
                   .IsRequired();

            builder.Property(x => x.Description)
                   .IsRequired()
                   .HasMaxLength(1000);
        }
    }
}

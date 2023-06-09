using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
            new IdentityRole
            {
                Id = "1c5e174e-3b0e-446f-86af-483d56fd7210",
                Name = "USER",
                NormalizedName = "USER"
            },
            new IdentityRole
            {
                Id = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            }
            );
        }
    }
}
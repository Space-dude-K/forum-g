using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.Configuration
{
    public class ForumAccountTypeConfiguration : IEntityTypeConfiguration<ForumAccountType>
    {
        public void Configure(EntityTypeBuilder<ForumAccountType> builder)
        {
            builder
                .ToTable("ForumAccountType");

            builder
                .Property(p => p.Id)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.TypeName)
                .HasColumnType("TEXT")
                .HasMaxLength(256)
            .IsRequired(false);

            builder
                .HasKey(p => p.Id)
                .HasName("PK_ForumAccountType");
        }
    }
}
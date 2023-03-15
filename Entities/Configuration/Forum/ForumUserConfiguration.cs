using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Models.Forum;

namespace Entities.Configuration.Forum
{
    public class ForumUserConfiguration : IEntityTypeConfiguration<ForumUser>
    {
        public void Configure(EntityTypeBuilder<ForumUser> builder)
        {
            builder
            .ToTable("ForumUser");

            builder
                .Property(p => p.Id)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.Name)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(256)
                .IsRequired(false)
                .IsUnicode(true);
            builder
                .Property(p => p.Surname)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(256)
                .IsRequired(false)
                .IsUnicode(true);
            builder
                .Property(p => p.Lastname)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(256)
                .IsRequired(false)
                .IsUnicode(true);
            builder
                .Property(p => p.Karma)
                .HasColumnType("INTEGER")
                .IsRequired(true)
                .HasDefaultValue(0);
            builder
                .Property(p => p.CreatedAt)
                .HasColumnType("TEXT")
                .IsRequired(false);
            builder
                .Property(p => p.UpdatedAt)
                .HasColumnType("TEXT")
                .IsRequired(false);

            builder
                .HasKey(p => p.Id)
                .HasName("PK_ForumUser");

            // Data seed
            builder.HasData(
                new ForumUser()
                {
                    Id = 1,
                    Name = "Константин",
                    Surname = "Феофанов",
                    Lastname = "Сергеевич",
                    CreatedAt = DateTime.Now.ToShortDateString()
                },
                new ForumUser()
                {
                    Id = 2,
                    Name = "Александр",
                    Surname = "Петров",
                    Lastname = "Григорьевич",
                    CreatedAt = DateTime.Now.ToShortDateString()
                }
            );
        }
    }
}
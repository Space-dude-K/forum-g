using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.Configuration
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
                .HasColumnType("TEXT")
                .HasMaxLength(256)
                .IsRequired(false);
            builder
                .Property(p => p.Surname)
                .HasColumnType("TEXT")
                .HasMaxLength(256)
                .IsRequired(false);
            builder
                .Property(p => p.Lastname)
                .HasColumnType("TEXT")
                .HasMaxLength(256)
                .IsRequired(false);
            builder
                .Property(p => p.Email)
                .HasColumnType("TEXT")
                .IsRequired(false);
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
            builder
                .HasOne(e => e.ApplicationUser)
                .WithOne(p => p.ForumUser)
                .HasForeignKey<ForumUser>(e => e.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ForumUser_AspNetUser_UserId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
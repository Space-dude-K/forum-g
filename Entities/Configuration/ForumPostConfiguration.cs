using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.Configuration
{
    public class ForumPostConfiguration : IEntityTypeConfiguration<ForumPost>
    {
        public void Configure(EntityTypeBuilder<ForumPost> builder)
        {
            builder
                .ToTable("ForumPost");

            builder
                .Property(p => p.Id)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.ForumUserId)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.PostName)
                .HasColumnType("TEXT")
                .HasMaxLength(256)
                .IsRequired(false);
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
                .HasName("PK_ForumPost");
            builder
                .HasOne<ForumUser>(p => p.ForumUser)
                .WithMany()
                .HasConstraintName("FK_ForumPost_ForumUser_Id")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
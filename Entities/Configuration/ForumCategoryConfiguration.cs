using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.Configuration
{
    public class ForumCategoryConfiguration : IEntityTypeConfiguration<ForumCategory>
    {
        public void Configure(EntityTypeBuilder<ForumCategory> builder)
        {
            #region DbStructure
            builder
                .ToTable("ForumCategory");
            builder
                .Property(p => p.Id)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.Name)
                .HasColumnType("TEXT")
                .HasMaxLength(256)
                .IsRequired(false)
                .IsUnicode(true);
            builder
                .Property(p => p.CreatedAt)
                .HasColumnType("TEXT")
                .IsRequired(false);
            builder
                .Property(p => p.UpdatedAt)
                .HasColumnType("TEXT")
                .IsRequired(false);
            builder
                .Property(p => p.ForumUserId)
                .HasColumnType("INTEGER")
                .IsRequired(true);

            builder
                .Ignore(c => c.TotalPosts);

            builder
                .HasKey(p => p.Id)
                .HasName("PK_ForumCategory");
            builder
                .HasMany<ForumBase>(p => p.ForumBases)
                .WithOne(p => p.ForumCategory)
                .HasForeignKey(p => p.ForumCategoryId)
                .HasConstraintName("FK_ForumCategory_ForumBase_Id")
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne<ForumUser>(p => p.ForumUser)
                .WithMany()
                .HasConstraintName("FK_ForumCategory_ForumUser_Id")
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region DbDataSeed
            builder.HasData(
                new ForumCategory()
                {
                    Id = 1,
                    Name = "Test subtopic 1",
                    CreatedAt = DateTime.Now.ToShortDateString(),
                    ForumUserId = 1
                },
                new ForumCategory()
                {
                    Id = 2,
                    Name = "Test subtopic 2",
                    CreatedAt = DateTime.Now.ToShortDateString(),
                    ForumUserId = 2
                }
            );
            #endregion
        }
    }
}
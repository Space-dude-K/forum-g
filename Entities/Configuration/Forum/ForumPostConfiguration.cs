using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Models.Forum;

namespace Entities.Configuration.Forum
{
    public class ForumPostConfiguration : IEntityTypeConfiguration<ForumPost>
    {
        public void Configure(EntityTypeBuilder<ForumPost> builder)
        {
            #region DbStructure
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
                .HasColumnType("NVARCHAR")
                .HasMaxLength(256)
                .IsRequired(false)
                .IsUnicode(true);
            builder
                .Property(p => p.CreatedAt)
                .HasColumnType("Date")
                .IsRequired(false);
            builder
                .Property(p => p.UpdatedAt)
                .HasColumnType("Date")
                .IsRequired(false);

            builder
                .HasKey(p => p.Id)
                .HasName("PK_ForumPost");
            builder
                .HasOne(p => p.ForumUser)
                .WithMany()
                .HasConstraintName("FK_ForumPost_ForumUser_Id")
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
            #region DbDataSeed
            builder.HasData(
                new ForumPost()
                {
                    Id = 1,
                    PostName = "Post name 1",
                    CreatedAt = DateTime.Now,
                    ForumTopicId = 1,
                    ForumUserId = 1
                },
                new ForumPost()
                {
                    Id = 2,
                    PostName = "Post name 2",
                    CreatedAt = DateTime.Now,
                    ForumTopicId = 2,
                    ForumUserId = 2
                }
            );
            #endregion
        }
    }
}
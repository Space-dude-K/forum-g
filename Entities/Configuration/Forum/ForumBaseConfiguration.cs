using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Models.Forum;

namespace Entities.Configuration.Forum
{
    public class ForumBaseConfiguration : IEntityTypeConfiguration<ForumBase>
    {
        public void Configure(EntityTypeBuilder<ForumBase> builder)
        {
            #region DbStructure
            builder
                .ToTable("ForumBase");

            builder
                .Property(p => p.Id)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.ForumTitle)
                .HasColumnType("TEXT")
                .HasMaxLength(256)
                .IsRequired(true)
                .IsUnicode(true);
            builder
                .Property(p => p.ForumSubTitle)
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
                .HasName("PK_ForumBase");
            builder
                .HasMany(p => p.ForumTopics)
                .WithOne(p => p.ForumBase)
                .HasForeignKey(p => p.ForumBaseId)
                .HasConstraintName("FK_ForumBase_ForumTopic_ForumBaseId")
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(p => p.ForumUser)
                .WithMany()
                .HasConstraintName("FK_ForumBase_ForumUser_Id")
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
            #region DbDataSeed
            builder.HasData(
                new ForumBase()
                {
                    Id = 1,
                    ForumTitle = "Test forum title 1",
                    ForumSubTitle = "Test forum subtitle 1",
                    CreatedAt = DateTime.Now.ToShortDateString(),
                    ForumCategoryId = 1,
                    ForumUserId = 1
                },
                new ForumBase()
                {
                    Id = 2,
                    ForumTitle = "Test forum title 2",
                    ForumSubTitle = "Test forum subtitle 2",
                    CreatedAt = DateTime.Now.ToShortDateString(),
                    ForumCategoryId = 2,
                    ForumUserId = 2
                }
            );
            #endregion
        }
    }
}
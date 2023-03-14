using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.Configuration
{
    public class ForumTopicConfiguration : IEntityTypeConfiguration<ForumTopic>
    {
        public void Configure(EntityTypeBuilder<ForumTopic> builder)
        {
            #region DbStructure
            builder
                .ToTable("ForumTopic");

            builder
                .Property(p => p.Id)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.ForumUserId)
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
                .Property(p => p.TopicViewCounter)
                .HasColumnType("INTEGER")
                .IsRequired(true);

            builder
                .Ignore(c => c.TotalPosts);

            builder
                .HasKey(p => p.Id)
                .HasName("PK_ForumTopic");
            builder
                .HasMany<ForumPost>(p => p.ForumPosts)
                .WithOne(p => p.ForumTopic)
                .HasForeignKey(p => p.ForumTopicId)
                .HasConstraintName("FK_ForumTopic_ForumPost_ForumTopicId")
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne<ForumUser>(p => p.ForumUser)
                .WithMany()
                .HasConstraintName("FK_ForumTopic_ForumUser_Id")
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
            #region DbDataSeed
            builder.HasData(
                new ForumTopic()
                {
                    Id = 1,
                    Name = "Test forum topic 1",
                    CreatedAt = DateTime.Now.ToShortDateString(),
                    ForumBaseId = 1,
                    ForumUserId = 1
                },
                new ForumTopic()
                {
                    Id = 2,
                    Name = "Test forum topic 2",
                    CreatedAt = DateTime.Now.ToShortDateString(),
                    ForumBaseId = 2,
                    ForumUserId = 2
                }
            );
            #endregion
        }
    }
}
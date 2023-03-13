using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.Configuration
{
    public class ForumTopicConfiguration : IEntityTypeConfiguration<ForumTopic>
    {
        public void Configure(EntityTypeBuilder<ForumTopic> builder)
        {
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
        }
    }
}
using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.Configuration
{
    public class ForumBaseConfiguration : IEntityTypeConfiguration<ForumBase>
    {
        public void Configure(EntityTypeBuilder<ForumBase> builder)
        {
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
                .IsRequired(true);
            builder
                .Property(p => p.ForumSubTitle)
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
                .Property(p => p.ForumUserId)
                .HasColumnType("INTEGER")
            .IsRequired(true);

            builder
                .Ignore(c => c.TotalPosts);

            builder
                .HasKey(p => p.Id)
                .HasName("PK_ForumBase");
            builder
                .HasMany<ForumTopic>(p => p.ForumTopics)
                .WithOne(p => p.ForumBase)
                .HasForeignKey(p => p.ForumBaseId)
                .HasConstraintName("FK_ForumBase_ForumTopic_ForumBaseId")
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne<ForumUser>(p => p.ForumUser)
                .WithMany()
                .HasConstraintName("FK_ForumBase_ForumUser_Id")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
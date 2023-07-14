﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Models.Forum;
using Entities.Models;

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
                .Property(p => p.Karma)
                .HasColumnType("INTEGER")
                .IsRequired(false)
                .HasDefaultValue(0);
            builder
                .Property(p => p.TotalPostCounter)
                .HasColumnType("INTEGER")
                .IsRequired(false)
                .HasDefaultValue(0);
            builder
                .Property(p => p.CreatedAt)
                .HasColumnType("Date")
                .IsRequired(true);
            builder
                .Property(p => p.UpdatedAt)
                .HasColumnType("Date")
                .IsRequired(false);
            builder
                .Property(p => p.AppUserId)
                .HasColumnType("INTEGER")
                .IsRequired(true);

            builder.Ignore(p => p.SimplifiedName);
            builder.Ignore(p  => p.AvatarImgSrc);

            builder
                .HasKey(p => p.Id)
                .HasName("PK_ForumUser");

            builder
                .HasOne(p => p.AppUser)
                .WithOne(p => p.ForumUser)
                .HasForeignKey<ForumUser>(p => p.Id)
                .HasConstraintName("FK_ForumUser_AppUser_Id")
                .OnDelete(DeleteBehavior.NoAction);

            // Data seed


            builder.HasData(
                new ForumUser()
                {
                    Id = 1,
                    CreatedAt = DateTime.Now,
                    AppUserId = 1
                }
            );
        }

    }
}
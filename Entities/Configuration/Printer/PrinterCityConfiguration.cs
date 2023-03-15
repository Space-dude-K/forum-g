using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Models.Printer;

namespace Entities.Configuration.Printer
{
    public class PrinterCityConfiguration : IEntityTypeConfiguration<PrinterCity>
    {
        public void Configure(EntityTypeBuilder<PrinterCity> builder)
        {
            #region DbStructure
            builder
                .ToTable("PrinterCity");

            builder
                .Property(p => p.Id)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.Name)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(256)
                .IsRequired(true)
                .IsUnicode(true);

            builder
                .HasKey(p => p.Id)
                .HasName("PK_PrinterCity");
            builder
                .HasMany(p => p.Organizations)
                .WithOne(p => p.PrinterCity)
                .HasForeignKey(p => p.PrinterCityId)
                .HasConstraintName("FK_PrinterCity_PrinterOrgranization_PrinterCityId")
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
            #region DbDataSeed
            builder.HasData(
                new PrinterCity()
                {
                    Id = 1,
                    Name = "Могилёв"
                },
                new PrinterCity()
                {
                    Id = 2,
                    Name = "Бобруйск"
                }
            );
            #endregion
        }
    }
}

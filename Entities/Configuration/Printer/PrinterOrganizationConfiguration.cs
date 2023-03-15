using Entities.Models.Printer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.Configuration.Printer
{
    public class PrinterOrganizationConfiguration : IEntityTypeConfiguration<PrinterOrganization>
    {
        public void Configure(EntityTypeBuilder<PrinterOrganization> builder)
        {
            #region DbStructure
            builder
                .ToTable("PrinterOrganization");

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
                .Property(p => p.PrinterCityId)
                .HasColumnType("INTEGER")
                .IsRequired(true);

            builder
                .HasKey(p => p.Id)
                .HasName("PK_PrinterOrganization");
            builder
                .HasMany(p => p.Rooms)
                .WithOne(p => p.PrinterOrganization)
                .HasForeignKey(p => p.PrinterOrganizationId)
                .HasConstraintName("FK_PrinterOrganization_PrinterRoom_PrinterOrganizationId")
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
            #region DbDataSeed
            builder.HasData(
                new PrinterOrganization()
                {
                    Id = 1,
                    Name = "ГУ по Могилёвской области",
                    PrinterCityId = 1
                },
                new PrinterOrganization()
                {
                    Id = 2,
                    Name = "ИВЦ Минфина",
                    PrinterCityId = 2
                }
            );
            #endregion
        }
    }
}
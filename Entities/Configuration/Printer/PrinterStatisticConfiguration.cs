using Entities.Models.Printer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.Configuration.Printer
{
    public class PrinterStatisticConfiguration : IEntityTypeConfiguration<PrinterStatistic>
    {
        public void Configure(EntityTypeBuilder<PrinterStatistic> builder)
        {
            #region DbStructure
            builder
                .ToTable("PrinterStatistic");

            builder
                .Property(p => p.Id)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.TonerLevel)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.DrumLevel)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.TotalPagesPrinted)
                .HasColumnType("INTEGER")
                .IsRequired(true);

            builder
                .HasKey(p => p.Id)
                .HasName("PK_PrinterStatistic");

            builder
                .HasOne(p => p.PrinterDevice)
                .WithOne(p => p.PrinterStatistic)
                .HasConstraintName("FK_PrinterStatistic_PrinterDevice_PrinterStatisticId")
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
            #region DbDataSeed
            builder.HasData(
                new PrinterStatistic()
                {
                    Id = 1,
                    TonerLevel = 45,
                    DrumLevel = 40,
                    TotalPagesPrinted = 123
                },
                new PrinterStatistic()
                {
                    Id = 2,
                    TonerLevel = 45,
                    DrumLevel = 40,
                    TotalPagesPrinted = 123
                }
            );
            #endregion
        }
    }
}

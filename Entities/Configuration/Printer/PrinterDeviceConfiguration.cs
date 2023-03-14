using Entities.Models.Printer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.Configuration.Printer
{
    public class PrinterDeviceConfiguration : IEntityTypeConfiguration<PrinterDevice>
    {
        public void Configure(EntityTypeBuilder<PrinterDevice> builder)
        {
            #region DbStructure
            builder
                .ToTable("PrinterDevice");

            builder
                .Property(p => p.Id)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.PrinterTypeId)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.PrinterRoomId)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.PrinterStatisticId)
                .HasColumnType("INTEGER");

            builder
                .HasKey(p => p.Id)
                .HasName("PK_PrinterDevice");
            builder
                .HasOne(p => p.PrinterType)
                .WithOne(p => p.PrinterDevice)
                .HasForeignKey<PrinterDevice>(p => p.PrinterTypeId)
                .HasConstraintName("FK_PrinterDevice_PrinterType_PrinterTypeId");
            builder
                .HasOne(p => p.PrinterStatistic)
                .WithOne(p => p.PrinterDevice)
                .HasForeignKey<PrinterDevice>(p => p.PrinterStatisticId)
                .HasConstraintName("FK_PrinterDevice_PrinterStatistic_PrinterStatisticId");
            #endregion
            #region DbDataSeed
            builder.HasData(
                new PrinterDevice()
                {
                    Id = 1,
                    PrinterTypeId = 1,
                    PrinterRoomId = 1,
                    PrinterStatisticId = 1
                },
                new PrinterDevice()
                {
                    Id = 2,
                    PrinterTypeId = 2,
                    PrinterRoomId = 2,
                    PrinterStatisticId = 2
                }
            );
            #endregion
        }
    }
}
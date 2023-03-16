using Entities.Models.Printer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.Configuration.Printer
{
    public class PrinterRoomHistoryConfiguration : IEntityTypeConfiguration<PrinterRoomHistory>
    {
        public void Configure(EntityTypeBuilder<PrinterRoomHistory> builder)
        {
            #region DbStructure
            builder
                .ToTable("PrinterRoomHistory");

            builder
                .Property(p => p.Id)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.Reason)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(256)
                .IsRequired(true)
                .IsUnicode(true);
            builder
                .Property(p => p.InstalledAt)
                .HasColumnType("DATE")
                .IsRequired(true);
            builder
                .Property(p => p.DeletedAt)
                .HasColumnType("DATE")
                .IsRequired(true);
            builder
                .Property(p => p.PrinterDeviceId)
                .HasColumnType("INTEGER")
                .IsRequired(true);

            builder
                .HasKey(p => p.Id)
                .HasName("PK_PrinterRoomHistory");
            #endregion
            #region DbDataSeed
            builder.HasData(
                new PrinterRoomHistory()
                {
                    Id = 1,
                    Reason = "Тех. неисправность",
                    InstalledAt = DateTime.Now,
                    DeletedAt = DateTime.Now,
                    PrinterDeviceId = 1
                },
                new PrinterRoomHistory()
                {
                    Id = 2,
                    Reason = "Тех. неисправность",
                    InstalledAt = DateTime.Now,
                    DeletedAt = DateTime.Now,
                    PrinterDeviceId = 2
                }
            );
            #endregion
        }
    }
}
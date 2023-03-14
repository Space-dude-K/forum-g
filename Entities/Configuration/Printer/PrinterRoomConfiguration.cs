using Entities.Models.Printer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.Configuration.Printer
{
    public class PrinterRoomConfiguration : IEntityTypeConfiguration<PrinterRoom>
    {
        public void Configure(EntityTypeBuilder<PrinterRoom> builder)
        {
            #region DbStructure
            builder
                .ToTable("PrinterRoom");

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
                .Property(p => p.PrinterOrganizationId)
                .HasColumnType("INTEGER")
                .IsRequired(true);
            builder
                .Property(p => p.PrinterRoomHistoryId)
                .HasColumnType("INTEGER");

            builder
                .HasKey(p => p.Id)
                .HasName("PK_PrinterRoom");
            builder
                .HasOne(p => p.RoomHistory)
                .WithOne(p => p.PrinterRoom)
                .HasForeignKey<PrinterRoom>(p => p.PrinterRoomHistoryId)
                .HasConstraintName("FK_PrinterRoom_PrinterRoomHistory_PrinterRoomHistoryId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
            builder
                .HasMany(p => p.Printers)
                .WithOne(p => p.PrinterRoom)
                .HasForeignKey(p => p.PrinterRoomId)
                .HasConstraintName("FK_PrinterRoom_PrinterDevice_PrinterRoomId")
                .OnDelete(DeleteBehavior.Restrict);
            #endregion
            #region DbDataSeed
            builder.HasData(
                new PrinterRoom()
                {
                    Id = 1,
                    Name = "511",
                    PrinterOrganizationId = 1

                },
                new PrinterRoom()
                {
                    Id = 2,
                    Name = "512",
                    PrinterOrganizationId = 2
                }
            );
            #endregion
        }
    }
}
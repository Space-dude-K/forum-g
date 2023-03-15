using Entities.Models.Printer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.Configuration.Printer
{
    public class PrinterTypeConfiguration : IEntityTypeConfiguration<PrinterType>
    {
        public void Configure(EntityTypeBuilder<PrinterType> builder)
        {
            #region DbStructure
            builder
                .ToTable("PrinterType");

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
                .HasName("PK_PrinterType");
            #endregion
            #region DbDataSeed
            builder.HasData(
                new PrinterType()
                {
                    Id = 1,
                    Name = "Lexmark 421"
                },
                new PrinterType()
                {
                    Id = 2,
                    Name = "Lexmark 2200"
                }
            );
            #endregion
        }
    }
}
using Entities.Configuration;
using Entities.Configuration.Forum;
using Entities.Configuration.Printer;
using Entities.Models;
using Entities.Models.Forum;
using Entities.Models.Printer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class PrinterContext : DbContext
    {
        public PrinterContext(DbContextOptions<PrinterContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PrinterCityConfiguration());
            modelBuilder.ApplyConfiguration(new PrinterDeviceConfiguration());
            modelBuilder.ApplyConfiguration(new PrinterOrganizationConfiguration());
            modelBuilder.ApplyConfiguration(new PrinterRoomConfiguration());
            modelBuilder.ApplyConfiguration(new PrinterRoomHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new PrinterStatisticConfiguration());
            modelBuilder.ApplyConfiguration(new PrinterTypeConfiguration());
        }
        public DbSet<PrinterCity> Cities { get; set; }
        public DbSet<PrinterRoomHistory> RoomHistories { get; set; }
        public DbSet<PrinterOrganization> Organizations { get; set; }
        public DbSet<PrinterDevice> Printers { get; set; }
        public DbSet<PrinterRoom> Rooms { get; set; }
        public DbSet<PrinterStatistic> Statistics { get; set; }
        public DbSet<PrinterType> Types { get; set; }
    }
}

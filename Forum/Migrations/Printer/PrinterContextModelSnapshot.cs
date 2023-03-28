﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Forum.Migrations.Printer
{
    [DbContext(typeof(PrinterContext))]
    partial class PrinterContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Entities.Models.Printer.PrinterCity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(true)
                        .HasColumnType("NVARCHAR");

                    b.HasKey("Id")
                        .HasName("PK_PrinterCity");

                    b.ToTable("PrinterCity", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Могилёв"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Бобруйск"
                        });
                });

            modelBuilder.Entity("Entities.Models.Printer.PrinterDevice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("PrinterRoomId")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PrinterStatisticId")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PrinterTypeId")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id")
                        .HasName("PK_PrinterDevice");

                    b.HasIndex("PrinterRoomId");

                    b.HasIndex("PrinterStatisticId")
                        .IsUnique();

                    b.HasIndex("PrinterTypeId")
                        .IsUnique();

                    b.ToTable("PrinterDevice", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            PrinterRoomId = 1,
                            PrinterStatisticId = 1,
                            PrinterTypeId = 1
                        },
                        new
                        {
                            Id = 2,
                            PrinterRoomId = 2,
                            PrinterStatisticId = 2,
                            PrinterTypeId = 2
                        });
                });

            modelBuilder.Entity("Entities.Models.Printer.PrinterOrganization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(true)
                        .HasColumnType("NVARCHAR");

                    b.Property<int>("PrinterCityId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id")
                        .HasName("PK_PrinterOrganization");

                    b.HasIndex("PrinterCityId");

                    b.ToTable("PrinterOrganization", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "ГУ по Могилёвской области",
                            PrinterCityId = 1
                        },
                        new
                        {
                            Id = 2,
                            Name = "ИВЦ Минфина",
                            PrinterCityId = 2
                        });
                });

            modelBuilder.Entity("Entities.Models.Printer.PrinterRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(true)
                        .HasColumnType("NVARCHAR");

                    b.Property<int>("PrinterOrganizationId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PrinterRoomHistoryId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id")
                        .HasName("PK_PrinterRoom");

                    b.HasIndex("PrinterOrganizationId");

                    b.HasIndex("PrinterRoomHistoryId")
                        .IsUnique()
                        .HasFilter("[PrinterRoomHistoryId] IS NOT NULL");

                    b.ToTable("PrinterRoom", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "511",
                            PrinterOrganizationId = 1
                        },
                        new
                        {
                            Id = 2,
                            Name = "512",
                            PrinterOrganizationId = 2
                        });
                });

            modelBuilder.Entity("Entities.Models.Printer.PrinterRoomHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DeletedAt")
                        .IsRequired()
                        .HasColumnType("DATE");

                    b.Property<DateTime?>("InstalledAt")
                        .IsRequired()
                        .HasColumnType("DATE");

                    b.Property<int>("PrinterDeviceId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(true)
                        .HasColumnType("NVARCHAR");

                    b.HasKey("Id")
                        .HasName("PK_PrinterRoomHistory");

                    b.ToTable("PrinterRoomHistory", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DeletedAt = new DateTime(2023, 3, 27, 19, 20, 46, 132, DateTimeKind.Local).AddTicks(2859),
                            InstalledAt = new DateTime(2023, 3, 27, 19, 20, 46, 132, DateTimeKind.Local).AddTicks(2849),
                            PrinterDeviceId = 1,
                            Reason = "Тех. неисправность"
                        },
                        new
                        {
                            Id = 2,
                            DeletedAt = new DateTime(2023, 3, 27, 19, 20, 46, 132, DateTimeKind.Local).AddTicks(2862),
                            InstalledAt = new DateTime(2023, 3, 27, 19, 20, 46, 132, DateTimeKind.Local).AddTicks(2861),
                            PrinterDeviceId = 2,
                            Reason = "Тех. неисправность"
                        });
                });

            modelBuilder.Entity("Entities.Models.Printer.PrinterStatistic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DrumLevel")
                        .HasMaxLength(60)
                        .HasColumnType("INTEGER");

                    b.Property<int>("TonerLevel")
                        .HasMaxLength(60)
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalPagesPrinted")
                        .HasMaxLength(60)
                        .HasColumnType("INTEGER");

                    b.HasKey("Id")
                        .HasName("PK_PrinterStatistic");

                    b.ToTable("PrinterStatistic", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DrumLevel = 40,
                            TonerLevel = 45,
                            TotalPagesPrinted = 123
                        },
                        new
                        {
                            Id = 2,
                            DrumLevel = 40,
                            TonerLevel = 45,
                            TotalPagesPrinted = 123
                        });
                });

            modelBuilder.Entity("Entities.Models.Printer.PrinterType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(true)
                        .HasColumnType("NVARCHAR");

                    b.HasKey("Id")
                        .HasName("PK_PrinterType");

                    b.ToTable("PrinterType", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Lexmark 421"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Lexmark 2200"
                        });
                });

            modelBuilder.Entity("Entities.Models.Printer.PrinterDevice", b =>
                {
                    b.HasOne("Entities.Models.Printer.PrinterRoom", "PrinterRoom")
                        .WithMany("Printers")
                        .HasForeignKey("PrinterRoomId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_PrinterRoom_PrinterDevice_PrinterRoomId");

                    b.HasOne("Entities.Models.Printer.PrinterStatistic", "PrinterStatistic")
                        .WithOne("PrinterDevice")
                        .HasForeignKey("Entities.Models.Printer.PrinterDevice", "PrinterStatisticId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_PrinterStatistic_PrinterDevice_PrinterStatisticId");

                    b.HasOne("Entities.Models.Printer.PrinterType", "PrinterType")
                        .WithOne("PrinterDevice")
                        .HasForeignKey("Entities.Models.Printer.PrinterDevice", "PrinterTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_PrinterDevice_PrinterType_PrinterTypeId");

                    b.Navigation("PrinterRoom");

                    b.Navigation("PrinterStatistic");

                    b.Navigation("PrinterType");
                });

            modelBuilder.Entity("Entities.Models.Printer.PrinterOrganization", b =>
                {
                    b.HasOne("Entities.Models.Printer.PrinterCity", "PrinterCity")
                        .WithMany("Organizations")
                        .HasForeignKey("PrinterCityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_PrinterCity_PrinterOrgranization_PrinterCityId");

                    b.Navigation("PrinterCity");
                });

            modelBuilder.Entity("Entities.Models.Printer.PrinterRoom", b =>
                {
                    b.HasOne("Entities.Models.Printer.PrinterOrganization", "PrinterOrganization")
                        .WithMany("Rooms")
                        .HasForeignKey("PrinterOrganizationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_PrinterOrganization_PrinterRoom_PrinterOrganizationId");

                    b.HasOne("Entities.Models.Printer.PrinterRoomHistory", "RoomHistory")
                        .WithOne("PrinterRoom")
                        .HasForeignKey("Entities.Models.Printer.PrinterRoom", "PrinterRoomHistoryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .HasConstraintName("FK_PrinterRoom_PrinterRoomHistory_PrinterRoomHistoryId");

                    b.Navigation("PrinterOrganization");

                    b.Navigation("RoomHistory");
                });

            modelBuilder.Entity("Entities.Models.Printer.PrinterCity", b =>
                {
                    b.Navigation("Organizations");
                });

            modelBuilder.Entity("Entities.Models.Printer.PrinterOrganization", b =>
                {
                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("Entities.Models.Printer.PrinterRoom", b =>
                {
                    b.Navigation("Printers");
                });

            modelBuilder.Entity("Entities.Models.Printer.PrinterRoomHistory", b =>
                {
                    b.Navigation("PrinterRoom")
                        .IsRequired();
                });

            modelBuilder.Entity("Entities.Models.Printer.PrinterStatistic", b =>
                {
                    b.Navigation("PrinterDevice")
                        .IsRequired();
                });

            modelBuilder.Entity("Entities.Models.Printer.PrinterType", b =>
                {
                    b.Navigation("PrinterDevice")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

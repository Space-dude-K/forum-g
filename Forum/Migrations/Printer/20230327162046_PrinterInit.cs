using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Forum.Migrations.Printer
{
    /// <inheritdoc />
    public partial class PrinterInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrinterCity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrinterCity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrinterRoomHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: false),
                    InstalledAt = table.Column<DateTime>(type: "DATE", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "DATE", nullable: false),
                    PrinterDeviceId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrinterRoomHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrinterStatistic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TonerLevel = table.Column<int>(type: "INTEGER", maxLength: 60, nullable: false),
                    DrumLevel = table.Column<int>(type: "INTEGER", maxLength: 60, nullable: false),
                    TotalPagesPrinted = table.Column<int>(type: "INTEGER", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrinterStatistic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrinterType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrinterType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrinterOrganization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: false),
                    PrinterCityId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrinterOrganization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrinterCity_PrinterOrgranization_PrinterCityId",
                        column: x => x.PrinterCityId,
                        principalTable: "PrinterCity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrinterRoom",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: false),
                    PrinterOrganizationId = table.Column<int>(type: "INTEGER", nullable: false),
                    PrinterRoomHistoryId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrinterRoom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrinterOrganization_PrinterRoom_PrinterOrganizationId",
                        column: x => x.PrinterOrganizationId,
                        principalTable: "PrinterOrganization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrinterRoom_PrinterRoomHistory_PrinterRoomHistoryId",
                        column: x => x.PrinterRoomHistoryId,
                        principalTable: "PrinterRoomHistory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PrinterDevice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrinterTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    PrinterStatisticId = table.Column<int>(type: "INTEGER", nullable: false),
                    PrinterRoomId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrinterDevice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrinterDevice_PrinterType_PrinterTypeId",
                        column: x => x.PrinterTypeId,
                        principalTable: "PrinterType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrinterRoom_PrinterDevice_PrinterRoomId",
                        column: x => x.PrinterRoomId,
                        principalTable: "PrinterRoom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrinterStatistic_PrinterDevice_PrinterStatisticId",
                        column: x => x.PrinterStatisticId,
                        principalTable: "PrinterStatistic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "PrinterCity",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Могилёв" },
                    { 2, "Бобруйск" }
                });

            migrationBuilder.InsertData(
                table: "PrinterRoomHistory",
                columns: new[] { "Id", "DeletedAt", "InstalledAt", "PrinterDeviceId", "Reason" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 3, 27, 19, 20, 46, 132, DateTimeKind.Local).AddTicks(2859), new DateTime(2023, 3, 27, 19, 20, 46, 132, DateTimeKind.Local).AddTicks(2849), 1, "Тех. неисправность" },
                    { 2, new DateTime(2023, 3, 27, 19, 20, 46, 132, DateTimeKind.Local).AddTicks(2862), new DateTime(2023, 3, 27, 19, 20, 46, 132, DateTimeKind.Local).AddTicks(2861), 2, "Тех. неисправность" }
                });

            migrationBuilder.InsertData(
                table: "PrinterStatistic",
                columns: new[] { "Id", "DrumLevel", "TonerLevel", "TotalPagesPrinted" },
                values: new object[,]
                {
                    { 1, 40, 45, 123 },
                    { 2, 40, 45, 123 }
                });

            migrationBuilder.InsertData(
                table: "PrinterType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Lexmark 421" },
                    { 2, "Lexmark 2200" }
                });

            migrationBuilder.InsertData(
                table: "PrinterOrganization",
                columns: new[] { "Id", "Name", "PrinterCityId" },
                values: new object[,]
                {
                    { 1, "ГУ по Могилёвской области", 1 },
                    { 2, "ИВЦ Минфина", 2 }
                });

            migrationBuilder.InsertData(
                table: "PrinterRoom",
                columns: new[] { "Id", "Name", "PrinterOrganizationId", "PrinterRoomHistoryId" },
                values: new object[,]
                {
                    { 1, "511", 1, null },
                    { 2, "512", 2, null }
                });

            migrationBuilder.InsertData(
                table: "PrinterDevice",
                columns: new[] { "Id", "PrinterRoomId", "PrinterStatisticId", "PrinterTypeId" },
                values: new object[,]
                {
                    { 1, 1, 1, 1 },
                    { 2, 2, 2, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrinterDevice_PrinterRoomId",
                table: "PrinterDevice",
                column: "PrinterRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_PrinterDevice_PrinterStatisticId",
                table: "PrinterDevice",
                column: "PrinterStatisticId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrinterDevice_PrinterTypeId",
                table: "PrinterDevice",
                column: "PrinterTypeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrinterOrganization_PrinterCityId",
                table: "PrinterOrganization",
                column: "PrinterCityId");

            migrationBuilder.CreateIndex(
                name: "IX_PrinterRoom_PrinterOrganizationId",
                table: "PrinterRoom",
                column: "PrinterOrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PrinterRoom_PrinterRoomHistoryId",
                table: "PrinterRoom",
                column: "PrinterRoomHistoryId",
                unique: true,
                filter: "[PrinterRoomHistoryId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrinterDevice");

            migrationBuilder.DropTable(
                name: "PrinterType");

            migrationBuilder.DropTable(
                name: "PrinterRoom");

            migrationBuilder.DropTable(
                name: "PrinterStatistic");

            migrationBuilder.DropTable(
                name: "PrinterOrganization");

            migrationBuilder.DropTable(
                name: "PrinterRoomHistory");

            migrationBuilder.DropTable(
                name: "PrinterCity");
        }
    }
}

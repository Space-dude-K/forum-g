using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Forum.Migrations
{
    /// <inheritdoc />
    public partial class ForumBaseUnicodeSupport_ForumBaseDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1c19f9b7-6160-48de-a3dc-b9e9bb2efca2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a7068527-c11d-49b8-b8ee-c25215db07b8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "667d023b-3bc1-451b-8f91-e1ae0e6e062f", null, "USER", "USER" },
                    { "b4165309-0db7-48eb-92e2-370f7cdc275b", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "ForumBase",
                columns: new[] { "Id", "CreatedAt", "ForumCategoryId", "ForumSubTitle", "ForumTitle", "ForumUserId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "14.03.2023", 1, "Test forum subtitle 1", "Test forum title 1", 1, null },
                    { 2, "14.03.2023", 2, "Test forum subtitle 2", "Test forum title 2", 2, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "667d023b-3bc1-451b-8f91-e1ae0e6e062f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4165309-0db7-48eb-92e2-370f7cdc275b");

            migrationBuilder.DeleteData(
                table: "ForumBase",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ForumBase",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1c19f9b7-6160-48de-a3dc-b9e9bb2efca2", null, "Administrator", "ADMINISTRATOR" },
                    { "a7068527-c11d-49b8-b8ee-c25215db07b8", null, "USER", "USER" }
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Forum.Migrations
{
    /// <inheritdoc />
    public partial class CategoryUnicodeSupport_CategoryDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14896d8e-92ce-4a8f-853c-509055261734");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a8950f8a-ad0f-4f18-8fc7-e94f37a8d8a9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1c19f9b7-6160-48de-a3dc-b9e9bb2efca2", null, "Administrator", "ADMINISTRATOR" },
                    { "a7068527-c11d-49b8-b8ee-c25215db07b8", null, "USER", "USER" }
                });

            migrationBuilder.InsertData(
                table: "ForumCategory",
                columns: new[] { "Id", "CreatedAt", "ForumUserId", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "14.03.2023", 1, "Test subtopic 1", null },
                    { 2, "14.03.2023", 2, "Test subtopic 2", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1c19f9b7-6160-48de-a3dc-b9e9bb2efca2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a7068527-c11d-49b8-b8ee-c25215db07b8");

            migrationBuilder.DeleteData(
                table: "ForumCategory",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ForumCategory",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "14896d8e-92ce-4a8f-853c-509055261734", null, "Administrator", "ADMINISTRATOR" },
                    { "a8950f8a-ad0f-4f18-8fc7-e94f37a8d8a9", null, "USER", "USER" }
                });
        }
    }
}

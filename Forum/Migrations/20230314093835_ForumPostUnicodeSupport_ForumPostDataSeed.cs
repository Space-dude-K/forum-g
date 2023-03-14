using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Forum.Migrations
{
    /// <inheritdoc />
    public partial class ForumPostUnicodeSupport_ForumPostDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ccec49bb-4b3e-4fe0-bd45-77193f2593e4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4d78dda-78ad-4b71-ba48-44cc5d356c0c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "21f1e89a-ffcc-4dd4-8bad-2665ec44a687", null, "USER", "USER" },
                    { "5bd65d66-9a98-49b0-9de7-28ee8a9e5d18", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "ForumPost",
                columns: new[] { "Id", "CreatedAt", "ForumTopicId", "ForumUserId", "PostName", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "14.03.2023", 1, 1, "Post name 1", null },
                    { 2, "14.03.2023", 2, 2, "Post name 2", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "21f1e89a-ffcc-4dd4-8bad-2665ec44a687");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5bd65d66-9a98-49b0-9de7-28ee8a9e5d18");

            migrationBuilder.DeleteData(
                table: "ForumPost",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ForumPost",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ccec49bb-4b3e-4fe0-bd45-77193f2593e4", null, "USER", "USER" },
                    { "d4d78dda-78ad-4b71-ba48-44cc5d356c0c", null, "Administrator", "ADMINISTRATOR" }
                });
        }
    }
}

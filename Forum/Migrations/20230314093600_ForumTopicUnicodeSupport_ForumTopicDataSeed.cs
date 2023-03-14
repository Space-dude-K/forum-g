using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Forum.Migrations
{
    /// <inheritdoc />
    public partial class ForumTopicUnicodeSupport_ForumTopicDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "667d023b-3bc1-451b-8f91-e1ae0e6e062f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4165309-0db7-48eb-92e2-370f7cdc275b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ccec49bb-4b3e-4fe0-bd45-77193f2593e4", null, "USER", "USER" },
                    { "d4d78dda-78ad-4b71-ba48-44cc5d356c0c", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "ForumTopic",
                columns: new[] { "Id", "CreatedAt", "ForumBaseId", "ForumUserId", "Name", "TopicViewCounter", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "14.03.2023", 1, 1, "Test forum topic 1", 0, null },
                    { 2, "14.03.2023", 2, 2, "Test forum topic 2", 0, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ccec49bb-4b3e-4fe0-bd45-77193f2593e4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4d78dda-78ad-4b71-ba48-44cc5d356c0c");

            migrationBuilder.DeleteData(
                table: "ForumTopic",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ForumTopic",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "667d023b-3bc1-451b-8f91-e1ae0e6e062f", null, "USER", "USER" },
                    { "b4165309-0db7-48eb-92e2-370f7cdc275b", null, "Administrator", "ADMINISTRATOR" }
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Forum.Migrations
{
    /// <inheritdoc />
    public partial class ForumAccountRemovingLoginAndPass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "21f1e89a-ffcc-4dd4-8bad-2665ec44a687");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5bd65d66-9a98-49b0-9de7-28ee8a9e5d18");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "ForumAccount");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "ForumAccount");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1b16243d-65d9-4124-90ce-22155a034197", null, "USER", "USER" },
                    { "d188f92d-a477-45b9-a4d8-9b9d34e0a0b1", null, "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b16243d-65d9-4124-90ce-22155a034197");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d188f92d-a477-45b9-a4d8-9b9d34e0a0b1");

            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "ForumAccount",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "ForumAccount",
                type: "TEXT",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "21f1e89a-ffcc-4dd4-8bad-2665ec44a687", null, "USER", "USER" },
                    { "5bd65d66-9a98-49b0-9de7-28ee8a9e5d18", null, "Administrator", "ADMINISTRATOR" }
                });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Forum.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEmailFromForumUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5594b5fc-f1e8-41ff-a187-99476e51e6f2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bf71b575-9905-49ec-95da-dac8c6849058");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ForumUser");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "14896d8e-92ce-4a8f-853c-509055261734", null, "Administrator", "ADMINISTRATOR" },
                    { "a8950f8a-ad0f-4f18-8fc7-e94f37a8d8a9", null, "USER", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "14896d8e-92ce-4a8f-853c-509055261734");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a8950f8a-ad0f-4f18-8fc7-e94f37a8d8a9");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ForumUser",
                type: "TEXT",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5594b5fc-f1e8-41ff-a187-99476e51e6f2", null, "Administrator", "ADMINISTRATOR" },
                    { "bf71b575-9905-49ec-95da-dac8c6849058", null, "USER", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "ForumUser",
                keyColumn: "Id",
                keyValue: 1,
                column: "Email",
                value: "test1@mail.ru");

            migrationBuilder.UpdateData(
                table: "ForumUser",
                keyColumn: "Id",
                keyValue: 2,
                column: "Email",
                value: "test2@mail.ru");
        }
    }
}

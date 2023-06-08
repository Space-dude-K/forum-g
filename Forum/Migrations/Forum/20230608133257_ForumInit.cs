using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Forum.Migrations.Forum
{
    /// <inheritdoc />
    public partial class ForumInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cabinet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternalPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForumAccountType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumAccountType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForumUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true),
                    Surname = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true),
                    Lastname = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true),
                    Karma = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "Date", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "Date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForumAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Ip = table.Column<string>(type: "NVARCHAR", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumAccount_ForumAccountType_Id",
                        column: x => x.AccountTypeId,
                        principalTable: "ForumAccountType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ForumAccount_ForumUser_Id",
                        column: x => x.Id,
                        principalTable: "ForumUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForumCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "Date", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "Date", nullable: true),
                    ForumUserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumCategory_ForumUser_Id",
                        column: x => x.ForumUserId,
                        principalTable: "ForumUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ForumBase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ForumTitle = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: false),
                    ForumSubTitle = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "Date", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "Date", nullable: true),
                    ForumCategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    ForumUserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumBase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumBase_ForumUser_Id",
                        column: x => x.ForumUserId,
                        principalTable: "ForumUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ForumCategory_ForumBase_Id",
                        column: x => x.ForumCategoryId,
                        principalTable: "ForumCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForumTopic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "Date", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "Date", nullable: true),
                    TopicViewCounter = table.Column<int>(type: "INTEGER", nullable: false),
                    ForumUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ForumBaseId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumTopic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumBase_ForumTopic_ForumBaseId",
                        column: x => x.ForumBaseId,
                        principalTable: "ForumBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ForumTopic_ForumUser_Id",
                        column: x => x.ForumUserId,
                        principalTable: "ForumUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ForumPost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostName = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true),
                    Likes = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "Date", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "Date", nullable: true),
                    ForumTopicId = table.Column<int>(type: "INTEGER", nullable: false),
                    ForumUserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumPost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumPost_ForumUser_Id",
                        column: x => x.ForumUserId,
                        principalTable: "ForumUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ForumTopic_ForumPost_ForumTopicId",
                        column: x => x.ForumTopicId,
                        principalTable: "ForumTopic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b96ab6b0-7086-4595-b634-ebbbbcc72799", null, "USER", "USER" },
                    { "baf98e30-f9e3-4ec6-a7d0-93c4f36cb2ea", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "Cabinet", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "InternalPhone", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "054134b5-354f-4dd3-b1d3-681a4a15c4b1", 0, "02.12.1996 0:00:00", "877", "93a05d86-f748-420c-8b87-0c6a4c11e4d5", "G600-U2@mfrb.by", false, "Ivan2", "1048734400", "Petrov2", false, null, null, null, null, "200806498820912972061096593173", false, "d6a82302-79e9-41b4-8f4e-a6010f4ec48a", false, "G600-U2" },
                    { "38169076-fecd-4b37-9424-4ac9d583ff9e", 0, "27.09.2014 0:00:00", "817", "b9d8edd5-25b7-43cf-b9f7-8c0525905e1c", "G600-U3@mfrb.by", false, "Ivan3", "1553677160", "Petrov3", false, null, null, null, null, "74726025031652140637024588", false, "8d05e15e-68c0-44ed-a535-36e93a4a2821", false, "G600-U3" },
                    { "75eaff78-efda-40cd-9572-95faeea51ef1", 0, "15.07.2000 0:00:00", "88", "0e1913e1-a299-4c2f-b3e0-44bf1007adce", "G600-U1@mfrb.by", false, "Ivan1", "1571008649", "Petrov1", false, null, null, null, null, "147722010316561249631028463220", false, "bf5e15bb-209b-490e-b6db-0119686f1e3e", false, "G600-U1" },
                    { "b29b2479-2165-47f0-a021-258919a8990b", 0, "21.10.2014 0:00:00", "688", "e85eac6c-6d2f-4597-8e1e-4cf9efdb415f", "G600-U4@mfrb.by", false, "Ivan4", "1579087271", "Petrov4", false, null, null, null, null, "14956614547104796191860146980", false, "a3551607-82eb-4414-a51b-6af4654310b1", false, "G600-U4" },
                    { "bbde42d1-d609-4f8a-8b38-c21e6dc34efe", 0, "24.07.2020 0:00:00", "103", "25909e3c-0f2a-403c-b6b0-4a33eee9e91c", "G600-U0@mfrb.by", false, "Ivan0", "505510686", "Petrov0", false, null, null, null, null, "3233254331311595925519934433", false, "a032f9fa-4c2b-44e3-bdfc-f9050bb3d044", false, "G600-U0" }
                });

            migrationBuilder.InsertData(
                table: "ForumUser",
                columns: new[] { "Id", "CreatedAt", "Lastname", "Name", "Surname", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 6, 8, 16, 32, 57, 699, DateTimeKind.Local).AddTicks(2040), "Сергеевич", "Константин", "Феофанов", null },
                    { 2, new DateTime(2023, 6, 8, 16, 32, 57, 699, DateTimeKind.Local).AddTicks(2051), "Григорьевич", "Александр", "Петров", null }
                });

            migrationBuilder.InsertData(
                table: "ForumCategory",
                columns: new[] { "Id", "CreatedAt", "ForumUserId", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 6, 8, 16, 32, 57, 699, DateTimeKind.Local).AddTicks(6026), 1, "Test category 1", null },
                    { 2, new DateTime(2023, 6, 8, 16, 32, 57, 699, DateTimeKind.Local).AddTicks(6029), 2, "Test category 2", null },
                    { 3, new DateTime(2023, 6, 8, 16, 32, 57, 699, DateTimeKind.Local).AddTicks(6030), 1, "Test category 3", null },
                    { 4, new DateTime(2023, 6, 8, 16, 32, 57, 699, DateTimeKind.Local).AddTicks(6032), 2, "Test category 4", null },
                    { 5, new DateTime(2023, 6, 8, 16, 32, 57, 699, DateTimeKind.Local).AddTicks(6033), 2, "Test category 5", null },
                    { 6, new DateTime(2023, 6, 8, 16, 32, 57, 699, DateTimeKind.Local).AddTicks(6035), 2, "Test category 6", null }
                });

            migrationBuilder.InsertData(
                table: "ForumBase",
                columns: new[] { "Id", "CreatedAt", "ForumCategoryId", "ForumSubTitle", "ForumTitle", "ForumUserId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 6, 8, 16, 32, 57, 699, DateTimeKind.Local).AddTicks(9535), 1, "Test forum subtitle 1", "Test forum title 1", 1, null },
                    { 2, new DateTime(2023, 6, 8, 16, 32, 57, 699, DateTimeKind.Local).AddTicks(9538), 2, "Test forum subtitle 2", "Test forum title 2", 2, null },
                    { 3, new DateTime(2023, 6, 8, 16, 32, 57, 699, DateTimeKind.Local).AddTicks(9539), 2, "Test forum subtitle 3", "Test forum title 3", 2, null },
                    { 4, new DateTime(2023, 6, 8, 16, 32, 57, 699, DateTimeKind.Local).AddTicks(9541), 2, "Test forum subtitle 4", "Test forum title 4", 2, null },
                    { 5, new DateTime(2023, 6, 8, 16, 32, 57, 699, DateTimeKind.Local).AddTicks(9542), 2, "Test forum subtitle 5", "Test forum title 5", 2, null },
                    { 6, new DateTime(2023, 6, 8, 16, 32, 57, 699, DateTimeKind.Local).AddTicks(9544), 2, "Test forum subtitle 6", "Test forum title 6", 2, null }
                });

            migrationBuilder.InsertData(
                table: "ForumTopic",
                columns: new[] { "Id", "CreatedAt", "ForumBaseId", "ForumUserId", "Name", "TopicViewCounter", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 6, 8, 16, 32, 57, 700, DateTimeKind.Local).AddTicks(2742), 1, 1, "Test forum topic 1", 0, null },
                    { 2, new DateTime(2023, 6, 8, 16, 32, 57, 700, DateTimeKind.Local).AddTicks(2747), 2, 2, "Test forum topic 2", 0, null },
                    { 3, new DateTime(2023, 6, 8, 16, 32, 57, 700, DateTimeKind.Local).AddTicks(2749), 2, 2, "Test forum topic 3", 0, null },
                    { 4, new DateTime(2023, 6, 8, 16, 32, 57, 700, DateTimeKind.Local).AddTicks(2750), 2, 2, "Test forum topic 4", 0, null },
                    { 5, new DateTime(2023, 6, 8, 16, 32, 57, 700, DateTimeKind.Local).AddTicks(2751), 2, 2, "Test forum topic 5", 0, null }
                });

            migrationBuilder.InsertData(
                table: "ForumPost",
                columns: new[] { "Id", "CreatedAt", "ForumTopicId", "ForumUserId", "Likes", "PostName", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 6, 8, 16, 32, 57, 700, DateTimeKind.Local).AddTicks(4542), 1, 1, 1, "Post name 1", null },
                    { 2, new DateTime(2023, 6, 8, 16, 32, 57, 700, DateTimeKind.Local).AddTicks(4546), 2, 2, null, "Post name 2", null },
                    { 3, new DateTime(2023, 6, 8, 16, 32, 57, 700, DateTimeKind.Local).AddTicks(4547), 2, 2, null, "Post name 3", null },
                    { 4, new DateTime(2023, 6, 8, 16, 32, 57, 700, DateTimeKind.Local).AddTicks(4549), 2, 2, 34, "Post name 4", null },
                    { 5, new DateTime(2023, 6, 8, 16, 32, 57, 700, DateTimeKind.Local).AddTicks(4550), 2, 2, 65, "Post name 5", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ForumAccount_AccountTypeId",
                table: "ForumAccount",
                column: "AccountTypeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ForumBase_ForumCategoryId",
                table: "ForumBase",
                column: "ForumCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumBase_ForumUserId",
                table: "ForumBase",
                column: "ForumUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumCategory_ForumUserId",
                table: "ForumCategory",
                column: "ForumUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumPost_ForumTopicId",
                table: "ForumPost",
                column: "ForumTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumPost_ForumUserId",
                table: "ForumPost",
                column: "ForumUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopic_ForumBaseId",
                table: "ForumTopic",
                column: "ForumBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopic_ForumUserId",
                table: "ForumTopic",
                column: "ForumUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ForumAccount");

            migrationBuilder.DropTable(
                name: "ForumPost");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ForumAccountType");

            migrationBuilder.DropTable(
                name: "ForumTopic");

            migrationBuilder.DropTable(
                name: "ForumBase");

            migrationBuilder.DropTable(
                name: "ForumCategory");

            migrationBuilder.DropTable(
                name: "ForumUser");
        }
    }
}

﻿using System;
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
                    { "2e1343fd-550c-4cbb-ad9b-f860cfb950be", null, "USER", "USER" },
                    { "bdbec9b9-7c81-4495-a43f-44926c4cbbf8", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "ForumUser",
                columns: new[] { "Id", "CreatedAt", "Lastname", "Name", "Surname", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 4, 20, 20, 59, 23, 86, DateTimeKind.Local).AddTicks(6372), "Сергеевич", "Константин", "Феофанов", null },
                    { 2, new DateTime(2023, 4, 20, 20, 59, 23, 86, DateTimeKind.Local).AddTicks(6385), "Григорьевич", "Александр", "Петров", null }
                });

            migrationBuilder.InsertData(
                table: "ForumCategory",
                columns: new[] { "Id", "CreatedAt", "ForumUserId", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(621), 1, "Test category 1", null },
                    { 2, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(623), 2, "Test category 2", null },
                    { 3, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(625), 1, "Test category 3", null },
                    { 4, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(626), 2, "Test category 4", null },
                    { 5, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(628), 2, "Test category 5", null },
                    { 6, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(629), 2, "Test category 6", null }
                });

            migrationBuilder.InsertData(
                table: "ForumBase",
                columns: new[] { "Id", "CreatedAt", "ForumCategoryId", "ForumSubTitle", "ForumTitle", "ForumUserId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(4313), 1, "Test forum subtitle 1", "Test forum title 1", 1, null },
                    { 2, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(4316), 2, "Test forum subtitle 2", "Test forum title 2", 2, null },
                    { 3, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(4318), 2, "Test forum subtitle 3", "Test forum title 3", 2, null },
                    { 4, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(4320), 2, "Test forum subtitle 4", "Test forum title 4", 2, null },
                    { 5, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(4321), 2, "Test forum subtitle 5", "Test forum title 5", 2, null },
                    { 6, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(4323), 2, "Test forum subtitle 6", "Test forum title 6", 2, null }
                });

            migrationBuilder.InsertData(
                table: "ForumTopic",
                columns: new[] { "Id", "CreatedAt", "ForumBaseId", "ForumUserId", "Name", "TopicViewCounter", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(7705), 1, 1, "Test forum topic 1", 0, null },
                    { 2, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(7711), 2, 2, "Test forum topic 2", 0, null },
                    { 3, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(7712), 2, 2, "Test forum topic 3", 0, null },
                    { 4, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(7714), 2, 2, "Test forum topic 4", 0, null },
                    { 5, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(7715), 2, 2, "Test forum topic 5", 0, null }
                });

            migrationBuilder.InsertData(
                table: "ForumPost",
                columns: new[] { "Id", "CreatedAt", "ForumTopicId", "ForumUserId", "Likes", "PostName", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(9688), 1, 1, 1, "Post name 1", null },
                    { 2, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(9693), 2, 2, null, "Post name 2", null },
                    { 3, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(9694), 2, 2, null, "Post name 3", null },
                    { 4, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(9695), 2, 2, 34, "Post name 4", null },
                    { 5, new DateTime(2023, 4, 20, 20, 59, 23, 87, DateTimeKind.Local).AddTicks(9697), 2, 2, 65, "Post name 5", null }
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
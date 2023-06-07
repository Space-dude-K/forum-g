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
                    { "5b81f2dc-064c-4328-88bd-68aa490af914", null, "USER", "USER" },
                    { "8f3a0e3f-405a-4a84-8072-b20bceecbbd0", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "Cabinet", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "InternalPhone", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "05e25060-21c8-4fe9-8d81-bd4195c27a96", 0, "18.07.2004 0:00:00", "893", "b8d48635-5d10-4094-9fd9-a801f61d154d", "G600-U8@mfrb.by", false, "Ivan8", "471448790", "Petrov8", false, null, null, null, null, "4268934912581328972106156656", false, "751317e5-8024-4b3a-8d7a-aac0038b6e73", false, "G600-U8" },
                    { "06153dd3-5953-46da-a5e6-e945fb079c83", 0, "24.01.2012 0:00:00", "449", "a9a3b304-1931-42ed-95de-7dad707d0a6f", "G600-U9@mfrb.by", false, "Ivan9", "534488241", "Petrov9", false, null, null, null, null, "3232679242136176489392680665", false, "3979954f-6771-4d5a-9e69-72ef023970f0", false, "G600-U9" },
                    { "2051629d-cd21-4957-b873-bea51d29f7fd", 0, "27.02.2003 0:00:00", "944", "d139137b-1213-4f4b-b164-576de7c63e97", "G600-U0@mfrb.by", false, "Ivan0", "510880872", "Petrov0", false, null, null, null, null, "165798383316823993401248649137", false, "8d73fd23-7cad-4833-a660-726354a5de8d", false, "G600-U0" },
                    { "2ebf472b-a36c-44c5-81ab-13e130048ccc", 0, "11.11.1995 0:00:00", "261", "c798940e-1c90-41b6-8e8a-486439b35d6f", "G600-U13@mfrb.by", false, "Ivan13", "1849837374", "Petrov13", false, null, null, null, null, "2087792787212148135325682582", false, "595b78d7-89b3-44d0-a5cb-ec320b1e06b1", false, "G600-U13" },
                    { "2f575a2d-61b3-4653-bfad-65d333d0ea13", 0, "28.08.1999 0:00:00", "42", "25a94e93-8c43-4d78-aaa5-95927224b0f1", "G600-U5@mfrb.by", false, "Ivan5", "1597133684", "Petrov5", false, null, null, null, null, "87235371219028021411896607617", false, "55c67cfd-e1cf-4ed0-93e8-537680dc7c0c", false, "G600-U5" },
                    { "3def3a5b-e1c0-4478-b96f-aeb466648e93", 0, "12.10.2017 0:00:00", "501", "7a1a83d8-19fe-4218-b5c9-25fb146b4ae6", "G600-U6@mfrb.by", false, "Ivan6", "1675750551", "Petrov6", false, null, null, null, null, "16240580326863728211566808264", false, "c63caf56-12bf-430c-a4dd-001805eedc6f", false, "G600-U6" },
                    { "448b10cc-ce98-4e22-a0bd-d8c1a414df09", 0, "24.05.2016 0:00:00", "4", "6eb75546-3eac-4885-a2f1-057a46b6aa67", "G600-U15@mfrb.by", false, "Ivan15", "338138003", "Petrov15", false, null, null, null, null, "631858564583048892503024912", false, "9889ed97-ebb3-4d81-a59f-9e00449d7f85", false, "G600-U15" },
                    { "5540e31c-a272-4430-bcaa-6b0fe7f3ba14", 0, "19.12.2008 0:00:00", "274", "d333167c-bc5f-46ec-95ff-363528268080", "G600-U4@mfrb.by", false, "Ivan4", "713308458", "Petrov4", false, null, null, null, null, "639887927299758158787109123", false, "cb6af019-e5d2-438e-a511-461e64bb43b5", false, "G600-U4" },
                    { "5bd28483-9902-433f-9ab1-08d3092868b5", 0, "14.02.2018 0:00:00", "652", "f0505d66-df5a-4e9e-9c96-3387a6722cad", "G600-U7@mfrb.by", false, "Ivan7", "739485146", "Petrov7", false, null, null, null, null, "2023733221725847308564151679", false, "34168875-c398-4935-b05f-b955823e1d07", false, "G600-U7" },
                    { "61c0264e-2363-4d0a-b5ac-87403dd07e70", 0, "10.07.2014 0:00:00", "708", "b18b2c41-2c4f-445d-abae-6922a5e6c17c", "G600-U2@mfrb.by", false, "Ivan2", "710640452", "Petrov2", false, null, null, null, null, "145491056521403737292000233159", false, "b2aa278f-5297-4d86-ac01-b57601f422e3", false, "G600-U2" },
                    { "9928ee03-ae3e-442e-bd76-e2ed34c4ca6d", 0, "07.05.1999 0:00:00", "471", "42cafacd-8ef9-4ce5-ab36-e0886876c6bb", "G600-U18@mfrb.by", false, "Ivan18", "419803303", "Petrov18", false, null, null, null, null, "42646577719195852811967974044", false, "96788635-44af-4951-818e-cc98f3c852fa", false, "G600-U18" },
                    { "9dfb2e14-d57e-4b79-81b7-1cf4ae01b948", 0, "11.02.2003 0:00:00", "695", "3d98ffa6-4513-447d-8f14-d8b310a30711", "G600-U10@mfrb.by", false, "Ivan10", "540809420", "Petrov10", false, null, null, null, null, "932750206204460584438780434", false, "4d3cf709-007c-452e-8260-8ed635bb6763", false, "G600-U10" },
                    { "a1e75877-9159-4701-8c3f-4d12bbdf857c", 0, "04.06.2014 0:00:00", "673", "bfdc5974-8a2d-41f9-9103-5dc68f6cd323", "G600-U19@mfrb.by", false, "Ivan19", "2121212778", "Petrov19", false, null, null, null, null, "13784092032079354949378881090", false, "a87b3553-1f35-4c42-9d21-cbc683691d00", false, "G600-U19" },
                    { "a8fb1302-24fa-43b5-98df-6fb95aff55c8", 0, "23.08.1995 0:00:00", "53", "5eb51220-af58-4718-b801-3f8be997d843", "G600-U14@mfrb.by", false, "Ivan14", "918888906", "Petrov14", false, null, null, null, null, "16123261541314336666792912724", false, "a70d728d-6845-41cb-a611-cac1d81da2da", false, "G600-U14" },
                    { "b22e995d-d3c2-4319-9df6-c8af8e86141a", 0, "16.05.2009 0:00:00", "436", "a3e334e5-c97b-4bfd-94d6-044872dba16e", "G600-U16@mfrb.by", false, "Ivan16", "1949770999", "Petrov16", false, null, null, null, null, "212318729314521436471452845729", false, "1b787661-3c4c-4967-84a7-cfa353dfcd55", false, "G600-U16" },
                    { "beeeea72-546a-48b0-b5dd-6ec5bf0805b2", 0, "26.08.2000 0:00:00", "90", "cbc7ee96-ae7a-45d8-81fa-5e42313bce78", "G600-U17@mfrb.by", false, "Ivan17", "724819791", "Petrov17", false, null, null, null, null, "13667334742076341070264848158", false, "887676ac-f5fd-49d4-8642-d57034c0072f", false, "G600-U17" },
                    { "c6a32867-0013-49ec-a6d4-267cab546c28", 0, "27.09.1999 0:00:00", "494", "c2a68e28-7f07-464d-b5aa-38724479d150", "G600-U3@mfrb.by", false, "Ivan3", "1182277290", "Petrov3", false, null, null, null, null, "160407655250136365662063529", false, "6c896025-638a-4ecd-a9ed-59072b7f299f", false, "G600-U3" },
                    { "ca943fe5-6365-43d5-b5b5-1a7b303971bf", 0, "05.05.2015 0:00:00", "338", "08007ee7-ad18-487b-9937-2c56d2b38cc8", "G600-U12@mfrb.by", false, "Ivan12", "883661659", "Petrov12", false, null, null, null, null, "2660345191505330144138227614", false, "46286871-8a4c-40f7-9b75-fbd3605e666b", false, "G600-U12" },
                    { "d3080321-63ab-48bd-a275-c93067ba2f9d", 0, "22.03.1995 0:00:00", "611", "d33d008c-4afa-4ca1-abdc-5dad06b830c1", "G600-U1@mfrb.by", false, "Ivan1", "1533230432", "Petrov1", false, null, null, null, null, "15462945076167369451003679898", false, "32a514cd-a2f3-449c-9d78-4d153b42d9ef", false, "G600-U1" },
                    { "d88220a3-fa85-4b2d-b027-5b0b39e7dc15", 0, "13.03.2023 0:00:00", "554", "b6803a45-3605-4fa1-a46b-f25132ffab93", "G600-U11@mfrb.by", false, "Ivan11", "1634898877", "Petrov11", false, null, null, null, null, "7425950798067706461553283657", false, "47e4ffbf-e142-40c0-8b80-fb5cfa2cb352", false, "G600-U11" }
                });

            migrationBuilder.InsertData(
                table: "ForumUser",
                columns: new[] { "Id", "CreatedAt", "Lastname", "Name", "Surname", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 6, 7, 14, 27, 58, 882, DateTimeKind.Local).AddTicks(8098), "Сергеевич", "Константин", "Феофанов", null },
                    { 2, new DateTime(2023, 6, 7, 14, 27, 58, 882, DateTimeKind.Local).AddTicks(8110), "Григорьевич", "Александр", "Петров", null }
                });

            migrationBuilder.InsertData(
                table: "ForumCategory",
                columns: new[] { "Id", "CreatedAt", "ForumUserId", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 6, 7, 14, 27, 58, 883, DateTimeKind.Local).AddTicks(2401), 1, "Test category 1", null },
                    { 2, new DateTime(2023, 6, 7, 14, 27, 58, 883, DateTimeKind.Local).AddTicks(2403), 2, "Test category 2", null },
                    { 3, new DateTime(2023, 6, 7, 14, 27, 58, 883, DateTimeKind.Local).AddTicks(2404), 1, "Test category 3", null },
                    { 4, new DateTime(2023, 6, 7, 14, 27, 58, 883, DateTimeKind.Local).AddTicks(2405), 2, "Test category 4", null },
                    { 5, new DateTime(2023, 6, 7, 14, 27, 58, 883, DateTimeKind.Local).AddTicks(2406), 2, "Test category 5", null },
                    { 6, new DateTime(2023, 6, 7, 14, 27, 58, 883, DateTimeKind.Local).AddTicks(2407), 2, "Test category 6", null }
                });

            migrationBuilder.InsertData(
                table: "ForumBase",
                columns: new[] { "Id", "CreatedAt", "ForumCategoryId", "ForumSubTitle", "ForumTitle", "ForumUserId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 6, 7, 14, 27, 58, 883, DateTimeKind.Local).AddTicks(5713), 1, "Test forum subtitle 1", "Test forum title 1", 1, null },
                    { 2, new DateTime(2023, 6, 7, 14, 27, 58, 883, DateTimeKind.Local).AddTicks(5716), 2, "Test forum subtitle 2", "Test forum title 2", 2, null },
                    { 3, new DateTime(2023, 6, 7, 14, 27, 58, 883, DateTimeKind.Local).AddTicks(5718), 2, "Test forum subtitle 3", "Test forum title 3", 2, null },
                    { 4, new DateTime(2023, 6, 7, 14, 27, 58, 883, DateTimeKind.Local).AddTicks(5719), 2, "Test forum subtitle 4", "Test forum title 4", 2, null },
                    { 5, new DateTime(2023, 6, 7, 14, 27, 58, 883, DateTimeKind.Local).AddTicks(5721), 2, "Test forum subtitle 5", "Test forum title 5", 2, null },
                    { 6, new DateTime(2023, 6, 7, 14, 27, 58, 883, DateTimeKind.Local).AddTicks(5722), 2, "Test forum subtitle 6", "Test forum title 6", 2, null }
                });

            migrationBuilder.InsertData(
                table: "ForumTopic",
                columns: new[] { "Id", "CreatedAt", "ForumBaseId", "ForumUserId", "Name", "TopicViewCounter", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 6, 7, 14, 27, 58, 883, DateTimeKind.Local).AddTicks(8886), 1, 1, "Test forum topic 1", 0, null },
                    { 2, new DateTime(2023, 6, 7, 14, 27, 58, 883, DateTimeKind.Local).AddTicks(8892), 2, 2, "Test forum topic 2", 0, null },
                    { 3, new DateTime(2023, 6, 7, 14, 27, 58, 883, DateTimeKind.Local).AddTicks(8893), 2, 2, "Test forum topic 3", 0, null },
                    { 4, new DateTime(2023, 6, 7, 14, 27, 58, 883, DateTimeKind.Local).AddTicks(8894), 2, 2, "Test forum topic 4", 0, null },
                    { 5, new DateTime(2023, 6, 7, 14, 27, 58, 883, DateTimeKind.Local).AddTicks(8895), 2, 2, "Test forum topic 5", 0, null }
                });

            migrationBuilder.InsertData(
                table: "ForumPost",
                columns: new[] { "Id", "CreatedAt", "ForumTopicId", "ForumUserId", "Likes", "PostName", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 6, 7, 14, 27, 58, 884, DateTimeKind.Local).AddTicks(733), 1, 1, 1, "Post name 1", null },
                    { 2, new DateTime(2023, 6, 7, 14, 27, 58, 884, DateTimeKind.Local).AddTicks(738), 2, 2, null, "Post name 2", null },
                    { 3, new DateTime(2023, 6, 7, 14, 27, 58, 884, DateTimeKind.Local).AddTicks(740), 2, 2, null, "Post name 3", null },
                    { 4, new DateTime(2023, 6, 7, 14, 27, 58, 884, DateTimeKind.Local).AddTicks(741), 2, 2, 34, "Post name 4", null },
                    { 5, new DateTime(2023, 6, 7, 14, 27, 58, 884, DateTimeKind.Local).AddTicks(742), 2, 2, 65, "Post name 5", null }
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

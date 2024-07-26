using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Access_Layer.Migrations
{
    public partial class add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    ExpenseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidById = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.ExpenseId);
                    table.ForeignKey(
                        name: "FK_Expenses_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenses_Users_PaidById",
                        column: x => x.PaidById,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupMembers",
                columns: table => new
                {
                    GroupMemberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsSettled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMembers", x => x.GroupMemberId);
                    table.ForeignKey(
                        name: "FK_GroupMembers_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "AccessFailedCount", "Balance", "ConcurrencyStamp", "Email", "EmailConfirmed", "Id", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Password", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 1, 0, 0m, "89bf7f1b-c1c1-4379-9c2a-12aed86ea8e1", "admin@gmail.com", false, "b6c2b9fb-098d-41ea-bebd-10e2df31f276", false, null, "ADMIN@GMAIL.COM", null, "string", null, null, false, "admin", "", false, null },
                    { 2, 0, 0m, "c4fa2220-28f3-4bd8-9957-e34e4ad1467e", "rahul1@gmail.com", false, "1c602df1-dfef-41bd-87c1-5d37b1daccb4", false, null, "RAHUL1@GMAIL.COM", null, "string", null, null, false, "normal", "", false, null },
                    { 3, 0, 0m, "507b6902-ee74-4588-aa43-6b047dd7fef4", "rahul2@gmail.com", false, "6ef761dd-ec07-4bd4-b859-7b00bf84ff77", false, null, "RAHUL2@GMAIL.COM", null, "string", null, null, false, "normal", "", false, null },
                    { 4, 0, 0m, "8111bbcc-044b-4749-beaa-74d19d501f3b", "rahul3@gmail.com", false, "864ea69c-3a69-4401-9770-e18ed03e76e9", false, null, "RAHUL3@GMAIL.COM", null, "string", null, null, false, "normal", "", false, null },
                    { 5, 0, 0m, "03b5cb09-0b9d-4563-bc11-de6d7254ba68", "rahul4@gmail.com", false, "7c04bfef-3adc-45dd-9af0-43be4d5193ad", false, null, "RAHUL4@GMAIL.COM", null, "string", null, null, false, "normal", "", false, null },
                    { 6, 0, 0m, "4f19a8ab-169e-4f71-85a3-68d49d17cb41", "rahul5@gmail.com", false, "42d044a3-766f-4bf9-bde4-033e7fb4840b", false, null, "RAHUL5@GMAIL.COM", null, "string", null, null, false, "normal", "", false, null },
                    { 7, 0, 0m, "a108f602-ae4a-47c3-857c-b3d5fb483853", "rahul6@gmail.com", false, "d5dcfb5d-3cd3-4a4b-94ca-f64c76d57b1e", false, null, "RAHUL6@GMAIL.COM", null, "string", null, null, false, "normal", "", false, null },
                    { 8, 0, 0m, "4e947479-ba05-4a8a-aea2-d8a8aed5d917", "rahul7@gmail.com", false, "bd5dbc16-e40e-4f49-a745-eae03bf9793c", false, null, "RAHUL7@GMAIL.COM", null, "string", null, null, false, "normal", "", false, null },
                    { 9, 0, 0m, "bd825bd6-d5cb-49e0-89bd-27ecbe85659f", "rahul8@gmail.com", false, "62ff1a78-3e7b-41bc-af93-a9ccd73b2ecf", false, null, "RAHUL8@GMAIL.COM", null, "string", null, null, false, "normal", "", false, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_GroupId",
                table: "Expenses",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_PaidById",
                table: "Expenses",
                column: "PaidById");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_GroupId",
                table: "GroupMembers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_UserId",
                table: "GroupMembers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "GroupMembers");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

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
                    { 1, 0, 0m, "ffd238d2-7f4b-4bc7-89f2-7aa7f0d0bcd8", "admin@example.com", true, "a0e09152-dd0a-440a-a22a-03d5847752e5", false, null, "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", null, "AQAAAAEAACcQAAAAEAK8IWHJ6349v69gZ5NeQEi2OfHU7j0ap4Trcm5ut8npNgj6fInd1UFWNBdQtNoZ5Q==", null, false, "admin", "", false, "admin@example.com" },
                    { 2, 0, 0m, "1867d078-bc72-4f4d-b6b5-13be6e72d22c", "user1@example.com", true, "b2d86eba-74ef-476f-8027-ddef90ee0817", false, null, "USER1@EXAMPLE.COM", "USER1@EXAMPLE.COM", null, "AQAAAAEAACcQAAAAEFmCe/cyam4E3OoEUuVN9QTgaY1LwLr4M8+mR+gaWyJnCfoX8vyS+YUQQuzkq6vRwg==", null, false, "normal", "", false, "user1@example.com" },
                    { 3, 0, 0m, "02648ee7-f179-4674-913b-f529c87a556c", "user2@example.com", true, "2d4c8935-275a-41ea-a250-ff1285c73781", false, null, "USER2@EXAMPLE.COM", "USER2@EXAMPLE.COM", null, "AQAAAAEAACcQAAAAEI2dKdvZyOER1XsXU26fAfs7RkZQX8/4EFgzmYi89p3tFlwDCVd1TpMUiCp2xuSDdg==", null, false, "normal", "", false, "user2@example.com" },
                    { 4, 0, 0m, "dc8cda27-2ed8-4a6d-b355-b11bdb2cc4db", "user3@example.com", true, "2a94d515-0e40-4449-82e4-925da6bdc1d6", false, null, "USER3@EXAMPLE.COM", "USER3@EXAMPLE.COM", null, "AQAAAAEAACcQAAAAEJmhacDl1XO21FTBg4KdlZIMoqOGFyV+XoVHRBLAIpsiagYgm0lbcUANj2xTzaT5Ig==", null, false, "normal", "", false, "user3@example.com" },
                    { 5, 0, 0m, "31da065a-05b4-4006-a89c-b0d7d4781eb6", "user4@example.com", true, "6f50304f-be8e-4ef7-b485-445a52b3584b", false, null, "USER4@EXAMPLE.COM", "USER4@EXAMPLE.COM", null, "AQAAAAEAACcQAAAAENx3hKc686ipBeEf1JrwVTr4myWcdhcLThTF5nil6amm/EzBAy1W8erT43e7Vdkwww==", null, false, "normal", "", false, "user4@example.com" },
                    { 6, 0, 0m, "ba115864-e4e6-4b9b-9597-c7bfe43c3509", "user5@example.com", true, "d9fabc5d-e754-4a5b-999a-5e854ad2dfb0", false, null, "USER5@EXAMPLE.COM", "USER5@EXAMPLE.COM", null, "AQAAAAEAACcQAAAAEKEBve13YiRWjZijh9l1EF8ZHGsG6EL32V/tVmHN2VoxJl3NAjrZfBw3XV1v9krcEg==", null, false, "normal", "", false, "user5@example.com" },
                    { 7, 0, 0m, "d480741e-de78-4143-94bf-a39d3d501278", "user6@example.com", true, "eb076c00-e6b5-48d9-ac28-d60b870f788e", false, null, "USER6@EXAMPLE.COM", "USER6@EXAMPLE.COM", null, "AQAAAAEAACcQAAAAEDzEB9Ym00bb07N0byAgQdLHT/wThspyO5Y02Oodi0aVfs8Uk4Q25LB26M2ctxKnGQ==", null, false, "normal", "", false, "user6@example.com" },
                    { 8, 0, 0m, "e64b9e07-650c-40fe-9292-89e4c7f72188", "user7@example.com", true, "4166ebc0-ed14-4795-8af2-ce527d843843", false, null, "USER7@EXAMPLE.COM", "USER7@EXAMPLE.COM", null, "AQAAAAEAACcQAAAAELABp6wtTTTsl1dGVdqgO2gHP4P0a6x9cALY7BpW7yMKYq4XRF9avdHWEyiJxceibg==", null, false, "normal", "", false, "user7@example.com" },
                    { 9, 0, 0m, "3dbb7493-d2a5-4732-9175-3febb6445b53", "user8@example.com", true, "a62e93e3-ec17-4422-892a-bad71d35bb01", false, null, "USER8@EXAMPLE.COM", "USER8@EXAMPLE.COM", null, "AQAAAAEAACcQAAAAEO2kTJs248EKlj6X+pJ6NgW+2O34xWCjkJ32G8KgaM/VjfnhB9AC4dxqZ2vwhmQoPw==", null, false, "normal", "", false, "user8@example.com" }
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

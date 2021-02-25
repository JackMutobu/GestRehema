using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestRehema.Data.Migrations
{
    public partial class UpdateArticleStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f14162fb-6335-4175-8f9c-da4399cf02e0"));

            migrationBuilder.AlterColumn<double>(
                name: "InStock",
                table: "Articles",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Entreprises",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "DateDuJour" },
                values: new object[] { new DateTime(2021, 2, 25, 10, 47, 24, 114, DateTimeKind.Utc).AddTicks(8165), new DateTime(2021, 2, 25, 10, 47, 24, 114, DateTimeKind.Utc).AddTicks(5187) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "CreatedAt", "Password", "Role", "Username" },
                values: new object[] { new Guid("e6ad7ca5-690d-440d-9183-de4bdd6a2599"), "all", new DateTime(2021, 2, 25, 10, 47, 24, 116, DateTimeKind.Utc).AddTicks(2341), "10000.WuBN3W3NR1elThk5JsiAfA==.JdBevK91OYBDZIsbDkR3IXkOmT99Zo5koFfpocqsixU=", "SuperAdmin", "admin@rehema.com" });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2021, 2, 25, 10, 47, 24, 113, DateTimeKind.Utc).AddTicks(305));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: -1,
                column: "UserId",
                value: new Guid("e6ad7ca5-690d-440d-9183-de4bdd6a2599"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e6ad7ca5-690d-440d-9183-de4bdd6a2599"));

            migrationBuilder.AlterColumn<int>(
                name: "InStock",
                table: "Articles",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.UpdateData(
                table: "Entreprises",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "DateDuJour" },
                values: new object[] { new DateTime(2021, 2, 24, 14, 3, 6, 626, DateTimeKind.Utc).AddTicks(3508), new DateTime(2021, 2, 24, 14, 3, 6, 626, DateTimeKind.Utc).AddTicks(527) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "CreatedAt", "Password", "Role", "Username" },
                values: new object[] { new Guid("f14162fb-6335-4175-8f9c-da4399cf02e0"), "all", new DateTime(2021, 2, 24, 14, 3, 6, 628, DateTimeKind.Utc).AddTicks(2508), "10000.g9nesA5UCKiewpBHWp1R+Q==.Rxcv964BSbvuSVwZOKvQhifa5L6VPdCKFkcpDV5gKok=", "SuperAdmin", "admin@rehema.com" });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2021, 2, 24, 14, 3, 6, 623, DateTimeKind.Utc).AddTicks(7500));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: -1,
                column: "UserId",
                value: new Guid("f14162fb-6335-4175-8f9c-da4399cf02e0"));
        }
    }
}

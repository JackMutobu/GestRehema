using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestRehema.Data.Migrations
{
    public partial class AddLocationToEntreprise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("79f49352-53f3-4c81-9f46-348aba02a6db"));

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Entreprises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Entreprises",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "DateDuJour", "Location" },
                values: new object[] { new DateTime(2021, 2, 24, 14, 3, 6, 626, DateTimeKind.Utc).AddTicks(3508), new DateTime(2021, 2, 24, 14, 3, 6, 626, DateTimeKind.Utc).AddTicks(527), "Bunia" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f14162fb-6335-4175-8f9c-da4399cf02e0"));

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Entreprises");

            migrationBuilder.UpdateData(
                table: "Entreprises",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "DateDuJour" },
                values: new object[] { new DateTime(2021, 2, 24, 14, 1, 53, 767, DateTimeKind.Utc).AddTicks(8657), new DateTime(2021, 2, 24, 14, 1, 53, 767, DateTimeKind.Utc).AddTicks(6613) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "CreatedAt", "Password", "Role", "Username" },
                values: new object[] { new Guid("79f49352-53f3-4c81-9f46-348aba02a6db"), "all", new DateTime(2021, 2, 24, 14, 1, 53, 769, DateTimeKind.Utc).AddTicks(2049), "10000.eaaa6w4uNCr0TlKOykfF6w==.jVf2V1FyRKArmc/tX1Q5EmA+QfjRI2m9RvieTN5+bM0=", "SuperAdmin", "admin@rehema.com" });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2021, 2, 24, 14, 1, 53, 765, DateTimeKind.Utc).AddTicks(5094));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: -1,
                column: "UserId",
                value: new Guid("79f49352-53f3-4c81-9f46-348aba02a6db"));
        }
    }
}

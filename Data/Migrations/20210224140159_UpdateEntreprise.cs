using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestRehema.Data.Migrations
{
    public partial class UpdateEntreprise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("32daf858-ae0b-467f-bb77-cbbb5bdfb47e"));

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "Entreprises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IDNAT",
                table: "Entreprises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PoBox",
                table: "Entreprises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RCCM",
                table: "Entreprises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Slogan",
                table: "Entreprises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Entreprises",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "Contact", "CreatedAt", "DateDuJour", "IDNAT", "PoBox", "RCCM", "Slogan" },
                values: new object[] { "+243971871546\r\n+243822903906\r\n+243819521649", new DateTime(2021, 2, 24, 14, 1, 53, 767, DateTimeKind.Utc).AddTicks(8657), new DateTime(2021, 2, 24, 14, 1, 53, 767, DateTimeKind.Utc).AddTicks(6613), "493-N50888J", "76", "BIA/RCCM/19-A-1320265", "Chez FLORENCE" });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("79f49352-53f3-4c81-9f46-348aba02a6db"));

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "Entreprises");

            migrationBuilder.DropColumn(
                name: "IDNAT",
                table: "Entreprises");

            migrationBuilder.DropColumn(
                name: "PoBox",
                table: "Entreprises");

            migrationBuilder.DropColumn(
                name: "RCCM",
                table: "Entreprises");

            migrationBuilder.DropColumn(
                name: "Slogan",
                table: "Entreprises");

            migrationBuilder.UpdateData(
                table: "Entreprises",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "DateDuJour" },
                values: new object[] { new DateTime(2021, 2, 24, 10, 5, 21, 461, DateTimeKind.Utc).AddTicks(8785), new DateTime(2021, 2, 24, 10, 5, 21, 461, DateTimeKind.Utc).AddTicks(6233) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "CreatedAt", "Password", "Role", "Username" },
                values: new object[] { new Guid("32daf858-ae0b-467f-bb77-cbbb5bdfb47e"), "all", new DateTime(2021, 2, 24, 10, 5, 21, 463, DateTimeKind.Utc).AddTicks(1312), "10000.5fm+XWPVyB45n1Yo2mum6A==.PnlA1tcahWo7vGE/fLZg+DtvBh4m19mf0RkHzeWIF0Q=", "SuperAdmin", "admin@rehema.com" });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2021, 2, 24, 10, 5, 21, 459, DateTimeKind.Utc).AddTicks(6812));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: -1,
                column: "UserId",
                value: new Guid("32daf858-ae0b-467f-bb77-cbbb5bdfb47e"));
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestRehema.Data.Migrations
{
    public partial class AddSupplierTypeToSupplierTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("81530f12-8308-4754-93de-cf14e6c634c1"));

            migrationBuilder.AddColumn<string>(
                name: "SupplierType",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Entreprises",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "DateDuJour" },
                values: new object[] { new DateTime(2021, 3, 25, 9, 7, 7, 7, DateTimeKind.Utc).AddTicks(9681), new DateTime(2021, 3, 25, 9, 7, 7, 7, DateTimeKind.Utc).AddTicks(7677) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "CreatedAt", "Password", "Role", "Username" },
                values: new object[] { new Guid("f0ba19c8-a4c6-4e56-b93b-cfbfc45a41a4"), "all", new DateTime(2021, 3, 25, 9, 7, 7, 9, DateTimeKind.Utc).AddTicks(4996), "10000.hPcVN6YxHhspHnLYtq343w==.FUaVxNTfKaAYapjXzYvvcEhhqBg3PyosLlIn/eDC0Oc=", "SuperAdmin", "admin@rehema.com" });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2021, 3, 25, 9, 7, 7, 5, DateTimeKind.Utc).AddTicks(5778));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: -1,
                column: "UserId",
                value: new Guid("f0ba19c8-a4c6-4e56-b93b-cfbfc45a41a4"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f0ba19c8-a4c6-4e56-b93b-cfbfc45a41a4"));

            migrationBuilder.DropColumn(
                name: "SupplierType",
                table: "Suppliers");

            migrationBuilder.UpdateData(
                table: "Entreprises",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "DateDuJour" },
                values: new object[] { new DateTime(2021, 3, 22, 16, 32, 47, 725, DateTimeKind.Utc).AddTicks(9129), new DateTime(2021, 3, 22, 16, 32, 47, 725, DateTimeKind.Utc).AddTicks(7282) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "CreatedAt", "Password", "Role", "Username" },
                values: new object[] { new Guid("81530f12-8308-4754-93de-cf14e6c634c1"), "all", new DateTime(2021, 3, 22, 16, 32, 47, 727, DateTimeKind.Utc).AddTicks(2326), "10000.sQoH2sBOxKrZ4O92rHI3Cg==.vOmSxvBPLT7uDqMHHPsl4l2v9uwKOO1LckRtfFSZtjU=", "SuperAdmin", "admin@rehema.com" });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2021, 3, 22, 16, 32, 47, 724, DateTimeKind.Utc).AddTicks(2310));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: -1,
                column: "UserId",
                value: new Guid("81530f12-8308-4754-93de-cf14e6c634c1"));
        }
    }
}

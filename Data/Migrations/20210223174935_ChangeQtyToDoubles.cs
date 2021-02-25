using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestRehema.Data.Migrations
{
    public partial class ChangeQtyToDoubles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("2e743d90-e166-4e6f-9547-49fee5078f5b"));

            migrationBuilder.AlterColumn<double>(
                name: "DeliveredQuantity",
                table: "SaleDeliveries",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                table: "SaleArticles",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "QtyPerConditionement",
                table: "Articles",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Conditionement",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "AwaitingDeliveryToCustomers",
                table: "Articles",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "AwaitingDeliveryToCompany",
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
                values: new object[] { new DateTime(2021, 2, 23, 17, 49, 28, 731, DateTimeKind.Utc).AddTicks(7626), new DateTime(2021, 2, 23, 17, 49, 28, 731, DateTimeKind.Utc).AddTicks(5644) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "CreatedAt", "Password", "Role", "Username" },
                values: new object[] { new Guid("d9a91617-d12b-49da-b7d1-cfdd2cbdf98a"), "all", new DateTime(2021, 2, 23, 17, 49, 28, 732, DateTimeKind.Utc).AddTicks(7203), "10000.BCiqaT+NC8f1h61YoITyFQ==.4MENaOFE1atw2Xma5iRorSAoJKnNDeExAEEw1bB64Rs=", "SuperAdmin", "admin@rehema.com" });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2021, 2, 23, 17, 49, 28, 730, DateTimeKind.Utc).AddTicks(840));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: -1,
                column: "UserId",
                value: new Guid("d9a91617-d12b-49da-b7d1-cfdd2cbdf98a"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d9a91617-d12b-49da-b7d1-cfdd2cbdf98a"));

            migrationBuilder.AlterColumn<int>(
                name: "DeliveredQuantity",
                table: "SaleDeliveries",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "SaleArticles",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "QtyPerConditionement",
                table: "Articles",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Conditionement",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "AwaitingDeliveryToCustomers",
                table: "Articles",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "AwaitingDeliveryToCompany",
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
                values: new object[] { new DateTime(2021, 2, 22, 14, 32, 3, 457, DateTimeKind.Utc).AddTicks(8245), new DateTime(2021, 2, 22, 14, 32, 3, 457, DateTimeKind.Utc).AddTicks(6299) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "CreatedAt", "Password", "Role", "Username" },
                values: new object[] { new Guid("2e743d90-e166-4e6f-9547-49fee5078f5b"), "all", new DateTime(2021, 2, 22, 14, 32, 3, 458, DateTimeKind.Utc).AddTicks(7935), "10000.VEaG4Re9I8Z3HaB0T9niAQ==.TMhfWSLWX63Kbv6EndPUSX2rk9Sj5mGeBOk13kCphWM=", "SuperAdmin", "admin@rehema.com" });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2021, 2, 22, 14, 32, 3, 456, DateTimeKind.Utc).AddTicks(1979));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: -1,
                column: "UserId",
                value: new Guid("2e743d90-e166-4e6f-9547-49fee5078f5b"));
        }
    }
}

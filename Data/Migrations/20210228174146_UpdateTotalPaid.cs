using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestRehema.Data.Migrations
{
    public partial class UpdateTotalPaid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b18ed2ae-ebb3-4914-ac58-02c18109991f"));

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPaid",
                table: "Payements",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Entreprises",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "DateDuJour" },
                values: new object[] { new DateTime(2021, 2, 28, 17, 41, 29, 687, DateTimeKind.Utc).AddTicks(5574), new DateTime(2021, 2, 28, 17, 41, 29, 687, DateTimeKind.Utc).AddTicks(2892) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "CreatedAt", "Password", "Role", "Username" },
                values: new object[] { new Guid("800668c2-e14f-4b03-82c6-d89b2674ad2c"), "all", new DateTime(2021, 2, 28, 17, 41, 29, 689, DateTimeKind.Utc).AddTicks(120), "10000.Pq5InwEkJb/hQ0AVTVbIfg==.t1fnLy3f9cMnydgq+p2hRZuKYvJ1jSTMj/ujuzZsi9I=", "SuperAdmin", "admin@rehema.com" });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2021, 2, 28, 17, 41, 29, 685, DateTimeKind.Utc).AddTicks(7202));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: -1,
                column: "UserId",
                value: new Guid("800668c2-e14f-4b03-82c6-d89b2674ad2c"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("800668c2-e14f-4b03-82c6-d89b2674ad2c"));

            migrationBuilder.DropColumn(
                name: "TotalPaid",
                table: "Payements");

            migrationBuilder.UpdateData(
                table: "Entreprises",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "DateDuJour" },
                values: new object[] { new DateTime(2021, 2, 28, 8, 46, 22, 95, DateTimeKind.Utc).AddTicks(1169), new DateTime(2021, 2, 28, 8, 46, 22, 94, DateTimeKind.Utc).AddTicks(9378) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "CreatedAt", "Password", "Role", "Username" },
                values: new object[] { new Guid("b18ed2ae-ebb3-4914-ac58-02c18109991f"), "all", new DateTime(2021, 2, 28, 8, 46, 22, 96, DateTimeKind.Utc).AddTicks(3180), "10000./VPSmkPE/uXzMXdooaYsGw==.pz4ZX5P5MpoELOhPH6VhqJYeVmM5A3PfElWtcT9JOdk=", "SuperAdmin", "admin@rehema.com" });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2021, 2, 28, 8, 46, 22, 93, DateTimeKind.Utc).AddTicks(5636));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: -1,
                column: "UserId",
                value: new Guid("b18ed2ae-ebb3-4914-ac58-02c18109991f"));
        }
    }
}

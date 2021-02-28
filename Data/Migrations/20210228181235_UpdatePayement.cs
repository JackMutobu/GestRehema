using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestRehema.Data.Migrations
{
    public partial class UpdatePayement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WalletHistories_PayementId",
                table: "WalletHistories");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("800668c2-e14f-4b03-82c6-d89b2674ad2c"));

            migrationBuilder.UpdateData(
                table: "Entreprises",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "DateDuJour" },
                values: new object[] { new DateTime(2021, 2, 28, 18, 12, 33, 113, DateTimeKind.Utc).AddTicks(1305), new DateTime(2021, 2, 28, 18, 12, 33, 112, DateTimeKind.Utc).AddTicks(3640) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "CreatedAt", "Password", "Role", "Username" },
                values: new object[] { new Guid("0c3cd029-4614-4bb6-83ef-c4fe2aaf5140"), "all", new DateTime(2021, 2, 28, 18, 12, 33, 115, DateTimeKind.Utc).AddTicks(3928), "10000.DeHFkIiSC5aYLw/9GTDgwQ==.vo58pf7K0kTzZ6aF153w3j0/7zYy6Sa3HAsTPmGtHtA=", "SuperAdmin", "admin@rehema.com" });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2021, 2, 28, 18, 12, 33, 110, DateTimeKind.Utc).AddTicks(3724));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: -1,
                column: "UserId",
                value: new Guid("0c3cd029-4614-4bb6-83ef-c4fe2aaf5140"));

            migrationBuilder.CreateIndex(
                name: "IX_WalletHistories_PayementId",
                table: "WalletHistories",
                column: "PayementId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WalletHistories_PayementId",
                table: "WalletHistories");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0c3cd029-4614-4bb6-83ef-c4fe2aaf5140"));

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

            migrationBuilder.CreateIndex(
                name: "IX_WalletHistories_PayementId",
                table: "WalletHistories",
                column: "PayementId",
                unique: true,
                filter: "[PayementId] IS NOT NULL");
        }
    }
}

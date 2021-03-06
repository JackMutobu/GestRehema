using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestRehema.Data.Migrations
{
    public partial class AddDecimalPrecisionOnDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0c3cd029-4614-4bb6-83ef-c4fe2aaf5140"));

            migrationBuilder.UpdateData(
                table: "Entreprises",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "DateDuJour" },
                values: new object[] { new DateTime(2021, 3, 6, 9, 36, 1, 435, DateTimeKind.Utc).AddTicks(1817), new DateTime(2021, 3, 6, 9, 36, 1, 434, DateTimeKind.Utc).AddTicks(9217) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "CreatedAt", "Password", "Role", "Username" },
                values: new object[] { new Guid("0b75bb66-a0ef-4e56-bb8e-910b2fe0faa4"), "all", new DateTime(2021, 3, 6, 9, 36, 1, 436, DateTimeKind.Utc).AddTicks(4104), "10000.7RDGz9IGfuZdhHeCl5u77A==.mXieT0ffCMXQTaKHZ51M7azT0nnvTOdwlS27ZBN/4lw=", "SuperAdmin", "admin@rehema.com" });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2021, 3, 6, 9, 36, 1, 433, DateTimeKind.Utc).AddTicks(308));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: -1,
                column: "UserId",
                value: new Guid("0b75bb66-a0ef-4e56-bb8e-910b2fe0faa4"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0b75bb66-a0ef-4e56-bb8e-910b2fe0faa4"));

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
        }
    }
}

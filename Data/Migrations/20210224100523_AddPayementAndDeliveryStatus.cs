using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestRehema.Data.Migrations
{
    public partial class AddPayementAndDeliveryStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a2620c6b-7a7e-4326-97f3-ad3319c2887f"));

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Sales",
                newName: "PayementStatus");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryStatus",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("32daf858-ae0b-467f-bb77-cbbb5bdfb47e"));

            migrationBuilder.DropColumn(
                name: "DeliveryStatus",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "PayementStatus",
                table: "Sales",
                newName: "Status");

            migrationBuilder.UpdateData(
                table: "Entreprises",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "DateDuJour" },
                values: new object[] { new DateTime(2021, 2, 24, 9, 39, 55, 150, DateTimeKind.Utc).AddTicks(4711), new DateTime(2021, 2, 24, 9, 39, 55, 150, DateTimeKind.Utc).AddTicks(2113) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "CreatedAt", "Password", "Role", "Username" },
                values: new object[] { new Guid("a2620c6b-7a7e-4326-97f3-ad3319c2887f"), "all", new DateTime(2021, 2, 24, 9, 39, 55, 151, DateTimeKind.Utc).AddTicks(7463), "10000.Vq693Y76XeaMBR/R9hz8Tg==.KpPPgrDW68wz9nxUN2mn/BF9X3c2KcgBiHErLCh1y2Y=", "SuperAdmin", "admin@rehema.com" });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2021, 2, 24, 9, 39, 55, 148, DateTimeKind.Utc).AddTicks(1443));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: -1,
                column: "UserId",
                value: new Guid("a2620c6b-7a7e-4326-97f3-ad3319c2887f"));
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestRehema.Data.Migrations
{
    public partial class UpdateSale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletHistories_Wallets_AmountInWalletId",
                table: "WalletHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletHistories_Wallets_AmountOutWalletId",
                table: "WalletHistories");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d9a91617-d12b-49da-b7d1-cfdd2cbdf98a"));

            migrationBuilder.DropColumn(
                name: "AmountIn",
                table: "WalletHistories");

            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "Wallets",
                newName: "AmountOwned");

            migrationBuilder.RenameColumn(
                name: "AmountOutWalletId",
                table: "WalletHistories",
                newName: "AmountExcessWalletId");

            migrationBuilder.RenameColumn(
                name: "AmountOut",
                table: "WalletHistories",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "AmountInWalletId",
                table: "WalletHistories",
                newName: "AmountDebtWalletId");

            migrationBuilder.RenameIndex(
                name: "IX_WalletHistories_AmountOutWalletId",
                table: "WalletHistories",
                newName: "IX_WalletHistories_AmountExcessWalletId");

            migrationBuilder.RenameIndex(
                name: "IX_WalletHistories_AmountInWalletId",
                table: "WalletHistories",
                newName: "IX_WalletHistories_AmountDebtWalletId");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountInDebt",
                table: "Wallets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountInExcess",
                table: "Wallets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "WalletHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOperation",
                table: "Sales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

            migrationBuilder.AddForeignKey(
                name: "FK_WalletHistories_Wallets_AmountDebtWalletId",
                table: "WalletHistories",
                column: "AmountDebtWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletHistories_Wallets_AmountExcessWalletId",
                table: "WalletHistories",
                column: "AmountExcessWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletHistories_Wallets_AmountDebtWalletId",
                table: "WalletHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletHistories_Wallets_AmountExcessWalletId",
                table: "WalletHistories");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a2620c6b-7a7e-4326-97f3-ad3319c2887f"));

            migrationBuilder.DropColumn(
                name: "AmountInDebt",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "AmountInExcess",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "WalletHistories");

            migrationBuilder.DropColumn(
                name: "DateOperation",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "AmountOwned",
                table: "Wallets",
                newName: "Balance");

            migrationBuilder.RenameColumn(
                name: "AmountExcessWalletId",
                table: "WalletHistories",
                newName: "AmountOutWalletId");

            migrationBuilder.RenameColumn(
                name: "AmountDebtWalletId",
                table: "WalletHistories",
                newName: "AmountInWalletId");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "WalletHistories",
                newName: "AmountOut");

            migrationBuilder.RenameIndex(
                name: "IX_WalletHistories_AmountExcessWalletId",
                table: "WalletHistories",
                newName: "IX_WalletHistories_AmountOutWalletId");

            migrationBuilder.RenameIndex(
                name: "IX_WalletHistories_AmountDebtWalletId",
                table: "WalletHistories",
                newName: "IX_WalletHistories_AmountInWalletId");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountIn",
                table: "WalletHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

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

            migrationBuilder.AddForeignKey(
                name: "FK_WalletHistories_Wallets_AmountInWalletId",
                table: "WalletHistories",
                column: "AmountInWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletHistories_Wallets_AmountOutWalletId",
                table: "WalletHistories",
                column: "AmountOutWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

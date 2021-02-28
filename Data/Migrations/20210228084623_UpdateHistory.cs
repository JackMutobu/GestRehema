using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestRehema.Data.Migrations
{
    public partial class UpdateHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payement_Wallets_WalletId",
                table: "Payement");

            migrationBuilder.DropForeignKey(
                name: "FK_SalePayements_Payement_PayementId",
                table: "SalePayements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payement",
                table: "Payement");

            migrationBuilder.DropIndex(
                name: "IX_Payement_WalletId",
                table: "Payement");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e6ad7ca5-690d-440d-9183-de4bdd6a2599"));

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "Payement");

            migrationBuilder.RenameTable(
                name: "Payement",
                newName: "Payements");

            migrationBuilder.AddColumn<int>(
                name: "PayementId",
                table: "WalletHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "Payements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payements",
                table: "Payements",
                column: "Id");

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

            migrationBuilder.CreateIndex(
                name: "IX_WalletHistories_PayementId",
                table: "WalletHistories",
                column: "PayementId",
                unique: true,
                filter: "[PayementId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_SalePayements_Payements_PayementId",
                table: "SalePayements",
                column: "PayementId",
                principalTable: "Payements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletHistories_Payements_PayementId",
                table: "WalletHistories",
                column: "PayementId",
                principalTable: "Payements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalePayements_Payements_PayementId",
                table: "SalePayements");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletHistories_Payements_PayementId",
                table: "WalletHistories");

            migrationBuilder.DropIndex(
                name: "IX_WalletHistories_PayementId",
                table: "WalletHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payements",
                table: "Payements");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b18ed2ae-ebb3-4914-ac58-02c18109991f"));

            migrationBuilder.DropColumn(
                name: "PayementId",
                table: "WalletHistories");

            migrationBuilder.RenameTable(
                name: "Payements",
                newName: "Payement");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionId",
                table: "Payement",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                table: "Payement",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payement",
                table: "Payement",
                column: "Id");

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

            migrationBuilder.CreateIndex(
                name: "IX_Payement_WalletId",
                table: "Payement",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payement_Wallets_WalletId",
                table: "Payement",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalePayements_Payement_PayementId",
                table: "SalePayements",
                column: "PayementId",
                principalTable: "Payement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

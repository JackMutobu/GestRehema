using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestRehema.Data.Migrations
{
    public partial class AddSupplyItemsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplyArticles_SupplyItem_SupplyItemId",
                table: "SupplyArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyDeliveries_SupplyItem_SupplyItemId",
                table: "SupplyDeliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyItem_Suppliers_SupplierId",
                table: "SupplyItem");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyItem_Supplies_SupplyId",
                table: "SupplyItem");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyPayements_SupplyItem_SupplyItemId",
                table: "SupplyPayements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupplyItem",
                table: "SupplyItem");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f5ecaa76-9980-4fac-bfd8-ab85f69af426"));

            migrationBuilder.RenameTable(
                name: "SupplyItem",
                newName: "SupplyItems");

            migrationBuilder.RenameIndex(
                name: "IX_SupplyItem_SupplyId",
                table: "SupplyItems",
                newName: "IX_SupplyItems_SupplyId");

            migrationBuilder.RenameIndex(
                name: "IX_SupplyItem_SupplierId",
                table: "SupplyItems",
                newName: "IX_SupplyItems_SupplierId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupplyItems",
                table: "SupplyItems",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Entreprises",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "DateDuJour" },
                values: new object[] { new DateTime(2021, 4, 4, 12, 19, 24, 670, DateTimeKind.Utc).AddTicks(5436), new DateTime(2021, 4, 4, 12, 19, 24, 670, DateTimeKind.Utc).AddTicks(2747) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "CreatedAt", "Password", "Role", "Username" },
                values: new object[] { new Guid("04ba28d3-ddc5-4fbe-85fc-82bdd627c2f7"), "all", new DateTime(2021, 4, 4, 12, 19, 24, 672, DateTimeKind.Utc).AddTicks(6682), "10000.1TpDjLNsBNzjMEi/ZUXiRg==.ubct/0bOB8RxbcMppRfFY2D6Fi+7mz3MIYQu2daXB8c=", "SuperAdmin", "admin@rehema.com" });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2021, 4, 4, 12, 19, 24, 667, DateTimeKind.Utc).AddTicks(8559));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: -1,
                column: "UserId",
                value: new Guid("04ba28d3-ddc5-4fbe-85fc-82bdd627c2f7"));

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyArticles_SupplyItems_SupplyItemId",
                table: "SupplyArticles",
                column: "SupplyItemId",
                principalTable: "SupplyItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyDeliveries_SupplyItems_SupplyItemId",
                table: "SupplyDeliveries",
                column: "SupplyItemId",
                principalTable: "SupplyItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyItems_Suppliers_SupplierId",
                table: "SupplyItems",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyItems_Supplies_SupplyId",
                table: "SupplyItems",
                column: "SupplyId",
                principalTable: "Supplies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyPayements_SupplyItems_SupplyItemId",
                table: "SupplyPayements",
                column: "SupplyItemId",
                principalTable: "SupplyItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplyArticles_SupplyItems_SupplyItemId",
                table: "SupplyArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyDeliveries_SupplyItems_SupplyItemId",
                table: "SupplyDeliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyItems_Suppliers_SupplierId",
                table: "SupplyItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyItems_Supplies_SupplyId",
                table: "SupplyItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyPayements_SupplyItems_SupplyItemId",
                table: "SupplyPayements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupplyItems",
                table: "SupplyItems");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("04ba28d3-ddc5-4fbe-85fc-82bdd627c2f7"));

            migrationBuilder.RenameTable(
                name: "SupplyItems",
                newName: "SupplyItem");

            migrationBuilder.RenameIndex(
                name: "IX_SupplyItems_SupplyId",
                table: "SupplyItem",
                newName: "IX_SupplyItem_SupplyId");

            migrationBuilder.RenameIndex(
                name: "IX_SupplyItems_SupplierId",
                table: "SupplyItem",
                newName: "IX_SupplyItem_SupplierId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupplyItem",
                table: "SupplyItem",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Entreprises",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "DateDuJour" },
                values: new object[] { new DateTime(2021, 4, 4, 10, 42, 27, 74, DateTimeKind.Utc).AddTicks(5182), new DateTime(2021, 4, 4, 10, 42, 27, 74, DateTimeKind.Utc).AddTicks(3351) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "CreatedAt", "Password", "Role", "Username" },
                values: new object[] { new Guid("f5ecaa76-9980-4fac-bfd8-ab85f69af426"), "all", new DateTime(2021, 4, 4, 10, 42, 27, 76, DateTimeKind.Utc).AddTicks(7574), "10000.ikIma2MCgQhe1q2VTsr3sg==.CvQeB4jd5OS5PzjFEa9tBrVraP5w6ioZzQR8VdEVTNg=", "SuperAdmin", "admin@rehema.com" });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2021, 4, 4, 10, 42, 27, 72, DateTimeKind.Utc).AddTicks(8717));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: -1,
                column: "UserId",
                value: new Guid("f5ecaa76-9980-4fac-bfd8-ab85f69af426"));

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyArticles_SupplyItem_SupplyItemId",
                table: "SupplyArticles",
                column: "SupplyItemId",
                principalTable: "SupplyItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyDeliveries_SupplyItem_SupplyItemId",
                table: "SupplyDeliveries",
                column: "SupplyItemId",
                principalTable: "SupplyItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyItem_Suppliers_SupplierId",
                table: "SupplyItem",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyItem_Supplies_SupplyId",
                table: "SupplyItem",
                column: "SupplyId",
                principalTable: "Supplies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyPayements_SupplyItem_SupplyItemId",
                table: "SupplyPayements",
                column: "SupplyItemId",
                principalTable: "SupplyItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

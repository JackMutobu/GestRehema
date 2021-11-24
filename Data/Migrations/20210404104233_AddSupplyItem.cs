using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestRehema.Data.Migrations
{
    public partial class AddSupplyItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supplies_Suppliers_SupplierId",
                table: "Supplies");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyArticles_Supplies_SupplyId",
                table: "SupplyArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyDeliveries_Supplies_SupplyId",
                table: "SupplyDeliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyPayements_Supplies_SupplyId",
                table: "SupplyPayements");

            migrationBuilder.DropIndex(
                name: "IX_Supplies_SupplierId",
                table: "Supplies");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f0ba19c8-a4c6-4e56-b93b-cfbfc45a41a4"));

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Supplies");

            migrationBuilder.RenameColumn(
                name: "SupplyId",
                table: "SupplyPayements",
                newName: "SupplyItemId");

            migrationBuilder.RenameIndex(
                name: "IX_SupplyPayements_SupplyId",
                table: "SupplyPayements",
                newName: "IX_SupplyPayements_SupplyItemId");

            migrationBuilder.RenameColumn(
                name: "SupplyId",
                table: "SupplyDeliveries",
                newName: "SupplyItemId");

            migrationBuilder.RenameIndex(
                name: "IX_SupplyDeliveries_SupplyId",
                table: "SupplyDeliveries",
                newName: "IX_SupplyDeliveries_SupplyItemId");

            migrationBuilder.RenameColumn(
                name: "SupplyId",
                table: "SupplyArticles",
                newName: "SupplyItemId");

            migrationBuilder.RenameIndex(
                name: "IX_SupplyArticles_SupplyId",
                table: "SupplyArticles",
                newName: "IX_SupplyArticles_SupplyItemId");

            migrationBuilder.CreateTable(
                name: "SupplyItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOperation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayementStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    SupplyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplyItem_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupplyItem_Supplies_SupplyId",
                        column: x => x.SupplyId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_SupplyItem_SupplierId",
                table: "SupplyItem",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyItem_SupplyId",
                table: "SupplyItem",
                column: "SupplyId");

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
                name: "FK_SupplyPayements_SupplyItem_SupplyItemId",
                table: "SupplyPayements",
                column: "SupplyItemId",
                principalTable: "SupplyItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplyArticles_SupplyItem_SupplyItemId",
                table: "SupplyArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyDeliveries_SupplyItem_SupplyItemId",
                table: "SupplyDeliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyPayements_SupplyItem_SupplyItemId",
                table: "SupplyPayements");

            migrationBuilder.DropTable(
                name: "SupplyItem");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f5ecaa76-9980-4fac-bfd8-ab85f69af426"));

            migrationBuilder.RenameColumn(
                name: "SupplyItemId",
                table: "SupplyPayements",
                newName: "SupplyId");

            migrationBuilder.RenameIndex(
                name: "IX_SupplyPayements_SupplyItemId",
                table: "SupplyPayements",
                newName: "IX_SupplyPayements_SupplyId");

            migrationBuilder.RenameColumn(
                name: "SupplyItemId",
                table: "SupplyDeliveries",
                newName: "SupplyId");

            migrationBuilder.RenameIndex(
                name: "IX_SupplyDeliveries_SupplyItemId",
                table: "SupplyDeliveries",
                newName: "IX_SupplyDeliveries_SupplyId");

            migrationBuilder.RenameColumn(
                name: "SupplyItemId",
                table: "SupplyArticles",
                newName: "SupplyId");

            migrationBuilder.RenameIndex(
                name: "IX_SupplyArticles_SupplyItemId",
                table: "SupplyArticles",
                newName: "IX_SupplyArticles_SupplyId");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "Supplies",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_SupplierId",
                table: "Supplies",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Supplies_Suppliers_SupplierId",
                table: "Supplies",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyArticles_Supplies_SupplyId",
                table: "SupplyArticles",
                column: "SupplyId",
                principalTable: "Supplies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyDeliveries_Supplies_SupplyId",
                table: "SupplyDeliveries",
                column: "SupplyId",
                principalTable: "Supplies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyPayements_Supplies_SupplyId",
                table: "SupplyPayements",
                column: "SupplyId",
                principalTable: "Supplies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

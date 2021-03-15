using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestRehema.Data.Migrations
{
    public partial class UpdateWalletEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0b75bb66-a0ef-4e56-bb8e-910b2fe0faa4"));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Payements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpleyeeId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    PayementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expenses_Payements_PayementId",
                        column: x => x.PayementId,
                        principalTable: "Payements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Entreprises",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "DateDuJour" },
                values: new object[] { new DateTime(2021, 3, 11, 12, 34, 0, 968, DateTimeKind.Utc).AddTicks(9174), new DateTime(2021, 3, 11, 12, 34, 0, 968, DateTimeKind.Utc).AddTicks(6637) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "CreatedAt", "Password", "Role", "Username" },
                values: new object[] { new Guid("ac6bd2fd-c45b-4523-9ca0-433eaf0d5ed3"), "all", new DateTime(2021, 3, 11, 12, 34, 0, 970, DateTimeKind.Utc).AddTicks(9385), "10000.WcJqw1ohRrDAgUg+8okdiw==.StHvxejP/BLTjKmBrDAXFCSVb34EL1lic8hRrT1jCTI=", "SuperAdmin", "admin@rehema.com" });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2021, 3, 11, 12, 34, 0, 966, DateTimeKind.Utc).AddTicks(6150));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: -1,
                column: "UserId",
                value: new Guid("ac6bd2fd-c45b-4523-9ca0-433eaf0d5ed3"));

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_EmployeeId",
                table: "Expenses",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_PayementId",
                table: "Expenses",
                column: "PayementId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ac6bd2fd-c45b-4523-9ca0-433eaf0d5ed3"));

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Payements");

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
    }
}

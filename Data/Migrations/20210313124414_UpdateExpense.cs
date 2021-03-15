using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GestRehema.Data.Migrations
{
    public partial class UpdateExpense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Employees_EmployeeId",
                table: "Expenses");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("ac6bd2fd-c45b-4523-9ca0-433eaf0d5ed3"));

            migrationBuilder.DropColumn(
                name: "EmpleyeeId",
                table: "Expenses");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Entreprises",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedAt", "DateDuJour" },
                values: new object[] { new DateTime(2021, 3, 13, 12, 44, 13, 121, DateTimeKind.Utc).AddTicks(1569), new DateTime(2021, 3, 13, 12, 44, 13, 120, DateTimeKind.Utc).AddTicks(8836) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessLevel", "CreatedAt", "Password", "Role", "Username" },
                values: new object[] { new Guid("5960f86a-7a9b-4b5b-9b4b-c34d8aad23bd"), "all", new DateTime(2021, 3, 13, 12, 44, 13, 123, DateTimeKind.Utc).AddTicks(365), "10000.Mr65p0Vv+WOa/Andtunvww==.8GfV8y/0J58LWFLeVmvf2bQFCGcfl3iDePAY+50FjxA=", "SuperAdmin", "admin@rehema.com" });

            migrationBuilder.UpdateData(
                table: "Wallets",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedAt",
                value: new DateTime(2021, 3, 13, 12, 44, 13, 118, DateTimeKind.Utc).AddTicks(7890));

            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: -1,
                column: "UserId",
                value: new Guid("5960f86a-7a9b-4b5b-9b4b-c34d8aad23bd"));

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Employees_EmployeeId",
                table: "Expenses",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Employees_EmployeeId",
                table: "Expenses");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5960f86a-7a9b-4b5b-9b4b-c34d8aad23bd"));

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "Expenses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EmpleyeeId",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Employees_EmployeeId",
                table: "Expenses",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

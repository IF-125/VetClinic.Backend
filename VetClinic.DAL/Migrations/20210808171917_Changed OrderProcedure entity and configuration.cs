using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VetClinic.DAL.Migrations
{
    public partial class ChangedOrderProcedureentityandconfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "OrderProcedures");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "OrderProcedures");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Salaries",
                nullable: false,
                defaultValue: new DateTime(2021, 8, 8, 20, 19, 17, 81, DateTimeKind.Local).AddTicks(8746),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 8, 5, 20, 8, 37, 521, DateTimeKind.Local).AddTicks(7223));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Orders",
                nullable: false,
                defaultValue: new DateTime(2021, 8, 8, 20, 19, 17, 62, DateTimeKind.Local).AddTicks(711),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 8, 5, 20, 8, 37, 498, DateTimeKind.Local).AddTicks(8622));

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "OrderProcedures",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Conclusion",
                table: "OrderProcedures",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "OrderProcedures",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "OrderProcedures");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Salaries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 8, 5, 20, 8, 37, 521, DateTimeKind.Local).AddTicks(7223),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 8, 8, 20, 19, 17, 81, DateTimeKind.Local).AddTicks(8746));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 8, 5, 20, 8, 37, 498, DateTimeKind.Local).AddTicks(8622),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2021, 8, 8, 20, 19, 17, 62, DateTimeKind.Local).AddTicks(711));

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "OrderProcedures",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Conclusion",
                table: "OrderProcedures",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "OrderProcedures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Time",
                table: "OrderProcedures",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}

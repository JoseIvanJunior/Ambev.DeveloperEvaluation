using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSaleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BranchId",
                table: "Sales",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "BranchName",
                table: "Sales",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercentage",
                table: "Sales",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SaleNumber",
                table: "Sales",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b37b4d38-8dc7-45b4-b8e0-5a4ef2b2ad21"),
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2025, 10, 28, 23, 20, 39, 89, DateTimeKind.Utc).AddTicks(733), "$2a$11$hSLEowe9TBJIoX0cx8RN0ejsyXGygE/98e0SMddEjpw5rKfLCC3r6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "BranchName",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "SaleNumber",
                table: "Sales");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b37b4d38-8dc7-45b4-b8e0-5a4ef2b2ad21"),
                columns: new[] { "CreatedAt", "Password" },
                values: new object[] { new DateTime(2025, 10, 28, 20, 37, 14, 731, DateTimeKind.Utc).AddTicks(6990), "$2a$11$DxeUv0mBWqHElgco2J/gNuqn2kgYdJTQcgi9uD8Uz6Pezya8GzkN6" });
        }
    }
}

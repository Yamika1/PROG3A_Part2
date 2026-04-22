using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prog_part_2.Migrations
{
    /// <inheritdoc />
    public partial class create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_ServiceRequests_ServiceRequestsId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ServiceRequestsId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ServiceRequestsId",
                table: "Contracts");

            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "ServiceRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Contracts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Contracts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_ContractId",
                table: "ServiceRequests",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_Contracts_ContractId",
                table: "ServiceRequests",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_Contracts_ContractId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_ContractId",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "ServiceRequests");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "StartDate",
                table: "Contracts",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "EndDate",
                table: "Contracts",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "ServiceRequestsId",
                table: "Contracts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ServiceRequestsId",
                table: "Contracts",
                column: "ServiceRequestsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_ServiceRequests_ServiceRequestsId",
                table: "Contracts",
                column: "ServiceRequestsId",
                principalTable: "ServiceRequests",
                principalColumn: "Id");
        }
    }
}

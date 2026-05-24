using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prog_part_2.Migrations
{
    /// <inheritdoc />
    public partial class create2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractFiles_Contracts_ContractsId",
                table: "ContractFiles");

            migrationBuilder.DropIndex(
                name: "IX_ContractFiles_ContractsId",
                table: "ContractFiles");

            migrationBuilder.DropColumn(
                name: "ContractsId",
                table: "ContractFiles");

            migrationBuilder.CreateIndex(
                name: "IX_ContractFiles_ContractId",
                table: "ContractFiles",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractFiles_Contracts_ContractId",
                table: "ContractFiles",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractFiles_Contracts_ContractId",
                table: "ContractFiles");

            migrationBuilder.DropIndex(
                name: "IX_ContractFiles_ContractId",
                table: "ContractFiles");

            migrationBuilder.AddColumn<int>(
                name: "ContractsId",
                table: "ContractFiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractFiles_ContractsId",
                table: "ContractFiles",
                column: "ContractsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractFiles_Contracts_ContractsId",
                table: "ContractFiles",
                column: "ContractsId",
                principalTable: "Contracts",
                principalColumn: "Id");
        }
    }
}

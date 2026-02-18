using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceService.Infrastructure.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class InitialCreateSqlite_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "tax_year_id",
                table: "financial_statements",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "i_x_financial_statements_tax_year_id",
                table: "financial_statements",
                column: "tax_year_id");

            migrationBuilder.AddForeignKey(
                name: "f_k_financial_statements__tax_years_tax_year_id",
                table: "financial_statements",
                column: "tax_year_id",
                principalTable: "tax_years",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_financial_statements__tax_years_tax_year_id",
                table: "financial_statements");

            migrationBuilder.DropIndex(
                name: "i_x_financial_statements_tax_year_id",
                table: "financial_statements");

            migrationBuilder.DropColumn(
                name: "tax_year_id",
                table: "financial_statements");
        }
    }
}

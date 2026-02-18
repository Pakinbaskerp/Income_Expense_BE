using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceService.Infrastructure.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class InitialCreateSqlite_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "initial_balance",
                table: "financial_accounts");

            migrationBuilder.AddColumn<DateTime>(
                name: "credit_date",
                table: "financial_accounts",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "credit_date",
                table: "financial_accounts");

            migrationBuilder.AddColumn<long>(
                name: "initial_balance",
                table: "financial_accounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }
    }
}

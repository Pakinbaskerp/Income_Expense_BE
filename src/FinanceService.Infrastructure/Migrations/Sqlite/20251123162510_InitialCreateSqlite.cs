using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceService.Infrastructure.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class InitialCreateSqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    street = table.Column<string>(type: "TEXT", nullable: false),
                    city = table.Column<string>(type: "TEXT", nullable: false),
                    state = table.Column<string>(type: "TEXT", nullable: false),
                    postal_code = table.Column<string>(type: "TEXT", nullable: false),
                    country = table.Column<string>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    created_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    updated_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_addresses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bank_account_details",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    bank_name = table.Column<string>(type: "TEXT", nullable: false),
                    bank_type = table.Column<Guid>(type: "TEXT", nullable: false),
                    balance = table.Column<long>(type: "INTEGER", nullable: false),
                    currency_code = table.Column<Guid>(type: "TEXT", nullable: false),
                    is_countable = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    created_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    updated_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_bank_account_details", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "contacts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    phone_number = table.Column<string>(type: "TEXT", nullable: false),
                    mobile_code = table.Column<Guid>(type: "TEXT", nullable: false),
                    alternate_phone_number = table.Column<string>(type: "TEXT", nullable: false),
                    alternate_email = table.Column<string>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    created_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    updated_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_contacts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "provider_details",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    provider_name = table.Column<string>(type: "TEXT", nullable: false),
                    provider_sub_id = table.Column<string>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    created_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    updated_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_provider_details", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tax_years",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    year = table.Column<string>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    created_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    updated_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_tax_years", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    first_name = table.Column<string>(type: "TEXT", nullable: false),
                    last_name = table.Column<string>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    picture_url = table.Column<string>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    created_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    updated_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "financial_accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    account_name = table.Column<string>(type: "TEXT", nullable: false),
                    account_type = table.Column<Guid>(type: "TEXT", nullable: false),
                    initial_balance = table.Column<long>(type: "INTEGER", nullable: false),
                    bank_account_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    created_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    updated_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_financial_accounts", x => x.id);
                    table.ForeignKey(
                        name: "f_k_financial_accounts_bank_account_details_bank_account_id",
                        column: x => x.bank_account_id,
                        principalTable: "bank_account_details",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "auth_details",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    user_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    password_hash = table.Column<string>(type: "TEXT", nullable: true),
                    password_salt = table.Column<string>(type: "TEXT", nullable: true),
                    provider_id = table.Column<Guid>(type: "TEXT", nullable: true),
                    password_changed_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    created_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    updated_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_auth_details", x => x.id);
                    table.ForeignKey(
                        name: "f_k_auth_details__provider_details_provider_id",
                        column: x => x.provider_id,
                        principalTable: "provider_details",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "f_k_auth_details__users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "financial_statements",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    user_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    start_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    end_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    currency_code = table.Column<Guid>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    created_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    updated_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_financial_statements", x => x.id);
                    table.ForeignKey(
                        name: "f_k_financial_statements__users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    token_hash = table.Column<string>(type: "TEXT", nullable: false),
                    expires_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    user_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    created_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    updated_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "f_k_refresh_tokens__users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_bank_mappings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    user_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    bank_account_detail_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    created_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    updated_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_user_bank_mappings", x => x.id);
                    table.ForeignKey(
                        name: "f_k_user_bank_mappings_bank_account_details_bank_account_detail_id",
                        column: x => x.bank_account_detail_id,
                        principalTable: "bank_account_details",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_user_bank_mappings_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_locks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    user_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    lock_count = table.Column<long>(type: "INTEGER", nullable: false),
                    last_failed_attempt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    is_locked = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    created_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    updated_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_user_locks", x => x.id);
                    table.ForeignKey(
                        name: "f_k_user_locks_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "financial_statement_account_mappings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    financial_account_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    financial_statement_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    created_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    updated_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_financial_statement_account_mappings", x => x.id);
                    table.ForeignKey(
                        name: "f_k_financial_statement_account_mappings_financial_accounts_financial_account_id",
                        column: x => x.financial_account_id,
                        principalTable: "financial_accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_financial_statement_account_mappings_financial_statements_financial_statement_id",
                        column: x => x.financial_statement_id,
                        principalTable: "financial_statements",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_financial_statement_mappings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    user_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    financial_statement_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    created_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    updated_by = table.Column<Guid>(type: "TEXT", nullable: false),
                    is_active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_user_financial_statement_mappings", x => x.id);
                    table.ForeignKey(
                        name: "f_k_user_financial_statement_mappings_financial_statements_financial_statement_id",
                        column: x => x.financial_statement_id,
                        principalTable: "financial_statements",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_user_financial_statement_mappings_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_addresses_created_at",
                table: "addresses",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_auth_details_provider_id",
                table: "auth_details",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "i_x_auth_details_user_id",
                table: "auth_details",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_auth_details_created_at",
                table: "auth_details",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_bank_account_details_created_at",
                table: "bank_account_details",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_contacts_created_at",
                table: "contacts",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_financial_accounts_bank_account_id",
                table: "financial_accounts",
                column: "bank_account_id");

            migrationBuilder.CreateIndex(
                name: "ix_financial_accounts_created_at",
                table: "financial_accounts",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_financial_statement_account_mappings_financial_account_id",
                table: "financial_statement_account_mappings",
                column: "financial_account_id");

            migrationBuilder.CreateIndex(
                name: "i_x_financial_statement_account_mappings_financial_statement_id",
                table: "financial_statement_account_mappings",
                column: "financial_statement_id");

            migrationBuilder.CreateIndex(
                name: "ix_financial_statement_account_mappings_created_at",
                table: "financial_statement_account_mappings",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_financial_statements_user_id",
                table: "financial_statements",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_financial_statements_created_at",
                table: "financial_statements",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_provider_details_created_at",
                table: "provider_details",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_created_at",
                table: "refresh_tokens",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_tax_years_created_at",
                table: "tax_years",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_user_bank_mappings_bank_account_detail_id",
                table: "user_bank_mappings",
                column: "bank_account_detail_id");

            migrationBuilder.CreateIndex(
                name: "i_x_user_bank_mappings_user_id",
                table: "user_bank_mappings",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_bank_mappings_created_at",
                table: "user_bank_mappings",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_user_financial_statement_mappings_financial_statement_id",
                table: "user_financial_statement_mappings",
                column: "financial_statement_id");

            migrationBuilder.CreateIndex(
                name: "i_x_user_financial_statement_mappings_user_id",
                table: "user_financial_statement_mappings",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_financial_statement_mappings_created_at",
                table: "user_financial_statement_mappings",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "i_x_user_locks_user_id",
                table: "user_locks",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_locks_created_at",
                table: "user_locks",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_users_created_at",
                table: "users",
                column: "created_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "auth_details");

            migrationBuilder.DropTable(
                name: "contacts");

            migrationBuilder.DropTable(
                name: "financial_statement_account_mappings");

            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DropTable(
                name: "tax_years");

            migrationBuilder.DropTable(
                name: "user_bank_mappings");

            migrationBuilder.DropTable(
                name: "user_financial_statement_mappings");

            migrationBuilder.DropTable(
                name: "user_locks");

            migrationBuilder.DropTable(
                name: "provider_details");

            migrationBuilder.DropTable(
                name: "financial_accounts");

            migrationBuilder.DropTable(
                name: "financial_statements");

            migrationBuilder.DropTable(
                name: "bank_account_details");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}

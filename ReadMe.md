dotnet ef migrations add "InitialCreate"  --startup-project ../FinanceService.API --output-dir Migrations/Postgres -- --provider=Postgres


dotnet ef migrations add "InitialCreateSqlite"  --startup-project ../FinanceService.API --output-dir Migrations/Sqlite -- --provider=Sqlite
"# Income_Expense_BE" 

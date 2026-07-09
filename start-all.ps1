Write-Host "Starting Identity API..." -ForegroundColor Green
Start-Process dotnet -ArgumentList "run --project src/Services/Identity/ExpenseFlow.Identity.Api/ExpenseFlow.Identity.Api.csproj"

Write-Host "Starting Expense API..." -ForegroundColor Green
Start-Process dotnet -ArgumentList "run --project src/Services/Expense/ExpenseFlow.Expense.Api/ExpenseFlow.Expense.Api.csproj"

Write-Host "Starting Notification API..." -ForegroundColor Green
Start-Process dotnet -ArgumentList "run --project src/Services/Notification/ExpenseFlow.Notification.Api/ExpenseFlow.Notification.Api.csproj"

Write-Host "Starting Angular Client..." -ForegroundColor Green
Start-Process npm -ArgumentList "start --prefix src/Web/expenseflow-client"

Write-Host "All applications launched in separate windows!" -ForegroundColor Yellow

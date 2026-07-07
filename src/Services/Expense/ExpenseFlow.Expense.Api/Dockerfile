FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Expense/ExpenseFlow.Expense.Api/ExpenseFlow.Expense.Api.csproj", "Services/Expense/ExpenseFlow.Expense.Api/"]
COPY ["src/Services/Expense/ExpenseFlow.Expense.Application/ExpenseFlow.Expense.Application.csproj", "Services/Expense/ExpenseFlow.Expense.Application/"]
COPY ["src/Services/Expense/ExpenseFlow.Expense.Domain/ExpenseFlow.Expense.Domain.csproj", "Services/Expense/ExpenseFlow.Expense.Domain/"]
COPY ["src/Services/Expense/ExpenseFlow.Expense.Infrastructure/ExpenseFlow.Expense.Infrastructure.csproj", "Services/Expense/ExpenseFlow.Expense.Infrastructure/"]
COPY ["src/Services/Expense/ExpenseFlow.Expense.Persistence/ExpenseFlow.Expense.Persistence.csproj", "Services/Expense/ExpenseFlow.Expense.Persistence/"]
RUN dotnet restore "Services/Expense/ExpenseFlow.Expense.Api/ExpenseFlow.Expense.Api.csproj"
COPY src/Services/Expense/ Services/Expense/
WORKDIR "/src/Services/Expense/ExpenseFlow.Expense.Api"
RUN dotnet build "ExpenseFlow.Expense.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ExpenseFlow.Expense.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpenseFlow.Expense.Api.dll"]

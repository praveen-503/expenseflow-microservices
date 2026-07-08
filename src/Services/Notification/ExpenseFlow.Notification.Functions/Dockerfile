FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated10.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Notification/ExpenseFlow.Notification.Functions/ExpenseFlow.Notification.Functions.csproj", "Services/Notification/ExpenseFlow.Notification.Functions/"]
COPY ["src/Services/Notification/ExpenseFlow.Notification.Application/ExpenseFlow.Notification.Application.csproj", "Services/Notification/ExpenseFlow.Notification.Application/"]
COPY ["src/Services/Notification/ExpenseFlow.Notification.Domain/ExpenseFlow.Notification.Domain.csproj", "Services/Notification/ExpenseFlow.Notification.Domain/"]
COPY ["src/Services/Notification/ExpenseFlow.Notification.Infrastructure/ExpenseFlow.Notification.Infrastructure.csproj", "Services/Notification/ExpenseFlow.Notification.Infrastructure/"]
COPY ["src/Services/Notification/ExpenseFlow.Notification.Persistence/ExpenseFlow.Notification.Persistence.csproj", "Services/Notification/ExpenseFlow.Notification.Persistence/"]
RUN dotnet restore "Services/Notification/ExpenseFlow.Notification.Functions/ExpenseFlow.Notification.Functions.csproj"
COPY src/Services/Notification/ Services/Notification/
WORKDIR "/src/Services/Notification/ExpenseFlow.Notification.Functions"
RUN dotnet build "ExpenseFlow.Notification.Functions.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ExpenseFlow.Notification.Functions.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

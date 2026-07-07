FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Notification/ExpenseFlow.Notification.Api/ExpenseFlow.Notification.Api.csproj", "Services/Notification/ExpenseFlow.Notification.Api/"]
COPY ["src/Services/Notification/ExpenseFlow.Notification.Application/ExpenseFlow.Notification.Application.csproj", "Services/Notification/ExpenseFlow.Notification.Application/"]
COPY ["src/Services/Notification/ExpenseFlow.Notification.Domain/ExpenseFlow.Notification.Domain.csproj", "Services/Notification/ExpenseFlow.Notification.Domain/"]
COPY ["src/Services/Notification/ExpenseFlow.Notification.Infrastructure/ExpenseFlow.Notification.Infrastructure.csproj", "Services/Notification/ExpenseFlow.Notification.Infrastructure/"]
COPY ["src/Services/Notification/ExpenseFlow.Notification.Persistence/ExpenseFlow.Notification.Persistence.csproj", "Services/Notification/ExpenseFlow.Notification.Persistence/"]
RUN dotnet restore "Services/Notification/ExpenseFlow.Notification.Api/ExpenseFlow.Notification.Api.csproj"
COPY src/Services/Notification/ Services/Notification/
WORKDIR "/src/Services/Notification/ExpenseFlow.Notification.Api"
RUN dotnet build "ExpenseFlow.Notification.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ExpenseFlow.Notification.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpenseFlow.Notification.Api.dll"]

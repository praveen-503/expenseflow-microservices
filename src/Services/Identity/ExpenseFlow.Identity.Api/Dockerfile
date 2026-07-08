FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Identity/ExpenseFlow.Identity.Api/ExpenseFlow.Identity.Api.csproj", "Services/Identity/ExpenseFlow.Identity.Api/"]
COPY ["src/Services/Identity/ExpenseFlow.Identity.Application/ExpenseFlow.Identity.Application.csproj", "Services/Identity/ExpenseFlow.Identity.Application/"]
COPY ["src/Services/Identity/ExpenseFlow.Identity.Domain/ExpenseFlow.Identity.Domain.csproj", "Services/Identity/ExpenseFlow.Identity.Domain/"]
COPY ["src/Services/Identity/ExpenseFlow.Identity.Infrastructure/ExpenseFlow.Identity.Infrastructure.csproj", "Services/Identity/ExpenseFlow.Identity.Infrastructure/"]
COPY ["src/Services/Identity/ExpenseFlow.Identity.Persistence/ExpenseFlow.Identity.Persistence.csproj", "Services/Identity/ExpenseFlow.Identity.Persistence/"]
RUN dotnet restore "Services/Identity/ExpenseFlow.Identity.Api/ExpenseFlow.Identity.Api.csproj"
COPY src/Services/Identity/ Services/Identity/
WORKDIR "/src/Services/Identity/ExpenseFlow.Identity.Api"
RUN dotnet build "ExpenseFlow.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ExpenseFlow.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpenseFlow.Identity.Api.dll"]

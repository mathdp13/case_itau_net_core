FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CaseItau.API/CaseItau.API.csproj", "CaseItau.API/"]
COPY ["CaseItau.Application/CaseItau.Application.csproj", "CaseItau.Application/"]
COPY ["CaseItau.Domain/CaseItau.Domain.csproj", "CaseItau.Domain/"]
COPY ["CaseItau.Infrastructure/CaseItau.Infrastructure.csproj", "CaseItau.Infrastructure/"]
RUN dotnet restore "CaseItau.API/CaseItau.API.csproj"
COPY . .
WORKDIR "/src/CaseItau.API"
RUN dotnet publish "CaseItau.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CaseItau.API.dll"]

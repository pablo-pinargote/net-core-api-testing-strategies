FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["demo-api/demo-api.csproj", "demo-api/"]
RUN dotnet restore "demo-api/demo-api.csproj"
COPY . .
WORKDIR "/src/demo-api"
RUN dotnet build "demo-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "demo-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "demo-api.dll"]

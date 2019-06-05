FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base

WORKDIR /app
EXPOSE 12345

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS publish
WORKDIR /src
COPY . .
RUN dotnet publish MagicOnion.Server.csproj -c Release -o /app

FROM base As final
WORKDIR /app
COPY --from=publish /app /app
ENTRYPOINT [ "dotnet", "MagicOnion.Server.dll" ]
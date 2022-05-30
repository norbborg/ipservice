#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/IpService.Host/IpService.Host.csproj", "src/IpService.Host/"]
COPY ["src/IpService.Contracts/IpService.Contracts.csproj", "src/IpService.Contracts/"]
COPY ["src/IpService/IpService.csproj", "src/IpService/"]
COPY ["src/IPStackIntegration/IPStackIntegration.csproj", "src/IPStackIntegration/"]
RUN dotnet restore "src/IpService.Host/IpService.Host.csproj"
COPY . .
WORKDIR "/src/src/IpService.Host"
RUN dotnet build "IpService.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IpService.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IpService.Host.dll"]

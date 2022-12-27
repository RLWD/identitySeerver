FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["VDW.SalesApp.IdentityServer/VDW.SalesApp.IdentityServer.csproj", "VDW.SalesApp.IdentityServer/"]
RUN dotnet restore "VDW.SalesApp.IdentityServer/VDW.SalesApp.IdentityServer.csproj"
COPY . .
WORKDIR "/src/VDW.SalesApp.IdentityServer"
RUN dotnet build "VDW.SalesApp.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VDW.SalesApp.IdentityServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VDW.SalesApp.IdentityServer.dll"]
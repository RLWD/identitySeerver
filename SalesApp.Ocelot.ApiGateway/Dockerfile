FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SalesApp.Ocelot.ApiGateway/SalesApp.Ocelot.ApiGateway.csproj", "SalesApp.Ocelot.ApiGateway/"]
RUN dotnet restore "SalesApp.Ocelot.ApiGateway/SalesApp.Ocelot.ApiGateway.csproj"
COPY . .
WORKDIR "/src/SalesApp.Ocelot.ApiGateway"
RUN dotnet build "SalesApp.Ocelot.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SalesApp.Ocelot.ApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SalesApp.Ocelot.ApiGateway.dll"]
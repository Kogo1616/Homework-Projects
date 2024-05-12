FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Girteka.ElectricAggregate.API/Girteka.ElectricAggregate.API.csproj", "Girteka.ElectricAggregate.API/"]
COPY ["Girteka.ElectricAggregate.Domain/Girteka.ElectricAggregate.Domain.csproj", "Girteka.ElectricAggregate.Domain/"]
COPY ["Girteka.ElectricAggregate.Integrations/Girteka.ElectricAggregate.Integrations.csproj", "Girteka.ElectricAggregate.Integrations/"]
COPY ["Girteka.ElectricAggregate.Persistence/Girteka.ElectricAggregate.Persistence.csproj", "Girteka.ElectricAggregate.Persistence/"]
RUN dotnet restore "Girteka.ElectricAggregate.API/Girteka.ElectricAggregate.API.csproj"
COPY . .
WORKDIR "/src/Girteka.ElectricAggregate.API"
RUN dotnet build "Girteka.ElectricAggregate.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Girteka.ElectricAggregate.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Girteka.ElectricAggregate.API.dll"]

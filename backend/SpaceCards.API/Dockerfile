#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SpaceCards.API/SpaceCards.API.csproj", "SpaceCards.API/"]
COPY ["SpaceCards.DataAccess.Postgre/SpaceCards.DataAccess.Postgre.csproj", "SpaceCards.DataAccess.Postgre/"]
COPY ["SpaceCards.Domain/SpaceCards.Domain.csproj", "SpaceCards.Domain/"]
COPY ["SpaceCards.BusinessLogic/SpaceCards.BusinessLogic.csproj", "SpaceCards.BusinessLogic/"]
RUN dotnet restore "SpaceCards.API/SpaceCards.API.csproj"
COPY . .
WORKDIR "/src/SpaceCards.API"
RUN dotnet build "SpaceCards.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SpaceCards.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpaceCards.API.dll"]

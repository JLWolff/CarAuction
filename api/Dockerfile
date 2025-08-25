FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/CarAuctionApi.Presentation/*.csproj ./CarAuctionApi.Presentation/
COPY src/CarAuctionApi.Application/*.csproj   ./CarAuctionApi.Application/
COPY src/CarAuctionApi.Domain/*.csproj        ./CarAuctionApi.Domain/
COPY src/CarAuctionApi.Infrastructure/*.csproj ./CarAuctionApi.Infrastructure/
COPY src/CarAuctionApi.CrossCutting/*.csproj  ./CarAuctionApi.CrossCutting/

RUN dotnet restore ./CarAuctionApi.Presentation/CarAuctionApi.Presentation.csproj

COPY src/ ./ 
WORKDIR /src/CarAuctionApi.Presentation
RUN dotnet publish -c Release -o /app/out /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "CarAuctionApi.Presentation.dll"]

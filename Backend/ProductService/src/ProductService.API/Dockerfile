# Build context: repository root
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Backend/ProductService/src/ProductService.Domain/ProductService.Domain.csproj ProductService/src/ProductService.Domain/
COPY Backend/ProductService/src/ProductService.Application/ProductService.Application.csproj ProductService/src/ProductService.Application/
COPY Backend/ProductService/src/ProductService.Infrastructure/ProductService.Infrastructure.csproj ProductService/src/ProductService.Infrastructure/
COPY Backend/ProductService/src/ProductService.API/ProductService.API.csproj ProductService/src/ProductService.API/

RUN dotnet restore ProductService/src/ProductService.API/ProductService.API.csproj

COPY Backend/ProductService/src/ ProductService/src/

RUN dotnet publish ProductService/src/ProductService.API/ProductService.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProductService.API.dll"]

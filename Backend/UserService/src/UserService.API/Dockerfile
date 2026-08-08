# Build context: repository root
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Backend/UserService/src/UserService.Domain/UserService.Domain.csproj UserService/src/UserService.Domain/
COPY Backend/UserService/src/UserService.Application/UserService.Application.csproj UserService/src/UserService.Application/
COPY Backend/UserService/src/UserService.Infrastructure/UserService.Infrastructure.csproj UserService/src/UserService.Infrastructure/
COPY Backend/UserService/src/UserService.API/UserService.API.csproj UserService/src/UserService.API/

RUN dotnet restore UserService/src/UserService.API/UserService.API.csproj

COPY Backend/UserService/src/ UserService/src/

RUN dotnet publish UserService/src/UserService.API/UserService.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UserService.API.dll"]

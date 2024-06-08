FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ./CrackHash.Manager ./CrackHash.Manager
COPY ./CrackHash.Common ./CrackHash.Common
RUN dotnet restore "/src/CrackHash.Manager/CrackHash.Manager/CrackHash.Manager.csproj"
RUN dotnet build "/src/CrackHash.Manager/CrackHash.Manager/CrackHash.Manager.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "/src/CrackHash.Manager/CrackHash.Manager/CrackHash.Manager.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "CrackHash.Manager.dll"]

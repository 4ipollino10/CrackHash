FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ./CrackHash.Worker ./CrackHash.Worker
COPY ./CrackHash.Common ./CrackHash.Common
RUN dotnet restore "/src/CrackHash.Worker/CrackHash.Worker/CrackHash.Worker.csproj"
RUN dotnet build "/src/CrackHash.Worker/CrackHash.Worker/CrackHash.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "/src/CrackHash.Worker/CrackHash.Worker/CrackHash.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
EXPOSE 8080
EXPOSE 8081

WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CrackHash.Worker.dll"]

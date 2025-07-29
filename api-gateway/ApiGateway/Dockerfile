FROM mcr.microsoft.com/dotnet/aspnet:9.0 as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
COPY ["api-gateway/ApiGateway/ApiGateway.csproj", "ApiGateway/"]
COPY ["local-nuget-packages/", "/local-nuget-packages/"]
COPY ["api-gateway/nuget.config", "./"]
RUN dotnet restore "ApiGateway/ApiGateway.csproj"

COPY ["api-gateway/ApiGateway/", "ApiGateway/"]
WORKDIR "ApiGateway"
RUN dotnet build "ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build as publish
RUN dotnet publish "ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
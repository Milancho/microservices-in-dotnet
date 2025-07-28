FROM mcr.microsoft.com/dotnet/aspnet:9.0 as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
COPY ["auth-microservice/Auth.Service/Auth.Service.csproj", "Auth.Service/"]
COPY ["local-nuget-packages/", "/local-nuget-packages/"]
COPY ["auth-microservice/nuget.config", "./"]
RUN dotnet restore "Auth.Service/Auth.Service.csproj"

COPY ["auth-microservice/Auth.Service/", "Auth.Service/"]
WORKDIR "Auth.Service"
RUN dotnet build "Auth.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build as publish
RUN dotnet publish "Auth.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.Service.dll"]
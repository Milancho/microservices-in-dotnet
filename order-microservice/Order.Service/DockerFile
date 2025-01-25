FROM mcr.microsoft.com/dotnet/aspnet:9.0 as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
COPY ["order-microservice/Order.Service/Order.Service.csproj", "Order.Service/"]
COPY ["local-nuget-packages/", "/local-nuget-packages/"]
COPY ["order-microservice/nuget.config", "./"]
RUN dotnet restore "Order.Service/Order.Service.csproj"

COPY ["order-microservice/Order.Service/", "Order.Service/"]
WORKDIR "Order.Service"
RUN dotnet build "Order.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build as publish
RUN dotnet publish "Order.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.Service.dll"]
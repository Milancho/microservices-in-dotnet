# microservices-in-dotnet
Microservices in .NET

## Docker Docs
``` cmd
docker rm -f sql rabbitmq redis
docker compose up sql rabbitmq redis
docker compose up product --build
docker compose up basket --build
docker compose up order --build
docker compose up jaeger

dotnet pack
dotnet nuget push ECommerce.Shared.1.5.0.nupkg -s C:\Projects\Source\microservices-in-dotnet\local-nuget-packages
dotnet add package ECommerce.Shared -v 1.5.0

```

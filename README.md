# microservices-in-dotnet
Microservices in .NET

## Scaffolding a new project
``` bat
mkdir auth-microservice && cd auth-microservice
dotnet new sln -n Auth.Service
dotnet new web -n Auth.Service
dotnet sln add Auth.Service\Auth.Service.csproj
```

## Docker
``` bash  
docker rm -f sql rabbitmq redis
docker compose up sql rabbitmq redis
docker compose up product --build
docker compose up basket --build
docker compose up order --build
docker compose up jaeger
```

## Nuget
``` powershell   
dotnet pack
dotnet nuget push ECommerce.Shared.1.5.0.nupkg -s C:\Projects\Source\microservices-in-dotnet\local-nuget-packages
dotnet add package ECommerce.Shared -v 1.5.0
```

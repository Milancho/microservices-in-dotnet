# microservices-in-dotnet
Microservices in .NET

## Scaffolding a new project
``` cmd
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
docker compose up auth --build
docker compose up gateway --build
docker compose up jaeger
```

## Nuget
``` powershell   
dotnet pack
dotnet nuget push ECommerce.Shared.1.5.0.nupkg -s C:\Projects\Source\microservices-in-dotnet\local-nuget-packages
dotnet add package ECommerce.Shared -v 1.5.0
```

## Migration
``` bash
dotnet ef migrations add Initial -o Infrastructure\Data\EntityFramework\Migrations
dotnet ef database update --context AuthContext
```

## Kubernetes
``` bat
kubectl apply -f sql.yaml
kubectl get pods
kubectl get services

:: product
docker build -t milanchoars/productservice:latest -f product-microservice\Product.Service\Dockerfile .
docker login -u milanchoars
docker push milanchoars/productservice:latest
kubectl apply -f product-microservice.yaml
kubectl logs -f deployment/productservice

:: auth
docker build -t milanchoars/authservice:latest -f auth-microservice\Auth.Service\Dockerfile .
docker push milanchoars/authservice:latest
kubectl apply -f auth-microservice.yaml
kubectl logs -f deployment/authservice

:: apigateway
docker build -t milanchoars/apigateway:latest -f api-gateway\ApiGateway\Dockerfile .
docker push milanchoars/apigateway:latest
kubectl apply -f api-gateway.yaml
kubectl logs -f deployment/apigateway
```
:: basket
docker build -t milanchoars/basketservice:latest -f basket-microservice\Basket.Service\Dockerfile .
docker login -u milanchoars
docker push milanchoars/basketservice:latest
kubectl apply -f basket-microservice.yaml
kubectl logs -f deployment/basketservice

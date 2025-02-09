# Product.Service Infrastructure - Entity Framework

This directory contains the Entity Framework related code for the Product.Service microservice.

## Getting Started

To get started with the Entity Framework setup for the Product.Service, follow the steps below.

### Prerequisites

- .NET 6.0 SDK or later
- SQL Server or any other supported database

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/your-repo/microservices-in-dotnet.git
    cd microservices-in-dotnet/product-microservice/Product.Service
    ```

2. Restore the dependencies:
    ```sh
    dotnet restore
    ```

3. Update the database connection string in `appsettings.json`:
    ```json
    "ConnectionStrings": {
      "Default": "Server=localhost,1433;Database=Product;User Id=sa;Password=micR0S3rvice$;TrustServerCertificate=True"
    }
    ```

### Migrations

To add a new migration, use the following command:
```sh
dotnet ef migrations add Initial -o Infrastructure\Data\EntityFramework\Migrations
```

To update the database with the latest migrations, use:
```sh
dotnet ef database update
```

### Usage

To run the application, use:
```sh
dotnet run --project Product.Service.API
```

## Project Structure

- **Data**: Contains the DbContext and entity configurations.
- **Migrations**: Contains the database migrations.

## Contributing

Contributions are welcome! Please read the [contributing guidelines](CONTRIBUTING.md) first.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

# Microservice ASP.NET Core 6

This repository provides an example of a microservices architecture implemented using ASP.NET Core 6. The project follows best practices for building scalable and efficient microservices.

## Technologies Used
- **ASP.NET Core** - Primary framework for the API
- **.NET 6** - Runtime and development framework
- **OAuth2** - Authentication protocol
- **OpenID** - Identity layer on top of OAuth2
- **JWT (JSON Web Tokens)** - Token-based authentication
- **Identity Server** - Authentication and authorization service
- **RabbitMQ** - Message broker for inter-service communication
- **API Gateway with Ocelot** - Gateway for routing and load balancing
- **Swagger OpenAPI** - API documentation and testing

## Project Structure
The project adheres to a layered architecture pattern and follows industry best practices.

```plaintext
/micro-service-asp-net-core-6
│   README.md
│   docker-compose.yml
│   .gitignore
│
├───src
│   ├───Services
│   │   ├───OrderService
│   │   ├───PaymentService
│   │   └───...
│   ├───Shared
│   ├───APIGateway
│   ├───IdentityService
│   └───...
│
└───tests
    ├───OrderService.Tests
    ├───PaymentService.Tests
    └───...
```

## Setup and Execution

### Prerequisites
- .NET 6 SDK
- Docker
- Database (SQL Server, PostgreSQL, etc.)
- Message broker (RabbitMQ/Kafka, if required)

### Running the Application
1. Clone the repository:
   ```sh
   git clone https://github.com/victorfg21/micro-service-asp-net-core-6.git
   cd micro-service-asp-net-core-6
   ```
2. Configure environment variables based on `.env.example`.
3. To start with Docker, run:
   ```sh
   docker-compose up -d
   ```
4. To run locally, execute:
   ```sh
   dotnet run --project src/Services/OrderService
   ```
5. Open Swagger API documentation at `http://localhost:5000/swagger`

## Running Tests
To execute unit tests, use:
```sh
dotnet test
```

## Contributing
Contributions are welcome! Feel free to open issues or submit pull requests to improve the project.

## License
This project is licensed under the MIT License. For more details, refer to the `LICENSE` file.

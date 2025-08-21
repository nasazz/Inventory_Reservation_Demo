# Inventory Management Backend

.NET Core backend for the Inventory Reservation System using Clean Architecture principles.

## Architecture

The backend follows Clean Architecture with the following layers:
- **Domain**: Core business logic and entities
- **Application**: Use cases and business rules
- **Infrastructure**: External concerns like persistence
- **API**: HTTP interface and controllers

## Key Features Implemented

- Domain-driven design with aggregate roots and domain events
- CQRS pattern using MediatR for command/query separation
- Entity Framework Core for data persistence
- RESTful API endpoints for inventory operations
- Cross-Origin Resource Sharing (CORS) configuration for Angular frontend
- Global exception handling middleware
- Fluent Validation for request validation
# Inventory Reservation Demo

A full-stack inventory management system with reservation capabilities built using .NET Core and Angular.

## Project Overview

This application demonstrates a domain-driven design approach to inventory management with the following features:
- Inventory item tracking with SKU, name, and quantity
- Reservation system for inventory items
- Real-time inventory availability calculation
- Clean architecture with separation of concerns

## Project Structure

- `/frontend` - Angular frontend application
- `/src` - .NET Core backend application using Clean Architecture
- `/tests` - Test projects for backend components

## Getting Started

### Using Docker (Recommended)

#### Production Environment
1. Ensure Docker and Docker Compose are installed on your system
2. Clone the repository
3. From the root directory, run:
   ```bash
   docker compose up -d


###  Access the application:

- Frontend: http://localhost:8080
- Backend API: http://localhost:5211
- SQL Server: localhost:1433



### Manual Setup 
Backend
1. Navigate to the /src/Api directory
2. Run dotnet run to start the API server
3. API will be available at http://localhost:5211 

Frontend
1. Navigate to the /frontend/angular-app directory
2. Run npm install to install dependencies
3. Run ng serve to start the development server
4. Access the application at http://localhost:4200


## Docker Configuration
- Production containers:
  - Frontend: Nginx-based production build
  - Backend: .NET 9.0 runtime
  - Database: SQL Server 2022

- Development containers:
  - Frontend: Node.js development environment with hot-reload
  - Backend: .NET SDK with debugging capabilities
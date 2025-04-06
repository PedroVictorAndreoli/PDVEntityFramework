# PDV Entity Framework

A Point of Sale (PDV) system built with ASP.NET Core 8.0 and Entity Framework Core 9.0.

## 🚀 Features

- Modern ASP.NET Core 8.0 Web API
- Entity Framework Core 9.0 for data access
- SQL Server database integration
- Swagger UI for API documentation and testing
- Repository pattern implementation
- RESTful API architecture

## 📋 Prerequisites

- .NET 8.0 SDK
- SQL Server
- Visual Studio 2022 or later (recommended)

## 🛠️ Installation

1. Clone the repository:
```bash
git clone https://github.com/yourusername/PDVEntityFramework.git
```

2. Navigate to the project directory:
```bash
cd PDVEntityFramework
```

3. Restore the NuGet packages:
```bash
dotnet restore
```

4. Update the connection string in `CaixaSupermercadoContext.cs` to match your SQL Server instance.

5. Run the application:
```bash
dotnet run
```

## 📁 Project Structure

- `Controllers/` - API endpoints
- `Models/` - Data models and entities
- `Repository/` - Data access layer
- `Interfaces/` - Contract definitions
- `Views/` - MVC views (if applicable)

## 🔧 Configuration

The application uses the following configuration files:
- `appsettings.json` - Main configuration file
- `appsettings.Development.json` - Development-specific settings

## 📚 API Documentation

Once the application is running, you can access the Swagger UI documentation at:
```
https://localhost:5001/swagger
```

## 🛡️ Dependencies

- Microsoft.EntityFrameworkCore (9.0.1)
- Microsoft.EntityFrameworkCore.SqlServer (9.0.1)
- Swashbuckle.AspNetCore (7.2.0)

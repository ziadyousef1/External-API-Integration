# Task Manager API - CAT Stations - Backend

![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue.svg)
![C#](https://img.shields.io/badge/C%23-Latest-green.svg)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-orange.svg)
![License](https://img.shields.io/badge/License-MIT-yellow.svg)

A Task Management API built with ASP.NET Core 8.0, featuring JWT authentication, external API integration, and CRUD operations. This project demonstrates modern backend development practices with a focus on security, performance, and maintainability.

## 🌟 Features

### Core Functionality
- **Task Management**: Complete CRUD operations for todos with priority levels
- **User Authentication**: Secure JWT-based authentication and authorization
- **External API Integration**: integration with external prediction services
- **File Upload Support**: Image classification through external ML services
- **Database Integration**: Entity Framework Core with SQL Server

### Security & Performance
- **JWT Authentication**: Secure token-based authentication
- **Password Hashing**: BCrypt implementation for secure password storage
- **API Documentation**: Swagger/OpenAPI documentation
- **CORS Support**: Configurable cross-origin resource sharing
- **Input Validation**: Robust data validation and error handling




### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/ziadyousef1/External-API-Integration.git
   cd External-API-Integration
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure the database**
   
   Update the connection string in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=CATStationsDb1;Trusted_Connection=True;TrustServerCertificate=True"
     }
   }
   ```

4. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

The API will be available at:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger UI**: `https://localhost:5001/swagger`

## 📡 API Endpoints

### Authentication
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/account/register` | Register new user | ❌ |
| POST | `/api/account/login` | User login | ❌ |

### Task Management
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/todos` | Get all todos | ✅ |
| GET | `/api/todos/{id}` | Get todo by ID | ✅ |
| POST | `/api/todos` | Create new todo | ✅ |
| PUT | `/api/todos/{id}` | Update todo | ✅ |
| DELETE | `/api/todos/{id}` | Delete todo | ✅ |

### External Integrations
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/predictions` | Get external users | ❌ |
| POST | `/api/predictions/url` | Predict from image URL | ❌ |
| POST | `/api/predictions/upload` | Predict from uploaded image | ❌ |

### User Management
| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/users` | Get all users | ✅ |
| GET | `/api/users/{id}` | Get user by ID | ✅ |

## 🔧 Configuration

### Environment Variables

Create an `appsettings.Development.json` file for development settings:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Your_Development_Connection_String"
  },
  "Jwt": {
    "Key": "Your_JWT_Secret_Key_Here",
    "ExpiryInHours": 1
  }
}
```

### JWT Configuration

The application uses JWT for authentication. Configure the following in your `appsettings.json`:

- **Jwt:Key**: Secret key for signing tokens (keep this secure!)
- **Jwt:ExpiryInHours**: Token expiration time

## 🗃️ Database Schema

### Todo Entity
```csharp
public class Todo
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime DueDate { get; set; }
    public Priority Priority { get; set; } // Low, Medium, High
}
```

### User Entity
```csharp
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; } // Admin, User 
```

## 🔐 Authentication

This API uses JWT (JSON Web Tokens) for authentication. Here's how to use it:

1. **Register** a new user via `/api/account/register`
2. **Login** with credentials via `/api/account/login` to receive a JWT token
3. **Include** the token in the Authorization header for protected endpoints:
   ```
   Authorization: Bearer your_jwt_token_here
   ```

## 🧪 Testing

### Using Swagger UI
1. Navigate to `https://localhost:5001/swagger`
2. Use the "Authorize" button to add your JWT token
3. Test endpoints directly from the browser

### Using Postman or curl
```bash
# Login to get token
curl -X POST "https://localhost:5001/api/account/login" \
  -H "Content-Type: application/json" \
  -d '{"username":"your_username","password":"your_password"}'

# Use token for protected endpoints
curl -X GET "https://localhost:5001/api/todos" \
  -H "Authorization: Bearer your_jwt_token"
```


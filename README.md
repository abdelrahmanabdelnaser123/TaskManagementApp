#  Task Management API

A simple and scalable Task Management Backend API built with **.NET 8**, following clean architecture principles (DDD-style), implementing authentication, authorization, caching, and background processing.

---

##  Features

###  User Module

* User Registration
* User Login (JWT Authentication)
* Get Current User Profile

###  Authentication & Authorization

* JWT-based authentication
* Role-based authorization (Admin / User)

###  Admin Module

* Seeded Admin User (created automatically on first run)
* Admin capabilities:

  * Create users
  * Delete users
  * View all users

###  Task Module

* Create Task
* Get Task by ID
* Get My Tasks
* Update Task Status
* Admin: Get All Tasks

###  Background Processing

* Tasks are processed asynchronously using `.NET BackgroundService`
* Task status auto-updated:

  * Pending → InProgress → Done

###  Caching

* Redis caching for:

  * Get Task by ID
* Cache invalidation on update

###  Business Logic

* Prevent duplicate task titles per user per day
* Enforced task status transitions

---

##  Project Structure

```
TaskManagement
│
├── TaskManagement.API
├── TaskManagement.Application
├── TaskManagement.Domain
├── TaskManagement.Infrastructure
```

---

##  Technologies Used

* .NET 8 Web API
* Entity Framework Core
* SQL Server
* ASP.NET Identity
* JWT Authentication
* Redis (StackExchange.Redis)
* Background Services
* Swagger (OpenAPI)

---

##  Setup Instructions

### 1-  Clone Repository

```
git clone https://github.com/your-username/TaskManagementApp.git
cd TaskManagement
```

---

### 2- Configure appsettings.json

Create `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_SQL_SERVER_CONNECTION",
    "Redis": "localhost:6379"
  },
  "Jwt": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "TaskManagementApp",
    "Audience": "TaskManagementApp"
  }
}
```

---

### 3- Run Database Migrations

```
dotnet ef database update
```

---

### 4- Run Application

```
dotnet run
```

---

### 5- Open Swagger

```
https://localhost:xxxx/swagger
```

---

## 5- Default Admin Credentials


| Email    | [admin@example.com](mailto:admin@example.com) |
| Password | Admin@123                                     |

---

##  API Endpoints

###  Account

* `POST /api/Account/register`
* `POST /api/Account/login`
* `GET /api/Account/profile`

###  Tasks

* `POST /api/Task`
* `GET /api/Task/{id}`
* `GET /api/Task/my-tasks`
* `PUT /api/Task/status`
* `GET /api/Task/all` (Admin only)

###  Admin

* `GET /api/Admin/users`
* `POST /api/Admin/users`
* `DELETE /api/Admin/users/{id}`

---

##  Authorization Rules

* Users can:

  * Manage their own tasks only
* Admin can:

  * Manage all users
  * View all tasks

---

##  Important Notes

* Redis must be running locally:

```
localhost:6379
```

* Passwords are securely hashed using ASP.NET Identity
* JWT tokens include user roles

---

##  Testing

Use Swagger or Postman:

1. Register / Login
2. Copy JWT Token
3. Authorize using:

```
 YOUR_TOKEN
```



---

# URL Shortener

A full-stack URL shortening service built with an ASP.NET Core backend and an Angular frontend. This project allows users to generate short, shareable links, manage their created URLs, and provides role-based access control (Admin/User).

## Technical Stack

**Backend (.NET API)**
* .NET 8.0
* ASP.NET Core Web API
* Database: PostgreSQL (Entity Framework Core Code-First approach)
* JWT Authentication & Authorization

**Frontend (Angular)**
* Angular (TypeScript,Bootstrap 5)
* jwt-decode (Client-side token parsing)

**Testing**
* xUnit & FluentAssertions (Unit Testing),
* In-Memory Database(Microsoft.EntityFrameworkCore.InMemory)
---

## Prerequisites
1. **.NET 8.0 SDK**
2. **Node.js** (v18 or higher) and **npm**
3. **Angular CLI** (`npm install -g @angular/cli`)
4. **PostgreSQL**

---

## Setup & Installation

### 1. Database Configuration
1. Open PostgreSQL and ensure the server is running.
2. Open `UrlShortener.API/appsettings.json`.
3. Update the `DefaultConnection` string with your local PostgreSQL credentials:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=UrlShortenerDb;Username=postgres;Password=YOUR_PASSWORD"
   }
   ```

### 2. Backend Setup
Open a terminal in the `UrlShortener.API` directory and run the following commands:
```bash
# Restore NuGet packages
dotnet restore

# Apply Entity Framework Core migrations to create the database
dotnet ef database update

# Run the application
dotnet run
```
*Note: The application will automatically seed the database with initial Admin and User accounts on startup via DataSeeder.*

### 3. Frontend Setup (Angular)
Open a new terminal in the `UrlShortener.ClientApp directory and run:
```bash
# Install node modules
npm install

# Start the Angular development server
npm start
```
The frontend will be available at `http://localhost:4200`.

---

## Default Seeded Credentials

The database is automatically seeded on the first run. You can log in using the following accounts :

* **Admin Account**
  * Login: `admin` 
  * Password: `Secret123!` 

* **Standard User Account**
  * Login: `Viacheslav` 
  * Password: `Pass123!` 

* **Role-Based Controll**

  * Users: Can create, view, and delete their own shortened URLs.
  * Admins: Can view and manage all URLs, including those created by other users.
---

## Running Unit Tests

The project includes unit tests for the core business logic (URL generation and URL redirection endpoints) using xUnit and an In-Memory Database to ensure fast, isolated testing.

To run the tests, open a terminal in the `UrlShortener.Tests` directory:
```bash
dotnet test
```

---
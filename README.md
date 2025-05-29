# Event Management Solution

This repository contains a modular Event Management system built with ASP.NET Core. It includes backend APIs, a frontend web application, and a service layer for business logic.

## Solution Structure

```
Event.sln
DAL/
  Class1.cs
  DAL.csproj
  ...
EventManagement/
  appsettings.json
  EventManagement.csproj
  Program.cs
  Context/
  Migrations/
  Models/
  Repository/
  ...
EventManagementFrontend/
  appsettings.Development.json
  appsettings.json
  EventManagementFrontend.csproj
  Program.cs
  Controllers/
  Models/
  Views/
  wwwroot/
  ...
ServiceLayer/
  appsettings.Development.json
  appsettings.json
  Program.cs
  ServiceLayer.csproj
  Controllers/
  ...
```

## Projects

- **DAL/**  
  Data Access Layer for database operations.

- **EventManagement/**  
  Main backend ASP.NET Core Web API project.

- **EventManagementFrontend/**  
  ASP.NET Core MVC frontend for user interaction.

- **ServiceLayer/**  
  Contains business logic and service APIs.

## Getting Started

### Prerequisites

- [.NET 8.0 SDK or later](https://dotnet.microsoft.com/download)
- SQL Server or your preferred database (update `appsettings.json` as needed)
- Node.js (if you use frontend build tools)

### Build and Run

1. **Clone the repository:**
   ```sh
   git clone <your-repo-url>
   cd Event
   ```

2. **Restore NuGet packages:**
   ```sh
   dotnet restore
   ```

3. **Build the solution:**
   ```sh
   dotnet build
   ```

4. **Apply database migrations (if needed):**
   ```sh
   cd EventManagement
   dotnet ef database update
   ```

5. **Run the backend API:**
   ```sh
   cd EventManagement
   dotnet run
   ```

6. **Run the frontend:**
   ```sh
   cd ../EventManagementFrontend
   dotnet run
   ```

7. **Run the service layer (if needed):**
   ```sh
   cd ../ServiceLayer
   dotnet run
   ```

## Configuration

- Update connection strings and other settings in the respective `appsettings.json` files for each project.

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/YourFeature`)
3. Commit your changes (`git commit -am 'Add some feature'`)
4. Push to the branch (`git push origin feature/YourFeature`)
5. Create a new Pull Request

## License

This project is licensed under the MIT License.

---

**Note:**  
- For more details on each project, see the respective README or documentation in each folder.
- Make sure to keep your NuGet packages up to date.

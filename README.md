# Event Management System

A full-stack ASP.NET Core solution for managing events, sessions, speakers, and participant registrations with JWT-based authentication and role-based authorization.

---

## Table of Contents

- [Project Structure](#project-structure)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Configuration](#configuration)
  - [Database Setup](#database-setup)
  - [Running the Application](#running-the-application)
- [API Overview](#api-overview)
- [Authentication & Authorization](#authentication--authorization)
- [Frontend Usage](#frontend-usage)
- [Development Notes](#development-notes)
- [License](#license)

---

## Project Structure

```
Event/
├── DAL/                        # Data Access Layer (optional, for future expansion)
├── EventManagement/            # Core business logic and models
│   ├── Context/                # EF Core DbContext
│   ├── Migrations/             # EF Core migrations
│   ├── Models/                 # Entity models
│   ├── Program.cs              # Entry point for the core project
│   └── ...
├── EventManagementFrontend/    # ASP.NET Core MVC frontend (Razor views)
│   ├── Controllers/            # MVC controllers (Admin, Participant, Login, etc.)
│   ├── Views/                  # Razor views for UI
│   ├── wwwroot/                # Static files (CSS, JS, etc.)
│   ├── Program.cs
│   └── ...
├── ServiceLayer/               # ASP.NET Core Web API (backend)
│   ├── Controllers/            # API controllers (EventDetails, SessionInfo, etc.)
│   ├── Program.cs
│   └── ...
└── README.md                   # This file
```

---

## Features

- **User Registration & Login** (JWT-based)
- **Role-based Authorization** (Admin, Participant)
- **Event CRUD** (Create, Read, Update, Delete)
- **Session Management** (CRUD for event sessions)
- **Speaker Management** (CRUD for speakers)
- **Participant Registration** (register for events)
- **Admin Dashboard** (manage all entities)
- **Swagger/OpenAPI** for API documentation and testing
- **EF Core** for database access

---

## Technology Stack

- **Backend:** ASP.NET Core Web API
- **Frontend:** ASP.NET Core MVC (Razor)
- **Database:** SQL Server (via EF Core)
- **Authentication:** JWT Bearer Tokens
- **Authorization:** Role-based (Admin, Participant)
- **API Docs:** Swagger/OpenAPI

---

## Getting Started

### Prerequisites

- [.NET 8 SDK or later](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Node.js & npm](https://nodejs.org/) (optional, for frontend tooling)
- [Git](https://git-scm.com/) (for version control)

### Configuration

1. **Clone the repository:**
   ```sh
   git clone https://github.com/yourusername/EventManagement.git
   cd EventManagement
   ```

2. **Configure connection strings:**
   - Edit `EventManagement/appsettings.json` and `ServiceLayer/appsettings.json` to set your SQL Server connection string under `"DefaultConnection"`.
   - Configure JWT settings under `"JwtSettings"` in `ServiceLayer/appsettings.json`:
     ```json
     "JwtSettings": {
       "SecretKey": "your-very-strong-secret-key",
       "Issuer": "EventManagementAPI",
       "Audience": "EventManagementFrontend"
     }
     ```

### Database Setup

1. **Apply EF Core migrations:**
   ```sh
   cd EventManagement
   dotnet ef database update
   ```

2. **(Optional) Seed initial data**  
   Add admin and participant users directly to the database or via the registration endpoint.

### Running the Application

1. **Start the backend API:**
   ```sh
   cd ServiceLayer
   dotnet run
   ```
   The API will be available at `https://localhost:5199/` (or your configured port).

2. **Start the frontend:**
   ```sh
   cd ../EventManagementFrontend
   dotnet run
   ```
   The frontend will be available at `https://localhost:xxxx/` (see console output for port).

---

## API Overview

- **Authentication:**  
  `POST /api/Auth/login` — returns JWT token

- **User Management:**  
  `GET /api/userinfo` — list all users  
  `POST /api/userinfo` — register new user

- **Event Management:**  
  `GET /api/EventDetails` — list events  
  `POST /api/EventDetails` — create event (Admin only)  
  `PUT /api/EventDetails/{id}` — update event (Admin only)  
  `DELETE /api/EventDetails/{id}` — delete event (Admin only)

- **Session Management:**  
  `GET /api/SessionInfo` — list sessions  
  `POST /api/SessionInfo` — create session (Admin only)

- **Speaker Management:**  
  `GET /api/SpeakersDetails` — list speakers  
  `POST /api/SpeakersDetails` — create speaker (Admin only)

- **Participant Registration:**  
  `GET /api/ParticipantEventDetails` — list registrations  
  `POST /api/ParticipantEventDetails` — register for event

> **See Swagger UI at `/swagger` when the API is running for full documentation.**

---

## Authentication & Authorization

- **JWT Bearer Authentication** is used for all protected API endpoints.
- **Roles:**  
  - `Admin`: Full access to all CRUD operations  
  - `Participant`: Can view events/sessions and register for events

- **Frontend:**  
  - On login, the JWT token is stored in session and attached to all API requests.
  - UI is role-aware (admins see admin dashboard, participants see their events).

---

## Frontend Usage

- **Login:**  
  - Admins and participants log in via `/Login`.
  - JWT is stored in session and used for all API calls.

- **Admin Dashboard:**  
  - Manage events, sessions, speakers, and participants.

- **Participant Dashboard:**  
  - View available events and register.

---

## Development Notes

- **.gitignore:**  
  Add a `.gitignore` file to exclude `bin/`, `obj/`, `*.user`, `*.suo`, `.vs/`, `.vscode/`, and sensitive config files.

- **Swagger:**  
  The API is documented and testable via Swagger UI at `/swagger`.

- **Extending:**  
  Add more roles, endpoints, or UI features as needed.

---

## License

This project is licensed under the MIT License.  
See [LICENSE](EventManagementFrontend/wwwroot/lib/bootstrap/LICENSE) and [LICENSE](EventManagementFrontend/wwwroot/lib/jquery-validation/LICENSE.md) for third-party dependencies.

---

**For any issues or contributions, please open an issue or pull request on GitHub.**

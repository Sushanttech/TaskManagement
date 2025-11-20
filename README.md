# TaskManager Full-stack Starter

This archive contains a full-stack starter project:
- **server/**: ASP.NET Core 7 Web API using EF Core (SQLite), JWT auth, role-based access, middleware that logs requests/responses to DB, SignalR hub.
- **client/**: Angular 16+ skeleton (src) with AuthService (BehaviorSubject), SignalR integration, TaskService (BehaviorSubject), AuthGuard, basic components.

## Quick server setup

1. Install .NET 8 SDK.
2. `cd server`
3. `dotnet restore`
4. `dotnet tool install --global dotnet-ef` (if needed)
5. `dotnet ef migrations add InitialCreate`
6. `dotnet ef database update`
7. `dotnet run`

API will run on `http://localhost:5000` by default.

Seeded users:
- admin / Admin123!
- user / User123!

## Client

1. Ensure Node.js & Angular CLI installed
2. `cd client`
3. `npm install`
4. `ng serve`

Open `http://localhost:4200`.


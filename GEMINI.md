# Project Overview

"Forms" is a comprehensive dynamic form framework designed to bridge the gap between backend-defined models and frontend-rendered user interfaces. Built with .NET 10 and Angular 21, it enables developers to define complex form structures—including nested views, interactive grids, real-time validation, and recalculation logic—directly in C#. These definitions are served through a REST API and dynamically rendered by a responsive Angular frontend.

## Key Technologies

- **Backend:** .NET 10.0, ASP.NET Core, NSwag (OpenAPI & Client Generation)
- **Frontend:** Angular 21, TypeScript 5.9, Tailwind CSS 4, RxJS 7.8
- **Testing:** NUnit (Backend), Jasmine/Karma (Frontend)
- **Tooling:** Husky (Git hooks), Prettier, ESLint

# Project Structure

- **FormsApi/**: The core framework library. It contains the fluent `FormBuilder<TModel>` API, form definitions, repository handlers (`IRepositorySaveHandler`, etc.), and the recalculation engine.
- **FormsApi.Host/**: The ASP.NET Core web host that exposes the framework's controllers (`FormDefinitionController`, `RepositoryController`, `RecalculateEventController`). It also manages OpenAPI documentation and TypeScript client generation via NSwag.
- **Web/**: The Angular frontend application. It contains dynamic component rendering logic for various field types (text, date, currency, grids) and integrates with the backend via generated API clients.
- **Sample/**: A showcase project containing example form definitions (e.g., `TestForm.cs`) and sample services to demonstrate the framework's capabilities.
- **Tests/**: NUnit test suite for validating the backend logic, including builders, metadata, and repository resolution.

# Building and Running

### Prerequisites

- .NET 10 SDK
- Node.js (Latest LTS recommended)
- Angular CLI (`npm install -g @angular/cli`)

### Backend

1.  **Restore and Build:**
    ```bash
    dotnet restore
    dotnet build
    ```
2.  **Run the Host:**
    ```bash
    dotnet run --project FormsApi.Host/FormsApi.Host.csproj
    ```
    The API will be available at `http://localhost:5105` (as per `Web/proxy.conf.json`).

### Frontend

1.  **Install Dependencies:**
    ```bash
    cd Web
    npm install
    ```
2.  **Run Development Server:**
    ```bash
    npm start
    ```
    The application will be available at `http://localhost:4200`.

### Client Generation

The TypeScript API client is generated using NSwag. To update the client after backend changes:
```bash
cd FormsApi.Host
# Ensure the project is built first
dotnet build
nswag run
```
*(Note: Ensure `nswag` tool is installed globally or via dotnet tools if applicable).*

# Testing

### Backend Tests
```bash
dotnet test
```

### Frontend Tests
```bash
cd Web
npm test
```

# Development Conventions

### Backend
- **C# Standards:** Uses C# 13 features with `<ImplicitUsings>` and `<Nullable>` enabled.
- **Fluent Builders:** Form definitions should inherit from `FormBuilder<TModel>` and reside in the `Sample` project or a dedicated definitions library.
- **Repositories:** Data persistence is handled via repository handlers; implement `IRepositorySaveHandler<T>` and `IRepositoryQueryHandler<T>` as needed.

### Frontend
- **Strict Typing:** Leverage the generated `api.g.ts` for all backend interactions.
- **Component Pattern:** Follow the `dynamic-field` and `dynamic-view` patterns for extending form capabilities.
- **Styling:** Use Tailwind CSS 4 utility classes.

### Git Hooks
- **Husky:** Pre-commit hooks are configured to ensure code quality. Ensure you run `npm install` in the root or `Web/` directory to set them up.

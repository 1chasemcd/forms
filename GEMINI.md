# Gemini Code Assistant Context

This document provides context for the Gemini code assistant to understand the project structure, technologies, and conventions.

## Project Overview

This is a full-stack web application for dynamically generating forms. It consists of a .NET backend and an Angular frontend.

### Backend

The backend is a .NET 10 solution (`Forms.slnx`). It contains the following projects:

- **FormsApi**: A class library containing the core logic for building and representing forms. It seems to be using a builder pattern to construct form models.
- **Sample**: A .NET web application that hosts the `FormsApi` and exposes it through a RESTful API. It uses OpenAPI for API documentation.
- **Tests**: A project for unit tests.

The main API endpoint appears to be `/api/Form`, which likely returns a form definition based on a provided path.

### Frontend

The frontend is an Angular 20 application located in the `Web` directory. It is a standard Angular CLI project. It is set up with routing, but no routes are defined yet. The project uses `npm` for package management.

## Building and Running

### Backend

To run the backend, you need the .NET 10 SDK installed.

```bash
dotnet run --project Sample/Sample.csproj
```

The API will be available at `https://localhost:7123` (the port may vary). The OpenAPI documentation will be available at `https://localhost:7123/openapi`.

### Frontend

To run the frontend, you need Node.js and npm installed.

1.  Navigate to the `Web` directory:
    ```bash
    cd Web
    ```
2.  Install the dependencies:
    ```bash
    npm install
    ```
3.  Start the development server:
    ```bash
    npm start
    ```

The application will be available at `http://localhost:4200`.

## Development Conventions

### Backend

- **Code Style**: Code style is enforced on build (`<EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>`).
- **Warnings**: Warnings are treated as errors (`<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`).
- **Solution File**: The solution `Forms.slnx` uses the new text-based format, which is not tied to a specific Visual Studio version.

### Frontend

- **Linting**: ESLint is used for linting. Run `npm run lint` in the `Web` directory to check the code.
- **Formatting**: Prettier is used for code formatting.
- **Testing**: Karma and Jasmine are used for testing. Run `npm test` in the `Web` directory to run the tests.

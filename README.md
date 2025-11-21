# Introduction
This repository is a small coding exercise designed to showcase how I build full stack software. The project is broken into multiple C# projects, encouraging modularity and preparing for growth and code reuse.

The projects are:
- `TaskList.Domain` – Contains domain entities and interfaces that define the heart of the application. Buiness logic will also be placed here were the application to expand.
- `TaskList.Infrastructure` – Manages data persistence, database access, and integration with external services.
- `TaskList.WebApi` – Exposes the backend functionality via a clean, secure RESTful API layer.
- `TaskList.WebApp` – Provides the interactive front-end interface built for managing and visualizing tasks.

# Getting started
## Build and run
- Clone the repository at https://github.com/apjanes/lemon-exercise.
- Open a command prompt
- Change directory to your `lemon-exercise` repository location
- Run `run.bat`

This will build and run the application. The batch file launches two additional consoles (one for the backend and one for the frontend) where the code is built and run. This process can take a while, particularly on first build, due to the installation of NPM packages.

The command will also launch a browser which points at https://localhost:4000/. This will require a refresh once the application has built successfully.

# Usage
Once the application is running, view it in a web browser at https://localhost:4000/. On initial load you will be redirected to a Login screen where you can proceed to login. For this exercise there are two available, hardcoded logins:

```
[
  { username: "fred", password: "flintstone" },
  { username: "barney", password: "rubble" },
]
```

This is clearly not a production ready situation but is one used for ease of development and due to time constraints. In a production system of this nature, a user sign up would typically be provided with email verification and a forgotten password recovery process.

Once logged in using one of the pre-defined users, a Tasks page is shown

# Assumptions
It is assumed that the developer will have Microsoft .NET 9 and nodejs v22+. The ability to install NuGet and NPM packages is also assumed.

The availability of Visual Studio 2022 is also assumed. This may not strictly be necessary but development was conducted using it and it will make it easier to review source code using it. Alternative IDEs such as VS Code may also be used.

# Principles and choices
## Primary keys
One of the more debated decisions in EF and database design is the choice to use GUIDs as primary keys. My preference for GUIDs stems from two main advantages:

Avoiding identity conflicts. GUIDs allow data to be distributed across multiple environments or servers without risking ID collisions.

Predefining identities. Unlike integer identities, which are only available after persistence, GUIDs can be generated and known in advance, even before the entity is saved.

That said, GUIDs introduce certain challenges — particularly with indexing performance. Because standard GUIDs are randomly distributed, clustered indexes become inefficient and prone to fragmentation. SQL Server and similar systems mitigate this with NEWSEQUENTIALID(), which generates sequential GUIDs, but this prevents pre-generation and loses ordering consistency across distributed systems.

To achieve the best of both worlds, I use [COMB GUIDs](https://fastuuid.com/learn-about-uuids/comb-guids) (combined GUIDs). COMB GUIDs embed a UTC-based timestamp within otherwise random data, allowing for near-sequential ordering that supports efficient indexing while still enabling safe, distributed generation — assuming clock synchronization across systems.

## Entity Framework configuration
In EF Core, there are multiple ways to configure entities. Attributes such as `[MaxLength]` can be applied directly through data annotations, while more advanced configuration can be handled by overriding `OnModelCreating` in the `DbContext`. In many codebases, these two methods are used together.

My preference, however, is to use `IEntityTypeConfiguration` implementations. Each entity has a corresponding configuration class that encapsulates all of its mapping and rules. These configurations are then registered in the `DbContext`:

```
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfiguration(new UserConfiguration());
    modelBuilder.ApplyConfiguration(new WorkItemConfiguration());
}
```

This approach keeps configuration logic organized, ensures a clear separation of concerns, and groups related settings by entity for better maintainability.

## Warnings as errors
All projects are configured to treat compiler warnings as errors, enforcing a consistent level of code quality across the entire solution. A shared .editorconfig file defines and customizes code style and analysis rules, ensuring uniform formatting and adherence to best practices. However, certain rules are intentionally relaxed for specific contexts to balance readability and practicality. For example, the restriction on using underscores in C# identifiers is lifted for test methods, allowing the descriptive Subject_Action_Expectation naming convention that clearly communicates a test’s purpose and intent.

## Testing
As part of this exercise, several unit tests have been included. While not comprehensive, they demonstrate my approach and understanding of test design. Each code assembly has a matching test assembly, with test classes aligned directly to the components they verify. The folder structure within each test project mirrors that of the corresponding code assembly for clarity and organization.

An understanding of the role of interfaces and mocks is demonstrated, ensuring that tests remain repeatable and focused solely on the unit under test. The inclusion of database tests using an in-memory database context—with unique database names to prevent cross-test interference—further illustrates a clear approach to maintaining isolation and reliability in testing.

In a real-world application, additional tests would be implemented but were omitted here due to time constraints. The UI could be covered with Jest unit tests, integration tests could verify interactions between components, and browser automation tests could validate complete end-to-end functionality. Each of these testing layers introduces its own challenges and trade-offs - for example, managing and maintaining realistic test data that can be easily reset between runs while avoiding "test bleed" in browser automation test.

Automated testing in general presents some challenges and trade-offs like balancing test coverage against development speed, managing the maintenance cost of fragile UI or end-to-end tests, handling asynchronous behavior and state in integration scenarios, and ensuring tests remain fast, deterministic, and meaningful as the application evolves.

# Architecture
As described in the introduction, the solution is designed with two executable projects - one for the frontend (`TaskList.WebApp`) and one for the backend (`TaskList.WebApi`) as well as class libraries to keep the code modular and promote reusability should the project expand.

## TaskList.WebApp
This is the application for the frontend which provides the web server endpoint for viewing the application. This endpoint runs at https://localhost:4000/. Because the UI is written entirely in React and contained in a JavaScript bundle which is transpiled by webpack, this project is very minimal. The project simply launches a development server which serves an `index.html` file which references the `app.js` bundle from `wwwroot/dist`.

The React app is a TypeScript, React 18 single-page app wired up with Webpack, React Router v6, TanStack React Query, PrimeReact, and SCSS. It’s organized by responsibility: /pages for screens (Login, Home), /components for layout, dialogs, icons, and a ProtectedRoute, /hooks for data hooks like useWorkItems, /api for Axios clients (apiClient, auth, workItems) with a request interceptor that injects a bearer token from a minimal in-memory tokenStore and refresh logic via HttpOnly cookie, and /models for DTOs. 

Authentication is centralized in an AuthProvider (context) that exposes login/logout and guards routes; server data is fetched/cached with React Query, forms use react-hook-form, and the Home page renders work items with PrimeReact’s DataTable and modal dialogs. The app is composed in src/index.tsx where the providers and router are mounted.

## TaskList.WebApi
`TaskList.WebApi` is an ASP.NET Core 9 REST API that layers cleanly over the Domain and Infrastructure projects. It wires up EF Core (SQLite) via TaskListDbContext, repository interfaces (e.g., IUserRepository, IWorkItemRepository) through DI, and secures endpoints with JWT bearer auth plus a refresh-token flow using an HttpOnly cookie (/auth path) backed by an in-memory refresh store.

There are many improvements to be considered for a production-ready application. For example:

### Security and Authorization
Refresh tokens should use a database backed store. Token rotation can be added to prevent reuse and detection for reuse implemented. This will limit damage should a token be compromised as the tokens cannot be reused for repeated, unauthorized operations.

Fingerprinting of IP addresses and user agents will help with detection of possibly token misuse and allow mitigation of attacks.

Secrets should not be kept in appsettings but instead a KMS/Key Vault should be used. Easy key rollover, invalidation and re-issuing is needed.

Revocation lists will track compromised tokens for rejection

### Logging
A certain amount of example logging is done using the `ILogger`. This needs to be expanded with configuration for log levels and sinks.  Logs would be persisted to a log store such as a database or specific logging tools like Splunk. In a system with multiple, distributed endpoints, local logs can be aggregated for system wide tracking. Care needs to be taken to remove any sensitive system or user data when logging.

### Validation & Errors
Some error handling and contract validation has been added to this exercise. The `ErrorHandlingMiddleware` catches unhandled exceptions and returns a standard `Microsoft.AspNetCore.Mvc.ProblemDetails` response. The `FluentValidationActionFilter` validates incoming requests using validators like the `WorkItemDtoValidator`. This ensures the the input follows expected rules and returns 400 Bad Request errors are returned as problem details as defined by [RFC 7807](https://datatracker.ietf.org/doc/html/rfc7807).

Error handling and validation errors should be expanded and improved. The URLs in the type parameter point to fictitious URLs which would likely be replaced with valid values.

### Data & EF Core
In this exercise the EF Core migrations are run automatically on startup. This is not acceptable for a production system where management of this would be maintained elsewhere, usually as part of deployment and probably in a CI pipeline.

In a production application, particularly with high traffic, database concurrency needs better management. The use of timestamped version columns and concurrency checks would be benefical. The decision as to whether to practice optimistic or pessimistic concurrency management will depend on the nature of the production application. In the case of a task list, concurrency issues are low due to most lists being keyed to a restricted number of users with high levels of concurrent access being unlikely.


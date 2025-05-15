# RookiEcom - Clean Modulith RookiEcom with IdentityServer( plus .NET Identity), BackOffice(Vite React Typescripts), FrontStore(.NET MVC), and .NET WebAPI lmao

Welcome to RookiEcom! This is a full-stack e-commerce application developed as part of the Nashtech Rookies program, demonstrating a modern web application built using .NET 8, React, and following Clean Architecture principles with a Modulith approach.

## Project Overview

RookiEcom aims to provide a functional B2C e-commerce experience, featuring:

*   **Product Catalog:** Browse products, view details, filter by category.
*   **User Authentication & Authorization:** Secure login/registration via Duende IdentityServer (OpenID Connect/OAuth 2.0), distinct roles (Customer, Admin), policy-based authorization.
*   **Shopping Cart:** Add, update, remove, and clear items in the shopping cart.
*   **Product Ratings:** Authenticated users can rate and review products.
*   **Frontend Store:** Customer-facing store built with ASP.NET Core MVC & Razor.
*   **Backoffice:** Administrative interface built with React (Vite + TypeScript) for managing products and categories.
*   **Modulith Architecture:** Application divided into logical modules (Product, Cart, Identity) while potentially being deployed as a single unit initially.

## Technology Stack

*   **Backend API:** ASP.NET Core 8 Web API
*   **Frontend (Store):** ASP.NET Core 8 MVC with Razor Views
*   **Frontend (Backoffice):** React (Vite + TypeScript), React Query
*   **Authentication:** Duende IdentityServer on ASP.NET Core 8 Identity
*   **Database:** SQL Server
*   **ORM:** Entity Framework Core 8
*   **Architecture:** Clean Architecture, Modulith, CQRS (via MediatR), Repository Pattern, Unit of Work
*   **API Design:** RESTful APIs, Versioning (Asp.Versioning)
*   **Validation:** FluentValidation
*   **Other:** Bootstrap 5, Bootstrap Icons, Serilog (Implied), AutoMapper (Likely)

## Key Features & Milestones (Based on Commit History)

*   **Initial Setup:** Project structure initialization, Building Blocks setup. (Commit `26f7ded`)
*   **Identity Provider (IdP):**
    *   Setup Duende IdentityServer with ASP.NET Core Identity. (`2cfde86`)
    *   Implemented User Registration. (`1c7e365`)
    *   Configured FrontStore authentication via OIDC. (`d6bb1c0`, `#1`)
    *   Configured Backoffice authentication via OIDC. (`996e771`, `c3402c6`, `#4`)
    *   Added Admin Login functionality. (`1362b3d`)
    *   Fixed authentication state sharing issues between FrontStore/Backoffice. (`3b1a9fe`)
    *   Implemented user info API endpoints (Get Users/Profile). (`3b1a9fe`, `c00b6de`, `#15`)
    *   Configured JWT authentication and Policy-Based Authorization for WebAPI. (`5addb11`, `7344388`, `#13`)
*   **Product Module:**
    *   Implemented core CRUD operations for Products & Categories. (`e96d2e0`, `0d68bed`, `fc751d7`, `3bd8c07`, `#5`, `#6`, `#12`, `#20`)
    *   Added paginated fetching (`GetProductsPaging`). (`e96d2e0`)
    *   Added category tree fetching. (`0d68bed`)
    *   Implemented MediatR pipeline with FluentValidation. (`e96d2e0`, `ff0dac8`, `#24`)
    *   Exposed Product Module Contracts. (`3bd8c07`, `98f45d5`, `#19`)
    *   Implemented Product Ratings feature (Create, Get Paginated). (`4e025f2`, `cf6a287`, `#23`)
    *   Fixed various bugs related to queries, updates, and projections. (`6943f7f`, `a9261d0`, `#22`)
    *   Added Unit Tests for commands and queries. (`77f82b9`, `5f40c1c`, `#17`, `#24`)
*   **Cart Module:**
    *   Added initial Domain Entities and Contracts. (`3aff05c`, `#27`)
    *   Implemented Application Commands/Queries (Add, Remove, Update, Clear, Get). (`2db8ec4`, `#27`)
    *   Configured Infrastructure (Persistence, DI, UoW). (`d56882b`, `#27`)
    *   Added WebAPI Controller for Cart operations. (`d56882b`, `#27`)
*   **Backoffice (React):**
    *   Initial page setup (Product List/Create, Category List/Create). (`9abe779`, `#2`)
    *   Integrated Product and Category services using React Query. (`683d1ed`, `#7`, `#9`, `#11`, `#25`)
    *   Added/Refined CRUD operations and UI feedback (Toasts). (`c5aa101`, `359559a`, `#25`)
    *   Integrated User service calls. (`c00b6de`, `#15`)
    *   Fixed logout/state issues. (`a431e41`, `#21`)
*   **FrontStore (MVC):**
    *   Integrated Product fetching for Home, Category, and Detail pages. (`a923e5d`, `d67692b`, `cffe21a`, `5f65da2`, `#14`, `#26`)
    *   Implemented Cart integration (Cart Page UI, Add/Update/Remove/Clear logic). (`bb664a4`, `a83bcb6`, `#28`)
    *   Added Toast notifications. (`bb664a4`, `#28`)
    *   Refactored UI/CSS for responsiveness and modern look. (`bb664a4`, `#28`)
    *   Integrated User Profile page. (Implied by IdP/Auth setup)
    *   Implemented Product Rating display and submission form. (Related to `#23` and UI work)
*   **General:**
    *   Build/CI setup (Implied by workflow files if present).
    *   `.gitignore` updates. (`c521752`, `#8`)

## Getting Started

*(Add instructions here on how to build and run the project)*

1.  **Prerequisites:**
    *   .NET 8 SDK
    *   Node.js & npm/yarn (for Backoffice)
    *   SQL Server (or SQL LocalDB)
    *   Docker (Optional, for containerization)
2.  **Database Setup:**
    *   Update the connection string in `RookiEcom.WebAPI/appsettings.json`.
    *   Run Entity Framework migrations:
        ```bash
        cd src/Hosts/RookiEcom.WebAPI
        dotnet ef database update -c ProductContextImpl
        dotnet ef database update -c CartContext
        # Add command for IdentityDbContext if migrations are in WebAPI project
        # dotnet ef database update -c ApplicationDbContext
        ```
3.  **Run IdentityServer (IdP):**
    *   Navigate to `src/Hosts/RookiEcom.IdentityServer` (or wherever your IdP host is).
    *   Access the store at `https://localhost:8080` (or your configured port).
4.  **Run WebAPI:**
    *   Navigate to `src/Hosts/RookiEcom.WebAPI`.
    *   Access the store at `https://localhost:7670` (or your configured port).
5.  **Run FrontStore:**
    *   Navigate to `src/Hosts/RookiEcom.FrontStore`.
    *   Run `dotnet run`. (Ensure HTTPS profile is used).
    *   Access the store at `https://localhost:5001` (or your configured port).
6.  **Run Backoffice:**
    *   Navigate to `src/Hosts/RookiEcom.Backoffice` (or your React app folder).
    *   Run `npm install` (or `yarn install`).
    *   Run `npm run dev` (or `yarn dev`).
    *   Access the backoffice at `http://localhost:3000` (or your configured port).

## Project Structure

The solution is organized into several distinct layers and modules following Clean Architecture and Modulith principles:

*   **`src/BuildingBlocks`**: Contains foundational libraries shared across modules and hosts.
    *   `RookiEcom.Application`: Core application abstractions (e.g., CQRS interfaces, common DTOs like `PagedResult`, `IUnitOfWork`).
    *   `RookiEcom.Domain`: Core domain abstractions (e.g., `IEntity`, `IAggregateRoot`, `BaseEntity`, common Value Objects like `Address`).
    *   `RookiEcom.Infrastructure`: Shared infrastructure concerns (e.g., common EF Core configurations, Blob Service abstractions/implementations, base logging).

*   **`src/Hosts`**: Contains the runnable/deployable applications.
    *   `RookiEcom.FrontStore`: The customer-facing store built with ASP.NET Core MVC. Consumes the WebAPI.
    *   `RookiEcom.IdentityServer`: The authentication server using Duende IdentityServer and ASP.NET Core Identity.
    *   `RookiEcom.WebAPI`: The main backend API exposing business logic via REST endpoints. Integrates different modules.
    *   *(Note: `RookiEcom.Backoffice` React project might reside here or in a separate client-app folder depending on preference)*

*   **`src/Modules`**: Contains the core business logic divided into feature modules.
    *   **`Cart`**: Manages shopping cart functionality.
        *   `RookiEcom.Modules.Cart.Application`: Application logic (Commands, Queries, Handlers, Interfaces like `ICartRepository`).
        *   `RookiEcom.Modules.Cart.Contracts`: Data Transfer Objects (DTOs) specific to the Cart module exposed for API use.
        *   `RookiEcom.Modules.Cart.Domain`: Domain model (Aggregates like `Cart`, Entities like `CartItem`).
        *   `RookiEcom.Modules.Cart.Infrastructure`: Implementation details (EF Core `CartContext`, `CartRepository`, Configurations, `UnitOfWork`).
    *   **`ProductModule`**: Manages products, categories, and ratings.
        *   `RookiEcom.Modules.Product.Application`: Application logic (Commands, Queries, Handlers, Interfaces like `IProductRepository`).
        *   `RookiEcom.Modules.Product.Contracts`: DTOs specific to the Product module.
        *   `RookiEcom.Modules.Product.Domain`: Domain model (`Product`, `Category`, `ProductRating` Aggregates/Entities).
        *   `RookiEcom.Modules.Product.Infrastructure`: Implementation details (`ProductContext`, Repositories, Configurations).
        *   `Tests`: Contains tests specific to the Product module.
            *   `RookiEcom.Modules.Product.Application.UnitTests`: Unit tests for the application layer.

## Contributing

*(Add contribution guidelines if applicable)*

---

Feel free to adjust the level of detail, add specific setup nuances (like environment variables), and tailor the "Getting Started" section to your exact project configuration.

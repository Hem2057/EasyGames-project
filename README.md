# EasyGames – ASP.NET Core MVC Application

## Overview
**EasyGames** is a demo e-commerce web application built with **ASP.NET Core 9 MVC**, **Entity Framework Core (SQLite)**, and **ASP.NET Core Identity**.  
It demonstrates user authentication/authorization, catalog browsing, shopping cart, checkout, and owner-only stock/user management.

---

## Features
### Authentication & Authorization
- Register/Login using **ASP.NET Identity** (no email verification required).
- Roles:
  - **Owner** – full permissions (manage stock, manage users).
  - **User** – default role when registering.

### Catalog & Cart
- Browse products by category.
- Add items to cart, update quantities, remove items.
- Checkout page with grand total; demo checkout decrements stock and clears cart.

### Stock Management (Owner only)
- CRUD operations on stock items (Create, Edit, Delete, View Details).
- Accessible only to logged-in users with the `Owner` role.

### User Management (Owner only)
- List all users and their roles.
- Add/remove roles for users.
- Delete users.

### Database & Seeding
- SQLite database (`EasyGames.db`).
- Automatic seeding of:
  - Default **Owner** account.
  - Sample stock items (Book, Game, Toy).

---

## Test Accounts
Use the following credentials to log in:

**Owner**
- Email: `owner@easygames.local`
- Password: `Owner#123`

**User**
- Register a new account from the site.  
- Default role = `User`.

---

## Technology Stack
- **ASP.NET Core 9 MVC**
- **Entity Framework Core (SQLite)**
- **ASP.NET Core Identity (Roles + UserManager)**
- **Bootstrap 5** for UI styling

---

## Setup & Running the Project
1. **Clone or unzip the project.**
2. ### Prerequisites
- .NET 9 SDK installed ([download here](https://dotnet.microsoft.com/download/dotnet/9.0))
- SQLite (comes pre-bundled with .NET provider)

3. **Install dependencies:**
```bash
git clone https://github.com/Hem2057/EasyGames-project.git
cd EasyGames-project
dotnet restore
dotnet ef database update
dotnet run

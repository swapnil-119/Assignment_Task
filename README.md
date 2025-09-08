# Nimap ASP.NET MVC Machine Test

This is a complete ASP.NET MVC application with CRUD operations and server-side pagination, developed as per Nimap Infotech requirements.

## Features

- ✅ **Category Master** - Full CRUD operations
- ✅ **Product Master** - Full CRUD operations with category relationship
- ✅ **Server-Side Pagination** - Efficient pagination with 10 records per page
- ✅ **Required Columns** - ProductId, ProductName, CategoryId, CategoryName
- ✅ **Entity Framework Code First** approach
- ✅ **No Scaffolding** - All code written manually
- ✅ **Bootstrap UI** - Clean, responsive interface

 Technology Stack

- **Framework:** ASP.NET Core MVC (.NET 7.0)
- **Database:** SQL Server with Entity Framework Core
- **Frontend:** HTML5, CSS3, Bootstrap 5, JavaScript
- **Architecture:** Code First approach

 Database Schema

 Categories Table
- CategoryId (Primary Key, Identity)
- CategoryName (Required, Max 100 chars)

 Products Table  
- ProductId (Primary Key, Identity)
- ProductName (Required, Max 100 chars)
- CategoryId (Foreign Key to Categories)


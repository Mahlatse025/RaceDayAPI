# RaceDay Event Management System

## 1. Project Overview

RaceDay is a web-based event management system for running, walking and cycling events in South Africa.

The system allows organisers to manage events and results, while participants can browse events, join event categories and view their entries and results.

## 2. Technologies Used

- C#
- ASP.NET Core Web API
- ASP.NET Core MVC
- .NET 8
- Entity Framework Core
- SQL Server
- HTML and CSS
- Bootstrap
- GitHub Actions
- Visual Studio 2022

## 3. Main Features

### Organiser
- View the organiser dashboard.
- Create, view, update and delete events.
- View participant entries.
- Enter finishing positions and completion times.

### Participant
- View available events.
- View event categories.
- Join an event category.
- View joined events.
- View published results.

### Authentication
- Organiser and Participant roles.
- Login and session-based access.
- Role-based access to selected features.

## 4. Database

The application uses a SQL Server database named `RaceDaySystem`.

The database stores information about users, events, categories, event entries, routes and results.

To run the application, configure the database connection string in the API project's `appsettings.json` file.

## 5. How to Run the Application

Requirements:
- Visual Studio 2022
- .NET 8 SDK
- SQL Server
- SQL Server Management Studio (SSMS)

Steps:

1. Open the `RaceDayAPI` project in Visual Studio.
2. Configure the SQL Server connection string.
3. Make sure the `RaceDaySystem` database and required tables are available.
4. Build and run the API project.
5. Open the `RaceDayMVC` project in Visual Studio.
6. Check that the API address in `appsettings.json` matches the running API address.
7. Build and run the MVC project.
8. Register or log in using the available application functionality.

## 6. Automated Testing and GitHub Actions

The repository includes a test project named `RaceDayAPI.Tests`.

GitHub Actions is configured to build and test the API project when changes are pushed.

Latest recorded test results:
- Total tests: 2
- Passed: 2
- Failed: 0
- Skipped: 0

## 7. Project Structure

- `Controllers` – API controllers.
- `Data` – Database context and configuration.
- `Models` – Application models.
- `RaceDayAPI.Tests` – Automated tests.
- `RaceDayMVC` – MVC web application.

## 8. Project Limitations

Some features from the full assignment requirements, including Azure Blob Storage and Docker deployment, are not included in this submitted version.

## 9. GitHub Repository

https://github.com/Mahlatse025/RaceDayAPI


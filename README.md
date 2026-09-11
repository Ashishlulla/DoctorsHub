# DoctorsHub 🏥

> A modern healthcare management system built with **ASP.NET Core, C#, Entity Framework Core, SQL Server, and ASP.NET Core MVC**, following **Clean Architecture** and enterprise development practices.

DoctorsHub is a full-stack healthcare management application designed to simplify day-to-day operations for clinics and healthcare organizations.

The system provides centralized management of **doctors, patients, appointments, billing, departments, notifications, reports, authentication, and user access control** through a clean and responsive web interface.

---

## 📌 Project Overview

DoctorsHub was developed to demonstrate how a real-world healthcare management system can be designed using modern **.NET technologies and software architecture principles**.

The application separates business logic, data access, API functionality, and presentation concerns using a **Clean Architecture-based structure**.

### Core objectives

* Manage doctors and their information
* Manage patients and patient records
* Schedule and manage appointments
* Manage billing and payment status
* Organize doctors into departments
* Provide role-based access control
* Generate reports
* Send email notifications
* Provide dashboard insights
* Maintain a clean separation of application responsibilities

---

## ✨ Key Features

### 🔐 Authentication & Authorization

* JWT-based authentication
* JWT stored securely through authentication cookies
* Role-based authorization
* Login and logout
* Access denied handling
* Protected controllers and actions
* Configurable authentication expiration
* Role-based navigation and functionality

### 👨‍⚕️ Doctor Management

* Add doctors
* Edit doctor information
* View doctor details
* Delete doctors
* Search doctors
* Sort doctor records
* Pagination
* Department assignment
* Doctor specialization management

### 🧑‍🤝‍🧑 Patient Management

* Add patients
* Edit patient information
* View patient details
* Delete patients
* Search patients
* Sort records
* Pagination
* Patient appointment relationship

### 📅 Appointment Management

* Create appointments
* View appointment records
* Update appointment status
* Search and filter appointments
* Appointment status management
* Doctor and patient relationship
* Appointment-based notifications

### 💳 Billing Management

* Create bills
* Track billing records
* Payment status management
* Paid / Pending / Cancelled statuses
* Sort and filter billing records
* Generate billing PDFs
* Attach generated PDFs to email communication

### 🏢 Department Management

DoctorsHub supports a flexible doctor-to-department relationship.

* Create departments
* Edit departments
* Delete departments
* View doctors by department
* Filter doctors by department
* Support multiple departments for a doctor
* Keep specialization separate from department

The relationship is implemented using a many-to-many association through:

```text
Doctor
   │
   │
   ▼
DoctorDepartments
   ▲
   │
   │
Department
```

### 🔔 Notifications

The notification system provides centralized application notifications.

Supported notification information includes:

* User
* Title
* Message
* Type
* Read/unread status
* Created date
* Read date
* Appointment reference
* Bill reference

Examples:

* Appointment-related notifications
* Billing notifications
* System notifications
* User-specific notifications

### 📊 Dashboard

The dashboard provides an overview of important healthcare operations.

Dashboard information includes:

* Total doctors
* Total patients
* Appointments
* Billing information
* Operational KPIs
* Business insights
* Recent activity

### 📈 Reports

DoctorsHub includes reporting functionality for important operational data.

Reports include:

* Doctors report
* Patients report
* Appointments report
* Billing report

Reports support filtering by relevant criteria such as:

* Doctor
* Patient
* Appointment
* Billing/payment information

Export functionality is provided for report generation.

### 📧 Email Communication

DoctorsHub integrates email communication for application workflows.

Supported functionality includes:

* Test email functionality
* Appointment-related communication
* Billing-related communication
* Paid bill PDF attachment
* Password recovery emails
* MFA/OTP email verification

Email delivery is designed around an external email provider rather than hard-coding SMTP logic directly into the application.

### 🔑 Password Recovery

Users can recover their account using their registered personal email address.

The workflow supports:

1. Forgot password
2. Email verification
3. Password reset
4. Secure account recovery

### 🛡️ Multi-Factor Authentication

DoctorsHub includes optional email-based MFA.

The MFA flow uses a time-limited OTP.

Features include:

* Email OTP
* Short OTP validity period
* Profile-level MFA control
* Additional verification during authentication

---

# 🏗️ Architecture

DoctorsHub follows a **Clean Architecture** approach.

The solution is divided into separate projects based on responsibility.

```text
                    ┌──────────────────────┐
                    │     DoctorsHub.Web   │
                    │   ASP.NET Core MVC    │
                    └──────────┬───────────┘
                               │
                               ▼
                    ┌──────────────────────┐
                    │    DoctorsHub.API    │
                    │    RESTful Web API   │
                    └──────────┬───────────┘
                               │
                               ▼
                 ┌─────────────────────────────┐
                 │   DoctorsHub.Application    │
                 │                             │
                 │ Services / DTOs / Interfaces│
                 └──────────────┬──────────────┘
                                │
                                ▼
                 ┌─────────────────────────────┐
                 │     DoctorsHub.Domain       │
                 │                             │
                 │ Entities / Core Business    │
                 │ Rules / Domain Models       │
                 └─────────────────────────────┘
                                ▲
                                │
                                │
                 ┌──────────────┴──────────────┐
                 │ DoctorsHub.Infrastructure   │
                 │                             │
                 │ EF Core / SQL Server        │
                 │ Repositories / External     │
                 │ Services                    │
                 └─────────────────────────────┘
```

## Architecture Principles

The project follows:

* Separation of concerns
* Dependency inversion
* Dependency Injection
* Repository Pattern
* Service Layer
* DTO-based API communication
* SOLID principles
* Async programming
* Clean Architecture principles

The main goal is to keep business logic independent from infrastructure and presentation concerns.

---

# 🧩 Solution Structure

```text
DoctorsHub/
│
├── DoctorsHub.Domain/
│   ├── Entities/
│   ├── Enums/
│   └── ...
│
├── DoctorsHub.Application/
│   ├── DTOs/
│   ├── Interfaces/
│   ├── Services/
│   └── ...
│
├── DoctorsHub.Infrastructure/
│   ├── Data/
│   ├── Repositories/
│   ├── Services/
│   └── ...
│
├── DoctorsHub.API/
│   ├── Controllers/
│   ├── Middleware/
│   └── Program.cs
│
├── DoctorsHub.Web/
│   ├── Controllers/
│   ├── Views/
│   ├── Models/
│   ├── wwwroot/
│   └── Program.cs
│
└── DoctorsHub.sln
```

> Folder names may evolve as the project develops, but the architectural responsibility of each layer remains separated.

---

# 🛠️ Technology Stack

| Category           | Technology               |
| ------------------ | ------------------------ |
| Language           | C#                       |
| Framework          | ASP.NET Core / .NET 10   |
| Web Application    | ASP.NET Core MVC         |
| Backend            | ASP.NET Core Web API     |
| ORM                | Entity Framework Core    |
| Database           | Microsoft SQL Server     |
| Authentication     | JWT                      |
| Authorization      | Role-Based Authorization |
| Architecture       | Clean Architecture       |
| Design Principles  | SOLID                    |
| Data Access        | Repository Pattern       |
| API Documentation  | Swagger / OpenAPI        |
| PDF Generation     | QuestPDF                 |
| Report Export      | ClosedXML                |
| Email              | Brevo                    |
| Version Control    | Git                      |
| Repository Hosting | GitHub                   |
| IDE                | Visual Studio            |

---

# 👥 User Roles

DoctorsHub uses role-based authorization.

### 👑 Admin

The Admin has the highest level of application access.

Typical capabilities include:

* Manage doctors
* Manage patients
* Manage departments
* Manage appointments
* Manage billing
* Access reports
* View dashboard insights
* Manage application-level operations

### 👨‍⚕️ Doctor

Doctors have access to functionality relevant to their role.

Typical capabilities include:

* View relevant doctors
* View patients
* Manage appointments according to permissions
* View reports allowed for the role
* Receive personal notifications
* Access profile settings

### 🧑‍💼 Receptionist

Receptionists can manage day-to-day clinic operations.

Typical capabilities include:

* Patient management
* Appointment management
* Billing operations
* Relevant reports
* Notifications
* Profile settings

Access is enforced using ASP.NET Core authorization policies and role attributes.

---

# 🗄️ Database

DoctorsHub uses **Microsoft SQL Server** with **Entity Framework Core**.

Example database:

```text
DoctorsHubDb
```

Entity Framework Core is responsible for:

* Database access
* Entity mapping
* Relationships
* Migrations
* CRUD operations
* Querying
* Change tracking

---

# 🔗 Important Relationships

### Doctor ↔ Department

Many-to-many relationship:

```text
Doctor
  │
  ├──── DoctorDepartments ────┐
  │                           │
  └──── DoctorDepartments ────┘
                              │
                              ▼
                         Department
```

A doctor can belong to multiple departments, and a department can contain multiple doctors.

### Doctor ↔ Appointment

```text
Doctor
   │
   └──── Appointment
```

### Patient ↔ Appointment

```text
Patient
   │
   └──── Appointment
```

### Appointment ↔ Billing

```text
Appointment
      │
      └──── Bill
```

---

# 🔄 Application Flow

A typical request follows this structure:

```text
User
  │
  ▼
DoctorsHub.Web
  │
  ▼
Controller
  │
  ▼
Application Service
  │
  ▼
Application Interface
  │
  ▼
Infrastructure
  │
  ▼
Entity Framework Core
  │
  ▼
SQL Server
```

The response then travels back through the same layers.

This keeps controllers lightweight and prevents business logic from being tightly coupled to the database.

---

# ⚡ Async Programming

DoctorsHub uses asynchronous programming for database and service operations.

Examples:

```csharp
await service.GetDoctorsAsync();
```

```csharp
await repository.AddAsync(entity);
```

```csharp
await repository.SaveChangesAsync();
```

This helps improve application scalability and keeps I/O-bound operations non-blocking.

---

# 📋 Reporting & Export

DoctorsHub provides operational reports and export functionality.

### Report categories

* Doctors
* Patients
* Appointments
* Billing

### Export technologies

**ClosedXML**

Used for Excel-based report generation.

**QuestPDF**

Used for PDF generation, including billing documents.

---

# 🎨 User Interface

The application uses a modern administrative dashboard approach.

### UI characteristics

* Responsive layout
* Sidebar navigation
* Top navigation bar
* KPI cards
* Search
* Sorting
* Pagination
* Filters
* Data tables
* Forms
* Notifications
* Profile settings
* Light/Dark theme

The UI is designed to keep frequently used healthcare operations easy to access.

---

# 🌙 Dark Mode

DoctorsHub supports light and dark themes.

The selected theme is persisted so that the user's preference can be maintained across sessions.

---

# 📱 Responsive Design

The interface is designed to work across:

* Desktop
* Laptop
* Tablet
* Smaller screens

The goal is to maintain usable navigation and readable data tables across different screen sizes.

---

# 🔒 Security Considerations

DoctorsHub implements several security-focused practices:

* JWT authentication
* Role-based authorization
* Protected endpoints
* Cookie-based JWT handling
* Authentication expiration
* Access denied handling
* OTP-based MFA
* Password recovery through email
* Separation of application and infrastructure concerns

> Production deployments should additionally use HTTPS, secure cookie configuration, secret management, database security controls, rate limiting, logging/monitoring, and other environment-specific security hardening.

---

# ⚙️ Getting Started

## Prerequisites

Before running DoctorsHub locally, install:

* .NET 10 SDK
* SQL Server
* Visual Studio 2022 or later
* Git

Optional:

* SQL Server Management Studio
* Postman

---

## 1. Clone the Repository

```bash
git clone https://github.com/Ashishlulla/DoctorsHub.git
```

Move into the project directory:

```bash
cd DoctorsHub
```

---

## 2. Open the Solution

Open:

```text
DoctorsHub.sln
```

using Visual Studio.

---

## 3. Configure SQL Server

Update the database connection string in the appropriate configuration file.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=DoctorsHubDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Replace `YOUR_SERVER` with your local SQL Server instance.

---

## 4. Apply EF Core Migrations

From the appropriate project directory, run:

```bash
dotnet ef database update
```

If the solution uses a separate startup project, specify the required project explicitly:

```bash
dotnet ef database update \
  --project DoctorsHub.Infrastructure \
  --startup-project DoctorsHub.API
```

---

## 5. Configure Authentication

Configure the JWT settings in your application configuration.

Example:

```json
{
  "Jwt": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "DoctorsHub",
    "Audience": "DoctorsHubUsers"
  }
}
```

For production, do **not** store real secrets directly in source control.

Use:

* User Secrets
* Environment variables
* AWS Secrets Manager
* Azure Key Vault
* Another secure secret-management solution

---

## 6. Configure Email

DoctorsHub can use an external email provider for application email workflows.

Configure the required email settings through secure configuration.

Do not commit API keys or credentials to GitHub.

---

## 7. Run the Application

Build the solution:

```bash
dotnet build
```

Run the API:

```bash
dotnet run --project DoctorsHub.API
```

Run the MVC application:

```bash
dotnet run --project DoctorsHub.Web
```

Alternatively, use Visual Studio and run the configured startup project.

---

# 📖 API Documentation

DoctorsHub provides API documentation through **Swagger / OpenAPI**.

When running the API locally, open the Swagger endpoint configured by the project.

Example:

```text
https://localhost:<API-PORT>/swagger
```

Swagger can be used to:

* Explore endpoints
* Review request/response models
* Test API operations
* Understand available API routes

---

# 🧪 Testing API Endpoints

API endpoints can also be tested using tools such as:

* Swagger
* Postman

Example workflow:

```text
Login
  ↓
Receive authentication token
  ↓
Authenticate request
  ↓
Call protected API
  ↓
Receive response
```

---

# 📂 Main Modules

```text
DoctorsHub
│
├── Authentication
├── Dashboard
├── Doctors
├── Patients
├── Appointments
├── Billing
├── Departments
├── Notifications
├── Reports
├── Profile
├── MFA
├── Password Recovery
└── Settings
```

---

# 🚀 Future Enhancements

Potential future improvements include:

* Angular frontend
* Advanced appointment calendar
* Real-time notifications
* Audit logging
* Advanced analytics
* Cloud deployment
* Automated CI/CD
* Automated unit/integration testing
* Containerization
* Cloud-based file storage
* Advanced patient medical records
* Prescription management
* Online appointment booking
* Payment gateway integration
* Mobile application

---

# 📸 Screenshots

Screenshots can be added here to showcase the application.

Recommended screenshots:

```text
docs/
└── screenshots/
    ├── dashboard.png
    ├── doctors.png
    ├── patients.png
    ├── appointments.png
    ├── billing.png
    ├── departments.png
    ├── reports.png
    ├── notifications.png
    └── settings.png
```

Example:

```markdown
## Dashboard

![DoctorsHub Dashboard](docs/screenshots/dashboard.png)
```

---

# 💡 What This Project Demonstrates

DoctorsHub demonstrates practical experience with:

* C#
* ASP.NET Core
* ASP.NET Core MVC
* RESTful Web APIs
* Entity Framework Core
* SQL Server
* JWT authentication
* Role-based authorization
* Clean Architecture
* SOLID principles
* Dependency Injection
* Repository Pattern
* DTOs
* Async/Await
* Database relationships
* CRUD operations
* Search and filtering
* Pagination
* PDF generation
* Excel report generation
* Email communication
* MFA
* Password recovery
* Responsive UI
* Git/GitHub

---

# 📌 Development Practices

The project follows several practices commonly used in professional software development:

### Separation of Concerns

Each layer has a clearly defined responsibility.

### Dependency Injection

Dependencies are injected rather than tightly coupled to concrete implementations.

### Repository Pattern

Data-access operations are separated from business/application logic.

### DTOs

DTOs are used to control the data exchanged between application layers and APIs.

### SOLID

The project applies SOLID principles to improve:

* Maintainability
* Testability
* Extensibility
* Code organization

### Async/Await

I/O-bound operations use asynchronous programming wherever appropriate.

---

# 🌐 Deployment

DoctorsHub is designed to be deployable to environments such as:

* IIS
* AWS
* Azure

A production deployment should include:

* HTTPS
* Production SQL Server
* Secure configuration
* Environment-specific settings
* Proper secret management
* Logging and monitoring
* Database backup strategy

---

# 📝 Project Status

**Status:** ✅ Development Complete / Portfolio Ready

DoctorsHub has been developed as a full-stack .NET healthcare management project demonstrating real-world application architecture, authentication, authorization, CRUD workflows, reporting, billing, notifications, and external service integration.

---

# 👨‍💻 Author

**Ashish Lulla**

Full Stack .NET Developer

### Technologies

```text
C# | ASP.NET Core | Web API | MVC | EF Core | SQL Server
Clean Architecture | REST API | JWT | Angular
```

### GitHub

**DoctorsHub — Source Code**

https://github.com/Ashishlulla/DoctorsHub

---

# ⭐ Support

If you find this project useful or interesting, consider giving the repository a ⭐ on GitHub.

---

## 📄 License

This project is intended primarily as a portfolio and learning project.

Add an explicit open-source license such as MIT if you want others to legally reuse and modify the source code.

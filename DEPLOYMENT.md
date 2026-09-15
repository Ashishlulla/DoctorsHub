# DoctorsHub Deployment Guide

This document describes the production deployment of **DoctorsHub** on Windows using **IIS, ASP.NET Core/.NET 10, and SQL Server 2025**.

The deployment uses two IIS websites:

* **DoctorsHub Web** — ASP.NET Core MVC application
* **DoctorsHub API** — ASP.NET Core Web API

SQL Server and IIS are hosted on the same Windows machine.

---

## 1. Deployment Architecture

```text
                    ┌──────────────────────────┐
                    │        User / Browser     │
                    └────────────┬─────────────┘
                                 │
                                 │ HTTP :8080
                                 ▼
                    ┌──────────────────────────┐
                    │     DoctorsHub Web        │
                    │      ASP.NET Core MVC     │
                    │      IIS / App Pool       │
                    └────────────┬─────────────┘
                                 │
                                 │ HTTP :7103
                                 ▼
                    ┌──────────────────────────┐
                    │     DoctorsHub API        │
                    │      ASP.NET Core API     │
                    │      IIS / App Pool       │
                    └────────────┬─────────────┘
                                 │
                                 │ SQL Server :1433
                                 ▼
                    ┌──────────────────────────┐
                    │       DoctorsHubDb       │
                    │       SQL Server 2025     │
                    └──────────────────────────┘

                    API
                     │
                     │ HTTPS
                     ▼
                    Brevo SMTP API
```

### Production endpoints

| Component         | URL / Port                 |
| ----------------- | -------------------------- |
| Web Application   | `http://localhost:8080`    |
| Web IIS Site      | `DoctorsHub`               |
| Web App Pool      | `DoctorsHubAppPool`        |
| Web Physical Path | `C:\inetpub\DoctorsHub`    |
| API               | `http://localhost:7103`    |
| API IIS Site      | `DoctorsHubAPI`            |
| API App Pool      | `DoctorsHubAPIAppPool`     |
| API Physical Path | `C:\inetpub\DoctorsHubAPI` |
| SQL Server        | `localhost:1433`           |
| Database          | `DoctorsHubDb`             |

---

# 2. Prerequisites

The deployment machine should have:

* Windows Server or Windows 10/11
* IIS
* .NET 10 ASP.NET Core Hosting Bundle
* .NET 10 SDK
* SQL Server 2025
* SQL Server Management Studio (SSMS)
* Git
* Internet access for Brevo email API

Verify .NET:

```powershell
dotnet --info
```

Verify IIS:

```powershell
Get-Service W3SVC
```

Verify SQL Server:

```powershell
Get-Service MSSQLSERVER
```

---

# 3. Install and Configure SQL Server

DoctorsHub uses SQL Server instead of LocalDB for production.

The production SQL Server instance is:

```text
MSSQLSERVER
```

The SQL Server TCP port is:

```text
1433
```

## 3.1 Enable TCP/IP

Open:

```text
SQL Server Configuration Manager
```

Navigate to:

```text
SQL Server Network Configuration
    → Protocols for MSSQLSERVER
```

Enable:

```text
TCP/IP
```

Open TCP/IP properties and configure the TCP port as:

```text
1433
```

Restart SQL Server after changing the configuration.

Verify:

```powershell
netstat -ano | findstr :1433
```

The port should show as listening.

Test the connection:

```powershell
Test-NetConnection localhost -Port 1433
```

Expected:

```text
TcpTestSucceeded : True
```

---

# 4. Restore DoctorsHub Database

The original development database was hosted in:

```text
(localdb)\MSSQLLocalDB
```

Before production deployment, create a SQL Server backup.

Example backup:

```text
DoctorsHubDb.bak
```

Copy the backup to the SQL Server backup directory:

```text
C:\Program Files\Microsoft SQL Server\MSSQL17.MSSQLSERVER\MSSQL\Backup\
```

Restore the database using SSMS.

The resulting production database should be:

```text
DoctorsHubDb
```

Verify that the database is online:

```sql
SELECT
    name,
    state_desc
FROM sys.databases
WHERE name = 'DoctorsHubDb';
```

Expected:

```text
DoctorsHubDb    ONLINE
```

---

# 5. Production Connection String

Both the Web application and API use SQL Server.

Production connection string:

```json
"ConnectionStrings": {
  "Default": "Server=localhost;Database=DoctorsHubDb;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
}
```

The application retrieves the connection string using:

```csharp
configuration.GetConnectionString("Default")
```

Do not use the LocalDB connection string in production.

Development:

```text
(localdb)\MSSQLLocalDB
```

Production:

```text
localhost:1433
```

---

# 6. IIS SQL Server Permissions

DoctorsHub uses Windows authentication for SQL Server.

The Web application runs using:

```text
IIS AppPool\DoctorsHubAppPool
```

The API runs using:

```text
IIS AppPool\DoctorsHubAPIAppPool
```

Create SQL Server logins for both application pools.

## 6.1 Web application login

```sql
CREATE LOGIN [IIS APPPOOL\DoctorsHubAppPool]
FROM WINDOWS;
```

Create the database user:

```sql
USE DoctorsHubDb;
GO

CREATE USER [IIS APPPOOL\DoctorsHubAppPool]
FOR LOGIN [IIS APPPOOL\DoctorsHubAppPool];
GO
```

Grant application data access:

```sql
ALTER ROLE db_datareader
ADD MEMBER [IIS APPPOOL\DoctorsHubAppPool];
GO

ALTER ROLE db_datawriter
ADD MEMBER [IIS APPPOOL\DoctorsHubAppPool];
GO
```

## 6.2 API login

```sql
CREATE LOGIN [IIS APPPOOL\DoctorsHubAPIAppPool]
FROM WINDOWS;
```

Create the database user:

```sql
USE DoctorsHubDb;
GO

CREATE USER [IIS APPPOOL\DoctorsHubAPIAppPool]
FOR LOGIN [IIS APPPOOL\DoctorsHubAPIAppPool];
GO
```

Grant application data access:

```sql
ALTER ROLE db_datareader
ADD MEMBER [IIS APPPOOL\DoctorsHubAPIAppPool];
GO

ALTER ROLE db_datawriter
ADD MEMBER [IIS APPPOOL\DoctorsHubAPIAppPool];
GO
```

---

# 7. Build and Publish the API

Navigate to the API project:

```powershell
cd "C:\Users\Admin\source\repos\Ashishlulla\DoctorsHub\DoctorsHub.API"
```

Publish the API:

```powershell
dotnet publish "DoctorsHub.API.csproj" -c Release -o "C:\inetpub\DoctorsHubAPI"
```

The published application should contain:

```text
C:\inetpub\DoctorsHubAPI
```

including:

```text
DoctorsHub.API.dll
DoctorsHub.API.exe
appsettings.json
web.config
```

---

# 8. Configure API IIS App Pool

Create the API application pool:

```powershell
New-WebAppPool -Name "DoctorsHubAPIAppPool"
```

Configure it for ASP.NET Core:

```powershell
Set-ItemProperty "IIS:\AppPools\DoctorsHubAPIAppPool" `
    -Name "managedRuntimeVersion" `
    -Value ""
```

Configure the pipeline:

```powershell
Set-ItemProperty "IIS:\AppPools\DoctorsHubAPIAppPool" `
    -Name "managedPipelineMode" `
    -Value "Integrated"
```

The application pool should use:

```text
.NET CLR Version: No Managed Code
Managed Pipeline Mode: Integrated
Identity: ApplicationPoolIdentity
```

---

# 9. API Folder Permissions

Grant the API App Pool read/execute permissions:

```powershell
icacls "C:\inetpub\DoctorsHubAPI" `
    /grant "IIS AppPool\DoctorsHubAPIAppPool:(OI)(CI)RX" `
    /T
```

If the application requires a writable directory, such as logging, grant Modify only to that specific directory instead of the entire application folder.

---

# 10. Create the API IIS Website

Create the API website:

```powershell
New-Website -Name "DoctorsHubAPI" `
    -PhysicalPath "C:\inetpub\DoctorsHubAPI" `
    -Port 7103 `
    -ApplicationPool "DoctorsHubAPIAppPool"
```

Start the application pool:

```powershell
Start-WebAppPool -Name "DoctorsHubAPIAppPool"
```

Start the website:

```powershell
Start-Website -Name "DoctorsHubAPI"
```

---

# 11. Verify the API

The API does not expose a root page, so requesting:

```text
http://localhost:7103
```

may return:

```text
404 Not Found
```

This does **not** necessarily mean the API is broken.

A controller endpoint should be tested instead.

For example:

```powershell
Invoke-WebRequest `
    "http://localhost:7103/api/Auth/login" `
    -Method POST `
    -ContentType "application/json" `
    -Body "{}" `
    -UseBasicParsing
```

A `400 Bad Request` response indicates that the API route is reachable and processing the request.

Swagger is enabled only for the Development environment, so:

```text
http://localhost:7103/swagger
```

is not expected to be available in production.

---

# 12. API Production Configuration

The deployed API configuration is located at:

```text
C:\inetpub\DoctorsHubAPI\appsettings.json
```

The production SQL Server connection should point to:

```text
Server=localhost
Database=DoctorsHubDb
```

The API also contains JWT configuration.

Example:

```json
"Jwt": {
  "Key": "<PRODUCTION_JWT_SECRET>",
  "Issuer": "DoctorsHub.API",
  "Audience": "DoctorsHub.Web",
  "ExpiresInMinutes": 60
}
```

### Security

Never commit:

* Production JWT secrets
* Brevo API keys
* Passwords
* Database passwords
* Other credentials

The actual Brevo API key used by the deployed application must remain outside Git source control.

After changing production configuration, restart the API App Pool:

```powershell
Restart-WebAppPool -Name "DoctorsHubAPIAppPool"
```

---

# 13. Brevo Email Configuration

DoctorsHub uses Brevo for transactional email.

The API configuration contains:

```json
"Brevo": {
  "ApiKey": "<YOUR_BREVO_API_KEY>",
  "SenderEmail": "<YOUR_SENDER_EMAIL>",
  "SenderName": "DoctorsHub"
}
```

Replace the placeholders with the actual production values on the server.

**Do not commit the real API key to GitHub.**

After configuring the API key:

```powershell
Restart-WebAppPool -Name "DoctorsHubAPIAppPool"
```

Test an email-producing workflow such as appointment confirmation.

---

# 14. Build and Publish the Web Application

Navigate to the Web project:

```powershell
cd "C:\Users\Admin\source\repos\Ashishlulla\DoctorsHub\DoctorsHub"
```

Publish the application:

```powershell
dotnet publish "DoctorsHub.Web.csproj" -c Release -o "C:\inetpub\DoctorsHub"
```

The published Web application should be located at:

```text
C:\inetpub\DoctorsHub
```

---

# 15. Configure Web IIS App Pool

The Web application pool is:

```text
DoctorsHubAppPool
```

The application pool should use:

```text
.NET CLR Version: No Managed Code
Managed Pipeline Mode: Integrated
Identity: ApplicationPoolIdentity
```

---

# 16. Web Folder Permissions

Grant the Web App Pool read/execute access:

```powershell
icacls "C:\inetpub\DoctorsHub" `
    /grant "IIS AppPool\DoctorsHubAppPool:(OI)(CI)RX" `
    /T
```

Grant Modify permissions only where the application specifically requires write access.

---

# 17. Create the Web IIS Website

Create the website using port `8080`:

```powershell
New-Website -Name "DoctorsHub" `
    -PhysicalPath "C:\inetpub\DoctorsHub" `
    -Port 8080 `
    -ApplicationPool "DoctorsHubAppPool"
```

Start the application pool:

```powershell
Start-WebAppPool -Name "DoctorsHubAppPool"
```

Start the website:

```powershell
Start-Website -Name "DoctorsHub"
```

The application should now be accessible at:

```text
http://localhost:8080
```

---

# 18. Web → API Configuration

The Web application communicates with the API using the configured API base URL.

Production configuration:

```json
"MyAPI": {
  "BaseUrl": "http://localhost:7103/"
}
```

The Web application should not use the development API URL:

```text
https://localhost:7103/
```

when the production IIS API is configured for HTTP on port `7103`.

---

# 19. IIS Web.config

ASP.NET Core applications deployed to IIS use the ASP.NET Core Module.

A typical production `web.config` contains:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore"
             path="*"
             verb="*"
             modules="AspNetCoreModuleV2"
             resourceType="Unspecified" />
      </handlers>

      <aspNetCore
        processPath="dotnet"
        arguments=".\DoctorsHub.API.dll"
        stdoutLogEnabled="false"
        stdoutLogFile=".\logs\stdout"
        hostingModel="inprocess" />
    </system.webServer>
  </location>
</configuration>
```

The Web application's generated `web.config` should similarly have:

```text
stdoutLogEnabled="false"
```

in normal production operation.

---

# 20. Production Logging

ASP.NET Core stdout logging can be temporarily enabled for diagnosing IIS startup failures.

Example:

```xml
stdoutLogEnabled="true"
```

If enabled:

1. Create the logs directory.
2. Grant the appropriate App Pool permissions.
3. Reproduce the error.
4. Read the generated stdout log.
5. Disable stdout logging again.

Production configuration should normally use:

```xml
stdoutLogEnabled="false"
```

Do not leave verbose stdout logging enabled permanently.

---

# 21. Git Configuration

Deployment output should not be committed to the repository.

The repository `.gitignore` contains:

```gitignore
# Build folders
bin/
obj/

# Visual Studio files
.vs/
*.user
*.suo

# Logs
*.log

# OS files
.DS_Store
Thumbs.db

# Test results
TestResults/

# Sql files
.sql

# Publish output
publish/
```

Published IIS files should remain outside the Git working tree when possible.

---

# 22. Production Smoke Test

After deployment, verify the following.

## Authentication

* Open the Web application.
* Login as an Admin.
* Login as a Doctor.
* Login as a Receptionist.
* Verify role-based authorization.
* Verify unauthorized users are redirected to Access Denied.

## Doctors

* View doctors.
* Add a doctor as Admin.
* Edit a doctor.
* Verify department assignment.

## Patients

* View patients.
* Create/update patient information.

## Appointments

Test the complete workflow:

```text
Create
   ↓
Confirm
   ↓
Complete
   ↓
Generate Bill
   ↓
Mark Paid
```

## Billing

Verify:

* Paid bills
* Pending bills
* Cancelled bills
* Bill PDF generation

## Notifications

Verify:

* Appointment notifications
* Completion notifications
* Doctor notifications
* Receptionist/Admin notifications
* Read/unread state

## Email

Verify that transactional emails are delivered successfully through Brevo.

---

# 23. Verify IIS Services

List application pools:

```powershell
Get-WebAppPoolState -Name "DoctorsHubAppPool"
Get-WebAppPoolState -Name "DoctorsHubAPIAppPool"
```

Both should show:

```text
Started
```

List websites:

```powershell
Get-Website
```

Verify:

```text
DoctorsHub
DoctorsHubAPI
```

are started.

---

# 24. Verify Network Ports

Check Web:

```powershell
Test-NetConnection localhost -Port 8080
```

Check API:

```powershell
Test-NetConnection localhost -Port 7103
```

Check SQL Server:

```powershell
Test-NetConnection localhost -Port 1433
```

All required ports should report:

```text
TcpTestSucceeded : True
```

---

# 25. Troubleshooting

## HTTP Error 500.30

Typical meaning:

```text
ASP.NET Core app failed to start
```

Check:

1. IIS App Pool is started.
2. .NET 10 Hosting Bundle is installed.
3. Published files exist.
4. `web.config` is correct.
5. SQL Server is running.
6. Database is accessible.
7. Connection string is correct.

Temporarily enable stdout logging to diagnose startup failures.

After diagnosis, disable it again.

---

## SQL Connection Errors

Verify SQL Server:

```powershell
Get-Service MSSQLSERVER
```

Verify port:

```powershell
Test-NetConnection localhost -Port 1433
```

Verify database:

```sql
SELECT name, state_desc
FROM sys.databases
WHERE name = 'DoctorsHubDb';
```

Verify IIS SQL permissions.

Web:

```text
IIS APPPOOL\DoctorsHubAppPool
```

API:

```text
IIS APPPOOL\DoctorsHubAPIAppPool
```

---

## Brevo HTTP 401

A Brevo response of:

```text
401 Unauthorized
```

usually indicates an authentication/API-key problem.

Check:

* Brevo API key exists in the production configuration.
* The API key is valid.
* The key was not accidentally removed.
* The correct configuration file is being loaded.

Restart:

```powershell
Restart-WebAppPool -Name "DoctorsHubAPIAppPool"
```

Do not commit the API key to Git.

---

## API Returns 404 at `/`

The API may not have a root endpoint.

For example:

```text
http://localhost:7103
```

returning `404` does not necessarily indicate a deployment failure.

Test an actual API endpoint instead.

---

## Swagger Returns 404

Swagger is configured for Development only.

Therefore Swagger may not be available when the IIS application is running in Production.

This is expected behavior.

---

## Web Cannot Reach API

Verify the Web configuration:

```json
"MyAPI": {
  "BaseUrl": "http://localhost:7103/"
}
```

Verify API IIS website:

```powershell
Get-Website -Name "DoctorsHubAPI"
```

Verify API port:

```powershell
Test-NetConnection localhost -Port 7103
```

Restart the Web App Pool if configuration was changed:

```powershell
Restart-WebAppPool -Name "DoctorsHubAppPool"
```

---

## Port Already in Use

Check the port:

```powershell
netstat -ano | findstr :8080
```

or:

```powershell
netstat -ano | findstr :7103
```

For SQL Server:

```powershell
netstat -ano | findstr :1433
```

---

# 26. Updating the Application

When deploying a new version:

## 1. Build and test locally

```powershell
dotnet build
```

Run the application and verify the required functionality.

## 2. Publish the API

```powershell
dotnet publish "DoctorsHub.API.csproj" `
    -c Release `
    -o "C:\inetpub\DoctorsHubAPI"
```

## 3. Publish the Web application

```powershell
dotnet publish "DoctorsHub.Web.csproj" `
    -c Release `
    -o "C:\inetpub\DoctorsHub"
```

## 4. Restart IIS application pools

```powershell
Restart-WebAppPool -Name "DoctorsHubAPIAppPool"
Restart-WebAppPool -Name "DoctorsHubAppPool"
```

## 5. Run the production smoke test

Verify:

```text
Login
→ Appointment
→ Confirmation
→ Completion
→ Billing
→ Notification
→ Email
```

---

# 27. Deployment Security Checklist

Before considering a deployment production-ready:

* [x] SQL Server is used instead of LocalDB.
* [x] SQL Server TCP/IP is configured.
* [x] SQL Server port is verified.
* [x] Database is restored successfully.
* [x] IIS App Pool identities have database access.
* [x] IIS App Pools use `No Managed Code`.
* [x] ApplicationPoolIdentity is used.
* [x] Publish output is excluded from Git.
* [x] Production Brevo API key is not committed.
* [x] Production JWT secret is not documented in this file.
* [x] stdout logging is disabled after troubleshooting.
* [x] API and Web applications are hosted separately.
* [x] Web-to-API communication is configured.
* [x] Authentication works.
* [x] Role-based authorization works.
* [x] Appointment workflow works.
* [x] Billing workflow works.
* [x] Notifications work.
* [x] Transactional email works.

---

# 28. Production Deployment Summary

DoctorsHub production consists of:

```text
DoctorsHub Web
    IIS
    Port 8080
    C:\inetpub\DoctorsHub
    DoctorsHubAppPool

        ↓

DoctorsHub API
    IIS
    Port 7103
    C:\inetpub\DoctorsHubAPI
    DoctorsHubAPIAppPool

        ↓

SQL Server 2025
    MSSQLSERVER
    Port 1433
    DoctorsHubDb
```

The deployment uses **IIS + ASP.NET Core + SQL Server** and does not require Docker.

Production secrets such as the Brevo API key and JWT signing key must remain outside source control.

---

## Deployment Status

**DoctorsHub production deployment completed successfully.**

Verified production functionality includes:

* Authentication
* Role-based authorization
* SQL Server connectivity
* IIS Web application
* IIS API
* Appointment creation
* Appointment confirmation
* Appointment completion
* Billing
* Payment status
* Notifications
* Transactional email
* PDF generation
* Reports
* Dashboard functionality
* Production Web → API communication

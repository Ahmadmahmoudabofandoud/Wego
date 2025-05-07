# Stay Safe — Hotel Booking & Secure Payment System  

## Overview  
Secure hotel booking platform with encrypted data storage using **ASP.NET Core (.NET 8)** and **FastAPI** microservice for AI-based encryption.  

## Key Features  

### 🔒 Security  
- End-to-end encryption for sensitive data (passport, payment info, etc.)  
- JWT authentication & role-based access  

### 🛎️ Booking System  
- Room availability checks  
- Encrypted payment processing  

### 🌍 Discovery  
- Hotel/search with GPS-based recommendations  

## Tech Stack  
- Backend: ASP.NET Core (.NET 8)  
- Database: Entity Framework Core + SQL Server  
- Encryption: FastAPI microservice  
- Docs: Swagger/OpenAPI  

## Setup  
```bash
git clone https://github.com/your-org/stay-safe.git
cd stay-safe
dotnet ef database update
dotnet run

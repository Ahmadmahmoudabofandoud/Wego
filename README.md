# Stay Safe — Hotel Booking & Secure Payment System  

## Overview  
Secure hotel booking platform with encrypted data storage using **ASP.NET Core (.NET 8)** and **FastAPI** microservice for AI‑based encryption.  

## Key Features  

### 🔒 Security  
- End‑to‑end encryption for sensitive data (passport, payment info, personal fields)  
- JWT authentication & role‑based access control  
- Password reset via email  

### 🛎️ Booking System  
- Room availability checks, booking creation & modification  
- Encrypted payment processing & secure transaction reports  
- Cancellation and booking history retrieval  

### 👤 Profile Management  
- Secure CRUD operations on user profiles  
- AI‑powered encryption/decryption of profile data  
- Profile image upload and management  

### 🏨 Content Management  
- Hotels with amenities, room options & images  
- Locations with airports & attractions  
- User reviews and ratings  

### 🌍 Discovery & Search  
- GPS‑based nearby recommendations  
- Popular hotels and attractions  
- Filtering, sorting & pagination  

### 🔧 Infrastructure & UX  
- Onion Architecture & Repository Pattern (Unit of Work)  
- AutoMapper & Specification Pattern for flexible mapping & querying  
- Email service integration (SMTP)  
- Image upload/storage  
- Swagger / OpenAPI documentation  

## Tech Stack  
- **Backend:** ASP.NET Core (.NET 8)  
- **Database:** Entity Framework Core + SQL Server  
- **Encryption:** FastAPI microservice (AI‑based)  
- **Mapping:** AutoMapper  
- **Auth:** JWT, Identity  
- **Docs:** Swagger / OpenAPI  

## Setup  
```bash
git clone https://github.com/your-org/stay-safe.git
cd stay-safe
dotnet ef database update
dotnet run

# 🪙 Crypto Exchange API (.NET)

This project implements a **crypto-exchange engine** that helps users buy or sell Bitcoin (BTC) across multiple exchanges at the **best available price**, taking into account each exchange's order book and balance.

---

## 🚀 Features

- ⚡ Aggregate order books from multiple exchanges  
- 📈 Smart execution engine: buys low / sells high  
- 🔒 Balance-aware matching (no inter-exchange transfers)  
- 🧪 Unit tests for core logic  
- 🗃️ JSON-based seed data (for simplicity)  
- ♻️ Clean, testable, layered architecture (Domain, Infrastructure, Application, API)  

---

## 🧰 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- (Optional) [Postman](https://www.postman.com/) or just use the built-in Swagger UI

---

## 📦 Getting Started

### 1. Clone the repository
git clone (https://github.com/ReziKart/CryptoExchangeApi)
cd CryptoExchangeApi

### 2. Restore dependencies

Use the following command to restore all NuGet packages:
dotnet restore

### 3. Restore dependencies

Use the following command to build the solution:
dotnet build
### 4. Run the project

Use the following command to run the project:
dotnet run --project CryptoExchange.Api

It opens Swagger UI — an interactive interface to try out the API.


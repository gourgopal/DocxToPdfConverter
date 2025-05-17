# DOCX to PDF Converter using Microsoft Graph API

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![Static Badge](https://img.shields.io/badge/build-passing-brightgreen)

A robust C# console application that converts DOCX files to PDF using Microsoft Graph API, following SOLID principles and modern .NET practices.

## Features

- 🔐 Secure authentication with Microsoft Graph
- ⏱️ Conversion time tracking
- 🗑️ Automatic cleanup of temporary files
- 🏗️ SOLID architecture with dependency injection
- 📁 Supports both personal and organizational Microsoft accounts

## Prerequisites

- [.NET 8/9 SDK](https://dotnet.microsoft.com/download)
- [Microsoft 365 Account](https://www.microsoft.com/microsoft-365)
- [Azure App Registration](https://portal.azure.com/)

## Getting Started

### 1. Clone the Repository
```git clone https://github.com/yourusername/DocxToPdfConverter.git
cd DocxToPdfConverter
```

### 2. Install Dependencies
```
dotnet restore
```

### 3. Azure App Registration
1. Navigate to [Azure Portal](https://portal.azure.com/)
2. Create a new app registration with these settings:
   - **Supported account types**: Personal Microsoft accounts
   - **Redirect URI**: `http://localhost` (For Desktop or Mobile App)
   - Collect your client id (need to add it to appsettings.json)
3. Add API permissions:
   - `Files.ReadWrite.All` (Delegated)
   - `User.Read` (Delegated)

### 4. Configuration
Create `appsettings.json` in the project root (Make sure to Copy it to output directory):
```json
{
  "graph": {
    "ClientId": "YOUR_CLIENT_ID"
  }
}
```

## Usage

Example:
`dotnet run -- "C:\Documents\contract.docx" "C:\Exports\contract.pdf"`

## Project Structure
```
DocxToPdfConverter/
├── Services/ # Core implementation
├── Interfaces/ # Abstraction layer
├── Models/ # Data transfer objects
├── appsettings.json # Configuration
└── Program.cs # Entry point
```

## Authentication Flow
```sequenceDiagram
User->>+Azure AD: Initiate login
Azure AD->>+User: Provide credentials
Azure AD->>
```

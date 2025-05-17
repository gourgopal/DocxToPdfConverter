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

# DOCX to PDF Conversion: Limitations & Enterprise Solutions
## Microsoft Graph API Limitations
```graph TD
    A[Graph API Timeout] --> B[45-60 seconds]
    B --> C[File Complexity]
    C --> D[Images/Tables]
    C --> E[Large File Size]
    B --> F[Server Load]
```
### Key Constraints:
- Timeout: 45-60 seconds for complex documents
- File Size: Max 250MB (practical limit <50MB for reliable conversion)
- Throughput: ~10 requests/minute (organizational accounts)

## Alternative Solutions Matrix
|          Solution         |    Cost   | Timeout | Scalability | Setup Complexity |
|:-------------------------:|:---------:|:-------:|:-----------:|:----------------:|
| LibreOffice Headless      | Free      | None    | High        | Medium           |
| Azure Functions + PDFTron | $$        | 10min   | High        | High             |
| Google Docs API           | $0.05/doc | 60s     | Medium      | Low              |
| Aspose.Words Cloud        | $0.15/doc | None    | High        | Low              |

## Recommended Implementation: LibreOffice in Azure Container
```
[Azure Queue Storage] → [Azure Function] → [LibreOffice Container] → [Blob Storage]
```
### Sample Cost Comparison (Monthly):
|   Solution  | 72,000 docs | Reliability |  Support  | Setup Complexity |
|:-----------:|:-----------:|:-----------:|:---------:|:----------------:|
| LibreOffice | $300        | Medium      | Community | Medium           |
| Aspose      | $10,800     | High        | 24/7      | High             |
| Adobe       | $3,600      | High        | Business  | Low              |

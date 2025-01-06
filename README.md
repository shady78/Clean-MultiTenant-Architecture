# Clean-MultiTenant-Architecture

A sophisticated multi-tenant API solution implementing a hybrid approach for tenant data segregation using Clean Architecture principles.

## 🌟 Features
- Clean Architecture implementation
- Multiple database provider support (SQL Server, PostgreSQL)
- Per-tenant data isolation
- Automatic tenant database creation and migration
- Flexible tenant configuration
- Entity Framework Core integration
- Global query filtering for tenant data


## 🏗️ Architecture
The solution follows Clean Architecture with three main projects:
- **Core**: Contains business entities, interfaces, and contracts
- **Infrastructure**: Implements data access and services
- **API**: Handles HTTP requests and tenant identification

  ## 🔧 Technical Stack
- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server / PostgreSQL
- Clean Architecture
- Repository Pattern
- Dependency Injection

  
## 📦 Prerequisites
- .NET 8.0 SDK
- SQL Server or PostgreSQL
- Visual Studio 2022 or VS Code

  
### Hybrid Approach Implementation
This solution implements a hybrid multi-tenant architecture that combines the benefits of both dedicated and shared database approaches:

1. **Dedicated Databases**
   - Tenants can have their own dedicated database
   - Complete data isolation
   - Custom database configurations
   - Example: `alphaTenantDb`, `betaTenantDb`

2. **Shared Database**
   - Multiple tenants can share a single database
   - Cost-effective for smaller tenants
   - Automatic tenant filtering
   - Example: `sharedTenantDb`

3. **Hybrid Benefits**
   - Flexibility in tenant configuration
   - Cost optimization
   - Resource efficiency
   - Scalability per tenant needs

📦 Configuration Examples
```
Tenant Settings
{
  "TenantSettings": {
    "Defaults": {
      "DBProvider": "mssql",
      "ConnectionString": "Data Source=localhost;Initial Catalog=sharedTenantDb;..."
    },
    "Tenants": [
      {
        "Name": "alpha",
        "TID": "alpha",
        "ConnectionString": "Data Source=localhost;Initial Catalog=alphaTenantDb;..."
      },
      {
        "Name": "beta",
        "TID": "beta",
        "ConnectionString": "Data Source=localhost;Initial Catalog=betaTenantDb;..."
      },
      {
        "Name": "charlie",
        "TID": "charlie"
      }
    ]
  }
}
```
### Tenant Identification
The solution uses request headers for tenant identification:

1. **Request Header Implementation**
```csharp
public class TenantService : ITenantService
{
    private readonly TenantSettings _tenantSettings;
    private HttpContext _httpContext;
    
    public TenantService(IOptions<TenantSettings> tenantSettings, 
        IHttpContextAccessor contextAccessor)
    {
        _tenantSettings = tenantSettings.Value;
        _httpContext = contextAccessor.HttpContext;
        if (_httpContext != null)
        {
            if (_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
            {
                SetTenant(tenantId);
            }
        }
    }
}


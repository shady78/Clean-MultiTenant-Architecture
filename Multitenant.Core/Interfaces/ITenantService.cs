using Multitenant.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multitenant.Core.Interfaces;
public interface ITenantService
{
     string? GetDatabaseProvider();
     string? GetConnectionString();
     Tenant? GetTenant();
}
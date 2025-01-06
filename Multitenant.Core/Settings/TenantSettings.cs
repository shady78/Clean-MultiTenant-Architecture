using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multitenant.Core.Settings;
//Tenant Setting will also have a definition for the List of Tenants that are supposed to have access to the system.
public class TenantSettings
{
    public Configuration Defaults { get; set; }
    public List<Tenant> Tenants { get; set; }
}
// In cases where the tenant needs to use the shared database, the idea is to leave the Connection string of the tenant blank.
public class Tenant
{
    public string? Name { get; set; }
    public string? TID { get; set; }
    public string? ConnectionString { get; set; }
}
//Default Configuration which includes the DBProvider (MSSQL will be used in or PostgreSql), and a Connection String to the default shared database
public class Configuration
{
    public string DBProvider { get; set; }
    public string ConnectionString { get; set; }
}
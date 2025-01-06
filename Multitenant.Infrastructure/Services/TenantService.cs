using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Multitenant.Core.Interfaces;
using Multitenant.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Multitenant.Infrastructure.Services;
// Create Infrastructure/Services/TenantService.cs
public class TenantService : ITenantService
{
    private readonly TenantSettings _tenantSettings;
    // to read value in header 
    private HttpContext _httpContext;
    private Tenant _currentTenant;
    // We first check if HTTP context is not null, then we try to read the tenant key from the header of the request.
    // If a tenant value is found, we set the tenant using the SetTenant(string tenantId) method.
    // I will use header to deal with tenant 
    public TenantService(IOptions<TenantSettings> tenantSettings, IHttpContextAccessor contextAccessor)
    {
        _tenantSettings = tenantSettings.Value;
        _httpContext = contextAccessor.HttpContext;
        // if not null start deal with request
        if (_httpContext is not null)
        {
            // inside header check if key with name tenant or not if found will assign in tenantId
            if (_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
            {
                SetTenant(tenantId);
            }
            else
            {
                throw new Exception("Invalid Tenant!");
            }
        }
    }

    private void SetTenant(string tenantId)
    {
        _currentTenant = _tenantSettings.Tenants.Where(a => a.TID == tenantId).FirstOrDefault();
        if (_currentTenant is null) throw new Exception("Invalid Tenant!");
        // if connection string empty add in shared db
        if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
        {
            SetDefaultConnectionStringToCurrentTenant();
        }
    }

    // make shared database
    private void SetDefaultConnectionStringToCurrentTenant()
    {
        _currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;
    }

    public string? GetConnectionString()
    {
        var currenctConnectionString = _currentTenant is null 
            ? _tenantSettings.Defaults.ConnectionString : _currentTenant.ConnectionString;
        return currenctConnectionString;
    } 
    public string? GetDatabaseProvider() => _tenantSettings.Defaults?.DBProvider;
    public Tenant? GetTenant() => _currentTenant;
}
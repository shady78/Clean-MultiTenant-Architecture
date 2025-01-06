using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Multitenant.Core.Settings;
using Multitenant.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Multitenant.Infrastructure.Extensions;
// فئة امتداد لإضافة وترحيل قواعد بيانات المستأجرين
public static class ServiceCollectionExtensions
{
    // دالة لإضافة وترحيل قواعد البيانات
    public static IServiceCollection AddAndMigrateTenantDatabases(this IServiceCollection services, IConfiguration config)
    {
        // الحصول على إعدادات المستأجر الافتراضية
        var options = services.GetOptions<TenantSettings>(nameof(TenantSettings));
        var defaultConnectionString = options.Defaults?.ConnectionString;
        var defaultDbProvider = options.Defaults?.DBProvider;

        // إضافة سياق قاعدة البيانات إذا كان المزود هو SQL Server
        if (defaultDbProvider.ToLower() == "mssql")
        {
            services.AddDbContext<ApplicationDbContext>(m =>
                m.UseSqlServer(e => e.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }
        else if (defaultDbProvider.ToLower() == "npgsql")
        {
            services.AddDbContext<ApplicationDbContext>(m =>
                m.UseNpgsql(e => e.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        // التكرار على كل المستأجرين وإنشاء قواعد البيانات الخاصة بهم
        var tenants = options.Tenants;
        foreach (var tenant in tenants)
        {
            string connectionString;
            if (string.IsNullOrEmpty(tenant.ConnectionString))
            {
                connectionString = defaultConnectionString;
            }
            else
            {
                connectionString = tenant.ConnectionString;
            }

            // إنشاء نطاق جديد للخدمة
            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.SetConnectionString(connectionString);

            // تنفيذ الترحيلات إذا لم تتم 
            //if (dbContext.Database.GetPendingMigrations().Any())
            //{
            //    dbContext.Database.Migrate();
            //}
            if ((dbContext.Database.GetMigrations().Count() > 1))
            {
                dbContext.Database.Migrate();
            }
        }
        return services;
    }

    // دالة مساعدة للحصول على الإعدادات من ملف التكوين
    public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
    {
        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var section = configuration.GetSection(sectionName);
        var options = new T();
        section.Bind(options);
        return options;
    }
}
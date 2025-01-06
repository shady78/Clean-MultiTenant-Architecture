using Multitenant.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multitenant.Core.Interfaces;
// واجهة تحدد العمليات الأساسية على المنتجات
public interface IProductService
{
    // دالة إنشاء منتج جديد
    Task<Product> CreateAsync(string name, string description, int rate);
    // دالة الحصول على منتج بواسطة المعرف
    Task<Product> GetByIdAsync(int id);
    // دالة الحصول على كل المنتجات
    Task<IReadOnlyList<Product>> GetAllAsync();
}
using Microsoft.EntityFrameworkCore;
using Multitenant.Core.Entities;
using Multitenant.Core.Interfaces;
using Multitenant.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multitenant.Infrastructure.Services;
// تنفيذ خدمة المنتج
public class ProductService : IProductService
{
    // متغير لسياق قاعدة البيانات
    private readonly ApplicationDbContext _context;

    // المنشئ - يتم حقن سياق قاعدة البيانات
    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    // تنفيذ دالة إنشاء منتج جديد
    public async Task<Product> CreateAsync(string name, string description, int rate)
    {
        var product = new Product(name, description, rate);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    // تنفيذ دالة الحصول على كل المنتجات
    public async Task<IReadOnlyList<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    // تنفيذ دالة الحصول على منتج بواسطة المعرف
    public async Task<Product> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }
}
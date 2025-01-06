using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Multitenant.Core.Interfaces;

namespace Multitenant.API.Controllers;
// متحكم واجهة برمجة التطبيقات للمنتجات
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    // المنشئ - يتم حقن خدمة المنتج
    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAsync()
    {
        var  products = await _service.GetAllAsync();
        return Ok(products);
    }

    // نقطة نهاية للحصول على منتج بواسطة المعرف
    [HttpGet]
    public async Task<IActionResult> GetAsync(int id)
    {
        var productDetails = await _service.GetByIdAsync(id);
        return Ok(productDetails);
    }

    // نقطة نهاية لإنشاء منتج جديد
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateProductRequest request)
    {
        return Ok(await _service.CreateAsync(request.Name, request.Description, request.Rate));
    }
}

// فئة لتمثيل طلب إنشاء منتج جديد
public class CreateProductRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Rate { get; set; }
}
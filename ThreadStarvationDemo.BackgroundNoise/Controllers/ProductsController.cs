using Microsoft.AspNetCore.Mvc;
using ThreadStarvationDemo.BackgroundNoise.Models;
using ThreadStarvationDemo.BackgroundNoise.Repositories;

namespace ThreadStarvationDemo.BackgroundNoise.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductsRepository _productsRepository;

    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ILogger<ProductsController> logger, ProductsRepository productsRepository)
    {
        _logger = logger;
        _productsRepository = productsRepository;
    }

    [HttpGet]
    public IEnumerable<Product> Get()
    {
        return _productsRepository.GetProducts();
    }

    [HttpGet("async")] //i.e. /products/async
    public async Task<IEnumerable<Product>> GetAsync()
    {
        return await _productsRepository.GetProductsAsync();
    }
}
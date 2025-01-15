using Microsoft.AspNetCore.Mvc;
using ThreadStarvationDemo.SyncWebAPI.Models;
using ThreadStarvationDemo.SyncWebAPI.Repositories;

namespace ThreadStarvationDemo.SyncWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController(ProductsRepository productsRepository) : ControllerBase
{
    [HttpGet]
    public IEnumerable<Product> Get()
    {
        return productsRepository.GetProducts();
    }

    [HttpGet("async")] //i.e. /products/async
    public async Task<IEnumerable<Product>> GetAsync()
    {
        return await productsRepository.GetProductsAsync();
    }
}

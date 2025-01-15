using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThreadStarvationDemo.CoreWebAPI.Models;
using ThreadStarvationDemo.CoreWebAPI.Repositories;

namespace ThreadStarvationDemo.CoreWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsRepository _productsRepository;

        public ProductsController(ProductsRepository productsRepository)
        {
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
}

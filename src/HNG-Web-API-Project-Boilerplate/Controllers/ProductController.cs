using HNG_Boilerplate_Application.Services;
using HNG_Boilerplate_Domain.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HNG_Web_API_Project_Boilerplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        // GET: api/<ProductController>
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            var result = _productService.GetAllProducts();
            return result.Result;
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public Product Get(Guid id)
        {
            var result = _productService.GetProduct(id);
            return result.Result;
        }

        // POST api/<ProductController>
        [HttpPost]
        public bool Post([FromBody] Product product)
        {
            var result = _productService.AddProduct(product);
            return result;
        }
    }
}

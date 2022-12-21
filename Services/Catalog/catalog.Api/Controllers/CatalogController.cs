using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using catalog.Api.Entities;
using catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace catalog.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        #region ctor
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        #endregion

        #region get products

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var product = await _productRepository.GetProducts();
            return Ok(product);
        }

        #endregion

        #region get product by id
        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct([FromRoute]string id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
            {
                _logger.LogError($"product with id :{id} not found");
                return NotFound();
            }
            return Ok(product);
        }
        #endregion

        #region get products by categories 
        [HttpGet("[action]/{category}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory([FromRoute]string category)
        {
            var product = await _productRepository.GetProductByCategory(category);
            return Ok(product);
        }
        #endregion

        #region create product
        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _productRepository.CreateProduct(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }   

        #endregion 

        #region update product
        [HttpDelete("{id:length(24)}",Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> DeleteProduct(string id)
        {
            return Ok(await _productRepository.DeleteProduct(id));
        }

        #endregion      

    }
}

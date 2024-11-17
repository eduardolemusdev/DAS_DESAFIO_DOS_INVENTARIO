using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System.Diagnostics;
using System.Security.Claims;

namespace DAS_DESAFIO_DOS_INVENTARIO.Controllers
{
    [Authorize]
    [Route("Products")]
    public class ProductsController : Controller
    {
        private IProductRepository _productRepository;
        private IUserRepository _userRepository;

        public ProductsController(IProductRepository productRepository, IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        [HttpDelete]
        [Route("Delete/{id?}")]
        public IActionResult Delete(int? id)
        {

            Debug.WriteLine($"Product id *: {id}");


            if (!id.HasValue)
            {
                return NotFound();
            }

            var result = _productRepository.DeleteProductById(id.Value);
            if (result == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpGet]
        [Route("GetProductById/{id?}")]
        public IActionResult GetProductById(int? id)
        {



            if (!id.HasValue)
            {
                return NotFound();
            }

            var result = _productRepository.GetProductById(id.Value);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("Edit/{id}")]
        public IActionResult Edit(int id, [FromBody] Product updatedProduct)
        {
            if (id != updatedProduct.Id)
            {
                return BadRequest("Product ID mismatch.");
            }

            var result = _productRepository.UpdateProduct(updatedProduct);
            if (result == null)
            {
                return NotFound("Product not found.");
            }

            return Ok("Product updated successfully.");
        }


        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody] NewProduct newProduct)
        {
            Debug.WriteLine($"Entra => {newProduct.Name}");
            if (!ModelState.IsValid)
            {

                Debug.WriteLine($"Nuevo producto modelo invalido");

                return BadRequest("Invalid product data.");
            }
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            Debug.WriteLine(email);
            if (email == null)
            {
                return BadRequest("User not available");
            }

            var user = _userRepository.GetByEmail(email);
            var productToAdd = new Product { Name = newProduct.Name, Description = newProduct.Description, Price = newProduct.Price, Quantity = newProduct.Quantity, UserId = user.Id };
            var result = _productRepository.AddProduct(productToAdd);

            if (result == null)
            {
                return StatusCode(500, "Failed to save product.");
            }

            return Ok(result);
        }


    }
}

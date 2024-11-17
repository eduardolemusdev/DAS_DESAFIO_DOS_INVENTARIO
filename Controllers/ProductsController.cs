﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System.Diagnostics;

namespace DAS_DESAFIO_DOS_INVENTARIO.Controllers
{
    [Authorize]
    [Route("Products")]
    public class ProductsController : Controller
    {
        private IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
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
    }
}

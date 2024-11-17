using BussinesLogic;
using DAS_DESAFIO_DOS_INVENTARIO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System.Diagnostics;
using System.Security.Claims;

namespace DAS_DESAFIO_DOS_INVENTARIO.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RegisterProductService _registerProductService;
        private readonly IProductRepository _productRepository;
        public HomeController(ILogger<HomeController> logger, RegisterProductService r, IProductRepository productRepository)
        {
            _logger = logger;
            _registerProductService = r;
            _productRepository = productRepository;
        }

        public IActionResult Index(decimal? minPrice, decimal? maxPrice, int? minQuantity, int? maxQuantity, string? filterStrategy, string? target, int page = 1, int pageSize = 10)
        {
            _generateNavbarClass();

            ProductFilter filter = new ProductFilter();
            IEnumerable<Product> products = Enumerable.Empty<Product>();
            filter.MinPrice = minPrice;
            filter.MaxPrice = maxPrice;
            filter.MinQuantity = minQuantity;
            filter.MaxQuantity = maxQuantity;
            filter.Page = page;
            filter.PageSize = pageSize;




            ViewData["ProductFilter"] = filter;



            if (filterStrategy.IsNullOrEmpty() || target.IsNullOrEmpty())
            {
                var productsResult = _productRepository.GetProducts(filter);
                products = productsResult.Data;
                ViewData["TotalPages"] = productsResult.TotalPages;
            }

            Debug.WriteLine($"Search criteria {filterStrategy}: => {target}");

            if (filterStrategy == "name" && !target.IsNullOrEmpty())
            {
                var productsResult = _productRepository.GetProductsByName(target, filter);
                products = productsResult.Data;
                ViewData["TotalPages"] = productsResult.TotalPages;
            }

            if (filterStrategy == "description" && !target.IsNullOrEmpty())
            {
                var productsResult = _productRepository.GetProductsByDescription(target, filter);
                products = productsResult.Data;
                ViewData["TotalPages"] = productsResult.TotalPages;
            }

            return View(products);
        }

        public IActionResult Privacy()
        {
            _generateNavbarClass();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void _generateNavbarClass()
        {
            string navbarClass;

            if (!User.Identity.IsAuthenticated)
            {
                navbarClass = "bg-white"; // No autenticado
            }
            else
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;

                if (role == "admin")
                {
                    navbarClass = "bg-black"; // Administrador
                }
                else
                {
                    navbarClass = "bg-blue"; // Usuario normal
                }
            }

            ViewData["NavbarClass"] = navbarClass;
        }
    }
}

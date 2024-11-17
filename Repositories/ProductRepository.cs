namespace DAS_DESAFIO_DOS_INVENTARIO.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly InventoryContext _inventoryContext;

        public ProductRepository(InventoryContext inventoryContext)
        {
            _inventoryContext = inventoryContext;
        }

        public Product AddProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            _inventoryContext.Products.Add(product);
            _inventoryContext.SaveChanges();
            return product;
        }

        public Product DeleteProductById(int id)
        {
            var product = _inventoryContext.Products.SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return null;
            }


            _inventoryContext.Products.Remove(product);
            _inventoryContext.SaveChanges();
            return product;
        }

        public Product GetProductById(int id)
        {
            return _inventoryContext.Products.SingleOrDefault(p => p.Id == id);
        }

        public ProductResponse GetProducts(ProductFilter productFilter)
        {
            var query = _inventoryContext.Products.AsQueryable();

            _setPriceFilter(ref query, productFilter.MinPrice, productFilter.MaxPrice);
            _setQuantityFilter(ref query, productFilter.MinQuantity, productFilter.MaxQuantity);

            var queryDescription = query.OrderBy(pr => pr.Id);

            var totalProducts = query.Count();

            var totalPages = (int)Math.Ceiling(totalProducts / (double)productFilter.PageSize);

            var products = queryDescription.Skip((productFilter.Page - 1) * productFilter.PageSize)
                                .Take(productFilter.PageSize)
                                .ToList();

            return new ProductResponse(products, totalPages);
        }

        public ProductResponse GetProductsByDescription(string description, ProductFilter productFilter)
        {
            var query = _inventoryContext.Products.AsQueryable();

            _setPriceFilter(ref query, productFilter.MinPrice, productFilter.MaxPrice);
            _setQuantityFilter(ref query, productFilter.MinQuantity, productFilter.MaxQuantity);

            var queryDescription = query.OrderBy(pr => pr.Id).Where(pr => pr.Description.Contains(description));

            var totalProducts = query.Count();

            var totalPages = (int)Math.Ceiling(totalProducts / (double)productFilter.PageSize);

            var products = queryDescription.Skip((productFilter.Page - 1) * productFilter.PageSize)
                                .Take(productFilter.PageSize)
                                .ToList();

            return new ProductResponse(products, totalPages);
        }

        public ProductResponse GetProductsByName(string name, ProductFilter productFilter)
        {
            var query = _inventoryContext.Products.AsQueryable();

            _setPriceFilter(ref query, productFilter.MinPrice, productFilter.MaxPrice);
            _setQuantityFilter(ref query, productFilter.MinQuantity, productFilter.MaxQuantity);

            var queryDescription = query.OrderBy(pr => pr.Id).Where(pr => pr.Name.Contains(name));

            var totalProducts = query.Count();

            var totalPages = (int)Math.Ceiling(totalProducts / (double)productFilter.PageSize);

            var products = queryDescription.Skip((productFilter.Page - 1) * productFilter.PageSize)
                                .Take(productFilter.PageSize)
                                .ToList();

            return new ProductResponse(products, totalPages);
        }

        public Product UpdateProduct(Product product)
        {
            ArgumentNullException.ThrowIfNull(product);

            var existingProduct = _inventoryContext.Products.SingleOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
                throw new InvalidOperationException($"No se encontró el producto con el ID {product.Id}.");

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Quantity = product.Quantity;
            existingProduct.Description = product.Description;

            _inventoryContext.SaveChanges();
            return existingProduct;
        }


        private void _setPriceFilter(ref IQueryable<Product> productQuery, decimal? minPrice, decimal? maxPrice)
        {
            if (minPrice.HasValue)
            {
                productQuery = productQuery.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                productQuery = productQuery.Where(p => p.Price <= maxPrice.Value);
            }
        }

        private void _setQuantityFilter(ref IQueryable<Product> productQuery, decimal? minQuantity, decimal? maxQuantity)
        {
            if (minQuantity.HasValue)
            {
                productQuery = productQuery.Where(p => p.Quantity >= minQuantity.Value);
            }
            if (maxQuantity.HasValue)
            {
                productQuery = productQuery.Where(p => p.Quantity <= maxQuantity.Value);
            }
        }
    }
}

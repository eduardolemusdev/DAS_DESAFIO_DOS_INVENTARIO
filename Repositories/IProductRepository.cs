namespace DAS_DESAFIO_DOS_INVENTARIO.Repositories
{
    public interface IProductRepository
    {

        public Product AddProduct(Product product);
        public Product UpdateProduct(Product product);
        public Product GetProductById(int id);
        public Product DeleteProductById(int id);

        public ProductResponse GetProducts(ProductFilter productFilter);
        public ProductResponse GetProductsByName(string name, ProductFilter productFilter);
        public ProductResponse GetProductsByDescription(string description, ProductFilter productFilter);
    }

    public class ProductResponse
    {
        public ProductResponse(IEnumerable<Product> data, int totalPages)
        {
            Data = data;
            TotalPages = totalPages;
        }

        public IEnumerable<Product> Data { get; set; }
        public int TotalPages { get; set; }
    }

    public class ProductFilter
    {
        public string Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}

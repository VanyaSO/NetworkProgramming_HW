using GameStore.Models;
using GameStore.Models.Pages;

namespace GameStore.Interfaces;

public interface IProduct
{
    PagedList<Product> GetProducts(QueryOptions options, int category = 0);
    
    IQueryable<Product> GetAllProducts();
    void AddProduct(Product product);
    Product GetProduct(int id);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
} 
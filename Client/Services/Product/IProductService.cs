using e_CommerceContinental.Shared.Models;

namespace e_CommerceContinental.Client.Services;

interface IProductService
{
    ValueTask<Category[]> GetCategories();
    ValueTask<Category> GetCategory(Guid id);
    ValueTask<Product[]> GetProductsByCategory(Guid id);    
    ValueTask<Product[]> GetProducts();
    ValueTask<Product[]> GetProductsOnly();
    ValueTask<Product> ExistProductCode(string code);
    ValueTask<Product> GetProduct(Guid id);
    ValueTask<Product[]> GetFabricProducts();
    ValueTask<Product[]> GetProductsByBranch(Guid id);
    ValueTask<Product[]> GetProductsByShop(Guid id);
    ValueTask<bool> AddProduct(Product product);
    ValueTask<bool> EditProduct(Guid id, Product product);
    ValueTask<bool> EditProducts(List<Product> products);
    ValueTask<bool> DeleteProduct(Guid id);
    ValueTask<bool> AddCategory(Category category);
    ValueTask<bool> EditCategory(Guid id, Category category);
    ValueTask<bool> DeleteCategory(Guid id);
    ValueTask<bool> AddStock(Stock stock);
    ValueTask<bool> EditStock(Guid id, Stock stock);
    ValueTask<bool> DeleteStock(Guid id);
}


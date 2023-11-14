using IMS.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(int productId);
        Task<Product> GetProductByIdAsync(int productId);
        Task<List<Product>> GetProductsByNameAsync(string name);
        Task UpdateProductAsync(Product oldProduct);
    }
}

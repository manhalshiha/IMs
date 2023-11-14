using IMS.CoreBusiness;
using IMS.UseCases.Interfaces;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.Activities
{
    public class SellProductUseCase : ISellProductUseCase
    {
        private readonly IProductRepository productRepository;
        private readonly IProductTransactionRepository productTransactionRepository;

        public SellProductUseCase(IProductRepository productRepository, IProductTransactionRepository productTransactionRepository)
        {
            this.productRepository = productRepository;
            this.productTransactionRepository = productTransactionRepository;
        }
        public async Task ExecuteAsync(string salesOrderNumber, Product product, int quantity, string doneBy)
        {
            await productTransactionRepository.SellProductAsync(salesOrderNumber, product, quantity, product.Price, doneBy);
            product.Quantity -= quantity;
            await productRepository.UpdateProductAsync(product);
        }
    }
}

using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.EFCore
{
    public class ProductTransactionRepository : IProductTransactionRepository
    {
        private readonly IMSContext db;
        private readonly IProductRepository productRepository;

        public ProductTransactionRepository(IMSContext db, IProductRepository productRepository)
        {
            this.db = db;
            this.productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductTransaction>> GetPorductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? productTransactionType)
        {
            if (dateTo.HasValue) dateTo.Value.AddDays(1);
            var query = from pt in db.ProductTransactions
                        join prod in db.Products on pt.ProductId equals prod.ProductId
                        where
                        //(string.IsNullOrWhiteSpace(productName) || prod.ProductName.Contains(productName, StringComparison.OrdinalIgnoreCase))
                        (string.IsNullOrWhiteSpace(productName) || prod.ProductName.ToLower().IndexOf(productName.ToLower())>=0)&& 
                        (!dateFrom.HasValue || pt.TransactionDate >= dateFrom.Value.Date) && 
                        (!dateTo.HasValue || pt.TransactionDate <= dateTo.Value.Date) && 
                        (!productTransactionType.HasValue || pt.ActivityType == productTransactionType)
                        select pt;
            return await query.Include(x => x.Product).ToListAsync();
        }

        public async Task ProduceAsync(string productionNumber, Product product, int quantity, double price, string doneBy)
        {
            //old code
            //var prod = await db.Products.Include(x => x.ProductInventories).ThenInclude(x => x.Inventory).FirstOrDefaultAsync(x=>x.ProductId==product.ProductId);
            var prod = await this.productRepository.GetProductByIdAsync(product.ProductId);
            if (prod != null)
            {
                foreach (var pi in prod.ProductInventories)
                {
                    int quantityBefor = pi.Inventory.Quantity;
                    pi.Inventory.Quantity -= quantity * pi.InventoryQuantity;
                    this.db.InventoryTransactions.Add(new InventoryTransaction
                    {
                        ProductionNumber = productionNumber,
                        InventoryId = pi.Inventory.InventoryId,
                        QuantityBefor = quantityBefor,
                        ActivityType = InventoryTransactionType.ProduceProduct,
                        QuantityAfter = pi.Inventory.Quantity,
                        TransactionDate = DateTime.UtcNow,
                        DoneBy = doneBy,
                        UnitPrice = price
                    });

                }
            }
            this.db.ProductTransactions.Add(new ProductTransaction
            {
                ProductionNumber = productionNumber,
                ProductId = product.ProductId,
                QuantityBefor = product.Quantity,
                ActivityType = ProductTransactionType.ProduceProduct,
                QuantityAfter = product.Quantity + quantity,
                TransactionDate = DateTime.UtcNow,
                DoneBy = doneBy,
                UnitPrice = price
            });
            await this.db.SaveChangesAsync();
        }

        public async Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double price, string doneBy)
        {
            this.db.ProductTransactions.Add(new ProductTransaction
            {
                SalesOrderNumber = salesOrderNumber,
                ProductId = product.ProductId,
                QuantityBefor = product.Quantity,
                QuantityAfter = product.Quantity - quantity,
                TransactionDate = DateTime.UtcNow,
                DoneBy = doneBy,
                UnitPrice = price,
                ActivityType = ProductTransactionType.SellProduct
            }) ;
            await this.db.SaveChangesAsync();
        }
    }
}

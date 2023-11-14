using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.EFCore
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMSContext db;

        public ProductRepository(IMSContext db)
        {
            this.db = db;
        }

        public async Task AddProductAsync(Product product)
        {

            //if (await db.Products.AnyAsync(x =>x.ProductId!=product.ProductId && x.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))return;
            if (await db.Products.AnyAsync(x => x.ProductId != product.ProductId && x.ProductName.ToLower()==product.ProductName.ToLower())) return;
            await this.db.AddAsync(product);
            db.SaveChanges();

            //add test to verify if we actually added product
            //var prod = db.Products.Include(x => x.ProductInventories).ThenInclude(x => x.Inventory).ToList();
        }

        public async Task DeleteProductAsync(int productId)
        {
            var prod = await this.db.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
            if(prod != null)
            //delete permanently
            //this.db.Products.Remove(prod);
            //await this.db.SaveChangesAsync();
            // delete smoothly
            prod.IsActive = false;
            await this.db.SaveChangesAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            var Product = await this.db.Products.Include(x => x.ProductInventories).ThenInclude(x => x.Inventory).FirstOrDefaultAsync(x => x.ProductId == productId);
            var oldProduct = new Product() {ProductId=Product.ProductId, ProductName = Product.ProductName, Quantity = Product.Quantity, Price = Product.Price, ProductInventories = Product.ProductInventories };
            return oldProduct;
        }

        public async Task<List<Product>> GetProductsByNameAsync(string name)
        {
            //return await this.db.Products.Where(x => (x.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(name))&&x.IsActive==true).ToListAsync();
            return await this.db.Products.Where(x => (x.ProductName.ToLower().IndexOf(name.ToLower())>=0 || string.IsNullOrWhiteSpace(name)) && x.IsActive == true).ToListAsync();
        }

        public async Task UpdateProductAsync(Product newProduct)
        {
            //prevent same name 
            //if (await db.Products.AnyAsync(x =>x.ProductId!=newProduct.ProductId&& x.ProductName.Equals(newProduct.ProductName, StringComparison.OrdinalIgnoreCase))) return;
            if (await db.Products.AnyAsync(x => x.ProductId != newProduct.ProductId && x.ProductName.ToLower()==newProduct.ProductName.ToLower())) return;
            var prod=await this.db.Products.FindAsync(newProduct.ProductId);
            if (prod != null)
            {
                prod.ProductId = newProduct.ProductId;
                prod.Quantity = newProduct.Quantity;
                prod.Price = newProduct.Price;
                prod.ProductInventories = newProduct.ProductInventories;
                await db.SaveChangesAsync();
            }
            
        }
    }
}

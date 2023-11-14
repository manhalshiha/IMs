using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCore
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IMSContext db;

        public InventoryRepository(IMSContext db)
        {
            this.db = db;
        }

        public async Task AddInventoryAsync(Inventory inventory)
        {
            //to prevent tow inventories from have same name
            //if (this.db.Inventories.Any(x => x.InventoryName. Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase))) return;
            if (this.db.Inventories.Any(x => x.InventoryName.ToLower() == inventory.InventoryName.ToLower())) return;
            await this.db.AddAsync(inventory);
            await db.SaveChangesAsync();
        }

        public async Task EditInventoryAsync(Inventory newInventory)
        {
            //to prevent tow inventories from have same name
            //if (this.db.Inventories.Any(x=>x.InventoryId!=newInventory.InventoryId && x.InventoryName.Equals(newInventory.InventoryName, StringComparison.OrdinalIgnoreCase))) return;
            if (this.db.Inventories.Any(x => x.InventoryId != newInventory.InventoryId && x.InventoryName.ToLower() == newInventory.InventoryName.ToLower())) return;
            var inv = await this.db.Inventories.FindAsync(newInventory.InventoryId);
            if (inv != null)
            {
                inv.InventoryName = newInventory.InventoryName;
                inv.Quantity = newInventory.Quantity;
                inv.Price = newInventory.Price;

                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
        {
            //return await this.db.Inventories.Where(x => x.InventoryName.Contains(name, StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(name)).ToListAsync();
            return await this.db.Inventories.Where(x => x.InventoryName.ToLower().IndexOf(name.ToLower()) >= 0).ToListAsync();
        }
        public async Task<Inventory?> GetInventoryByIdAsync(int InventoryId)
        {
            var inv=await this.db.Inventories.FirstOrDefaultAsync(x=>x.InventoryId==InventoryId) ;
            var newInv = new Inventory();
            if (inv!=null)
            newInv = new Inventory() { InventoryId=inv.InventoryId,InventoryName=inv.InventoryName,Quantity=inv.Quantity,Price=inv.Price};

            return (newInv);
        }
    }
}
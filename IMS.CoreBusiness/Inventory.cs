using System.ComponentModel.DataAnnotations;

namespace IMS.CoreBusiness
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        [Required]
        public string? InventoryName { get; set; }=string.Empty;
        [Range(0,int.MaxValue,ErrorMessage ="Quantity must be greater than or equal to {0}")]
        public int Price { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = " must be greater than or equal to {0}")]
        public int Quantity { get; set; }
        public List<ProductInventory>? PoductInventories { get; set; }
    }
}
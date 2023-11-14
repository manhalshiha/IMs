using IMS.CoreBusiness;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class PurchaseViewModel
    {
        [Required]
        public string? PuchaseOrder { get; set; }
        [Required]
        public int InventoryId { get; set; } 
        [Required]
        public string? InventoryName { get; set; }
        [Required]
        [Range(minimum:1, maximum: int.MaxValue,ErrorMessage ="Quantity should be greaten than 0")]
        public int QuantityToPurchase { get; set; }
        public double InventoryPrice { get; set; }

    }
}

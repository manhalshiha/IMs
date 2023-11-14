using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IProductTransactionRepository
    {
        Task<IEnumerable<ProductTransaction>> GetPorductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? productTransactionType);
        Task ProduceAsync(string productionNumber, Product product, int quantity,double price, string doneBy);
        Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double price, string doneBy);
    }
}
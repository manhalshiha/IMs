using IMS.CoreBusiness;

namespace IMS.UseCases.Interfaces
{
    public interface IProduceProductUseCase
    {
        Task ExecuteAsync(string ProductionNumber, Product product, int quantity, string doneBy);
    }
}
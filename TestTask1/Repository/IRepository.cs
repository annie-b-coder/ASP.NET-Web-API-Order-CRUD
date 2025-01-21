using TestTask1.Models;
using TestTask1.Models.DTO;

namespace TestTask1.Repository
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int id);
        Task<IEnumerable<DTOOrder>> GetByNumberAsync(string number);
        Task<IEnumerable<DTOOrder>> GetByDateAsync(DateTime from, DateTime to);
        Task<IEnumerable<DTOOrder>> GetByProviderIdAsync(int id);
        Task<IEnumerable<DTOOrder>> GetAllAsync();
        Task AddAsync(DTONewOrder order);
        Task UpdateAsync(DTONewOrder order);
        Task DeleteAsync(int id);
    }

    public interface IOrderItemRepository
    {
        Task<OrderItem> GetByIdAsync(int id);
        Task<IEnumerable<DTOOrderItem>> GetByOrderIdAsync(int id);
        Task DeleteAsync(int id);
    }

    public interface IProviderRepository
    {
        Task<IEnumerable<Provider>> GetAllAsync();
    }
}

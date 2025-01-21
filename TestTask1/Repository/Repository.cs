using Microsoft.EntityFrameworkCore;
using TestTask1.Models;
using TestTask1.Models.DTO;

namespace TestTask1.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationContext _context;
        public OrderRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return  await _context.Orders.Include(o => o.OrderItems)
                       .Include(o => o.Provider)
                       .SingleOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<DTOOrder>> GetByNumberAsync(string number)
        {
            return await _context.Orders.Include(o => o.OrderItems)
                       .Include(o => o.Provider)
                       .Where(o => o.Number.Equals(number)).Select(x => OrderToDTO(x)).ToArrayAsync();
        }

        public async Task<IEnumerable<DTOOrder>> GetByDateAsync(DateTime from, DateTime to)
        {
            return await _context.Orders.Include(o => o.OrderItems)
                       .Include(o => o.Provider).Where(record => record.Date >= from && record.Date <= to).Select(x => OrderToDTO(x)).ToArrayAsync();
        }

        public async Task<IEnumerable<DTOOrder>> GetAllAsync()
        {
            return await _context.Orders.Include(o => o.OrderItems).Include(p=>p.Provider).Select(x=> OrderToDTO(x)).ToListAsync();
        }

        private static DTOOrder OrderToDTO(Order todoItem) =>
       new DTOOrder
       {
           Id = todoItem.Id,
           Number = todoItem.Number,
           Date = todoItem.Date,
           ProviderId = todoItem.ProviderId,
           ProviderName = todoItem.Provider.Name,
           OrderItems = todoItem.OrderItems.Select(x=> OrderItemToDTO(x)).ToList(),
       };

        private static DTOOrderItem OrderItemToDTO(OrderItem todoItem) =>
      new DTOOrderItem(
          todoItem.Id,
          todoItem.OrderId,
          todoItem.Order.Number,
          todoItem.Name,          
          todoItem.Quantity,
          todoItem.Unit
      );

        public async Task<IEnumerable<DTOOrder>> GetByProviderIdAsync(int providerId)
        {
            return await _context.Orders
           .Where(o => o.ProviderId == providerId)
           .Include(o => o.OrderItems).Select(x => OrderToDTO(x))
           .ToListAsync();
        }

        public async Task AddAsync(DTONewOrder order)
        {
            Order res = new Order
            {
                Number = order.Number,
                ProviderId = order.ProviderId,
                Date = DateTime.Now,
                OrderItems = order.OrderItems.Select(x => new OrderItem
                {                    
                    Name = x.Name,
                    Unit = x.Unit,
                    Quantity = x.Quantity
                }).ToList()
            };                
                await _context.Orders.AddAsync(res);
                await _context.SaveChangesAsync();                         
        }

        public async Task UpdateAsync(DTONewOrder order)
        {
            var item = await _context.Orders.FindAsync(order.Id);

            if (item != null)
            {
                _context.Entry(item).State = EntityState.Modified;

                item.Date = DateTime.Now;
                item.OrderItems = order.OrderItems.Select(x => new OrderItem
                {
                    Name = x.Name,
                    Unit = x.Unit,
                    Quantity = x.Quantity
                }).ToList();
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var order = await GetByIdAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationContext _context;

        public OrderItemRepository(ApplicationContext context) { _context = context; }

        public async Task<OrderItem> GetByIdAsync(int id)
        {
            return await _context.OrderItems
                       .SingleOrDefaultAsync(o => o.Id == id);
        }
        private static DTOOrderItem OrderItemToDTO(OrderItem todoItem) =>
      new DTOOrderItem(
          todoItem.Id,
          todoItem.OrderId,
          todoItem.Order.Number,
          todoItem.Name,
          todoItem.Quantity,
          todoItem.Unit
      );

        public async Task<IEnumerable<DTOOrderItem>> GetByOrderIdAsync(int orderid)
        {
            return await _context.OrderItems.Include(o => o.Order)
                              .Where(o => o.OrderId == orderid).Select(x => OrderItemToDTO(x)).ToArrayAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var orderitem = await GetByIdAsync(id);
            if (orderitem != null)
            {
                _context.OrderItems.Remove(orderitem);
                await _context.SaveChangesAsync();
            }
        }
    }

    public class ProviderRepository : IProviderRepository
    {
        private readonly ApplicationContext _context;

        public ProviderRepository(ApplicationContext context) { _context = context; }

        public async Task<IEnumerable<Provider>> GetAllAsync()
        {
            return await _context.Providers.ToListAsync();
        }
    }
}

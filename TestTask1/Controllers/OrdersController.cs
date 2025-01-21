using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TestTask1.Models;
using TestTask1.Models.DTO;
using TestTask1.Repository;

namespace TestTask1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProviderRepository _providerRepository;
        public OrdersController(IOrderRepository orderRepository, IProviderRepository providerRepository, IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _providerRepository = providerRepository;
        }

        [HttpPost("addorder")]
        public async Task<ActionResult<DTONewOrder>> AddOrder([FromBody] DTONewOrder item)
        {
            await _orderRepository.AddAsync(item);
            return Ok();
        }

        [HttpPut("updateorder/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, DTONewOrder item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            await _orderRepository.UpdateAsync(item);

            return Ok();
        }

        [HttpDelete("deleteorder/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderRepository.DeleteAsync(id);

            return Ok();
        }

        [HttpDelete("deleteorderitem/{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            await _orderItemRepository.DeleteAsync(id);

            return Ok();
        }

        [HttpGet("getorders")]
        public async Task<ActionResult<IEnumerable<DTOOrder>>> GetOrders()
        {
            return Ok(await _orderRepository.GetAllAsync());
                       
        }

        [HttpGet("getorderitemsbyorderid")]
        public async Task<ActionResult> GetOrderItemsByOrderId([FromQuery] int orderid)
        {
            return Ok(await _orderItemRepository.GetByOrderIdAsync(orderid));
        }

        [HttpGet("getproviders")]
        public async Task<IActionResult> GetProviders()
        {
            var providers = await _providerRepository.GetAllAsync();
            return Ok(providers);
        }
    }
}

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ddac.tp038654.services.orders.Interface;
using ddac.tp038654.services.orders.Models;
using ddac.tp038654.services.orders.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace ddac.tp038654.services.orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ITableStorageService _tableStorageService;
        private readonly ServiceBusSender _serviceBusSender;
        public OrdersController(ITableStorageService tableStorageService, ServiceBusSender serviceBusSender)
        {
            _tableStorageService = tableStorageService;
            _serviceBusSender = serviceBusSender;
        }

        // GET: api/Orders
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Orders/5
        [HttpGet("{id}", Name = "Get")]
        public IEnumerable<Order> Get(string id)
        {
            string userId = id;
            List<Order> orders = _tableStorageService.RetrieveMultipleEntityUsingPartitionQueryAsync<Order>(id, "orders").Result;

            foreach (var item in orders)
            {
                List<OrderItem> orderItems = _tableStorageService.RetrieveMultipleEntityUsingPartitionQueryAsync<OrderItem>(item.RowKey, "ordersitems").Result;
                item.OrderItems = orderItems;
            }

            return orders;
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            try
            {
                await _tableStorageService.InsertOrMergeMultipleEntityAsync(order.OrderItems, "ordersitems");

                await _tableStorageService.InsertOrMergeEntityAsync(order, "orders");
                await _serviceBusSender.SendMessage(order);
                return Ok(new { Success = true, message="" });
            }
            catch (System.Exception ex)
            {
                return Ok(new { Success = false, ex.Message });
                
            }
            
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] OrderItem orderItem)
        {
            _tableStorageService.InsertOrMergeEntityAsync<OrderItem>(orderItem, "ordersitems");
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

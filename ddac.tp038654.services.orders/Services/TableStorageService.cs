using ddac.tp038654.services.orders.Interface;
using ddac.tp038654.services.orders.Models;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddac.tp038654.services.orders.Services
{
    public class TableStorageService : ITableStorageService
    {
        private readonly CloudTableClient _client;
        private readonly CloudTable orders;
        private readonly CloudTable ordersItems;

        public TableStorageService(CloudTableClient client)
        {
            _client = client;
            orders = _client.GetTableReference("orders");
            ordersItems = _client.GetTableReference("ordersItems");
        }

        public async Task<Order> InsertOrMergeEntityAsync(Order entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
                TableResult result = await orders.ExecuteAsync(insertOrMergeOperation);
                Order order = result.Result as Order;



                foreach (OrderItem item in entity.OrderItems)
                {
                    item.Id = Guid.NewGuid().ToString();
                    item.PartitionKey = order.Id;
                    insertOrMergeOperation = TableOperation.InsertOrMerge(item);
                    await ordersItems.ExecuteAsync(insertOrMergeOperation);
                    item.PartitionKey = order.Id;
                }

                return order;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        public async Task<Order> RetrieveEntityUsingPointQueryAsync(string partitionKey, string rowKey)
        {
            try
            {
                TableOperation retrieveOperation = TableOperation.Retrieve<Order>(partitionKey, rowKey);
                TableResult result = await orders.ExecuteAsync(retrieveOperation);
                Order order = result.Result as Order;

                return order;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }
    }
}

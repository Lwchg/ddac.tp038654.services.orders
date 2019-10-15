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

        public TableStorageService(CloudTableClient client)
        {
            _client = client;
        }

        public async Task<T> InsertOrMergeEntityAsync<T>(T entity, string tableName) where T : TableEntity
        {
            CloudTable table = _client.GetTableReference(tableName);

            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
                TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
                T order = result.Result as T;

                insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

                return order;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        public async Task<List<T>> InsertOrMergeMultipleEntityAsync<T>(List<T> entities, string tableName) where T : TableEntity
        {
            CloudTable table = _client.GetTableReference(tableName);

            if (entities == null)
            {
                throw new ArgumentNullException("entity");
            }
            try
            {
                TableBatchOperation tableOperations = new TableBatchOperation();
                foreach (var item in entities)
                {
                    tableOperations.Insert(item);
                }
                TableBatchResult result = await table.ExecuteBatchAsync(tableOperations);
                List<T> order = result.ToList() as List<T>;

                return order;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        public async Task<Order> RetrieveEntityUsingPointQueryAsync(string partitionKey, string rowKey, string tableName)
        {
            try
            {
                CloudTable table = _client.GetTableReference(tableName);
                TableOperation retrieveOperation = TableOperation.Retrieve<Order>(partitionKey, rowKey);
                TableResult result = await table.ExecuteAsync(retrieveOperation);
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

        public async Task<List<T>> RetrieveMultipleEntityUsingPartitionQueryAsync<T>(string partitionKey, string tableName) where T :TableEntity, new()
        {
            try
            {
                CloudTable table = _client.GetTableReference(tableName);
                TableQuery<T> tableQuery = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

                var result =  table.ExecuteQuery<T>(tableQuery);
                List<T> list = result.ToList() as List<T>;

                return list;
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

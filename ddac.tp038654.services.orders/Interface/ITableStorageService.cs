using ddac.tp038654.services.orders.Models;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddac.tp038654.services.orders.Interface
{
    public interface ITableStorageService
    {
        Task<T> InsertOrMergeEntityAsync<T>(T entity, string tableName) where T : TableEntity;
        Task<Order> RetrieveEntityUsingPointQueryAsync(string partitionKey, string rowKey, string tableName);
        Task<List<T>> InsertOrMergeMultipleEntityAsync<T>(List<T> entities, string tableName) where T : TableEntity;
        Task<List<T>> RetrieveMultipleEntityUsingPartitionQueryAsync<T>(string partitionKey, string tableName) where T : TableEntity, new();

    }
}

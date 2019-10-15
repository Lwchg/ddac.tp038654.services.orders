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
        Task<Order> InsertOrMergeEntityAsync(Order entity);
        Task<Order> RetrieveEntityUsingPointQueryAsync(string partitionKey, string rowKey);

    }
}

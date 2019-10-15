using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddac.tp038654.services.orders.Models
{
    public class Order : TableEntity
    {
        public Order()
        {
        }

        private string _id;
        private string _userId;

        public string Id {
            get {
                return _id;
            }
            set {
                RowKey = value;
                _id = value;
            }
        }
        public string UserId {
            get {
                return _userId;
            }
            set {
                PartitionKey = value ;
                _userId = value;
            } }
        public List<OrderItem> OrderItems { get; set; }
    }
}

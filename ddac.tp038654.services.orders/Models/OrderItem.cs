using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddac.tp038654.services.orders.Models
{
    public class OrderItem : TableEntity
    {

        private string _id;
        private string _orderId;

        public OrderItem()
        {
            Processed = false;
        }

        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                RowKey = value;
                _id = value;
            }
        }
        public int PageFrom { get; set; }
        public int PageTo { get; set; }
        public int BookId { get; set; }
        public double Total { get; set; }
        public string VolumeId { get; set; }
        public bool Processed { get; set; }

        public string Url { get; set; }

        public string OrderId
        {
            get
            {
                return _orderId;
            }
            set
            {
                PartitionKey = value;
                _orderId = value;
            }
        }
    }
}

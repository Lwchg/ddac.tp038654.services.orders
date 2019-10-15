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

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ddac.tp038654.services.orders.Models
{
    public class Book
    {

        public int BookId { get; set; }
        [NotMapped]
        public string FilePath { get; set; }
        public string ThumbnailPath { get; set; }
        public string BlobUrlPath { get; set; }
        public int PageNumber { get; set; }
        public string VolumeId { get; set; }
        public List<Book> Orders { get; set; }
        public double Price { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V8.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string CartId { get; set; }
        public int Item { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

    }
}

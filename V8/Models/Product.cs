using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V8.Models;

namespace V8.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public V8User User { get; set; }
        public string Name { get; set; }
        public Double Price { get; set; }
        public string Photo { get; set; }
        public int Quantity { get; set; }


    }
}

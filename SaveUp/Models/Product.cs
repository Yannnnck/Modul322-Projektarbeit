using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveUp.Models
{
    public class Product
    {
        public string Name { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
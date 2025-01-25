using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaveUp.Models
{
    public class ChartData
    {
        public string Date { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public double Height { get; set; }
        public double RemainingHeight { get; set; }
    }

}
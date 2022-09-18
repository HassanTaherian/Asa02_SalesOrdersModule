using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Discount
{
    public class DiscountProductRequestDto
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public int UnitPrice { get; set; }
    }
}
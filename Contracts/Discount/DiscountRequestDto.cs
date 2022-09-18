using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Discount
{
    public class DiscountRequestDto
    {
        public int UserId { get; set; }

        public int TotalPrice { get; set; }

        public string DiscountCode { get; set; }

        public ICollection<DiscountProductRequestDto>? Products { get; set; }
    }
}

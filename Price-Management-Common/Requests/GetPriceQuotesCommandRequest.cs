using Price_Management_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Price_Management_Common.Requests
{
    public class GetPriceQuotesCommandRequest : ICommandRequest
    {
        public string? Pickup_postcode { get; set; }
        public string? Delivery_postcode { get; set; }
        public string? Vehicle { get; set; }

    }
}

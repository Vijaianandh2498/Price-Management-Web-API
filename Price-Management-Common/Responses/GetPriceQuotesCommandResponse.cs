using Price_Management_Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Price_Management_Common.Responses
{
    public class GetPriceQuotesCommandResponse : IResponse
    {
        public string? Pickup_postcode { get; set; }
        public string? Delivery_postcode { get; set; }
        public string? Vehicle { get; set; }
        public List<PriceList>? PriceList { get; set; }

    }

    public class PriceList
    {
        public string? service { get; set; }
        public int Price { get; set; }
        public int delivery_time { get; set; }

    }
}

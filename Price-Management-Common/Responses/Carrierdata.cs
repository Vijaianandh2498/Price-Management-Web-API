

namespace Price_Management_Common.Responses
{

    public class Rootobject
    {
        public Carrier[]? carriers { get; set; }
    }

    public class Carrier
    {
        public string? carrier_name { get; set; }
        public int base_price { get; set; }
        public Service[]? services { get; set; }
    }

    public class Service
    {
        public int delivery_time { get; set; }
        public int markup { get; set; }
        public string[]? vehicles { get; set; }
    }

}

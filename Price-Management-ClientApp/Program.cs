using System.Text.Json;
using System.Text;


var getPriceQuotesCommandRequest = new GetPriceQuotesCommandRequest
{
    Delivery_postcode = "EC2A3LT",
    Pickup_postcode = "SW1A1AA",
    Vehicle = "small_van"
};
HttpClient _httpClient = new HttpClient();
Uri requestUri = new Uri("http://localhost:16756/PriceCalculator/quotes");
var requestJson = JsonSerializer.Serialize(getPriceQuotesCommandRequest);

var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

var response = _httpClient.PostAsync(requestUri, requestContent).Result;
response.EnsureSuccessStatusCode();

var result = response.Content.ReadAsStringAsync().Result;

Console.WriteLine(result);
Console.ReadLine();

public class GetPriceQuotesCommandRequest
{
    public string? Pickup_postcode { get; set; }
    public string? Delivery_postcode { get; set; }
    public string? Vehicle { get; set; }

}


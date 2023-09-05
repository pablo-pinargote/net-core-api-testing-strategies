using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace demo_api.controllers
{
    [Route("exchange-rates")]
    public class ExchangeRatesController : Controller
    {
        
        [HttpGet]
        public IActionResult FetchLatestExchangeRates([FromQuery] string @base="USD")
        {
            var requestUrl = $"https://api.exchangerate.host/latest?base={@base}";

            using var client = new HttpClient();
            var response = client.GetAsync(requestUrl).Result;

            if (!response.IsSuccessStatusCode) return NotFound();
            var jsonResponse = response.Content.ReadAsStringAsync().Result;
            return Ok(JsonSerializer.Deserialize<dynamic>(jsonResponse));
        }
        
    }
}
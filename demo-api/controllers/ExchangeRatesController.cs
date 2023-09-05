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
        public async Task<IActionResult> FetchLatestExchangeRates([FromQuery] string @base="USD")
        {
            var requestUrl = $"https://api.exchangerate.host/latest?base={@base}";

            using var client = new HttpClient();
            var response = await client.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode) return NotFound();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return Ok(JsonSerializer.Deserialize<dynamic>(jsonResponse));
        }
        
    }
}
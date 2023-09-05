using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using demo_api.controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace tests
{
    public class ExchangeRatesControllerTests
    {
        [Fact]
        public void FetchLatestExchangeRates_ReturnsOkResult()
        {
            var sut = new ExchangeRatesController();
            var result = sut.FetchLatestExchangeRates();
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public void FetchLatestExchangeRates_ReturnsDynamicObject()
        {
            var sut = new ExchangeRatesController();
            var result = sut.FetchLatestExchangeRates();
            var okResult = (OkObjectResult)result;
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
            var jsonResponse = okResult.Value.ToString();
            var dynamicObject = JsonSerializer.Deserialize<dynamic>(jsonResponse, jsonOptions);
            Assert.NotNull(dynamicObject);
        }
        
        [Fact]
        public void FetchLatestExchangeRates_Succeed()
        {
            var sut = new ExchangeRatesController();
            var result = sut.FetchLatestExchangeRates();
            var okResult = (OkObjectResult)result;
            var jsonResponse = okResult.Value.ToString();
            var dynamicObject = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);
            Assert.True(((JsonElement)dynamicObject["success"]).GetBoolean());
        }

        [Fact]
        public void FetchLatestExchangeRates_ReturnUSDAsBaseCurrency()
        {
            var sut = new ExchangeRatesController();
            var result = sut.FetchLatestExchangeRates();
            var okResult = (OkObjectResult)result;
            var jsonResponse = okResult.Value.ToString();
            var dynamicObject = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);
            Assert.Equal("USD", ((JsonElement)dynamicObject["base"]).GetString());
        }

    }
}
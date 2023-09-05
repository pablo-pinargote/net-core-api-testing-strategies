using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace demo_api.controllers
{

    [Route("")]
    public class HomeController : Controller
    {
        
        private readonly EndpointDataSource _endpointDataSource;

        public HomeController(EndpointDataSource endpointDataSource)
        {
            _endpointDataSource = endpointDataSource;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            var endpoints = _endpointDataSource.Endpoints
                .OfType<RouteEndpoint>()
                .Where(routeEndpoint => !string.IsNullOrWhiteSpace(routeEndpoint.RoutePattern.RawText))
                .Select(routeEndpoint => $"({routeEndpoint.Metadata.GetMetadata<HttpMethodMetadata>()?.HttpMethods.FirstOrDefault()}) {routeEndpoint.RoutePattern.RawText}")
                .ToList();
            var result = new
            {
                hi = "stranger",
                theseAreMyEndpoints = endpoints
                    
            };
            return Ok(result);
        }
        
    }

}
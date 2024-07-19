using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Filters;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/coffee/orders", Name = "coffee")]
    [ApiController]
    public class CoffeeController : ControllerBase
    {
        private readonly CoffeeService _coffeeService;

        public CoffeeController(CoffeeService coffeeService)
        {
            _coffeeService = coffeeService;
        }

        
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CoffeeOrderInfo))]
        [ServiceFilter(typeof(LogResponceContent))]
        public JsonResult MakeCoffee([FromBody]CoffeeSettings? coffeeSettings = null)
        {
            
            return new JsonResult(_coffeeService.DoMakeCoffee(coffeeSettings));
            
        }

        [HttpGet]
        [Route("{orderId}")]
        public CoffeeOrderInfo GetOrderInfo([FromRoute]int orderId)
        {
            return _coffeeService.LookUpOrder(orderId);
        }


        [HttpGet]
        public IReadOnlyCollection<CoffeeOrderInfo> GetOrders([FromQuery] OrderStatus? displayOrders)
        {
            return _coffeeService.GetOrders(displayOrders);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/service", Name = "service")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly CoffeeService _coffeeService;

        public ServiceController(CoffeeService coffeeService)
        {
            _coffeeService = coffeeService;
        }

        [HttpPost]
        [Route("turn_on")]
        public void TurnOn()
        {
            _coffeeService.GetCoffeeMachine()?.TurnOn();
        }

        [HttpPost]
        [Route("turn_off")]
        public void TurnOff()
        {
            _coffeeService.GetCoffeeMachine()?.TurnOff();
        }

        [HttpGet]
        [Route("info")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CoffeeMachineInfo))]
        public CoffeeMachineInfo? GetInfo()
        {
            return _coffeeService.GetCoffeeMachine()?.Info;
        }

       
    }
}

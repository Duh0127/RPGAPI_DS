using Microsoft.AspNetCore.Mvc;
using RpgApi.Models;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArmasController : ControllerBase
    {
        private Arma a = new Arma();

        [HttpGet]
        public IActionResult Get()
        {            
            return Ok(a);
        }
        
    }
}
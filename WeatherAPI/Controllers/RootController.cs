using Microsoft.AspNetCore.Mvc;

namespace WeatherAPI.Controllers {
    [Route ("/")]
    [ApiController]
    [ApiVersion ("1.0")]
    public class RootController : ControllerBase {
        [HttpGet (Name = nameof (GetRoot))]
        [ProducesResponseType (200)]
        public IActionResult GetRoot () {
            var response = new {
            href = Url.Link (nameof (GetRoot), null)
            };

            return Ok (response);
        }
    }
}
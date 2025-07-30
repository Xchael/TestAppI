using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestAppI.Services.Interfaces;

namespace TestAppI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAppController(ITestAppService _service) : ControllerBase
    {
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllTestApps()
        {
            return Ok(await _service.GetAllTestTableAsync());
        }
    }
}

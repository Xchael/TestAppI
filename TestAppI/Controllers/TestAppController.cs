using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestAppI.Services.Interfaces;

namespace TestAppI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAppController : ControllerBase
    {
        private readonly ITestAppService _service;

        // Constructor that initializes the controller with the service.
        // The service is injected to adhere to the Dependency Inversion Principle, allowing for easier testing and maintenance.
        public TestAppController(ITestAppService service, HttpContext httpContext)
        {
            _service = service;
        }

        // This method handles GET requests to the "getAll" endpoint.
        // It returns all TestApp resources asynchronously, allowing for non-blocking I/O operations.
        // The IActionResult return type provides flexibility in returning different HTTP responses.
        // If you add CancellationToken cancellationToken as a parameter, ASP.NET Core will automatically bind it to HttpContext.RequestAborted.
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllTestApps(CancellationToken cancellationToken)
        {
            // Calls the service to retrieve all TestApp records and returns them in the response.
            return Ok(await _service.GetAllTestTableAsync(cancellationToken));
        }
    }
}

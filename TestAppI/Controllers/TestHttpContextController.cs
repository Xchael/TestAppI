using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestData.Models;

namespace TestAppI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestHttpContextController : ControllerBase
    {
        private readonly string _targetUrl;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TestHttpContextController> _logger;

        public TestHttpContextController(HttpClient httpClient, IConfiguration configuration, ILogger<TestHttpContextController> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var baseUrl = _configuration["Host:Url"] ?? throw new InvalidOperationException("Host:Url. not found");
            var port = _configuration["Host:Port"] ?? throw new InvalidOperationException("Host:Port not found");

            if(!Uri.TryCreate($"{baseUrl}:{port}", UriKind.Absolute, out var uri))
            {
                throw new InvalidOperationException("Invalid target URL configuration");
            }

            if(!uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Target URL must use HTTPS");
            }

            _targetUrl = uri.ToString();
        }


        [HttpPost("GetTestTableDataMS")]
        [ProducesResponseType(typeof(TestTable), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetInfo([FromBody] TestTable testTable, CancellationToken cancelationToken)
        {
            if (testTable == null)
                return BadRequest("TestTable cannot be null");

            try
            {
                //basic input validation
                if (string.IsNullOrWhiteSpace(testTable.Name))
                    return BadRequest("Name cannot be null or empty");

                var requestDetails = MapToRequest(testTable);

                var responseObject = await PostJsonAsync<TestTable, TestTable>(
                    $"{_targetUrl}/target",
                    requestDetails,
                    cancelationToken);

                return responseObject == null
                    ? StatusCode(StatusCodes.Status500InternalServerError, "Failed to parse response")
                    : Ok(responseObject);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error calling target API");
                return StatusCode(StatusCodes.Status502BadGateway, "Error calling target API");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error parsing JSON");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error parsing JSON");
            }
            catch (TaskCanceledException ex) when (!cancelationToken.IsCancellationRequested)
            {
                _logger.LogWarning(ex, "Request to target API timed out");
                return StatusCode(StatusCodes.Status504GatewayTimeout, "Error calling target API. The service took too long to respond");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }
        }

        private async Task<TResponse?> PostJsonAsync<TRequest, TResponse>(
            string url, 
            TRequest requestDetails, 
            CancellationToken cancelationToken)
        {

            var jsonOptions = new JsonSerializerOptions
            {
                //map to lowercase
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var jsonContent = JsonSerializer.Serialize(requestDetails, jsonOptions);

            using var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Avoid infinite waits (anti DOS)
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancelationToken);
            cts.CancelAfter(TimeSpan.FromSeconds(30)); // Set a timeout of 30 seconds

            using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cts.Token);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStreamAsync(cancelationToken);
            return await JsonSerializer.DeserializeAsync<TResponse>(responseContent, jsonOptions, cancelationToken);
        }

        private TestTable MapToRequest(TestTable source) => new TestTable
        {
            Id = source.Id,
            Name = source.Name,
            IsEnabled = source.IsEnabled,
            Data = source.Data
        };

       
    }
}

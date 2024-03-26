using DataAnalytic.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalytic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AnalyzeTextController : ControllerBase
    {
        private readonly ProviderFactory _provider;
        public AnalyzeTextController(ProviderFactory provider) => _provider = provider;

        [HttpPost]
        public async Task<IActionResult> AnalyzeText([FromBody] string text, [FromQuery] string provider)
        {
            var service = _provider.GetTextService(provider);

            return Ok(await service.AnalyseTextAsync(text));
        }
    }
}

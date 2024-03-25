using DataAnalytic.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataAnalytic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyzeTextController : ControllerBase
    {
        private readonly ProviderFactory _provider;
        public AnalyzeTextController(ProviderFactory provider) => _provider = provider;

        [HttpGet]
        public async Task<IActionResult> AnalyzeText([FromQuery] string text, [FromQuery] string provider)
        {
            var service = _provider.GetTextService(provider);

            return Ok(await service.AnalyseTextAsync(text));
        }
    }
}

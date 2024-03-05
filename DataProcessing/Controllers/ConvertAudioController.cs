using DataProcessing.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataProcessing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertAudioController : ControllerBase
    {
        private readonly ProviderFactory _provider;
        public ConvertAudioController(ProviderFactory provider) => _provider = provider;

        [HttpGet]
        public async Task<IActionResult> ConvertToText([FromQuery] string path, [FromQuery] string provider)
        {
            //Check if the file exists and is an image
            if (!System.IO.File.Exists(path))
                return BadRequest("File not found");

            var service = _provider.GetAudioService(provider);

            return Ok(await service.AnalyseAsync(path));
        }
    }
}

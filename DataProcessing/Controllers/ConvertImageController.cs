using DataProcessing.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataProcessing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertImageController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> ConvertImageToText([FromQuery] string path, [FromQuery] string provider)
        {
            var service = ProviderFactory.GetImageService(provider);

            //Check if the file exists and is an image
            if (!System.IO.File.Exists(path))
                return BadRequest("File not found");


            return Ok(await service.AnalyseAsync(path));
        }
    }
}

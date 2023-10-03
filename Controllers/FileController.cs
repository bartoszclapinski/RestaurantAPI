using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace RestaurantAPI.Controllers;

[Route("file")]
[Authorize]
public class FileController : ControllerBase
{
    [HttpGet]
    [ResponseCache(Duration = 1200, VaryByQueryKeys = new[] { "fileName" })]
    public ActionResult GetFile([FromQuery] string fileName)
    {
        var rootPath = Directory.GetCurrentDirectory();
        var filePath = $"{rootPath}/PrivateFiles/{fileName}";
        
        var fileExists = System.IO.File.Exists(filePath);
        if (!fileExists)
        {
            return NotFound();
        }

        var contentProvider = new FileExtensionContentTypeProvider();
        contentProvider.TryGetContentType(filePath, out string contentType);
        var content = System.IO.File.ReadAllBytes(filePath);

        return File(content, contentType, fileName);
    }

    
    [HttpPost]
    public ActionResult Upload([FromForm] IFormFile file)
    {
        if (file is { Length: > 0 })
        {
            var rootPath = Directory.GetCurrentDirectory();
            var fileName = file.FileName;
            var fullPath = $"{rootPath}/PrivateFiles/{fileName}";
            using var stream = new FileStream(fullPath, FileMode.Create);
            file.CopyTo(stream);

            return Ok();
        }

        return BadRequest();
    }
}
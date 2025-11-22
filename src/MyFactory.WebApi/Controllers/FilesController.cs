using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Files;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    [HttpPost("upload")]
    public IActionResult Upload()
    {
        return Ok(new UploadFileResponse("file-001", "image.jpg"));
    }

    [HttpGet("{id}")]
    public IActionResult Download(string id)
        => File([], "application/octet-stream", "download.bin");

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
        => Ok(new DeleteFileResponse("deleted", id));
}

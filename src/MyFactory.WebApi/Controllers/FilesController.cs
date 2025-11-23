using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Files;
using MyFactory.WebApi.SwaggerExamples.Files;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/files")]
[Produces("application/json")]
public class FilesController : ControllerBase
{
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(UploadFileResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(UploadFileResponseExample))]
    public IActionResult Upload([FromForm] UploadFileRequest request)
    {
        return Ok(new UploadFileResponse(
            FileId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            FileName: "image.jpg"
        ));
    }

    [HttpGet("{id}")]
    [Produces("application/octet-stream")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Download(Guid id)
        => File([], "application/octet-stream", "download.bin");

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(DeleteFileResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(DeleteFileResponseExample))]
    public IActionResult Delete(Guid id)
        => Ok(new DeleteFileResponse(FileStatus.Deleted, id));
}


using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Files;
using MyFactory.WebApi.SwaggerExamples.Files;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    // ---------------------------
    //  Upload file
    // ---------------------------
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(UploadFileResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(UploadFileResponseExample))]
    public IActionResult Upload([FromForm] UploadFileRequest request)
    {
        return Ok(new UploadFileResponse(
            FileId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            FileName: "image.jpg"
        ));
    }

    // ---------------------------
    //  Download file
    // ---------------------------
    [HttpGet("{id}")]
    [Produces("application/octet-stream")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Download(Guid id)
        => File([], "application/octet-stream", "download.bin");

    // ---------------------------
    //  Delete file
    // ---------------------------
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(DeleteFileResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(DeleteFileResponseExample))]
    public IActionResult Delete(Guid id)
        => Ok(new DeleteFileResponse(FileStatus.Deleted, id));
}

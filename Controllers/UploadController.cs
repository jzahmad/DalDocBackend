using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("[controller]")]
[Authorize]

public class UploadController : ControllerBase
{
    private readonly S3Service _s3Service;

    public UploadController(S3Service s3Service)
    {
        _s3Service = s3Service;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file, [FromForm] string course, [FromForm] string type, [FromForm] string documentType, [FromForm] string term, [FromForm] string year)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var fileName = $"{course}_{type}_{documentType}_{term}_{year}_{file.FileName}";
        using var stream = file.OpenReadStream();
        
        var result = await _s3Service.UploadFileAsync(stream, fileName);
        return Ok(result);
    }
}

using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

[Route("pdf")]
[ApiController]
public class PdfController : ControllerBase
{
    private readonly S3Service _s3Service;
    private readonly string _bucketName = "daldoc"; // Ensure this matches your bucket name

    public PdfController(S3Service s3Service)
    {
        _s3Service = s3Service;
    }

    [HttpGet]
    public async Task<IActionResult> GetFiles([FromQuery] string course)
    {
        if (string.IsNullOrEmpty(course))
        {
            return BadRequest("Course name is required.");
        }

        try
        {
            var fileKeys = await _s3Service.GetFilesByCoursePrefixAsync(course, _bucketName);
            if (fileKeys.Count == 0)
            {
                return NotFound("No files found for the specified course.");
            }

            var files = fileKeys.Select(key => new 
            { 
                FileName = key.Split('/').Last(), // Extract the filename from the key
                Url = _s3Service.GeneratePreSignedUrl(key, _bucketName) 
            }).ToList();

            return Ok(new { files });
        }
        catch (AmazonS3Exception ex)
        {
            // Log detailed error message
            return StatusCode(500, $"S3 error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

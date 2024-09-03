using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("[controller]")]
public class CourseReqController : ControllerBase
{
    private readonly IAmazonSimpleNotificationService _snsClient;

    public CourseReqController(IAmazonSimpleNotificationService snsClient)
    {
        _snsClient = snsClient;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CourseRequest request)
    {
        try
        {
            string message = $"User requested the addition of \n Course: {request.Course} to \n" +
                             $" Department: {request.Department} ";

            var publishRequest = new PublishRequest
            {
                TopicArn = "arn:aws:sns:us-east-1:872515282994:daldoc_department",
                Message = message
            };

            var response = await _snsClient.PublishAsync(publishRequest);

            return Ok(new { message = "Course request sent successfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

public class CourseRequest
{
    public required string Department { get; set; }
    public required string Course { get; set; }
}

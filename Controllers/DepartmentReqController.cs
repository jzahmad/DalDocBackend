using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("[controller]")]
public class DepartmentReqController : ControllerBase
{
    private readonly IAmazonSimpleNotificationService _snsClient;
    private readonly string _topicArn = "arn:aws:sns:us-east-1:872515282994:daldoc_department";

    public DepartmentReqController(IAmazonSimpleNotificationService snsClient)
    {
        _snsClient = snsClient;
    }

    [HttpPost]
    public async Task<IActionResult> AddFaculty([FromBody] DepartmentRequest request)
    {
        if (string.IsNullOrEmpty(request.Department))
        {
            return BadRequest("Course is required.");
        }

        var message = $"User requested an addition of the following departmnet: \n{request.Department}";

        try
        {
            var publishRequest = new PublishRequest
            {
                TopicArn = _topicArn,
                Message = message
            };

            var response = await _snsClient.PublishAsync(publishRequest);

            return Ok(new { Message = "Department request successfully submitted." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}

public class DepartmentRequest
{
    public required string Department { get; set; }
}

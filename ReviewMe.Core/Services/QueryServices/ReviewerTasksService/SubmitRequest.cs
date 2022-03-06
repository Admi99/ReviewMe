
namespace ReviewMe.Core.Services.QueryServices.ReviewerTasksService;

public class SubmitRequest
{
    public string Feedback { get; set; } = string.Empty;

    public string AreasForImprovements { get; set; } = string.Empty;

}

namespace ReviewMe.Core.Services.QueryServices.ReviewersService;

public class Reviewer
{
    public int EmployeeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsSelected { get; set; }
    public int ProjectId { get; set; }
    public bool IsProjectManager { get; set; }

}
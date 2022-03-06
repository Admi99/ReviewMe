namespace ReviewMe.Core.Services.QueryServices.TimurService;

public class ProjectResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ProjectManager ProjectManager { get; set; } = new();
}

public class ProjectManager
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
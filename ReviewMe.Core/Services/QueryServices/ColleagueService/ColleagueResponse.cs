using System.Text.Json.Serialization;

namespace ReviewMe.Core.Services.QueryServices.ColleagueService;

public class ColleagueResponse
{
    [JsonPropertyName("Id")]
    public int TimurId { get; set; }

    public string Name { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
}
using Microsoft.Extensions.Configuration;

namespace ReviewMe.Core.Services.QueryServices.TimurService;

public class TimurService : ITimurService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientService _httpClientService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public TimurService(IConfiguration configuration, IHttpClientService httpClientService, IDateTimeProvider dateTimeProvider)
    {
        _configuration = configuration;
        _httpClientService = httpClientService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<IReadOnlyCollection<ColleagueResponse>> GetEmployeeColleaguesAsync(int timurId)
    {
        var timurSection = _configuration.GetSection("TimurApi");
        var timurUrl = timurSection["Url"];

        int monthsOffset = int.TryParse(_configuration.GetValue<string>("ReviewersMonthsOffset"), out monthsOffset) ? monthsOffset : 6;

        var datetime = _dateTimeProvider.Now();
        return await _httpClientService
                   .GetFromJsonAsync<IReadOnlyCollection<ColleagueResponse>>($"{timurUrl}/colleagues/id/{timurId}" +
                                                                             $"?dateFrom={datetime.Date.AddMonths((-1) * monthsOffset):dd.MM.yyyy}" +
                                                                             $"&dateTo={datetime.Date:dd.MM.yyyy}")
               ?? new List<ColleagueResponse>();
    }

    public async Task<IReadOnlyCollection<ProjectResponse>> GetAllProjectsAsync()
    {
        var timurSection = _configuration.GetSection("TimurApi");
        var timurUrl = timurSection["Url"];

        return await _httpClientService
            .GetFromJsonAsync<IReadOnlyCollection<ProjectResponse>>($"{timurUrl}/projects")
               ?? new List<ProjectResponse>();
    }

}
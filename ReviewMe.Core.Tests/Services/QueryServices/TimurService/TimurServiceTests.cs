using ReviewMe.Core.Services.QueryServices.TimurService;

namespace ReviewMe.Core.Tests.Services.QueryServices.TimurService;

[TestFixture]
public class TimurServiceTests
{
    [Test]
    public async Task GetEmployeeColleaguesAsync()
    {
        // Arrange
        var now = DateTimeOffset.Now;

        var configurationSection = Substitute.For<IConfigurationSection>();
        configurationSection["Url"].Returns("https://timur.domain.local:8080/v2");

        var employeeColleagueTimurId1 = 196;
        var employeeColleagueSurnameFirstName1 = "Brown John";
        var employeeColleagueProjectName1 = "Avarda";

        var employeeColleagueTimurId2 = 197;
        var employeeColleagueSurnameFirstName2 = "Red John";
        var employeeColleagueProjectName2 = "Interflex";

        var expectedResult = new List<ColleagueResponse>
        {
            new()
            {
                TimurId = employeeColleagueTimurId1,
                Name = employeeColleagueSurnameFirstName1,
                ProjectName = employeeColleagueProjectName1,
                ProjectId = 10230
            },
            new()
            {
                TimurId = employeeColleagueTimurId2,
                Name = employeeColleagueSurnameFirstName2,
                ProjectName = employeeColleagueProjectName2,
                ProjectId = 10231
            }
        };

        var configuration = Substitute.For<IConfiguration>();
        configuration.GetSection("ReviewersMonthsOffset").Value.Returns("8");
        configuration.GetSection("TimurApi").Returns(configurationSection);

        var httpClientService = Substitute.For<IHttpClientService>();
        httpClientService.GetFromJsonAsync<IReadOnlyCollection<ColleagueResponse>>(
            "https://timur.domain.local:8080/v2/colleagues/id/191" +
            $"?dateFrom={now.Date.AddMonths((-1) * 8):dd.MM.yyyy}" +
            $"&dateTo={now.Date:dd.MM.yyyy}").Returns(expectedResult);

        var dateTimeProvider = Substitute.For<IDateTimeProvider>();
        dateTimeProvider.Now().Returns(now);

        var testee = new Core.Services.QueryServices.TimurService.TimurService(configuration, httpClientService, dateTimeProvider);

        // Act
        var actualResult = await testee.GetEmployeeColleaguesAsync(191);

        // Assert
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}

namespace ReviewMe.Core.Services.QueryServices.ColleagueService;

public class ColleagueService : IColleagueService
{
    public IReadOnlyCollection<ProjectResponse> GetAllProjectsAsync()
        => MockProjects();

    public IReadOnlyCollection<ColleagueResponse> GetEmployeeColleaguesAsync(int timurId)
    {
        var t = MockEmployeeColleagues()
        .Where(colleague => colleague.TimurId == timurId)
        .ToList();

        var project = MockEmployeeColleagues()
            .FirstOrDefault(a => a.TimurId == timurId);

        if(project == null) return new List<ColleagueResponse>();

        return MockEmployeeColleagues()
            .Where(a => a.TimurId != timurId)
            .Where(a => a.ProjectId == project.ProjectId)
            .ToList();
    } 

    private IReadOnlyCollection<ColleagueResponse> MockEmployeeColleagues() 
        => new List<ColleagueResponse>
        {
            new ColleagueResponse
            {
                TimurId = 1056,
                Name = "Jech, Jan",
                ProjectId = 100,
                ProjectName = "Free Bank"
            },
            new ColleagueResponse
            {
                TimurId = 1057,
                Name = "Holzmann, Felix",
                ProjectId = 101,
                ProjectName = "Talents"
            },
            new ColleagueResponse
            {
                TimurId = 1058,
                Name = "Donutil, Mirek",
                ProjectId = 101,
                ProjectName = "Talents"
            },
            new ColleagueResponse
            {
                TimurId = 1059,
                Name = "Polivka, Bolek",
                ProjectId = 100,
                ProjectName = "Free Bank"
            },
            new ColleagueResponse
            {
                TimurId = 1061,
                Name = "Dáda Patrasová",
                ProjectId = 100,
                ProjectName = "Free Bank"
            },
            new ColleagueResponse
            {
                TimurId = 1062,
                Name = "Pepa z Depa",
                ProjectId = 100,
                ProjectName = "Free Bank"
            },
            new ColleagueResponse
            {
                TimurId = 1063,
                Name = "Pepa bez Depa",
                ProjectId = 102,
                ProjectName = "Hr"
            },
              new ColleagueResponse
            {
                TimurId = 1064,
                Name = "Wolfeschlegelsteinhausenbergerdorff, Hubert",
                ProjectId = 102,
                ProjectName = "Hr"
            },
        };

    private IReadOnlyCollection<ProjectResponse> MockProjects()
        => new List<ProjectResponse>
        {
            new ProjectResponse
            {
                Id = 100,
                Name = "Free Bank",
                ProjectManager = new ProjectManager
                {
                    Id = 150,
                    Name = "Doe John"
                }
            },
            new ProjectResponse
            {
                Id = 101,
                Name = "Talents",
                ProjectManager = new ProjectManager
                {
                    Id = 151,
                    Name = "Procházka Karel"
                }
            },
            new ProjectResponse
            {
                Id = 102,
                Name = "Talents",
                ProjectManager = new ProjectManager
                {
                    Id = 152,
                    Name = "Hr"
                }
            }
        };
}
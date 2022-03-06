namespace ReviewMe.Infrastructure.DbStorage;

public class AutoMapProfile : Profile
{
    public AutoMapProfile()
    {
        CreateMap<Employee, EmployeeDo>()
            .ForMember(dest => dest.AssessmentReviewers, opt => opt.MapFrom(src => src.AssessmentReviewers))
            .ForMember(dest => dest.Assessments, opt => opt.MapFrom(src => src.Assessments))
            .ReverseMap();

        CreateMap<AssessmentReviewer, AssessmentReviewerDo>()
            .ForMember(dest => dest.EmployeeDo, opt => opt.MapFrom(src => src.Employee))
            .ForMember(dest => dest.EmployeeDoId, opt => opt.MapFrom(src => src.EmployeeId))
            .ForMember(dest => dest.AssessmentDoId, opt => opt.MapFrom(src => src.AssessmentId))
            .ForMember(dest => dest.AssessmentDo, opt => opt.MapFrom(src => src.Assessment))
            .ReverseMap();

        CreateMap<Assessment, AssessmentDo>()
            .ForMember(dest => dest.EmployeeDoId, opt => opt.MapFrom(src => src.EmployeeId))
            .ForMember(dest => dest.EmployeeDo, opt => opt.MapFrom(src => src.Employee))
            .ForMember(dest => dest.AssessmentReviewers, opt => opt.MapFrom(src => src.AssessmentReviewers))
            .ForMember(dest => dest.CreatedByEmployeeDoId, opt => opt.MapFrom(src => src.CreatedByEmployeeId))
            .ForMember(dest => dest.CreatedByEmployeeDo, opt => opt.MapFrom(src => src.CreatedByEmployee))
            .ReverseMap();
    }
}
using ReviewMe.Infrastructure.RabbitMQConsumer.Objects;

namespace ReviewMe.Infrastructure.RabbitMQConsumer;

public class AutoMapProfile : Profile
{
    public AutoMapProfile()
    {
        CreateMap<EmployeeRo, Employee>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ContractId))
            .ForMember(dest => dest.TimurId, opt => opt.MapFrom(src => src.TimurId))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => Convert.ToBoolean(src.IsActive)))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Branch))
            .ForMember(dest => dest.Login, opt => opt.NullSubstitute(string.Empty))
            .ForMember(dest => dest.SurnameFirstName, opt => opt.NullSubstitute(string.Empty))
            .ForMember(dest => dest.TeamLeaderLogin, opt => opt.NullSubstitute(string.Empty));


    }
}
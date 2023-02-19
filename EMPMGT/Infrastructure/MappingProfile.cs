using AutoMapper;
using EMPMGT.Model.DTO;
using EMPMGT.Model.PageViewModel;
using EMPMGT.Model.Tentant;

namespace EMPMGT.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserPageViewModel, UserPageViewModel>()
               .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
               .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
               .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Email))
               .ForMember(dest => dest.IsActive, opts => opts.MapFrom(src => src.IsActive))
               .ForMember(dest => dest.IsActiveFormatted, opts => opts.MapFrom(src => src.IsActiveFormatted))
               .ReverseMap();

            CreateMap<EmployeePageViewModel, EmployeePageViewModel>()
               .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
               .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
               .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Email))
               .ForMember(dest => dest.DateOfBirthFormatted, opts => opts.MapFrom(src => src.DateOfBirthFormatted))
               .ForMember(dest => dest.Department, opts => opts.MapFrom(src => src.Department))
               .ReverseMap();
        }


    }
}

using AutoMapper;
using RoboticsManagement.Models;
using RoboticsManagement.ViewModels;

namespace RoboticsManagement.Configuration
{
    public class AutoMapperConfig
    {
        private IMapper _mapper;
        public AutoMapperConfig()
        {
            _mapper = new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<ApplicationUser, CompanyInfoViewModel>().ReverseMap();

           }).CreateMapper();
        }
        public CompanyInfoViewModel MapToCompanyInfoViewModel(ApplicationUser appUser)
        {
            return _mapper.Map<CompanyInfoViewModel>(appUser);
        }
    }
}

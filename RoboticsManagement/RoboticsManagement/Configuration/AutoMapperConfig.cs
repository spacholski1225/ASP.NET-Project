using AutoMapper;
using RoboticsManagement.Models;
using RoboticsManagement.Models.ComplaintForm;
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
               cfg.CreateMap<FormModel, SentFormViewModel>().ForMember(x => x.Robot,
                   x => x.MapFrom(y=> y.ERobotsCategory.ToString())).ForMember(x => x.FormId,
                   x => x.MapFrom(y => y.Id)).ReverseMap();
               cfg.CreateMap<CompanyRegistartionViewModel, ApplicationUser>().ForMember(x => x.UserName,
                   x => x.MapFrom(y => y.Email)).ReverseMap();
               cfg.CreateMap<EmployeeRegistrationViewModel, ApplicationUser>().ReverseMap();

           }).CreateMapper();
        }
        public CompanyInfoViewModel MapApplicationUserToCompanyInfoViewModel(ApplicationUser appUser)
        {
            return _mapper.Map<CompanyInfoViewModel>(appUser);
        }
        public SentFormViewModel MapFormModelToSentFormViewModel(FormModel formModel)
        {
            return _mapper.Map<SentFormViewModel>(formModel);
        }
        public ApplicationUser MapCompanyRegistartionViewModelToApplicationUser(CompanyRegistartionViewModel companyRegistartionViewModel)
        {
            return _mapper.Map<ApplicationUser>(companyRegistartionViewModel);
        } 
        public ApplicationUser MapEmployeeRegistrationViewModelToApplicationUser(EmployeeRegistrationViewModel employeeRegistrationViewModel)
        {
            return _mapper.Map<ApplicationUser>(employeeRegistrationViewModel);
        }
    }
}

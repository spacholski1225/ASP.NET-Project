using AutoMapper;
using RoboticsManagement.Models;
using RoboticsManagement.Models.ComplaintForm;
using RoboticsManagement.Models.Home;
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
                   x => x.MapFrom(y => y.ERobotsCategory.ToString())).ForMember(x => x.FormId,
                   x => x.MapFrom(y => y.Id)).ReverseMap();
               cfg.CreateMap<CompanyRegistartionViewModel, ApplicationUser>().ForMember(x => x.UserName,
                   x => x.MapFrom(y => y.Email)).ReverseMap();
               cfg.CreateMap<ApplicationUser, TaskViewModel>().ForMember(x => x.AppUserId, x => x.MapFrom(y => y.Id));
               cfg.CreateMap<ApplicationUser, EmployeeTaskViewModel>().ForMember(x => x.EmployeeId, x => x.MapFrom(y => y.Id));
               cfg.CreateMap<EmployeeTask, EmployeeTaskViewModel>();
               cfg.CreateMap<SummaryViewModel, FormModel>();
               cfg.CreateMap<SummaryViewModel, EmployeeTask>().ForMember(x => x.AppUserId, x => x.MapFrom(y => y.UserId));
               cfg.CreateMap<ApplicationUser, SummaryViewModel>().ForMember(x => x.Company, x => x.MapFrom(y => y.CompanyName))
                                                                 .ForMember(x => x.UserId, x => x.MapFrom(y => y.Id));
               cfg.CreateMap<ContactViewModel, Contact>();

           }).CreateMapper();
        }
        public Contact MapContactViewModelToContact(ContactViewModel contactViewModel)
        {
            return _mapper.Map<Contact>(contactViewModel);
        }
        public SummaryViewModel MapApplicationUserToSummaryViewModel(ApplicationUser appUser)
        {
            return _mapper.Map<SummaryViewModel>(appUser);
        }
        public EmployeeTask MapSummaryViewModelToEmployeeTask(SummaryViewModel summaryViewModel)
        {
            return _mapper.Map<EmployeeTask>(summaryViewModel);
        }
        public FormModel MapSummaryViewModelToFormModel(SummaryViewModel summaryViewModel)
        {
            return _mapper.Map<FormModel>(summaryViewModel);
        }
        public EmployeeTaskViewModel MapEmployeeTaskToEmployeeTaskViewModel(EmployeeTask empTask)
        {
            return _mapper.Map<EmployeeTaskViewModel>(empTask);

        }

        public EmployeeTaskViewModel MapApplicationUserToEmployeeTaskViewModel(ApplicationUser appUser)
        {
            return _mapper.Map<EmployeeTaskViewModel>(appUser);
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
        public TaskViewModel MapApplicationUserToTaskViewModel(ApplicationUser appUser)
        {
            return _mapper.Map<TaskViewModel>(appUser);
        }
        public ApplicationUser MapEmployeeRegistrationViewModelToApplicationUser(EmployeeRegistrationViewModel employeeRegistrationViewModel)
        {
            return _mapper.Map<ApplicationUser>(employeeRegistrationViewModel);
        }
    }
}

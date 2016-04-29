using AutoMapper;
using DAIS.App.MVC.Models;
using DAIS.ORM.DTO;

namespace DAIS.App.MVC.App_Start
{
    public class AutoMapperConfig
    {
        public static void RegisterAutoMapper()
        {
            Mapper.CreateMap<IssueDTO, IssueDetailModel>()
                .ForMember(m => m.CreatedBy, m => m.Ignore())
                .ForMember(m => m.Assignee, m => m.Ignore());

            Mapper.CreateMap<UserDTO, UserModel>()
                .ForMember(m => m.LastName, m => m.MapFrom(p => p.Surname));

            Mapper.CreateMap<IssueDTO, IssueEditModel>()
                .ForMember(m => m.CreatedBy, m => m.Ignore())
                .ForMember(m => m.Assignee, m => m.Ignore())
                .ForMember(m => m.AssigneeList, m => m.Ignore())
                .ForMember(m => m.EstimatedTime, m => m.MapFrom(p => p.EstimatedTime.TotalHours))
                .ForMember(m => m.RemainingTime, m => m.MapFrom(p => p.RemainingTime.TotalHours));
        }
    }
}
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.WorkItems.Common
{
    public class ShallowWorkItemDtoMappingProfile : Profile
    {
        public ShallowWorkItemDtoMappingProfile()
        {
            CreateMap<WorkItem, ShallowWorkItemDto>()
                .ForMember(item => item.AssignedTo, options => options.MapFrom(original => original.AssignedTo.Id));
        }
    }
}

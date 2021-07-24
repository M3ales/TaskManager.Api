using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Api.Application.Common.Mappings
{
    // From: https://github.com/jasontaylordev/CleanArchitecture/blob/80f70bb3caeca90d8325052c32a6d3217cb1e26b/src/Application/Common/Mappings/IMapFrom.cs
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}

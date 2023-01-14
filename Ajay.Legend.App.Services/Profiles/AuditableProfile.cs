using System;
using System.Security.AccessControl;
using AutoMapper;
using Ajay.Legend.App.Models;
using Ajay.Legend.App.Repositories;
using Ajay.Legend.App.Repositories.Entities;

namespace Ajay.Legend.App.Services.Profiles
{
    public class AuditableProfile : Profile
    {
        public AuditableProfile()
        {
            CreateMap<AuditableModel, AuditableEntity>()
            .ForMember(dest => dest.ChangedByUserId, opt => opt.MapFrom(src => src.ChangedByUserId))
            .ForMember(dest => dest.AuditId, opt => opt.MapFrom(src => src.AuditId))
            .ReverseMap()
            ;
        }
    }
}

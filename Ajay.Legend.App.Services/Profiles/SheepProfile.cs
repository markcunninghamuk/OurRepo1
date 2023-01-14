using System;
using System.Collections.Generic;
using AutoMapper;
using Ajay.Legend.App.Models;
using Ajay.Legend.App.Repositories.Entities;

namespace Ajay.Legend.App.Services.Profiles
{
    public class SheepProfile : Profile
    {
        public SheepProfile()
        {
            CreateMap<SheepModel, Sheep>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.ColourType, opt => opt.MapFrom(src => src.ColourType))
            .ForMember(dest => dest.VisualAttributesAsCsv, opt => opt.MapFrom(src => string.Join("|", src.VisualAttributes ?? new List<string>())))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.MedicalHistory, opt => opt.MapFrom(src => src.MedicalHistory))
            ;

            CreateMap<Sheep, SheepModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.ColourType, opt => opt.MapFrom(src => src.ColourType))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.MedicalHistory, opt => opt.MapFrom(src => src.MedicalHistory))
            .AfterMap((sheep, sheepModel) => sheepModel.VisualAttributes = 
                (sheep.VisualAttributesAsCsv != null)
                ? sheep.VisualAttributesAsCsv.Split('|').ToList()
                : new List<string>());
            ;
        }
    }
}

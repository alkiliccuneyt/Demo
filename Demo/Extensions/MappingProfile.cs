using AutoMapper;
using Demo.Data.Models;
using Demo.Dtos;
using Demo.Views;

// ReSharper disable All

namespace Demo.Extensions;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PayTypeEntity, ConstantView>()
            .ForMember(d => d.Key, map => map.MapFrom(s => s.Id))
            .ForMember(d => d.Value, map => map.MapFrom(s => GlobalExtensions.StringCapitalize(GlobalExtensions.TurkishCharacterToEnglish(s.Deffination))))
            .ReverseMap();
        
        CreateMap<PayTypeEntity, PayTypeAddDto>()
            .ReverseMap();
        
        CreateMap<PayTypeEntity, PayTypeUpdateDto>()
            .ForMember(d => d.PayTypeId, map => map.MapFrom(s => s.Id))
            .ReverseMap();
        
        CreateMap<PeriodEntity, ConstantView>()
            .ForMember(d => d.Key, map => map.MapFrom(s => s.Id))
            .ForMember(d => d.Value, map => map.MapFrom(s => GlobalExtensions.StringCapitalize(GlobalExtensions.TurkishCharacterToEnglish(s.Deffination))))
            .ReverseMap();
        
        CreateMap<PeriodEntity, PeriodAddDto>()
            .ReverseMap();
        
        CreateMap<PeriodEntity, PeriodUpdateDto>()
            .ForMember(d => d.PeriodId, map => map.MapFrom(s => s.Id))
            .ReverseMap();
        
        CreateMap<PersonalEntity, PersonalView>()
            .ForMember(d => d.PersonalId, map => map.MapFrom(s => s.IdentityNumber))
            .ForMember(d => d.PersonalName, map => map.MapFrom(s => GlobalExtensions.StringCapitalize(GlobalExtensions.TurkishCharacterToEnglish(s.Name))))
            .ForMember(d => d.PersonalSurname, map => map.MapFrom(s => GlobalExtensions.StringCapitalize(GlobalExtensions.TurkishCharacterToEnglish(s.Surname))))
            .ReverseMap();
        
        CreateMap<PersonalAddUpdateDto, PersonalEntity>()
            .ForMember(d => d.IdentityNumber, map => map.MapFrom(s => s.PersonalId))
            .ForMember(d => d.Name, map => map.MapFrom(s => s.PersonalName))
            .ForMember(d => d.Surname, map => map.MapFrom(s => s.PersonalSurname))
            .ReverseMap();
        
        CreateMap<PersonalPayTypeEntity, PersonalPayTypeAddDto>()
            .ReverseMap();
        
        CreateMap<PayrollAddDto, PersonalPayrollEntity>()
            .ForMember(d => d.PeriodId, map => map.MapFrom(s => s.Period))
            .ReverseMap();
        
        CreateMap<PayrollUpdateDto, PersonalPayrollEntity>()
            .ForMember(d => d.Id, map => map.MapFrom(s => s.PayrollId))
            .ForMember(d => d.PeriodId, map => map.MapFrom(s => s.Period))
            .ReverseMap();
    }
}
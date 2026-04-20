using AutoMapper;
using CaseItau.Application.DTOs;
using CaseItau.Domain.Entities;

namespace CaseItau.Application.Mappings;

public class FundoProfile : Profile
{
    public FundoProfile()
    {
        CreateMap<Fundo, FundoDto>()
            .ForMember(d => d.NomeTipo, o => o.MapFrom(s => s.TipoFundo.Nome));

        CreateMap<CreateFundoDto, Fundo>();

        CreateMap<TipoFundo, TipoFundoDto>();
    }
}

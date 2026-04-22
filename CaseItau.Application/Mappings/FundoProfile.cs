using AutoMapper;
using CaseItau.Application.DTOs;
using CaseItau.Domain.Entities;

namespace CaseItau.Application.Mappings;

public class FundoProfile : Profile
{
    public FundoProfile()
    {
        CreateMap<Fundo, FundoDto>()
            .ConstructUsing(s => new FundoDto(
                s.Codigo,
                s.Nome,
                s.Cnpj,
                s.CodigoTipo,
                s.TipoFundo.Nome,
                s.Patrimonio));

        CreateMap<CreateFundoDto, Fundo>()
            .ForMember(d => d.TipoFundo, o => o.Ignore());

        CreateMap<TipoFundo, TipoFundoDto>()
            .ConstructUsing(s => new TipoFundoDto(s.Codigo, s.Nome));
    }
}

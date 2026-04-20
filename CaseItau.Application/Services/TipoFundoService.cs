using AutoMapper;
using CaseItau.Application.DTOs;
using CaseItau.Application.Exceptions;
using CaseItau.Application.Interfaces;
using CaseItau.Domain.Entities;
using CaseItau.Domain.Interfaces;

namespace CaseItau.Application.Services;

public class TipoFundoService : ITipoFundoService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public TipoFundoService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TipoFundoDto>> GetAllAsync()
    {
        var tipos = await _uow.TiposFundo.GetAllAsync();
        return _mapper.Map<IEnumerable<TipoFundoDto>>(tipos);
    }

    public async Task<TipoFundoDto> GetByCodigoAsync(int codigo)
    {
        var tipo = await _uow.TiposFundo.GetByCodigoAsync(codigo)
            ?? throw new NotFoundException(nameof(TipoFundo), codigo);

        return _mapper.Map<TipoFundoDto>(tipo);
    }
}

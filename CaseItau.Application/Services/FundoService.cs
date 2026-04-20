using AutoMapper;
using CaseItau.Application.DTOs;
using CaseItau.Application.Exceptions;
using CaseItau.Application.Interfaces;
using CaseItau.Domain.Entities;
using CaseItau.Domain.Interfaces;

namespace CaseItau.Application.Services;

public class FundoService : IFundoService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public FundoService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FundoDto>> GetAllAsync()
    {
        var fundos = await _uow.Fundos.GetAllAsync();
        return _mapper.Map<IEnumerable<FundoDto>>(fundos);
    }

    public async Task<FundoDto> GetByCodigoAsync(string codigo)
    {
        var fundo = await _uow.Fundos.GetByCodigoAsync(codigo)
            ?? throw new NotFoundException(nameof(Fundo), codigo);

        return _mapper.Map<FundoDto>(fundo);
    }

    public async Task<FundoDto> CreateAsync(CreateFundoDto dto)
    {
        var tipoExiste = await _uow.TiposFundo.GetByCodigoAsync(dto.CodigoTipo);
        if (tipoExiste is null)
            throw new NotFoundException(nameof(TipoFundo), dto.CodigoTipo);

        var fundo = _mapper.Map<Fundo>(dto);
        await _uow.Fundos.AddAsync(fundo);
        await _uow.CommitAsync();

        return await GetByCodigoAsync(fundo.Codigo);
    }

    public async Task UpdateAsync(string codigo, UpdateFundoDto dto)
    {
        var fundo = await _uow.Fundos.GetByCodigoAsync(codigo)
            ?? throw new NotFoundException(nameof(Fundo), codigo);

        var tipoExiste = await _uow.TiposFundo.GetByCodigoAsync(dto.CodigoTipo);
        if (tipoExiste is null)
            throw new NotFoundException(nameof(TipoFundo), dto.CodigoTipo);

        fundo.Nome = dto.Nome;
        fundo.Cnpj = dto.Cnpj;
        fundo.CodigoTipo = dto.CodigoTipo;

        _uow.Fundos.Update(fundo);
        await _uow.CommitAsync();
    }

    public async Task DeleteAsync(string codigo)
    {
        var fundo = await _uow.Fundos.GetByCodigoAsync(codigo)
            ?? throw new NotFoundException(nameof(Fundo), codigo);

        _uow.Fundos.Delete(fundo);
        await _uow.CommitAsync();
    }

    public async Task MovimentarPatrimonioAsync(string codigo, decimal valor)
    {
        var fundo = await _uow.Fundos.GetByCodigoAsync(codigo)
            ?? throw new NotFoundException(nameof(Fundo), codigo);

        fundo.Patrimonio = (fundo.Patrimonio ?? 0) + valor;

        _uow.Fundos.Update(fundo);
        await _uow.CommitAsync();
    }
}

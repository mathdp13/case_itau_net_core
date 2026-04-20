using AutoMapper;
using CaseItau.Application.DTOs;
using CaseItau.Application.Exceptions;
using CaseItau.Application.Services;
using CaseItau.Domain.Entities;
using CaseItau.Domain.Interfaces;
using Moq;

namespace CaseItau.UnitTests;

public class FundoServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock = new();
    private readonly Mock<IFundoRepository> _fundoRepoMock = new();
    private readonly Mock<ITipoFundoRepository> _tipoRepoMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly FundoService _service;

    public FundoServiceTests()
    {
        _uowMock.Setup(u => u.Fundos).Returns(_fundoRepoMock.Object);
        _uowMock.Setup(u => u.TiposFundo).Returns(_tipoRepoMock.Object);
        _service = new FundoService(_uowMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedList()
    {
        var fundos = new List<Fundo> { new() { Codigo = "F001", Nome = "Fundo A", TipoFundo = new TipoFundo { Nome = "Renda Fixa" } } };
        var dtos = new List<FundoDto> { new("F001", "Fundo A", "00000000000000", 1, "Renda Fixa", null) };
        _fundoRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(fundos);
        _mapperMock.Setup(m => m.Map<IEnumerable<FundoDto>>(fundos)).Returns(dtos);

        var result = await _service.GetAllAsync();

        Assert.Single(result);
        Assert.Equal("F001", result.First().Codigo);
    }

    [Fact]
    public async Task GetByCodigoAsync_WhenFound_ShouldReturnDto()
    {
        var fundo = new Fundo { Codigo = "F001", Nome = "Fundo A", TipoFundo = new TipoFundo { Nome = "Renda Fixa" } };
        var dto = new FundoDto("F001", "Fundo A", "60701190000104", 1, "Renda Fixa", null);
        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("F001")).ReturnsAsync(fundo);
        _mapperMock.Setup(m => m.Map<FundoDto>(fundo)).Returns(dto);

        var result = await _service.GetByCodigoAsync("F001");

        Assert.Equal("F001", result.Codigo);
    }

    [Fact]
    public async Task GetByCodigoAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("XXX")).ReturnsAsync((Fundo?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByCodigoAsync("XXX"));
    }

    [Fact]
    public async Task CreateAsync_WhenTipoFundoNotFound_ShouldThrowNotFoundException()
    {
        var dto = new CreateFundoDto("F001", "Fundo A", "60701190000104", 99);
        _tipoRepoMock.Setup(r => r.GetByCodigoAsync(99)).ReturnsAsync((TipoFundo?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_WhenValid_ShouldAddAndCommit()
    {
        var dto = new CreateFundoDto("F001", "Fundo A", "60701190000104", 1);
        var tipo = new TipoFundo { Codigo = 1, Nome = "Renda Fixa" };
        var fundo = new Fundo { Codigo = "F001", Nome = "Fundo A", TipoFundo = tipo };
        var fundoDto = new FundoDto("F001", "Fundo A", "60701190000104", 1, "Renda Fixa", null);

        _tipoRepoMock.Setup(r => r.GetByCodigoAsync(1)).ReturnsAsync(tipo);
        _mapperMock.Setup(m => m.Map<Fundo>(dto)).Returns(fundo);
        _uowMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);
        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("F001")).ReturnsAsync(fundo);
        _mapperMock.Setup(m => m.Map<FundoDto>(fundo)).Returns(fundoDto);

        var result = await _service.CreateAsync(dto);

        _fundoRepoMock.Verify(r => r.AddAsync(fundo), Times.Once);
        _uowMock.Verify(u => u.CommitAsync(), Times.Once);
        Assert.Equal("F001", result.Codigo);
    }

    [Fact]
    public async Task UpdateAsync_WhenFundoNotFound_ShouldThrowNotFoundException()
    {
        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("XXX")).ReturnsAsync((Fundo?)null);
        var dto = new UpdateFundoDto("Fundo B", "60701190000104", 1);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync("XXX", dto));
    }

    [Fact]
    public async Task UpdateAsync_WhenTipoNotFound_ShouldThrowNotFoundException()
    {
        var fundo = new Fundo { Codigo = "F001", Nome = "Fundo A" };
        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("F001")).ReturnsAsync(fundo);
        _tipoRepoMock.Setup(r => r.GetByCodigoAsync(99)).ReturnsAsync((TipoFundo?)null);
        var dto = new UpdateFundoDto("Fundo B", "60701190000104", 99);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync("F001", dto));
    }

    [Fact]
    public async Task UpdateAsync_WhenValid_ShouldMutateEntityAndCommit()
    {
        var fundo = new Fundo { Codigo = "F001", Nome = "Fundo A", Cnpj = "old", CodigoTipo = 1 };
        var tipo = new TipoFundo { Codigo = 2, Nome = "Ações" };
        var dto = new UpdateFundoDto("Fundo B", "60701190000104", 2);

        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("F001")).ReturnsAsync(fundo);
        _tipoRepoMock.Setup(r => r.GetByCodigoAsync(2)).ReturnsAsync(tipo);
        _uowMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        await _service.UpdateAsync("F001", dto);

        Assert.Equal("Fundo B", fundo.Nome);
        Assert.Equal("60701190000104", fundo.Cnpj);
        Assert.Equal(2, fundo.CodigoTipo);
        _uowMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("XXX")).ReturnsAsync((Fundo?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteAsync("XXX"));
    }

    [Fact]
    public async Task DeleteAsync_WhenFound_ShouldDeleteAndCommit()
    {
        var fundo = new Fundo { Codigo = "F001" };
        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("F001")).ReturnsAsync(fundo);
        _uowMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        await _service.DeleteAsync("F001");

        _fundoRepoMock.Verify(r => r.Delete(fundo), Times.Once);
        _uowMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task MovimentarPatrimonioAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("XXX")).ReturnsAsync((Fundo?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.MovimentarPatrimonioAsync("XXX", 1000m));
    }

    [Fact]
    public async Task MovimentarPatrimonioAsync_WhenPatrimonioIsNull_ShouldStartFromZero()
    {
        var fundo = new Fundo { Codigo = "F001", Patrimonio = null };
        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("F001")).ReturnsAsync(fundo);
        _uowMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        await _service.MovimentarPatrimonioAsync("F001", 5000m);

        Assert.Equal(5000m, fundo.Patrimonio);
        _uowMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task MovimentarPatrimonioAsync_WhenPatrimonioExists_ShouldAccumulate()
    {
        var fundo = new Fundo { Codigo = "F001", Patrimonio = 10000m };
        _fundoRepoMock.Setup(r => r.GetByCodigoAsync("F001")).ReturnsAsync(fundo);
        _uowMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

        await _service.MovimentarPatrimonioAsync("F001", 3000m);

        Assert.Equal(13000m, fundo.Patrimonio);
        _uowMock.Verify(u => u.CommitAsync(), Times.Once);
    }
}

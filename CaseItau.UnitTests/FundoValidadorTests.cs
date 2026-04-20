using CaseItau.Application.DTOs;
using CaseItau.Application.Validators;
using FluentValidation.TestHelper;

namespace CaseItau.UnitTests;

public class CreateFundoValidatorTests
{
    private readonly CreateFundoValidator _validator = new();

    [Fact]
    public void Should_Pass_When_All_Fields_Are_Valid()
    {
        var dto = new CreateFundoDto("F001", "Fundo Real", "60701190000104", 1);
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_CNPJ_Is_Invalid()
    {
        var dto = new CreateFundoDto("F001", "Fundo Teste", "12345678901234", 1);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Cnpj).WithErrorMessage("CNPJ inválido.");
    }

    [Fact]
    public void Should_Fail_When_CNPJ_Has_All_Same_Digits()
    {
        var dto = new CreateFundoDto("F001", "Fundo Teste", "00000000000000", 1);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Cnpj);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Should_Fail_When_Codigo_Is_Empty(string codigo)
    {
        var dto = new CreateFundoDto(codigo, "Fundo Teste", "60701190000104", 1);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Codigo);
    }

    [Fact]
    public void Should_Fail_When_Codigo_Exceeds_MaxLength()
    {
        var dto = new CreateFundoDto(new string('X', 21), "Fundo Teste", "60701190000104", 1);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Codigo);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Should_Fail_When_Nome_Is_Empty(string nome)
    {
        var dto = new CreateFundoDto("F001", nome, "60701190000104", 1);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Nome);
    }

    [Fact]
    public void Should_Fail_When_Nome_Exceeds_MaxLength()
    {
        var dto = new CreateFundoDto("F001", new string('A', 201), "60701190000104", 1);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Nome);
    }

    [Fact]
    public void Should_Fail_When_CodigoTipo_Is_Zero()
    {
        var dto = new CreateFundoDto("F001", "Fundo Teste", "60701190000104", 0);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.CodigoTipo);
    }

    [Fact]
    public void Should_Fail_When_CodigoTipo_Is_Negative()
    {
        var dto = new CreateFundoDto("F001", "Fundo Teste", "60701190000104", -1);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.CodigoTipo);
    }
}

using CaseItau.Application.DTOs;
using CaseItau.Application.Validators;
using FluentValidation.TestHelper;

namespace CaseItau.UnitTests;

public class UpdateFundoValidatorTests
{
    private readonly UpdateFundoValidator _validator = new();

    [Fact]
    public void Should_Pass_When_All_Fields_Are_Valid()
    {
        var dto = new UpdateFundoDto("Fundo Real", "60701190000104", 1);
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Should_Fail_When_Nome_Is_Empty(string nome)
    {
        var dto = new UpdateFundoDto(nome, "60701190000104", 1);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Nome);
    }

    [Fact]
    public void Should_Fail_When_Nome_Exceeds_MaxLength()
    {
        var dto = new UpdateFundoDto(new string('A', 201), "60701190000104", 1);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Nome);
    }

    [Fact]
    public void Should_Fail_When_CNPJ_Is_Invalid()
    {
        var dto = new UpdateFundoDto("Fundo Teste", "12345678901234", 1);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Cnpj).WithErrorMessage("CNPJ inválido.");
    }

    [Fact]
    public void Should_Fail_When_CNPJ_Has_All_Same_Digits()
    {
        var dto = new UpdateFundoDto("Fundo Teste", "11111111111111", 1);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Cnpj);
    }

    [Fact]
    public void Should_Fail_When_CodigoTipo_Is_Zero()
    {
        var dto = new UpdateFundoDto("Fundo Teste", "60701190000104", 0);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.CodigoTipo);
    }

    [Fact]
    public void Should_Fail_When_CodigoTipo_Is_Negative()
    {
        var dto = new UpdateFundoDto("Fundo Teste", "60701190000104", -1);
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.CodigoTipo);
    }
}

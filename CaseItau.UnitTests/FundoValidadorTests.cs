using CaseItau.Application.DTOs;
using CaseItau.Application.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace CaseItau.UnitTests
{
    public class FundoValidatorTests
    {
        private readonly CreateFundoValidator _validator;

        public FundoValidatorTests()
        {
            _validator = new CreateFundoValidator();
        }

        [Fact]
        public void Should_Have_Error_When_CNPJ_Is_Invalid()
        {
            // Arrange 
            // Ordem: Codigo, Nome, CNPJ, CodigoTipo
            var request = new CreateFundoDto("F001", "Fundo Teste", "12345678901234", 1);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Cnpj)
                  .WithErrorMessage("CNPJ inválido.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_CNPJ_Is_Valid()
        {
            // Arrange
            // Usando um CNPJ válido para garantir que o validador aceita dados corretos
            var request = new CreateFundoDto("F001", "Fundo Real", "60701190000104", 1);

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
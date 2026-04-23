using CaseItau.Application.DTOs;
using FluentValidation;

namespace CaseItau.Application.Validators;

public class CreateFundoValidator : AbstractValidator<CreateFundoDto>
{
    public CreateFundoValidator()
    {
        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("Código é obrigatório.")
            .MaximumLength(20).WithMessage("Código deve ter no máximo 20 caracteres.");

        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Cnpj)
            .NotEmpty().WithMessage("CNPJ é obrigatório.")
            .Must(CnpjValidation.IsValid).WithMessage("CNPJ inválido.");

        RuleFor(x => x.CodigoTipo)
            .GreaterThan(0).WithMessage("Tipo de fundo inválido.");
    }
}

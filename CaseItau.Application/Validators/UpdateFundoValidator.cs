using CaseItau.Application.DTOs;
using FluentValidation;

namespace CaseItau.Application.Validators;

public class UpdateFundoValidator : AbstractValidator<UpdateFundoDto>
{
    public UpdateFundoValidator()
    {
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

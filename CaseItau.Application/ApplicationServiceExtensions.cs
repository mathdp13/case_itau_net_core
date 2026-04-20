using CaseItau.Application.Interfaces;
using CaseItau.Application.Mappings;
using CaseItau.Application.Services;
using CaseItau.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CaseItau.Application;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<FundoProfile>());
        services.AddValidatorsFromAssemblyContaining<CreateFundoValidator>();

        services.AddScoped<IFundoService, FundoService>();
        services.AddScoped<ITipoFundoService, TipoFundoService>();

        return services;
    }
}

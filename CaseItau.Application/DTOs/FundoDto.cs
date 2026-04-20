namespace CaseItau.Application.DTOs;

public record FundoDto(
    string Codigo,
    string Nome,
    string Cnpj,
    int CodigoTipo,
    string NomeTipo,
    decimal? Patrimonio);

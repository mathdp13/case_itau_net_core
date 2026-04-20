namespace CaseItau.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entity, object key)
        : base($"{entity} '{key}' não encontrado.") { }
}

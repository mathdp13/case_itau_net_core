namespace CaseItau.Application.Validators;

internal static class CnpjValidation
{
    internal static bool IsValid(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj)) return false;

        var digits = new string(cnpj.Where(char.IsDigit).ToArray());
        if (digits.Length != 14 || digits.Distinct().Count() == 1) return false;

        return VerificaDigito(digits, 12) && VerificaDigito(digits, 13);
    }

    private static bool VerificaDigito(string digits, int position)
    {
        int[] weights = position == 12
            ? [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2]
            : [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        var sum = digits.Take(position).Select((c, i) => (c - '0') * weights[i]).Sum();
        var remainder = sum % 11;
        var expected = remainder < 2 ? 0 : 11 - remainder;

        return (digits[position] - '0') == expected;
    }
}

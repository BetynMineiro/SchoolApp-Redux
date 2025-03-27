namespace SchoolApp.CrossCutting.Extensions;

public static class ValidationExtension
{
    private static int CalculateMod(int mod) =>
        mod < 2 ? 0 : (11 - mod);

    private static string RemoveChars(string cnpj) =>
        cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");

    public static bool CompanyTaxDocumentValidation(this string taxDocument)
    {
        if (string.IsNullOrEmpty(taxDocument))
        {
            return false;
        }

        int[] a = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] b = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        taxDocument = RemoveChars(taxDocument);

        if (taxDocument.ToCharArray().All(c => c == taxDocument[0]))
        {
            return false;
        }

        if (taxDocument.Length != 14)
        {
            return false;
        }

        if (!long.TryParse(taxDocument, out long x))
        {
            return false;
        }

        var tempTaxCode = taxDocument.Substring(0, 12);

        var count = 0;
        for (int i = 0; i < 12; i++)
        {
            count += int.Parse(tempTaxCode[i].ToString()) * a[i];
        }

        var mod = count % 11;
        mod = CalculateMod(mod);

        var digit = mod.ToString();
        tempTaxCode += digit;
        count = 0;

        for (int i = 0; i < 13; i++)
        {
            count += int.Parse(tempTaxCode[i].ToString()) * b[i];
        }

        mod = count % 11;
        mod = CalculateMod(mod);

        digit += mod.ToString();

        return taxDocument.EndsWith(digit);
    }

    public static bool PersonTaxDocumentValidation(this string cpf)
    {
        if (string.IsNullOrEmpty(cpf))
        {
            return false;
        }

        cpf = RemoveChars(cpf);

        if (cpf.Length != 11)
        {
            return false;
        }

        if (cpf.ToCharArray().All(c => c == cpf[0]))
        {
            return false;
        }

        if (!long.TryParse(cpf, out _))
        {
            return false;
        }

        int[] peso1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] peso2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        var tempCpf = cpf.Substring(0, 9);
        int soma = 0;

        for (int i = 0; i < 9; i++)
        {
            soma += int.Parse(tempCpf[i].ToString()) * peso1[i];
        }

        var mod = soma % 11;
        var primeiroDigito = CalculateMod(mod).ToString();
        tempCpf += primeiroDigito;

        soma = 0;

        for (int i = 0; i < 10; i++)
        {
            soma += int.Parse(tempCpf[i].ToString()) * peso2[i];
        }

        mod = soma % 11;
        var segundoDigito = CalculateMod(mod).ToString();

        return cpf.EndsWith(primeiroDigito + segundoDigito);
    }
}
using System.Globalization;

//Aceita vírgula nos decimais e formata em R$
var ptBR = new CultureInfo("pt-BR");

// Pede o valor da conta. Se digitar algo inválido (ou zero/negativo), encerra.
Console.Write("Valor (ex: 1500,00): ");
if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Number, ptBR, out decimal valor) || valor <= 0)
{
    Console.WriteLine("Valor inválido.");
    return;
}

// Pede a data de vencimento no formato dia/mês/ano.
// ParseExact exige exatamente esse formato, evitando confusão entre dia e mês.
Console.Write("Data de vencimento (dd/MM/yyyy): ");
if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", ptBR, DateTimeStyles.None, out DateTime vencimento))
{
    Console.WriteLine("Data inválida.");
    return;
}

// Quantos dias se passaram desde o vencimento até hoje
int diasAtraso = (DateTime.Today - vencimento.Date).Days;

// Se ainda não venceu (ou vence hoje), não há juros
if (diasAtraso <= 0)
{
    Console.WriteLine("Título não está vencido. Juros: R$ 0,00");
    return;
}

// Taxa de 2,5% ao dia
const decimal taxaDiaria = 0.025m;

// Juros simples
decimal juros = Math.Round(valor * taxaDiaria * diasAtraso, 2);

//resultado
Console.WriteLine($"Dias em atraso: {diasAtraso}");
Console.WriteLine($"Juros: {juros.ToString("C2", ptBR)}");
Console.WriteLine($"Total a pagar: {(valor + juros).ToString("C2", ptBR)}");
using System.Globalization;  
using System.Text.Json;       

//  Gravar a variavel com o json de vendas
var json = File.ReadAllText("vendas.json");

//    Converte o texto JSON em objetos C# (a classe DadosVendas lá embaixo).
//    PropertyNameCaseInsensitive faz "vendas" do JSON casar com "Vendas" do C#.
var dados = JsonSerializer.Deserialize<DadosVendas>(json,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

//Usa vírgula nos decimais e o símbolo R$
var ptBR = new CultureInfo("pt-BR");

//    Agrupa as vendas por vendedor e soma a comissão de cada venda.
//    Exemplo: todas as vendas do "João Silva" ficam juntas num grupo.
var comissoes = dados.Vendas
    .GroupBy(v => v.Vendedor)
    .Select(g => new
    {
        Vendedor = g.Key,                                   // nome do vendedor
        Comissao = g.Sum(v => CalcularComissao(v.Valor))    // soma das comissões das vendas dele
    });

//Resultado
foreach (var c in comissoes)
    Console.WriteLine($"{c.Vendedor}: {c.Comissao.ToString("C2", ptBR)}");

// Função que aplica a regra da comissão em UMA venda
static decimal CalcularComissao(decimal valor)
{
    if (valor < 100m) return 0m;                          //Sem comissão
    if (valor < 500m) return Math.Round(valor * 0.01m, 2); // 1%
    return Math.Round(valor * 0.05m, 2);                  //  5%
}

record Venda(string Vendedor, decimal Valor);
record DadosVendas(List<Venda> Vendas);
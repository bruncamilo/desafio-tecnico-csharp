using System.Text.Json;

// Configurações para ler/gravar o JSON:
var opts = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true
};

// Carrega os produtos e o estoque atual do arquivo
var dados = JsonSerializer.Deserialize<DadosEstoque>(File.ReadAllText("estoque.json"), opts)!;

// Arquivo onde vamos guardar o histórico das movimentações
const string arqMov = "movimentacoes.json";

// Se o histórico já existe, carrega. Se não, começa com uma lista vazia.
var movimentacoes = File.Exists(arqMov)
    ? JsonSerializer.Deserialize<List<Movimentacao>>(File.ReadAllText(arqMov), opts)!
    : new List<Movimentacao>();

// Define o próximo número identificador (ID).
int proximoId = movimentacoes.Count == 0 ? 1 : movimentacoes.Max(m => m.Id) + 1;

// Loop do menu: repete até o usuário digitar 0
while (true)
{
    // Mostra o estoque atual de todos os produtos
    Console.WriteLine("\n=== ESTOQUE ===");
    foreach (var p in dados.Estoque)
        Console.WriteLine($"{p.CodigoProduto} - {p.DescricaoProduto}: {p.Estoque}");

    // Menu de opções
    Console.WriteLine("\n1 - Entrada | 2 - Saída | 0 - Sair");
    Console.Write("Opção: ");
    var opcao = Console.ReadLine();

    if (opcao == "0") break;   // sai do loop e encerra o programa
    if (opcao != "1" && opcao != "2") { Console.WriteLine("Opção inválida."); continue; }

    // Pede o código do produto. TryParse evita erro se a pessoa digitar letras.
    Console.Write("Código do produto: ");
    if (!int.TryParse(Console.ReadLine(), out int codigo)) { Console.WriteLine("Código inválido."); continue; }

    // Procura o produto na lista. Se não achar, volta para o menu.
    var produto = dados.Estoque.FirstOrDefault(p => p.CodigoProduto == codigo);
    if (produto is null) { Console.WriteLine("Produto não encontrado."); continue; }

    // Pede a quantidade (precisa ser um número maior que zero)
    Console.Write("Quantidade: ");
    if (!int.TryParse(Console.ReadLine(), out int qtde) || qtde <= 0) { Console.WriteLine("Quantidade inválida."); continue; }

    // A descrição identifica o motivo da movimentação (pedido do enunciado)
    Console.Write("Descrição da movimentação (ex: Compra, Venda, Devolução, Ajuste): ");
    var descricao = Console.ReadLine() ?? "";

    bool entrada = opcao == "1";   

    // Não deixa tirar mais do que existe no estoque
    if (!entrada && qtde > produto.Estoque)
    {
        Console.WriteLine($"Estoque insuficiente. Disponível: {produto.Estoque}");
        continue;
    }

    // Atualiza o estoque: soma se for entrada, subtrai se for saída
    produto.Estoque += entrada ? qtde : -qtde;

    // Cria o registro da movimentação com o ID único e o aumenta para a próxima (proximoId++)
    var mov = new Movimentacao(proximoId++, produto.CodigoProduto,
        entrada ? "Entrada" : "Saída", qtde, descricao, DateTime.Now);
    movimentacoes.Add(mov);

    // Salva tudo nos arquivos, para não perder os dados ao fechar o programa
    File.WriteAllText("estoque.json", JsonSerializer.Serialize(dados, opts));
    File.WriteAllText(arqMov, JsonSerializer.Serialize(movimentacoes, opts));

    // Mostra o resultado final pedido no enunciado: a quantidade final do produto
    Console.WriteLine($"\nMovimentação #{mov.Id} registrada ({mov.Descricao}).");
    Console.WriteLine($"Estoque final de '{produto.DescricaoProduto}': {produto.Estoque}");
}

// Molde de um produto (precisa de "set" porque vamos alterar o Estoque)
class Produto
{
    public int CodigoProduto { get; set; }
    public string DescricaoProduto { get; set; } = "";
    public int Estoque { get; set; }
}

// Molde do arquivo: uma lista de produtos chamada "estoque"
class DadosEstoque { public List<Produto> Estoque { get; set; } = new(); }

// Molde de uma movimentação: guarda o que aconteceu, quando e por quê
record Movimentacao(int Id, int CodigoProduto, string Tipo, int Quantidade, string Descricao, DateTime Data);
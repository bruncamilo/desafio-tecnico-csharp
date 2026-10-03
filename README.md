# Desafio Técnico em C#

Solução de três exercícios em C# (.NET), organizados em uma solution com um projeto de console para cada questão.

## Estrutura

```
DesafioCsharp/
├── Questao1/   → Comissão por vendedor
├── Questao2/   → Movimentação de estoque
├── Questao3/   → Juros por atraso
└── DesafioCsharp.sln
```

## Pré-requisitos

- [.NET SDK](https://dotnet.microsoft.com/download) (versão 10, ou a que estiver configurada nos projetos)
- Visual Studio 2022+ ou VS Code (opcional, a CLI do `dotnet` já basta)

## Como executar

Pela linha de comando, na pasta da solution:

```bash
dotnet run --project Questao1
dotnet run --project Questao2
dotnet run --project Questao3
```

Pelo Visual Studio: clique com o botão direito no projeto desejado → **Set as Startup Project** → **F5**.

---

## Questão 1: Comissão por vendedor

Lê o arquivo `vendas.json`, agrupa as vendas por vendedor e soma a comissão de cada venda.

| Valor da venda | Comissão |
|---|---|
| Abaixo de R$ 100,00 | Sem comissão |
| De R$ 100,00 a R$ 499,99 | 1% |
| A partir de R$ 500,00 | 5% |

**Decisões:**
- Usei `decimal` em vez de `double`, por ser o tipo adequado para valores monetários.
- Uma venda de exatamente R$ 500,00 entra na faixa de 5% ("a partir de").
- O arredondamento é feito a cada venda, para 2 casas, com `MidpointRounding.AwayFromZero` (arredondamento tradicional).

**Resultado esperado:**

```
João Silva: R$ 495,69
Maria Souza: R$ 465,96
Carlos Oliveira: R$ 379,38
Ana Lima: R$ 404,98
```

---

## Questão 2: Movimentação de estoque

Programa de console interativo para lançar **entradas** e **saídas** dos produtos listados em `estoque.json`.

Cada movimentação possui:
- um **identificador único** e sequencial;
- uma **descrição** informada pelo usuário (ex.: Compra, Venda, Devolução, Ajuste);
- tipo (entrada ou saída), quantidade e data/hora.

Ao final de cada lançamento, o programa exibe a **quantidade final em estoque** do produto movimentado.

**Decisões:**
- Não é permitido dar saída em quantidade maior que o estoque disponível.
- O estoque atualizado é gravado em `estoque.json` e o histórico em `movimentacoes.json` (na pasta de execução, `bin/Debug/...`). Por isso o ID continua de onde parou ao reabrir o programa.

---

## Questão 3: Juros por atraso

Recebe um **valor** e uma **data de vencimento** e calcula os juros até a data de hoje, considerando taxa de **2,5% ao dia**.

```
juros = valor × 0,025 × dias de atraso
```

**Decisões:**
- O enunciado não especifica o regime, então usei **juros simples**.
- Se o título não estiver vencido (ou vencer hoje), os juros são R$ 0,00.
- O programa exibe os dias em atraso, os juros e o total a pagar.

---

## Tecnologias

- C# / .NET
- System.Text.Json
- LINQ
// See https://aka.ms/new-console-template for more information
using Grafos.Utils;

Console.WriteLine("1 - Informe o local do arquivo .csv que contém as informações do grafo:");
string caminho = Console.ReadLine();

var grafo = CsvReader.ProcessarCsv(caminho);
Console.WriteLine("Grafo carregado com sucesso!");
Console.WriteLine(grafo.ExibirMatriz());
Console.WriteLine("Digite uma das opções abaixo:");
Console.WriteLine("1 - Remover vértice:");
Console.WriteLine("2 - Adicionar vértice:");
Console.WriteLine("3 - Adicionar aresta:");

var opcao = int.Parse(Console.ReadLine());
switch (opcao)
{
    case 1:
        Console.WriteLine("Escolha qual o vértice que deseja remover:");
        Console.WriteLine(grafo.MostrarVertices());
        var verticeParaRemover = Console.ReadLine();
        grafo.RemoverVertice(verticeParaRemover);
        Console.WriteLine(grafo.ExibirMatriz());
        Console.WriteLine($"Vertice {verticeParaRemover} removido com sucesso!");
        break;
    case 2:
        Console.WriteLine("Qual o nome do vértice que deseja adicionar?");
        var novoVertice = Console.ReadLine();
        grafo.AdicionarVertice(novoVertice);
        Console.WriteLine(grafo.ExibirMatriz());
        Console.WriteLine("Vértice adicionado com sucesso!");
        break;
     case 3:
        Console.WriteLine("Informe o vértice de origem:");
        var verticeOrigem = Console.ReadLine();
        Console.WriteLine("Informe o vértice de destino:");
        var verticeDestino = Console.ReadLine();
        Console.WriteLine("Informe o peso da aresta, caso não tenha, digite 0:");
        var peso = int.Parse(Console.ReadLine());
        grafo.AdicionarAresta(verticeOrigem,verticeDestino, peso);
        Console.WriteLine("Aresta adicionada com sucesso!");
        Console.WriteLine(grafo.ExibirMatriz());
        break;
}






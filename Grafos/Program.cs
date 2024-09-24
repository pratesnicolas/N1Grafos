// See https://aka.ms/new-console-template for more information
using Grafos.Utils;

Console.WriteLine("1 - Informe o local do arquivo .csv que contém as informações do grafo:");
string caminho = Console.ReadLine();

var grafo = CsvReader.ProcessarCsv(caminho);
Console.WriteLine("Grafo carregado com sucesso!");
Console.WriteLine(grafo.ExibirMatriz());
Console.WriteLine("Digite uma das opções abaixo:");
Console.WriteLine("1 - Remover vértice:");
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

}






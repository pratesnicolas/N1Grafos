// See https://aka.ms/new-console-template for more information
using Grafos.Utils;

Console.WriteLine("1 - Selecione o local do arquivo!");
string caminho = Console.ReadLine();

var grafo = CsvReader.ProcessarCsv(caminho);
Console.WriteLine(grafo.ExibirMatriz());


